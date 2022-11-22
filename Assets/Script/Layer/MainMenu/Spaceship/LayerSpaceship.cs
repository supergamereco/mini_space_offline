using System;
using System.Collections.Generic;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.Manager;
using NFT1Forge.OSY.System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NFT1Forge.OSY.JsonModel;

namespace NFT1Forge.OSY.Layer
{
    public class LayerSpaceship : BaseLayer
    {
        [SerializeField] private Transform m_ItemHolder;
        [SerializeField] private Button m_ButtonClose;
        [SerializeField] private Button m_UpgradeButton;
        [SerializeField] private TextMeshProUGUI m_TextGoldAmountUpgrade;
        [SerializeField] private Button m_SwitchButton;
        [SerializeField] private Button m_ButtonMint;
        [SerializeField] private RawImage m_RawImageSelectedSpaceship;

        [SerializeField] private TextMeshProUGUI m_TextUid;
        [SerializeField] private TextMeshProUGUI m_TextGold;

        [SerializeField] private TextMeshProUGUI m_TextSpaceshipName;
        [SerializeField] private TextMeshProUGUI m_TextSpaceshipLevel;
        [SerializeField] private TextMeshProUGUI m_TextSpaceshipWeaponDamage;
        [SerializeField] private TextMeshProUGUI m_TextSpaceshipArmor;
        [SerializeField] private Image m_Sstar;
        [SerializeField] private Image m_Astar;
        [SerializeField] private Image m_Bstar;

        [Header("Mint item")]
        [SerializeField] private Transform m_PanelMintConfirmation;
        [SerializeField] private Button m_ButtonMintConfirm;
        [SerializeField] private Button m_ButtonMintCancel;

        [Header("SFX")]
        [SerializeField] private AudioClip m_SfxOk;
        [SerializeField] private AudioClip m_SfxCancel;

        public Action OnClosed;
        public Action<string> OnSwitchSpaceship;

