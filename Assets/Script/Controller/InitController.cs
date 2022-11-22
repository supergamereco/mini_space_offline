using System.Collections;
using NFT1Forge.OSY.API;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Layer;
using NFT1Forge.OSY.System;
using Script.System;
using UnityEngine;

namespace NFT1Forge.OSY.Controller
{
    public class InitController : MonoBehaviour
    {
        [SerializeField] private GameObject m_PrefabCanvas;
        [SerializeField] private BuildType m_BuildType;
        [SerializeField] private string m_ServerConfigUrl;

        private LayerStart m_LayerStart;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
#if !UNITY_EDITOR
            Debug.unityLogger.logEnabled = false;
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                Firebase.DependencyStatus dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    // app = Firebase.FirebaseApp.DefaultInstance;

                    StartCoroutine(InitializeSystem());
                }
                else
                {
                    m_LayerStart.SetMessage($"Could not start dependencies module: {dependencyStatus}");
                }
            });
#else
            StartCoroutine(InitializeSystem());
#endif
        }
        /// <summary>
        /// Initialize all system for the game
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitializeSystem()
        {
            SystemConfig.BuildType = m_BuildType;
            if(!string.IsNullOrEmpty(PlayerPrefs.GetString("Language")))
                SystemConfig.LanguageCode = PlayerPrefs.GetString("Language");
            yield return LayerSystem.I.Initialize(m_PrefabCanvas);
            yield return new WaitUntil(() => LayerSystem.I.IsInitialized);
            LayerSystem.I.CreateLayer<LayerStart>("LayerStart", LayerType.NormalLayer, (layer) =>
            {
                layer.OnPressStart = OnPressStart;
                m_LayerStart = layer;
            });
            m_LayerStart.SetMessage("Loading Config");
            GetServerConfig();
            yield return new WaitUntil(() => !string.IsNullOrEmpty(SystemConfig.MetadataUrl));
            yield return new WaitForSeconds(0.3f);

            m_LayerStart.SetMessage("Loading Metadata");
            yield return LoadMetaData();
            yield return new WaitForSeconds(0.3f);

            m_LayerStart.SetMessage("Initializing System");
            SetGameConfig();
            SetItemData();
            //TODO
            //get list of preload from metadata
            StartCoroutine(AssetCacheManager.I.PreLoadTextureFromUrl($"{SystemConfig.BaseAssetPath}rendered/pilot1_p_11.png"));
            StartCoroutine(AssetCacheManager.I.PreLoadTextureFromUrl($"{SystemConfig.BaseAssetPath}rendered/pilot2_p_11.png"));
            StartCoroutine(AssetCacheManager.I.PreLoadTextureFromUrl($"{SystemConfig.BaseAssetPath}rendered/spaceship_01.png"));
            StartCoroutine(AssetCacheManager.I.PreLoadTextureFromUrl($"{SystemConfig.BaseAssetPath}rendered/spaceship_02.png"));

            GameplayDataManager.I.Initialize();
            ControllerSystem.I.InitSystem();
            InventorySystem.I.InitSystem();
#if UNITY_WEBGL
            WebBridgeSystem.I.InitSystem();
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.SetAppReady();
            }
