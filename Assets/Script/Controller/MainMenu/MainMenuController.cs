using System;
using System.Collections;
using NFT1Forge.OSY.API;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Layer;
using NFT1Forge.OSY.Manager;
using NFT1Forge.OSY.System;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.MainMenu
{
    public class MainMenuController : BaseController
    {
        [SerializeField] private Transform m_SpaceshipHolder;
        [SerializeField] private Transform m_PilotHolder;
        [Header("SFX")]
        [SerializeField] private AudioClip m_SfxButtonClick;
        [SerializeField] private AudioClip m_SfxBack;

        private PilotAvatar m_Pilot;
        private Transform m_Spaceship;
        private LayerDebugging m_LayerDebugging;
        private LayerMainMenu m_LayerMainMenu;
        private LayerSetting m_LayerSetting;
        private LayerGameMode m_LayerGameMode;
        private LayerPilot m_LayerPilot;
        private LayerSpaceship m_LayerSpaceship;
        private LayerInventory m_LayerInventory;
        private AudioSource m_Sfx;
        // <summary>
        /// Initialize controller
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Initialize()
        {
            yield return base.Initialize();
            SoundManager.I.MixMainMenu();
            m_Sfx = GetComponent<AudioSource>();
            LayerSystem.I.GetLoadingScreen().UpdateProgress(10f);
            LayerSystem.I.GetLoadingScreen().SetMessage(LocalizationSystem.I.GetLocalizeValue("LOADING_SCREEN_TILTLE"));
            yield return new WaitUntil(() => LayerSystem.I.GetLoadingScreen().IsAnimationFinish);
            LayerSystem.I.CreateLayer<LayerDebugging>("LayerDebugging", LayerType.NormalLayer, (layer) =>
            {
                m_LayerDebugging = layer;
                layer.OnClosed = ShowMainMenuLayer;
                layer.HideLayer();
            });
            LayerSystem.I.CreateLayer<LayerMainMenu>("LayerMainMenu", LayerType.NormalLayer, (layer) =>
            {
                m_LayerMainMenu = layer;
                layer.OnPlay = CheckActivationItem;
                layer.OnSetting = ShowSettingLayer;
                layer.OnManageNft = OpenManageNftUrl;
                layer.OnPilot = ShowPilotLayer;
                layer.OnSpaceship = ShowSpaceshipLayer;
                layer.OnInventory = ShowInventoryLayer;
                InventorySystem.I.OnItemActivated += layer.UpdateEventCallback;
                layer.OnDebug = OnDebug;
            });
            LayerSystem.I.CreateLayer<LayerSetting>("LayerSetting", LayerType.NormalLayer, (layer) =>
            {
                m_LayerSetting = layer;
                layer.OnClosed = ShowMainMenuLayer;
                layer.OnLangaugeChanged = () => StartCoroutine(OnLanguageChange());
                layer.HideLayer();
            });
            LayerSystem.I.CreateLayer<LayerGameMode>("LayerGameMode", LayerType.NormalLayer, (layer) =>
            {
                m_LayerGameMode = layer;
                layer.OnClosed = ShowMainMenuLayer;
                layer.OnPlayNormalStage = PlayNormalStage;
                layer.OnPlaySurvivalStage = PlaySurvivalStage;
                layer.HideLayer();
            });
            LayerSystem.I.CreateLayer<LayerPilot>("LayerPilot", LayerType.NormalLayer, (layer) =>
            {
                m_LayerPilot = layer;
                layer.OnSwitchPilot = SwitchPilot;
                layer.OnPilotColorChanged = SetPilotColor;
                layer.OnClosed = ShowMainMenuLayer;
                InventorySystem.I.OnPilotUpgraded += layer.LoadPilotData;
                m_LayerPilot.HideLayer();
            });
            LayerSystem.I.CreateLayer<LayerSpaceship>("LayerSpaceship", LayerType.NormalLayer, (layer) =>
            {
                m_LayerSpaceship = layer;
                layer.OnSwitchSpaceship = SwitchSpaceship;
                layer.OnClosed = ShowMainMenuLayer;
                InventorySystem.I.OnSpaceShipUpgraded += layer.LoadSpaceshipData;
                m_LayerSpaceship.HideLayer();
            });
            LayerSystem.I.CreateLayer<LayerInventory>("LayerInventory", LayerType.NormalLayer, (layer) =>
            {
                m_LayerInventory = layer;
                layer.OnClosed = ShowMainMenuLayer;
                m_LayerInventory.HideLayer();
            });
            //yield return new WaitUntil(() => InventorySystem.I.IsApiFinished);
            LayerSystem.I.GetLoadingScreen().UpdateProgress(66.66f);
            yield return new WaitUntil(() => LayerSystem.I.GetLoadingScreen().IsAnimationFinish);
            LoadPilot(InventorySystem.I.GetActivePilot());
            LoadSpaceship(InventorySystem.I.GetActiveSpaceship());
            LayerSystem.I.GetLoadingScreen().UpdateProgress(100f);
            yield return new WaitUntil(() => LayerSystem.I.GetLoadingScreen().IsAnimationFinish);
            LayerSystem.I.HideLoadingScreen();
            LayerSystem.I.HideSoftLoadingScreen();
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.PageAction(((int)PageActionType.EnterMainMenu).ToString(), "");
            }
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Shutdown()
        {
            yield return base.Shutdown();
            InventorySystem.I.OnSpaceShipUpgraded -= m_LayerSpaceship.LoadSpaceshipData;
            InventorySystem.I.OnPilotUpgraded -= m_LayerPilot.LoadPilotData;
            InventorySystem.I.OnItemActivated -= m_LayerMainMenu.UpdateEventCallback;
            m_LayerMainMenu.Delete();
            m_LayerGameMode.Delete();
            m_LayerInventory.Delete();
            m_LayerPilot.Delete();
            m_LayerSpaceship.Delete();
            if (m_Pilot)
                ObjectPoolManager.I.ReturnToPool(m_Pilot, ObjectType.Pilot);
            if (m_Spaceship)
                ObjectPoolManager.I.ReturnToPool(m_Spaceship, ObjectType.PlayerSpaceShip);
        }
        /// <summary>
        /// Set game mode to normal then change scene to gameplay
        /// </summary>
        private void PlayNormalStage()
        {
            EventLogManager.I.PlayGame("Normal Stage");
            StartCoroutine(StartNormalStage());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartNormalStage()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxButtonClick);
            GameplayDataManager.I.CurrentGameMode = GameMode.Normal;
            if (1 == PlayerMissionManager.I.PlayerMissionStep)
            {
                CoroutineWithData api = new CoroutineWithData(this, PlayerMissionManager.I.MissionCompleted(1));
                yield return api.coroutine;
                if (!(bool)api.result)
                {
                    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
                }
            }
            yield return ChangeToGameplay();
        }
        /// <summary>
        /// Set game mode to survival, activate survival pass then change scene to gameplay
        /// </summary>
        private void PlaySurvivalStage()
        {
            EventLogManager.I.PlayGame("Survival Stage");
            StartCoroutine(StartSurvivalStage());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartSurvivalStage()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxButtonClick);
            GameplayDataManager.I.CurrentGameMode = GameMode.Survival;
            if (15 == PlayerMissionManager.I.PlayerMissionStep)
            {
                CoroutineWithData api = new CoroutineWithData(this, PlayerMissionManager.I.MissionCompleted(15));
                yield return api.coroutine;
                if (!(bool)api.result)
                {
                    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
                }
            }
            yield return ChangeToGameplay();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangeToGameplay()
        {
            LayerSystem.I.ShowLoadingScreen();
            yield return Shutdown();
            ControllerSystem.I.SceneChange("Scene/Gameplay");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefabName"></param>
        private void LoadPilot(PilotDataModel pilot)
        {
            m_Pilot = ObjectPoolManager.I.GetObject<PilotAvatar>(ObjectType.Pilot, $"Pilot/{pilot.GetMaster().asset_path}", m_PilotHolder);
            m_Pilot.transform.localPosition = Vector3.zero;
            m_Pilot.transform.localEulerAngles = Vector3.zero;
            m_Pilot.transform.localScale = Vector3.one;
            SetPilotColor(pilot);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pilot"></param>
        private void SetPilotColor(PilotDataModel pilot)
        {
            Enum.TryParse(pilot.color1, out HairColor hairColor);
            if (HairColor.None == hairColor) hairColor = HairColor.Black;
            Enum.TryParse(pilot.color2, out CostumeColor costumeColor);
            if (CostumeColor.None == costumeColor) costumeColor = CostumeColor.Orange;

            m_Pilot.SetHeadColor(hairColor);
            m_Pilot.SetBodyColor(costumeColor);
            m_LayerMainMenu.UpdateAllInfo();
        }
        /// <summary>
        /// Load spaceship 3D model
        /// </summary>
        private void LoadSpaceship(SpaceshipDataModel spaceship)
        {
            if (null != spaceship)
            {
                m_Spaceship = ObjectPoolManager.I.GetObject(ObjectType.PlayerSpaceShip,
                    spaceship.GetMaster().asset_path, m_SpaceshipHolder);
            }
        }
        /// <summary>
        /// Unload spaceship and load a new one
        /// </summary>
        private void SwitchSpaceship(string tokenId)
        {
            ObjectPoolManager.I.ReturnToPool(m_Spaceship, ObjectType.PlayerSpaceShip);
            LoadSpaceship(InventorySystem.I.GetSpaceship(tokenId));
            m_LayerMainMenu.UpdateAllInfo();
        }
        /// <summary>
        /// Unload pilot and load a new one
        /// </summary>
        private void SwitchPilot(string tokenId)
        {
            ObjectPoolManager.I.ReturnToPool(m_Pilot, ObjectType.Pilot);
            LoadPilot(InventorySystem.I.GetPilot(tokenId));
            m_LayerMainMenu.UpdateAllInfo();
        }
        /// <summary>
        /// Hide all layer
        /// </summary>
        private void HideAllLayer()
        {
            if (m_LayerDebugging) m_LayerDebugging.HideLayer();
            if (m_LayerSetting) m_LayerSetting.HideLayer();
            if (m_LayerMainMenu) m_LayerMainMenu.HideLayer();
            if (m_LayerGameMode) m_LayerGameMode.HideLayer();
            if (m_LayerPilot) m_LayerPilot.HideLayer();
            if (m_LayerSpaceship) m_LayerSpaceship.HideLayer();
            if (m_LayerInventory) m_LayerInventory.HideLayer();
        }
        /// <summary>
        /// Show main menu
        /// </summary>
        private void ShowMainMenuLayer()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxBack);
            HideAllLayer();
            m_LayerMainMenu.UpdateAllInfo();
            m_LayerMainMenu.ShowLayer();
        }
        /// <summary>
        /// Check the player did not activate any item and Ask for open inventory for activation.
        /// </summary>
        private void CheckActivationItem()
        {
            bool IsCanActivateChest = (null == InventorySystem.I.GetActiveChest() && 0 != InventorySystem.I.ChestCount);
            bool IsCanActivateSpecialWeapon = (null == InventorySystem.I.GetActiveWeapon() && 0 != InventorySystem.I.WeaponCount);

            if (IsCanActivateChest || IsCanActivateSpecialWeapon)
            {
                LayerSystem.I.ShowPopupConfirmationDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_CONFIRMATION_ACTIVATE"), ShowInventoryLayer
                    , ShowGameModeLayer);
            }
            else
                ShowGameModeLayer();
        }
        /// <summary>
        /// Show game mode selection layer
        /// </summary>
        private void ShowGameModeLayer()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxButtonClick);
            HideAllLayer();
            m_LayerGameMode.ShowLayer();
        }
        /// <summary>
        /// Open external app for DApp url
        /// </summary>
        private void OpenManageNftUrl()
        {
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                if (1 == WebBridgeUtils.GetMobileStatus())
                {
                    if (null != m_Sfx)
                        m_Sfx.PlayOneShot(m_SfxButtonClick);
                    Application.OpenURL("https://metamask.app.link/dapp/minispace.nft1.global/manage");
                }
            }