        private readonly List<InventorySlot> m_ItemList = new List<InventorySlot>();
        private SpaceshipDataModel m_SelectedSpaceship;
        private ushort m_CurrentUpgradeCost;
        private AudioSource m_Sfx;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override void Initialize()
        {
            m_Sfx = GetComponent<AudioSource>();
            m_ButtonClose.onClick.AddListener(OnCloseButton);
            m_UpgradeButton.onClick.AddListener(OnUpgradeButton);
            m_SwitchButton.onClick.AddListener(OnSwitchButton);
            m_ButtonMint.onClick.AddListener(OnMintButton);
            m_ButtonMintConfirm.onClick.AddListener(OnConfirmMintButton);
            m_ButtonMintCancel.onClick.AddListener(OnCancleMintButton);

            string uidText = "UID"; //We might have wallet address / UID in the future
            m_TextUid.text = $"{uidText}: {AuthenticationSystem.I.WalletAddress.Ellipsis(10)}";

            OnSoftCurrency1Changed(InventorySystem.I.SoftCurrency1);
            InventorySystem.I.OnSoftCurrency1Changed += OnSoftCurrency1Changed;
            InventorySystem.I.OnInventoryRefresh += OnInventoryRefresh;
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateInventoryList()
        {
            ObjectPoolManager.I.SetGlobalObjectHolder(m_ItemHolder);
            for (int i = 0; i < InventorySystem.I.SpaceshipCount; i++)
            {
                CreateInventoryItem(InventorySystem.I.GetSpaceshipByIndex(i));
            }
            SpaceshipDataModel current = InventorySystem.I.GetActiveSpaceship();
            if (null != current)
            {
                LoadSpaceshipData(current);
                m_SelectedSpaceship = current;
            }
        }
        /// <summary>
        /// Load spaceship level data from meta and display on screen
        /// </summary>
        /// <param name="data"></param>
        public void LoadSpaceshipData(SpaceshipDataModel data)
        {
            AssetCacheManager.I.SetTextureToRawImage(m_RawImageSelectedSpaceship, data.GetImagePath());

            m_TextSpaceshipName.text = data.name;
            if (data.rank.Equals("B"))
            {
                m_Bstar.gameObject.SetActive(true);
                m_Astar.gameObject.SetActive(false);
                m_Sstar.gameObject.SetActive(false);
            }
            else if (data.rank.Equals("A"))
            {
                m_Bstar.gameObject.SetActive(true);
                m_Astar.gameObject.SetActive(true);
                m_Sstar.gameObject.SetActive(false);
            }
            else if (data.rank.Equals("S"))
            {
                m_Bstar.gameObject.SetActive(true);
                m_Astar.gameObject.SetActive(true);
                m_Sstar.gameObject.SetActive(true);
            }
            m_SwitchButton.gameObject.SetActive(InventorySystem.I.GetActiveSpaceship() != data);
            m_ButtonMint.gameObject.SetActive(!data.is_nft);
            m_TextSpaceshipLevel.text = data.level.ToString();
            m_TextSpaceshipWeaponDamage.text = $"{data.damage:0.00}";
            m_TextSpaceshipArmor.text = $"{data.armor:0.00}";
            m_CurrentUpgradeCost = data.GetLevel().upgrade_cost;
            m_TextGoldAmountUpgrade.text = $"{m_CurrentUpgradeCost:#,###,##0} GOLD";
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                ActiveObjectRequestModel activeModel = new ActiveObjectRequestModel("0", data.token_id, "0", "0", "0");
                WebBridgeUtils.SelectNFT(JsonUtility.ToJson(activeModel));
            }
#endif
        }
        /// <summary>
        /// Callback for close button
        /// </summary>
        private void OnCloseButton()
        {
            ClearItemList();
            OnClosed?.Invoke();
        }
        /// <summary>
        /// Return all slots to pool and clear item list 
        /// </summary>
        private void ClearItemList()
        {
            for (int i = 0; i < m_ItemList.Count; i++)
            {
                ObjectPoolManager.I.ReturnToPool(m_ItemList[i], ObjectType.UI);
            }
            m_ItemList.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        private void CreateInventoryItem(SpaceshipDataModel spaceship)
        {
            InventorySlot slot = ObjectPoolManager.I.GetObject<InventorySlot>(ObjectType.UI, "Layer/InventorySlot");
            slot.transform.localScale = Vector3.one;
            //bool isActive = spaceship.token_id == InventorySystem.I.ActiveObject.active_spaceship;
            slot.Setup(spaceship.token_id, spaceship.name, spaceship.GetImagePath(), spaceship.is_nft, spaceship.is_active, spaceship.is_active, OnItemSelect, NFTCollectionType.Spaceship);
            m_ItemList.Add(slot);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        private void OnItemSelect(string tokenId, NFTCollectionType type)
        {
            SpaceshipDataModel selected = InventorySystem.I.GetSpaceship(tokenId);
            if (null != selected)
            {
                if (null != m_Sfx)
                    m_Sfx.PlayOneShot(m_SfxOk);
                LoadSpaceshipData(selected);
                m_SelectedSpaceship = selected;
                UpdateSlot();
#if UNITY_WEBGL
                if (BuildType.WebPlayer == SystemConfig.BuildType)
                {
                    ActiveObjectRequestModel activeModel = new ActiveObjectRequestModel("0", tokenId, "0", "0", "0");
                    WebBridgeUtils.SelectNFT(JsonUtility.ToJson(activeModel));
                    WebBridgeUtils.PageAction(((int)PageActionType.SelectSpaceship).ToString(), "");
                }
#endif
                EventLogManager.I.SimpleEvent("game_demo_nft_info_spaceship_click_in_game");
            }
            else
            {
                Debug.LogError("Invalid spaceship token id");
            }
        }
        /// <summary>
        /// Set current spaceship and send callback
        /// </summary>
        private void OnSwitchButton()
        {
            if (null != m_SelectedSpaceship)
            {
                if (null != m_Sfx)
                    m_Sfx.PlayOneShot(m_SfxOk);
                StartCoroutine(InventorySystem.I.SetActiveSpaceship(m_SelectedSpaceship.token_id));
                UpdateSlot();
                m_SwitchButton.gameObject.SetActive(InventorySystem.I.GetActiveSpaceship() != m_SelectedSpaceship);
                OnSwitchSpaceship?.Invoke(m_SelectedSpaceship.token_id);
            }
            else
            {
                Debug.LogError("Invalid pilot uid");
            }
        }
        /// <summary>
        /// ButtonSwitch clicked callback
        /// </summary>
        private void OnMintButton()
        {
            if (null != m_SelectedSpaceship)
            {
                if (null != m_Sfx)
                    m_Sfx.PlayOneShot(m_SfxOk);
                m_PanelMintConfirmation.SetActive(true);
            }
            else
            {
                Debug.LogError("Invalid pilot uid");
            }
        }
        /// <summary>
        /// ButtonMintConfirm clicked callback
        /// </summary>
        private void OnConfirmMintButton()
        {
            //if (null != m_SelectedSpaceship)
            //{
            //    if (null != m_Sfx)
            //        m_Sfx.PlayOneShot(m_SfxOk);
            //    m_PanelMintConfirmation.SetActive(false);
            //    StartCoroutine(MintingSystem.I.CallAPIMinting(m_SelectedSpaceship));
            //}
            //else
            //{
            //    Debug.LogError("Invalid pilot uid");
            //}
        }
        /// <summary>
        /// ButtonMintCancel clicked callback
        /// </summary>
        private void OnCancleMintButton()
        {
            if (null != m_SelectedSpaceship)
            {
                if (null != m_Sfx)
                    m_Sfx.PlayOneShot(m_SfxOk);
                m_PanelMintConfirmation.SetActive(false);
            }
            else
            {
                Debug.LogError("Invalid pilot uid");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateSlot()
        {
            for (int i = 0; i < m_ItemList.Count; i++)
            {
                SpaceshipDataModel spaceship = InventorySystem.I.GetSpaceshipByIndex(i);
                bool isSelected = spaceship.token_id == m_SelectedSpaceship.token_id;
                m_ItemList[i].Setup(spaceship.token_id, spaceship.name, spaceship.image, spaceship.is_nft, spaceship.is_active, isSelected, OnItemSelect, NFTCollectionType.Spaceship);
            }
        }
        /// <summary>
        /// Upgrade button callback
        /// </summary>
        private void OnUpgradeButton()
        {
            if (m_CurrentUpgradeCost > InventorySystem.I.SoftCurrency1)
            {
                LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_NOT_ENOUGH_GOLD"));
                return;
            }
            if (99 <= m_SelectedSpaceship.level)
            {
                LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_SPACESHIP_MAX_LEVEL"));
                return;
            }
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxOk);

            LayerSystem.I.ShowSoftLoadingScreen();
            StartCoroutine(InventorySystem.I.LevelupSpaceship(m_SelectedSpaceship.token_id, m_CurrentUpgradeCost));
        }

        #region Event Listener
        /// <summary>
        /// Display current currency on screen
        /// </summary>
        /// <param name="balance"></param>
        private void OnSoftCurrency1Changed(uint balance)
        {
            m_TextGold.text = $"{balance:###,###,##0}";
        }
        /// <summary>
        /// Subscribe to Inventorysystem
        /// </summary>
        private void OnInventoryRefresh()
        {
            ClearItemList();
            UpdateInventoryList();
        }
        #endregion
    }
}