#endif
            yield return new WaitForSeconds(0.3f);
            m_LayerStart.Ready();
        }
        /// <summary>
        /// Load config from server
        /// </summary>
        private void GetServerConfig()
        {
            StartCoroutine(NetworkSystem.I.GetRequest(m_ServerConfigUrl, (json) =>
            {
                if (null != json)
                {
                    ServerConfigModel data = JsonUtility.FromJson<ServerConfigModel>(json);
#if UNITY_WEBGL
                    int serverId = BuildType.WebPlayer == SystemConfig.BuildType ? WebBridgeUtils.GetServerId() : 1;
#else
                    int serverId = 1;
#endif
                    ServerConfigData server = data.server_list.Find(a => a.id.Equals((ushort)serverId));
                    if (null != server)
                    {
                        SystemConfig.EnvironmentShortName = data.environment_short_name;
                        APIConfig.OneSyncBaseUrl = server.base_1sync_url;
                        APIConfig.ApiServerBaseUrl = server.base_api_url;
                        SystemConfig.MetadataUrl = data.metadata_url;
                        SystemConfig.BaseAssetPath = data.base_asset_path;

                        APIConfig.AppId = data.app_id;
                        APIConfig.AppSecret = data.app_secret;
                        APIConfig.PilotContractAddress = data.pilot_contract_address;
                        APIConfig.SpaceshipContractAddress = data.spaceship_contract_address;
                        APIConfig.PassContractAddress = data.survival_pass_contract_address;
                        APIConfig.SpecialweaponContractAddress = data.special_weapon_contract_address;
                        APIConfig.ChestContractAddress = data.chest_contract_address;
                    }
                    else
                    {
                        m_LayerStart.SetMessage("Invalid Server");
                    }
                }
            }));
        }
        /// <summary>
        /// Load all metadata to database system
        /// </summary>
        public IEnumerator LoadMetaData()
        {
            DatabaseSystem.I.LoadMetadata<ChestMaster>("chest_master");
            DatabaseSystem.I.LoadMetadata<GameConfig>("game_config");
            DatabaseSystem.I.LoadMetadata<GameLevel>("game_level");
            DatabaseSystem.I.LoadMetadata<ImagePath>("image_path");
            DatabaseSystem.I.LoadMetadata<ItemMaster>("item_master");
            DatabaseSystem.I.LoadMetadata<MonsterMaster>("monster_master");
            DatabaseSystem.I.LoadMetadata<PassMaster>("pass_master");
            DatabaseSystem.I.LoadMetadata<PilotMaster>("pilot_master");
            DatabaseSystem.I.LoadMetadata<PilotRankLevel>("pilot_rank_level");
            DatabaseSystem.I.LoadMetadata<SpaceshipMaster>("spaceship_master");
            DatabaseSystem.I.LoadMetadata<SpaceshipRankLevel>("spaceship_rank_level");
            DatabaseSystem.I.LoadMetadata<StartupReward>("startup_reward");
            DatabaseSystem.I.LoadMetadata<SpecialWeaponMaster>("weapon_master");
            DatabaseSystem.I.LoadMetadata<Localization>("localization");
            DatabaseSystem.I.LoadMetadata<ErrorMassage>("error_massage");
            DatabaseSystem.I.LoadMetadata<PilotMasterDisplay>("pilot_master_display");
            DatabaseSystem.I.LoadMetadata<SpaceshipMasterDisplay>("spaceship_master_display");
            DatabaseSystem.I.LoadMetadata<ChestMasterDisplay>("chest_master_display");
            DatabaseSystem.I.LoadMetadata<PassMasterDisplay>("pass_master_display");
            DatabaseSystem.I.LoadMetadata<SpecialWeaponMasterDisplay>("weapon_master_display");
            DatabaseSystem.I.LoadMetadata<TooltipMaster>("tooltips");
            yield return new WaitUntil(() => DatabaseSystem.I.IsMetadataReady);
        }
        /// <summary>
        /// Set gameplay config
        /// </summary>
        private void SetGameConfig()
        {
            GameConfig config = DatabaseSystem.I.GetMetadata<GameConfig>();
            for (int i = 0; i < config.game_config.Count; i++)
            {
                if (config.game_config[i].key.Equals("normal_weapon_interval"))
                {
                    GameplayDataManager.I.NormalWeaponInterval = config.game_config[i].value;
                }
                else if (config.game_config[i].key.Equals("boost_normal_weapon_interval"))
                {
                    GameplayDataManager.I.RapidNormalWeaponInterval = config.game_config[i].value;
                }
                else if (config.game_config[i].key.Equals("is_debug_enabled"))
                {
                    if (Mathf.Approximately(config.game_config[i].value, 0f))
                    {
                        SystemConfig.IsDebugEnabled = false;
                    }
                    else
                    {
                        SystemConfig.IsDebugEnabled = true;
                    }
                }
                else if (config.game_config[i].key.Equals("gameplay_chest_drop_rate"))
                {
                    GameplayDataManager.I.ChestDropRate = config.game_config[i].value;
                }
                else if (config.game_config[i].key.Equals("gameplay_item_drop_rate"))
                {
                    GameplayDataManager.I.ItemDropRate = config.game_config[i].value;
                }
                else if (config.game_config[i].key.Equals("timing_get_minting_status"))
                {
                    SystemConfig.TimingGetMintingStatus = config.game_config[i].value;
                }
            }
        }
        /// <summary>
        /// Set item value from metadata
        /// </summary>
        private void SetItemData()
        {
            ItemMaster item = DatabaseSystem.I.GetMetadata<ItemMaster>();
            for (int i = 0; i < item.item_master.Count; i++)
            {
                //Not a best practice but we are running out of time
                if (item.item_master[i].id.Equals(1))
                {
                    GameplayDataManager.I.ItemGoldValue = (ushort)item.item_master[i].value;
                }
                else if (item.item_master[i].id.Equals(3))
                {
                    GameplayDataManager.I.ItemHealthValue = item.item_master[i].value;
                }
                else if (item.item_master[i].id.Equals(4))
                {
                    GameplayDataManager.I.ItemMagnetValue = item.item_master[i].value;
                }
                else if (item.item_master[i].id.Equals(5))
                {
                    GameplayDataManager.I.ItemRapidFireValue = item.item_master[i].value;
                }
            }
        }
        /// <summary>
        /// Called when player press on screen
        /// </summary>
        private void OnPressStart()
        {
            m_LayerStart.Delete();
            ControllerSystem.I.SceneChange("Scene/Authentication");
        }
    }
}
