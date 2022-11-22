using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NFT1Forge.OSY.System;
using NFT1Forge.OSY.JsonModel;
using System.Collections.Generic;
using NFT1Forge.OSY.DataModel;

namespace NFT1Forge.OSY.Layer
{
    public class LayerMainMenu : BaseLayer
    {
        [SerializeField] private TextMeshProUGUI m_TextUid;
        [SerializeField] private TextMeshProUGUI m_TextGold;
        [SerializeField] private TextMeshProUGUI m_TextCurrentPilot;
        [SerializeField] private TextMeshProUGUI m_TextCurrentSpaceship;
        [SerializeField] private TextMeshProUGUI m_TextHighestScore;
        [SerializeField] private TextMeshProUGUI m_TextNameHighestScorePilot;
        [SerializeField] private Image m_ImageHighScorePilot;
        [SerializeField] private Image m_ImageCurrentPilot;
        [SerializeField] private Image m_ImageCurrentSpaceship;
        [SerializeField] private Button m_ButtonSetting;
        [SerializeField] private Button m_ButtonPlay;
        [SerializeField] private Button m_ButtonManageNft;
        [SerializeField] private Button m_ButtonPilot;
        [SerializeField] private Button m_ButtonSpaceship;
        [SerializeField] private Button m_ButtonInventory;
        [SerializeField] private Button m_ButtonDebug;
        [SerializeField] private TextMeshProUGUI m_TextBuildVersion;
        [SerializeField] private List<Image> m_ImageActiveInventoryList;

        public Action OnPlay;
        public Action OnSetting;
        public Action OnManageNft;
        public Action OnPilot;
        public Action OnSpaceship;
        public Action OnInventory;
        public Action OnDebug;

        /// <summary>
        /// Called right after the layer was created
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            m_ButtonPlay.onClick.AddListener(OnPlayButton);
            m_ButtonSetting.onClick.AddListener(OnSettingButton);
            m_ButtonPilot.onClick.AddListener(OnPilotButton);
            m_ButtonSpaceship.onClick.AddListener(OnSpaceshipButton);
            m_ButtonInventory.onClick.AddListener(OnInventoryButton);
            if (SystemConfig.IsDebugEnabled)
            {
                m_ButtonDebug.gameObject.SetActive(true);
                m_ButtonDebug.onClick.AddListener(OnDebugButton);
            }
            else
            {
                m_ButtonDebug.gameObject.SetActive(false);
            }

            m_ButtonManageNft.image.enabled = false;
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                if (1 == WebBridgeUtils.GetMobileStatus())
                {
                    m_ButtonManageNft.image.enabled = true;
                    m_ButtonManageNft.onClick.AddListener(OnManageNftButton);
                }
            }
#endif

            m_TextBuildVersion.text = $"Version : {Application.version}_{SystemConfig.EnvironmentShortName}";
            string uidText = "UID"; //We might have wallet address / UID in the future
            m_TextUid.text = $"{uidText}: {AuthenticationSystem.I.WalletAddress.Ellipsis(10)}";

            InventorySystem.I.OnSoftCurrency1Changed += OnSoftCurrency1Changed;
            UpdateAllInfo();
        }
        public void UpdateEventCallback(string tokenId, NFTCollectionType type)
        {
            UpdateAllInfo();
        }
        /// <summary>
        /// Update display info
        /// </summary>
        public void UpdateAllInfo()
        {
            OnSoftCurrency1Changed(InventorySystem.I.SoftCurrency1);
            OnHighScoreChanged(InventorySystem.I.GetHighestScorePilot());
            PilotDataModel pilot = InventorySystem.I.GetActivePilot();
            if (null != pilot)
            {
                m_TextCurrentPilot.text = pilot.name;
                AssetCacheManager.I.SetSprite(m_ImageCurrentPilot, pilot.GetProfileImagePath());
            }

            SpaceshipDataModel spaceship = InventorySystem.I.GetActiveSpaceship();
            if (null != spaceship)
            {
                m_TextCurrentSpaceship.text = spaceship.name;
                AssetCacheManager.I.SetSprite(m_ImageCurrentSpaceship, spaceship.GetImagePath());
            }

            int activeIndex = 0;
            for (int i = 0; i < m_ImageActiveInventoryList.Count; i++)
            {
                m_ImageActiveInventoryList[i].enabled = false;
            }
            ChestDataModel chest = InventorySystem.I.GetActiveChest();
            if (null != chest)
            {
                AssetCacheManager.I.SetSprite(m_ImageActiveInventoryList[activeIndex], chest.GetImagePath());
                activeIndex++;
            }
            SpecialWeaponDataModel weapon = InventorySystem.I.GetActiveWeapon();
            if (null != weapon)
            {
                AssetCacheManager.I.SetSprite(m_ImageActiveInventoryList[activeIndex], weapon.GetImagePath());
            }
        }
        /// <summary>
        /// On Play button clicked
        /// </summary>
        private void OnPlayButton()
        {
            OnPlay?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnManageNftButton()
        {
            OnManageNft?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnPilotButton()
        {
            OnPilot?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnSpaceshipButton()
        {
            OnSpaceship?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnInventoryButton()
        {
            OnInventory?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="balance"></param>
        private void OnSoftCurrency1Changed(uint balance)
        {
            m_TextGold.text = $"{balance:###,###,##0}";
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnHighScoreChanged(PilotDataModel pilot)
        {
            if (null != pilot)
            {
                m_TextNameHighestScorePilot.text = pilot.name;
                AssetCacheManager.I.SetSprite(m_ImageHighScorePilot, pilot.GetProfileImagePath());
                m_TextHighestScore.text = $"{pilot.highscore:###,###,##0}";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnDebugButton()
        {
            OnDebug?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnSettingButton()
        {
            OnSetting?.Invoke();
        }
    }
}
