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
    public class LayerPilot : BaseLayer
    {
        [SerializeField] private Transform m_ItemHolder;
        [SerializeField] private Button m_ButtonClose;
        [SerializeField] private Button m_UpgradeButton;
        [SerializeField] private TextMeshProUGUI m_TextGoldAmountUpgrade;
        [SerializeField] private Button m_ButtonSwitch;
        [SerializeField] private Button m_ButtonMint;
        [SerializeField] private Button m_ButtonWardrobe;
        [SerializeField] private RawImage m_RawImageSelectedPilot;

        [SerializeField] private TextMeshProUGUI m_TextUid;
        [SerializeField] private TextMeshProUGUI m_TextGold;

        [SerializeField] private TextMeshProUGUI m_TextPilotName;
        [SerializeField] private TextMeshProUGUI m_TextPilotLevel;
        [SerializeField] private TextMeshProUGUI m_TextHighestScore;
        [SerializeField] private TextMeshProUGUI m_TextPilotGoldBooster;
        [SerializeField] private TextMeshProUGUI m_TextPilotSPWeaponCharge;

        [SerializeField] private Image m_Sstar;
        [SerializeField] private Image m_Astar;
        [SerializeField] private Image m_Bstar;

        [Header("Mint item")]
        [SerializeField] private Transform m_PanelMintConfirmation;
        [SerializeField] private Button m_ButtonMintConfirm;
        [SerializeField] private Button m_ButtonMintCancel;

        [Header("Customize")]
        [SerializeField] private List<ButtonColor> m_ButtonHairColorList;
        [SerializeField] private List<ButtonColor> m_ButtonCostumeColorList;
        [SerializeField] private Button m_ButtonConfirm;
        [SerializeField] private RawImage m_RawImagePreview;

        [Header("Panel")]
        [SerializeField] private Transform m_PanelWardrobe;
        [SerializeField] private Transform m_PanelPilot;

        [Header("SFX")]
        [SerializeField] private AudioClip m_SfxOk;
        [SerializeField] private AudioClip m_SfxCancel;

        public Action OnClosed;
        public Action<string> OnSwitchPilot;
        public Action<PilotDataModel> OnPilotColorChanged;

        private readonly List<InventorySlot> m_ItemList = new List<InventorySlot>();
        private PilotDataModel m_SelectedPilot;
        private ushort m_CurrentUpgradeCost;
        private bool m_isShowWardrobe;

        private ushort m_CurrentHairColorId = 1;
        private ushort m_CurrentCostumeColorId = 1;

        private AudioSource m_Sfx;

        /// <summary>
        /// Initializing layer
        /// </summary>
        /// <returns></returns>
        public override void Initialize()
        {
            m_Sfx = GetComponent<AudioSource>();
            m_ButtonClose.onClick.AddListener(OnCloseButton);
            m_UpgradeButton.onClick.AddListener(OnUpgradeButton);
            m_ButtonSwitch.onClick.AddListener(OnSwitchButton);
            m_ButtonMint.onClick.AddListener(OnMintButton);
            m_ButtonMintConfirm.onClick.AddListener(OnConfirmMintButton);
            m_ButtonMintCancel.onClick.AddListener(OnCancleMintButton);
            m_ButtonWardrobe.onClick.AddListener(OnWardrobeButton);

            m_ButtonConfirm.onClick.AddListener(OnConfirmColorButton);

            for (int i = 0; i < m_ButtonHairColorList.Count; i++)
            {
                m_ButtonHairColorList[i].OnSelected = SetHairColor;
            }
            for (int i = 0; i < m_ButtonCostumeColorList.Count; i++)
            {
                m_ButtonCostumeColorList[i].OnSelected = SetCostumeColor;
            }
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
            for (int i = 0; i < InventorySystem.I.PilotCount; i++)
            {
                CreateInventorySlot(InventorySystem.I.GetPilotByIndex(i));
            }
            PilotDataModel current = InventorySystem.I.GetActivePilot();
            if (null != current)
            {
                LoadPilotData(current);
                m_SelectedPilot = current;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorId"></param>
        private void SetHairColor(ushort colorId)
        {
            m_CurrentHairColorId = colorId;
            for (int i = 0; i < m_ButtonHairColorList.Count; i++)
            {
                m_ButtonHairColorList[i].transform.GetChild(0).SetActive(i + 1 == colorId);
            }
            PreviewColor();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorId"></param>
        private void SetCostumeColor(ushort colorId)
        {
            m_CurrentCostumeColorId = colorId;
            for (int i = 0; i < m_ButtonCostumeColorList.Count; i++)
            {
                m_ButtonCostumeColorList[i].transform.GetChild(0).SetActive(i + 1 == colorId);
            }
            PreviewColor();
        }
        /// <summary>
        /// 
        /// </summary>
        private void PreviewColor()
        {
            PilotMasterData pilotMaster = DatabaseSystem.I.GetMetadata<PilotMaster>().pilot_master.Find(
                a => a.name.Equals(m_SelectedPilot.name)
            );
            string imageUrl = $"{SystemConfig.BaseAssetPath}rendered/{pilotMaster.full_image_prefix}{m_CurrentHairColorId}{m_CurrentCostumeColorId}.png";
            AssetCacheManager.I.SetTextureToRawImage(m_RawImagePreview, imageUrl);
        }
        /// <summary>
        /// Load pilot level data from meta and display on screen
        /// </summary>
        /// <param name="id"></param>
        public void LoadPilotData(PilotDataModel data)
        {
            PilotRankLevelData pilotLevel = DatabaseSystem.I.GetMetadata<PilotRankLevel>().pilot_rank_level.Find(
                    a => a.rank.Equals(data.rank) && a.level.Equals(data.level)
                );
            AssetCacheManager.I.SetTextureToRawImage(m_RawImageSelectedPilot, data.GetFullImagePath());
            m_TextPilotName.text = data.name;
            DisplayRankAsStar(data.rank);
            m_ButtonSwitch.gameObject.SetActive(InventorySystem.I.GetActivePilot() != data);
            m_ButtonMint.gameObject.SetActive(!data.is_nft);
            m_TextPilotLevel.text = pilotLevel.level.ToString();
            m_TextHighestScore.text = $"{data.highscore:#,###,##0}";
            m_TextPilotGoldBooster.text = $"{pilotLevel.gold_booster:0.00}";
            m_TextPilotSPWeaponCharge.text = $"{pilotLevel.weapon_charge_speed:0.00}";
            m_TextGoldAmountUpgrade.text = $"{pilotLevel.upgrade_cost:#,###,##0} {LocalizationSystem.I.GetLocalizeValue("CURRENCY_GOLD")}";
            m_CurrentUpgradeCost = pilotLevel.upgrade_cost;
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                ActiveObjectRequestModel activeModel = new ActiveObjectRequestModel(data.token_id, "0", "0", "0", "0");
                WebBridgeUtils.SelectNFT(JsonUtility.ToJson(activeModel));
            }
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        private void DisplayRankAsStar(string rank)
        {
            if (rank.Equals("Cadet"))
            {
                m_Bstar.gameObject.SetActive(true);
                m_Astar.gameObject.SetActive(false);
                m_Sstar.gameObject.SetActive(false);
            }
            else if (rank.Equals("Veteran"))
            {
                m_Bstar.gameObject.SetActive(true);
                m_Astar.gameObject.SetActive(true);
                m_Sstar.gameObject.SetActive(false);
            }
            else if (rank.Equals("Elite"))
            {
                m_Bstar.gameObject.SetActive(true);
                m_Astar.gameObject.SetActive(true);
                m_Sstar.gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// ButtonClose clicked callback
        /// </summary>
        private void OnCloseButton()
        {
            if (m_isShowWardrobe)
            {
                m_isShowWardrobe = false;
                OnCloseWardrobeButton();
            }
            else
            {
                ClearItemList();
                OnClosed?.Invoke();
            }
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
        private void CreateInventorySlot(PilotDataModel pilot)
        {
            InventorySlot slot = ObjectPoolManager.I.GetObject<InventorySlot>(ObjectType.UI, "Layer/InventorySlot");
            slot.transform.localScale = Vector3.one;
            slot.Setup(pilot.token_id, pilot.name, pilot.GetProfileImagePath(), pilot.is_nft, pilot.is_active, pilot.is_active, OnItemSelect, NFTCollectionType.Pilot);
            m_ItemList.Add(slot);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        private void OnItemSelect(string tokenId, NFTCollectionType nftType)
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxOk);
            PilotDataModel selected = InventorySystem.I.GetPilot(tokenId);
            if (null != selected)
            {
                LoadPilotData(selected);
                m_SelectedPilot = selected;
                UpdateSlot();
#if UNITY_WEBGL
                if (BuildType.WebPlayer == SystemConfig.BuildType)
                {
                    ActiveObjectRequestModel activeModel = new ActiveObjectRequestModel(tokenId, "0", "0", "0", "0");
                    WebBridgeUtils.SelectNFT(JsonUtility.ToJson(activeModel));
                    WebBridgeUtils.PageAction(((int)PageActionType.SelectPilot).ToString(), "");
                }
#endif
                EventLogManager.I.SimpleEvent("game_demo_nft_info_pilot_click_in_game");
            }
            else
            {
                Debug.LogError("Invalid pilot uid");
            }
        }
        /// <summary>
        /// ButtonSwitch clicked callback
        /// </summary>
        private void OnSwitchButton()
        {
            if (null != m_SelectedPilot)
            {
                if (null != m_Sfx)
                    m_Sfx.PlayOneShot(m_SfxOk);
                StartCoroutine(InventorySystem.I.SetActivePilot(m_SelectedPilot.token_id));
                UpdateSlot();
                m_ButtonSwitch.gameObject.SetActive(InventorySystem.I.GetActivePilot() != m_SelectedPilot);
                OnSwitchPilot?.Invoke(m_SelectedPilot.token_id);
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
            if (null != m_SelectedPilot)
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
        /// ButtonSwitch clicked callback
        /// </summary>
        private void OnConfirmMintButton()
        {
            //if (null != m_SelectedPilot)
            //{
            //    if (null != m_Sfx)
            //        m_Sfx.PlayOneShot(m_SfxOk);
            //    m_PanelMintConfirmation.SetActive(false);
            //    StartCoroutine(MintingSystem.I.CallAPIMinting(m_SelectedPilot));
            //}
            //else
            //{
            //    Debug.LogError("Invalid pilot uid");
            //}
        }
        /// <summary>
        /// ButtonSwitch clicked callback
        /// </summary>
        private void OnCancleMintButton()
        {
            if (null != m_SelectedPilot)
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
                PilotDataModel pilot = InventorySystem.I.GetPilotByIndex(i);
                bool isSelected = pilot.token_id == m_SelectedPilot.token_id;
                m_ItemList[i].Setup(pilot.token_id, pilot.name, pilot.GetProfileImagePath(), pilot.is_nft, pilot.is_active, isSelected, OnItemSelect, NFTCollectionType.Pilot);
            }
        }
        /// <summary>
        /// Customize button callback
        /// </summary>
        private void OnWardrobeButton()
        {
            m_PanelWardrobe.SetActive(true);
            m_PanelPilot.SetActive(false);
            ShowWardrobe();
            EventLogManager.I.SimpleEvent("pilot_customize_click_in_game");
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
            if(99 <= m_SelectedPilot.level)
            {
                LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_PILOT_MAX_LEVEL"));
                return;
            }
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxOk);

            LayerSystem.I.ShowSoftLoadingScreen();
            StartCoroutine(InventorySystem.I.LevelupPilot(m_SelectedPilot.token_id, m_CurrentUpgradeCost));
        }
        /// <summary>
        /// CloseWardrobe
        /// </summary>
        private void OnCloseWardrobeButton()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxCancel);

            m_PanelPilot.SetActive(true);
            m_PanelWardrobe.SetActive(false);
            EventLogManager.I.SimpleEvent("pilot_cancel_customize_click_in_game");
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnConfirmColorButton()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxOk);

            StartCoroutine(InventorySystem.I.SetPilotColor(m_SelectedPilot.token_id, m_CurrentHairColorId, m_CurrentCostumeColorId));
            LoadPilotData(m_SelectedPilot);
            if (InventorySystem.I.GetActivePilot().token_id == m_SelectedPilot.token_id)
                OnPilotColorChanged?.Invoke(InventorySystem.I.GetActivePilot());
            m_PanelPilot.SetActive(true);
            m_PanelWardrobe.SetActive(false);
            UpdateSlot();
            EventLogManager.I.SimpleEvent("pilot_save_customize_click_in_game");
        }
        private void ShowWardrobe()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxOk);

            m_isShowWardrobe = true;

            Enum.TryParse(m_SelectedPilot.color1, out HairColor hairColor);
            Enum.TryParse(m_SelectedPilot.color2, out CostumeColor costumeColor);
            SetHairColor((ushort)hairColor);
            SetCostumeColor((ushort)costumeColor);
            PreviewColor();
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.PageAction(((int)PageActionType.CustomizePilot).ToString(), "");
            }
#endif
        }

        #region Event Listener
        /// <summary>
        /// Subscribe to Inventorysystem
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