#endif
        }
        /// <summary>
        /// Show spaceship layer
        /// </summary>
        private void ShowSettingLayer()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxButtonClick);
            HideAllLayer();
            m_LayerSetting.ShowLayer();
        }
        /// <summary>
        /// Show spaceship layer
        /// </summary>
        private void ShowPilotLayer()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxButtonClick);
            HideAllLayer();
            m_LayerPilot.UpdateInventoryList();
            m_LayerPilot.ShowLayer();
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.PageAction(((int)PageActionType.EnterPilotScreen).ToString(), "");
            }
#endif
        }
        /// <summary>
        /// Show spaceship layer
        /// </summary>
        private void ShowSpaceshipLayer()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxButtonClick);
            HideAllLayer();
            m_LayerSpaceship.UpdateInventoryList();
            m_LayerSpaceship.ShowLayer();
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.PageAction(((int)PageActionType.EnterSpaceshipScreen).ToString(), "");
            }
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        private void ShowInventoryLayer()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxButtonClick);
            HideAllLayer();
            m_LayerInventory.UpdateInventoryList();
            m_LayerInventory.ShowLayer();
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.PageAction(((int)PageActionType.EnterMissionChestScreen).ToString(), "");
            }
#endif
        }
        /// <summary>
        ///
        /// </summary>
        private IEnumerator OnLanguageChange()
        {
            InitController initController = GameObject.FindObjectOfType<InitController>();
            if (null != initController)
            {
                StartCoroutine(initController.LoadMetaData());
                yield return new WaitUntil(() => DatabaseSystem.I.IsMetadataReady);
                LayerSystem.I.ShowSoftLoadingScreen();
                yield return new WaitForSeconds(1f);
                LayerSystem.I.HideSoftLoadingScreen();
                InventorySystem.I.ChangedLanguageSyncMasterData();
                ShowMainMenuLayer();
            }
        }

        #region Debugging
        private void OnDebug()
        {
            HideAllLayer();
            m_LayerDebugging.ShowLayer();
        }
        #endregion
    }
}
