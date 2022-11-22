using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NFT1Forge.OSY.Controller.Bullet;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Layer.Gameplay;
using NFT1Forge.OSY.Manager;
using NFT1Forge.OSY.System;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NFT1Forge.OSY.Controller.Gameplay
{
    public class GameplayController : BaseController
    {
        [SerializeField] private Camera m_Camera;
        [SerializeField] private Vector3 m_CameraZoomPosition;
        [SerializeField] private Vector3 m_CameraZoomRotation;
        [SerializeField] private Vector3 m_CameraNormalPosition;
        [SerializeField] private Vector3 m_CameraNormalRotation;
        [SerializeField] private Transform m_ObjectHolder;
        [SerializeField] private float m_TimeGetReady = 3f;
        [SerializeField] private float m_TimeBossAppear = 30f;
        [SerializeField] private float m_TimeIncomingBoss = 3f;
        [SerializeField] private List<StageLevelData> m_LevelList;
        [SerializeField] private UICanvasControllerInput m_CanvasControllerInput;
        [SerializeField] private GameObject m_StageCollider;
        [Header("SFX")]
        [SerializeField] private AudioClip m_SfxNextLevel;

        public static GameplayController I;

        public event Action<GameState> OnGameStateChanged;
        public event Action<float> OnEnergyChanged;
        public event Action<bool, float> OnRapidFireEvent;
        public event Action<bool, float> OnMagnetEvent;
        public event Action<float> SetEnergy;
        public event Action<float> OnSetBossHp;
        public event Action<uint> OnScoreChanged;
        public event Action<uint> OnGoldChanged;
        public uint CurrentBulletId = 0;

        public GameState CurrentGameState { get; private set; } = GameState.Loading;

        private GameState m_LastGameState;
        private PlayerInputAction m_PlayerInputAction;
        private GameplaySpaceship m_PlayerSpaceship;
        private EnemySpawner m_EnemySpawner;
        private HazardSpawner m_HazardSpawner;

        private LayerGameState m_LayerGameState;
        private LayerGameTime m_LayerGameTime;
        private LayerBoss m_LayerBoss;
        private LayerGameScore m_LayerGameScore;
        private LayerGameGold m_LayerGameGold;
        private LayerGameResult m_LayerGameResult;

        private float m_CurrentLevelTime;
        private float m_MaxEnergy;
        private float m_CurrentEnergy;
        private float m_SpecialWeaponChargeTime = 120f;
        private float m_BossCurrentHP;
        private uint m_Score = 0;
        private uint m_Gold = 0;
        private ushort m_LevelIndex;
        private ushort m_LevelProgression = 0;
        private readonly Dictionary<uint, BaseEnemy> m_EnemyDict = new Dictionary<uint, BaseEnemy>();
        private readonly Dictionary<uint, BaseEnemy> m_HazardDict = new Dictionary<uint, BaseEnemy>();
        private readonly Dictionary<uint, BaseItem> m_ItemDict = new Dictionary<uint, BaseItem>();
        private readonly Dictionary<uint, BaseBullet> m_BulletDict = new Dictionary<uint, BaseBullet>();
        private BossEnemy m_Boss;
        private Vector3 m_CurrentCameraPosition;
        private Vector3 m_CurrentCameraRotation;
        private bool m_IsMissionChestCollected = false;
        private bool m_IsEvolvedChest = false;
        private bool m_IsEvolvedSpecialWeapon = false;
        private ushort m_BossKilledCount;
        private ushort m_EnemyKilldedCount;
        private bool m_IsFirstLevel = true;
        private AudioSource m_Sfx;
        private string m_LastBossKilled;

        SpaceshipRankLevelData m_SpaceshipLevel;

        #region getter
        public Transform PlayerTransform => m_PlayerSpaceship.transform;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            I = this;
            m_Sfx = GetComponent<AudioSource>();
            SetScaleCollider();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Initialize()
        {
            yield return base.Initialize();
            SoundManager.I.MixGameplay();
            //Setup variable
            AssignPlayerInput();
            m_EnemySpawner = GetComponent<EnemySpawner>();
            m_HazardSpawner = GetComponent<HazardSpawner>();
            LoadSpaceshipData();
            LoadSpecialWeaponData();

            ObjectPoolManager.I.SetGlobalObjectHolder(m_ObjectHolder);

            yield return CreateUI();
            LayerSystem.I.GetLoadingScreen().UpdateProgress(33.33f);
            yield return PreLoadGameObject();
            LayerSystem.I.GetLoadingScreen().UpdateProgress(77.77f);
            yield return CreatePlayerObject();
            LayerSystem.I.GetLoadingScreen().UpdateProgress(88.88f);
            m_LevelIndex = (ushort)UnityEngine.Random.Range(0, m_LevelList.Count);
            PrepareLevel();
            LayerSystem.I.GetLoadingScreen().UpdateProgress(100f);
            yield return new WaitUntil(() => LayerSystem.I.GetLoadingScreen().IsAnimationFinish);
            LayerSystem.I.HideLoadingScreen();
            m_CanvasControllerInput.gameObject.SetActive(true);
            m_CanvasControllerInput.SetUp(m_SpecialWeaponChargeTime);
            GetReady();
        }
        /// <summary>
        /// Assign controller using new unity input system
        /// </summary>
        /// <returns></returns>
        private void AssignPlayerInput()
        {
            m_PlayerInputAction = new PlayerInputAction();
            m_PlayerInputAction.Player.Enable();
            m_PlayerInputAction.Player.Pause.started += OnPlayPause;
        }
        /// <summary>
        /// Load spaceship data
        /// </summary>
        private void LoadSpaceshipData()
        {
            SpaceshipDataModel spaceship = InventorySystem.I.GetActiveSpaceship();
            m_SpaceshipLevel = DatabaseSystem.I.GetMetadata<SpaceshipRankLevel>().spaceship_rank_level.Find(
                a => a.rank.Equals(spaceship.rank) && a.level.Equals(spaceship.level)
                );
            m_MaxEnergy = m_SpaceshipLevel.armor;
            m_CurrentEnergy = m_MaxEnergy;
        }
        /// <summary>
        /// Load special weapon data
        /// </summary>
        private void LoadSpecialWeaponData()
        {
            SpecialWeaponDataModel specialWeapon = InventorySystem.I.GetActiveWeapon();
            if (null == specialWeapon) return;
            SpecialWeaponMasterData weaponMaster = DatabaseSystem.I.GetMetadata<SpecialWeaponMaster>().weapon_master.Find(
                a => a.name.Equals(specialWeapon.name)
                );
            if (null != weaponMaster)
                m_SpecialWeaponChargeTime = weaponMaster.charge_time;
        }
        /// <summary>
        /// Create UI and HUD
        /// </summary>
        /// <returns></returns>
        private IEnumerator CreateUI()
        {
            LayerSystem.I.CreateLayer<LayerGameState>("Gameplay/LayerGameState", LayerType.NormalLayer, (layer) =>
            {
                m_LayerGameState = layer;
            });
            LayerSystem.I.CreateLayer<LayerGameTime>("Gameplay/LayerGameTime", LayerType.NormalLayer, (layer) =>
            {
                layer.LoadPilotAndSpaceship();
                SetEnergy += layer.SetMaxTime;
                SetEnergy?.Invoke(m_CurrentEnergy);
                OnEnergyChanged += layer.OnTimeChanged;
                OnEnergyChanged?.Invoke(m_CurrentEnergy);
                OnRapidFireEvent += layer.OnRapidFireBuff;
                OnMagnetEvent += layer.OnMagnetBuff;
                m_LayerGameTime = layer;
            });
            LayerSystem.I.CreateLayer<LayerBoss>("Gameplay/LayerBoss", LayerType.NormalLayer, (layer) =>
            {
                layer.HideLayer();
                m_LayerBoss = layer;
            });
            LayerSystem.I.CreateLayer<LayerGameScore>("Gameplay/LayerGameScore", LayerType.NormalLayer, (layer) =>
            {
                OnScoreChanged += layer.OnScoreChanged;
                m_LayerGameScore = layer;
            });
            LayerSystem.I.CreateLayer<LayerGameGold>("Gameplay/LayerGameGold", LayerType.NormalLayer, (layer) =>
            {
                OnGoldChanged += layer.OnGoldChanged;
                m_LayerGameGold = layer;
            });
            LayerSystem.I.CreateLayer<LayerGameResult>("Gameplay/LayerGameResult", LayerType.NormalLayer, (layer) =>
            {
                layer.HideLayer();
                layer.OnDone = OnDoneResult;
                m_LayerGameResult = layer;
            });
            yield return null;
        }
        /// <summary>
        /// Pre-Loading game object in pool
        /// </summary>
        /// <returns></returns>
        private IEnumerator PreLoadGameObject()
        {
            ObjectPoolManager.I.SetGlobalObjectHolder(m_ObjectHolder);
            yield return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator CreatePlayerObject()
        {
            PilotDataModel pilot = InventorySystem.I.GetActivePilot();
            m_PlayerSpaceship = ObjectPoolManager.I.GetObject<GameplaySpaceship>(ObjectType.PlayerSpaceShip, "GameplaySpaceship");
            m_PlayerSpaceship.Setup(m_SpaceshipLevel.damage, m_SpecialWeaponChargeTime, pilot.weaponchargespeed, (mobileInput) =>
            {
                m_CanvasControllerInput.starterAssetsInputs = mobileInput;
                m_PlayerSpaceship.OnSpecialWeaponTimeChanged += m_CanvasControllerInput.OnSpecialWeaponTimeChanged;
                m_PlayerSpaceship.OnSpecialFired += m_CanvasControllerInput.OnSpecialFired;
            });
            yield return null;
        }
        /// <summary>
        /// Loading level data and prepare gameplay system
        /// </summary>
        private void PrepareLevel()
        {
            m_LevelProgression++;
            GameLevelData progressData = DatabaseSystem.I.GetMetadata<GameLevel>().game_level.Find(
                a => a.level.Equals(m_LevelProgression)
                );
            m_CurrentLevelTime = 0f;

            //clear object
            List<BaseEnemy> enemyList = m_EnemyDict.Values.ToList();
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].ForceDead(false);
            }
            m_EnemyDict.Clear();

            RenderSettings.skybox = m_LevelList[m_LevelIndex].MaterialSkybox;
            m_EnemySpawner.SetMultiplier(progressData.enemy_attack, progressData.enemy_armor, progressData.score_multiplier);
            m_EnemySpawner.SetEnemySpawnData(m_LevelList[m_LevelIndex].EnemySpawnData);
            m_EnemySpawner.SetBossSpawnData(m_LevelList[m_LevelIndex].BossSpawnData);
            m_EnemySpawner.SetItemSpawnData(m_LevelList[m_LevelIndex].ItemSpawnData);
            m_EnemySpawner.SetChestSpawnData(m_LevelList[m_LevelIndex].ChestSpawnData);
            m_EnemySpawner.SetGoldSpawnData(m_LevelList[m_LevelIndex].GoldSpawnData);
            m_HazardSpawner.SetHazardSpawnData(m_LevelList[m_LevelIndex].HazardSpawnData);
            m_HazardSpawner.SetMultiplier(progressData.enemy_attack, progressData.enemy_armor, progressData.score_multiplier);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void ChangeGameState(GameState state)
        {
            CurrentGameState = state;
            OnGameStateChanged?.Invoke(CurrentGameState);
        }
        /// <summary>
        /// Everything is initialized, show get ready message 
        /// </summary>
        /// <returns></returns>
        private void GetReady()
        {
            ChangeGameState(GameState.GetReady);
            m_PlayerSpaceship.Reset();
            m_LayerGameState.ShowGetReady(m_LevelList[m_LevelIndex].LevelName);
            m_Camera.transform.SetPositionAndRotation(m_CameraZoomPosition, Quaternion.Euler(m_CameraZoomRotation));
        }
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            switch (CurrentGameState)
            {
                case GameState.GetReady:
                    GetReadyTranslate();
                    break;
                case GameState.Playing:
                    UpdateEnergy();
                    UpdateStageTime();
                    break;

                case GameState.BossWarning:
                    UpdateIncomingBoss();
                    break;

                case GameState.Boss:
                    UpdateEnergy();
                    break;

                case GameState.BossKilled:
                    ChangeGameState(GameState.LevelProgress);
                    StartCoroutine(NextLevel());
                    break;

                case GameState.LevelProgress:
                    //do noting. Just wait for NextLevel coroutine
                    break;

                case GameState.GameOver:
                case GameState.Pause:
                case GameState.WaitForBoss:
                case GameState.NextLevel:
                default:
                    //update nothing
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void GetReadyTranslate()
        {
            m_CurrentLevelTime += Time.deltaTime / m_TimeGetReady;
            m_CurrentCameraPosition = Vector3.Lerp(m_CameraZoomPosition, m_CameraNormalPosition, m_CurrentLevelTime);
            m_CurrentCameraRotation = Vector3.Lerp(m_CameraZoomRotation, m_CameraNormalRotation, m_CurrentLevelTime);
            m_Camera.transform.SetPositionAndRotation(m_CurrentCameraPosition, Quaternion.Euler(m_CurrentCameraRotation));
            if (m_TimeGetReady < m_CurrentLevelTime)
            {
                if (!m_IsFirstLevel)
                {
                    ChangeGameState(GameState.Playing);
                    m_LayerGameState.HideGameState();
                    return;
                }
                bool isShowTutorial = true
                    ;
                if (0 < InventorySystem.I.SoftCurrency1)
                    isShowTutorial = false;
                for (int i = 0; i < InventorySystem.I.PilotCount; i++)
                {
                    if (1 < InventorySystem.I.GetPilotByIndex(i).level)
                        isShowTutorial = false;
                }
                for (int i = 0; i < InventorySystem.I.SpaceshipCount; i++)
                {
                    if (1 < InventorySystem.I.GetSpaceshipByIndex(i).level)
                        isShowTutorial = false;
                }

                m_LayerGameState.HideGameState();
                if (isShowTutorial)
                {
                    ChangeGameState(GameState.Pause);
                    LayerSystem.I.CreateLayer<LayerHowToControl>("Gameplay/LayerHowToControl", LayerType.TopLayer, (layer) =>
                        {
                            layer.OnClosed = () => { ChangeGameState(GameState.Playing); };
                        });
                    m_IsFirstLevel = false;
                }
                else
                {
                    ChangeGameState(GameState.Playing);
                }
            }
        }
        /// <summary>
        /// Reduct player's energy
        /// </summary>
        private void UpdateEnergy()
        {
            OnMagnetEvent?.Invoke(m_PlayerSpaceship.IsMagnet, m_PlayerSpaceship.MagnetDuration);
            OnRapidFireEvent?.Invoke(m_PlayerSpaceship.IsRapidFire, m_PlayerSpaceship.RapidFireDuration);

            if (GameMode.Survival == GameplayDataManager.I.CurrentGameMode)
                return;

            m_CurrentEnergy -= Time.deltaTime;
            OnEnergyChanged(m_CurrentEnergy);
            CheckGameOver();
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateStageTime()
        {
            m_CurrentLevelTime += Time.deltaTime;
            if (m_CurrentLevelTime > m_TimeBossAppear)
            {
                if (0 == m_EnemyDict.Count)
                {
                    ChangeGameState(GameState.BossWarning);
                    m_LayerGameState.ShowBossWarning();
                    m_CurrentLevelTime = 0;
                }
                else
                {
                    m_CurrentLevelTime = 0;
                    ChangeGameState(GameState.WaitForBoss);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateIncomingBoss()
        {
            m_CurrentLevelTime += Time.deltaTime;
            if (m_CurrentLevelTime > m_TimeIncomingBoss)
            {
                m_CurrentLevelTime = 0;
                ChangeGameState(GameState.Boss);
                m_EnemySpawner.SpawnBoss();
                m_LayerGameState.HideGameState();
            }
        }
        /// <summary>
        /// Wait a while then go to next level
        /// </summary>
        public void BossKilled()
        {
            m_LastBossKilled = m_Boss.name;
            m_BossKilledCount++;
            List<BaseBullet> bulletList = m_BulletDict.Values.ToList();
            for (int i = 0; i < m_BulletDict.Count; i++)
            {
                bulletList[i].ForceDead();
            }
            m_BulletDict.Clear();
            List<BaseEnemy> hazardList = m_HazardDict.Values.ToList();
            for (int i = 0; i < m_HazardDict.Count; i++)
            {
                hazardList[i].ForceDead(true);
            }
            m_HazardDict.Clear();
            StartCoroutine(CheckBossMission());
        }
        /// <summary>
        /// 
        /// </summary>
        private IEnumerator CheckBossMission()
        {
            if (3 == PlayerMissionManager.I.PlayerMissionStep)
            {
                yield return null;
                CoroutineWithData api = new CoroutineWithData(this, PlayerMissionManager.I.MissionCompleted(3));
                yield return api.coroutine;
                if ((bool)api.result)
                {
                    Pause(() => ChangeGameState(GameState.BossKilled));
                }
                else
                {
                    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
                }
            }
            else
            {
                ChangeGameState(GameState.BossKilled);
            }
        }
        /// <summary>
        /// Progression to next level
        /// </summary>
        /// <returns></returns>
        private IEnumerator NextLevel()
        {
            ClearItem();
            yield return new WaitForSeconds(2f);
            ChangeGameState(GameState.NextLevel);
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxNextLevel);
            yield return new WaitForSeconds(2f);
            m_LevelIndex++;
            if (m_LevelIndex >= m_LevelList.Count)
                m_LevelIndex = 0;
            PrepareLevel();
            GetReady();
        }
        /// <summary>
        /// Toggle play and pause : InputAction callback
        /// </summary>
        private void OnPlayPause(InputAction.CallbackContext context)
        {
            if (GameState.Pause != CurrentGameState && GameState.BossKilled != CurrentGameState
                && GameState.NextLevel != CurrentGameState && GameState.LevelProgress != CurrentGameState
                && GameState.GetReady != CurrentGameState && GameState.Loading != CurrentGameState)
            {
                Pause();
            }
            else
            {
                LayerSystem.I.HidePopupDialog();
            }
        }
        /// <summary>
        /// Play game
        /// </summary>
        public void Play()
        {
            ChangeGameState(m_LastGameState);
        }
        /// <summary>
        /// Pause game
        /// </summary>
        public void Pause(Action continueAction = null)
        {
            if (GameState.Pause == CurrentGameState) return;
            if (null == continueAction)
            {
                continueAction = Play;
            }
            m_LastGameState = CurrentGameState;
            ChangeGameState(GameState.Pause);
            LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_GAME_PAUSE"), continueAction);
        }
        /// <summary>
        /// Add score for player
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(uint score)
        {
            m_Score += score;
            OnScoreChanged?.Invoke(m_Score);
        }
        /// <summary>
        /// Add gold to player
        /// </summary>
        /// <param name="amount"></param>
        private void AddGold(uint amount)
        {
            m_Gold += amount;
            OnGoldChanged?.Invoke(m_Gold);
        }
        /// <summary>
        /// Add health to player
        /// </summary>
        /// <param name="amount"></param>
        private void AddHealth(float amount)
        {
            m_CurrentEnergy += amount;
            OnEnergyChanged?.Invoke(m_CurrentEnergy);
            if (m_CurrentEnergy > m_MaxEnergy)
                m_CurrentEnergy = m_MaxEnergy;
        }
        /// <summary>
        /// Add rapidfire to player
        /// </summary>
        /// <param name="amount"></param>
        private void AddRapidFire()
        {
            m_PlayerSpaceship.BoostFireRate();
        }
        /// <summary>
        /// Add magnetic to player
        /// </summary>
        /// <param name="amount"></param>
        private void AddMagnet()
        {
            List<BaseItem> itemList = m_ItemDict.Values.ToList();
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].GetComponent<FollowTargetMovement>().OnMagnetic();
            }
            m_PlayerSpaceship.Magnetic();
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearItem()
        {
            List<BaseItem> itemList = m_ItemDict.Values.ToList();
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].GetComponent<FollowTargetMovement>().OnClearItem();
            }
        }
        /// <summary>
        /// Keep enemy in dictionary: use for bomb item and counting enemy left in the scene
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enemy"></param>
        public void AddEnemy(uint id, BaseEnemy enemy)
        {
            if (m_EnemyDict.ContainsKey(id))
            {
                ObjectPoolManager.I.ReturnToPool(m_EnemyDict[id], ObjectType.Enemy);
            }
            m_EnemyDict[id] = enemy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hazard"></param>
        public void AddHazard(uint id, BaseEnemy hazard)
        {
            if (m_HazardDict.ContainsKey(id))
            {
                ObjectPoolManager.I.ReturnToPool(m_HazardDict[id], ObjectType.Hazard);
            }
            m_HazardDict[id] = hazard;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        public void AddItem(uint id, BaseItem item)
        {
            if (m_ItemDict.ContainsKey(id))
            {
                ObjectPoolManager.I.ReturnToPool(m_ItemDict[id], ObjectType.Item);
            }
            m_ItemDict[id] = item;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        public void AddBullet(uint id, BaseBullet bullet)
        {
            if (m_BulletDict.ContainsKey(id))
            {
                ObjectPoolManager.I.ReturnToPool(m_BulletDict[id], ObjectType.Bullet);
            }
            m_BulletDict[id] = bullet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="score"></param>
        public void EnemyKilled(uint id, bool isCount)
        {
            if (m_EnemyDict.ContainsKey(id))
            {
                if (isCount)
                {
                    AddScore(m_EnemyDict[id].BaseScore);
                    m_EnemyKilldedCount++;
                    SpecialWeaponDataModel weapon = InventorySystem.I.GetActiveWeapon();
                }
                m_EnemyDict.Remove(id);
            }
            if (m_HazardDict.ContainsKey(id))
            {
                if (isCount)
                    AddScore(m_HazardDict[id].BaseScore);
                m_HazardDict.Remove(id);
            }
            if (GameState.WaitForBoss == CurrentGameState && 0 == m_EnemyDict.Count)
            {
                ChangeGameState(GameState.BossWarning);
                m_LayerGameState.ShowBossWarning();
            }
        }
        /// <summary>
        /// Called when play catch an item
        /// </summary>
        /// <param name="ItemType"></param>
        public void CatchItem(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Gold:
                    AddGold(GameplayDataManager.I.ItemGoldValue);
                    break;
                case ItemType.Bomb:
                    List<BaseEnemy> enemyList = m_EnemyDict.Values.ToList();
                    for (int i = 0; i < enemyList.Count; i++)
                    {
                        enemyList[i].ForceDead(true);
                    }
                    m_EnemyDict.Clear();
                    List<BaseEnemy> hazardList = m_HazardDict.Values.ToList();
                    for (int i = 0; i < hazardList.Count; i++)
                    {
                        hazardList[i].ForceDead(true);
                    }
                    m_HazardDict.Clear();
                    break;
                case ItemType.Health:
                    AddHealth(GameplayDataManager.I.ItemHealthValue);
                    break;
                case ItemType.RapidFire:
                    AddRapidFire();
                    break;
                case ItemType.Magenet:
                    AddMagnet();
                    break;
                case ItemType.MissionChest:
                    StartCoroutine(CheckChestMission());
                    break;
            }
        }
        private IEnumerator CheckChestMission()
        {
            m_IsMissionChestCollected = true;
            if (2 == PlayerMissionManager.I.PlayerMissionStep)
            {
                CoroutineWithData api = new CoroutineWithData(this, PlayerMissionManager.I.MissionCompleted(2));
                yield return api.coroutine;
                if ((bool)api.result)
                {
                    Pause();
                }
                else
                {
                    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        public void DeductEnergy(float point)
        {
            m_CurrentEnergy -= point;
            OnEnergyChanged(m_CurrentEnergy);
            CheckGameOver();
        }
        /// <summary>
        /// 
        /// </summary>
        private bool CheckGameOver()
        {
            if (m_CurrentEnergy < 0f)
            {
                GameOver();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GameOver()
        {
            // ignore action if repeat calling method
            if (GameState.GameOver == CurrentGameState)
                return;

            ScreenActionData data = new ScreenActionData();

            //Reset gameplay data and object
            m_PlayerInputAction.Player.Disable();
            m_PlayerSpaceship.Reset();
            ObjectPoolManager.I.ReturnToPool(m_PlayerSpaceship, ObjectType.PlayerSpaceShip);
            m_CurrentEnergy = 0f;
            OnEnergyChanged(m_CurrentEnergy);
            ChangeGameState(GameState.GameOver);
            m_LayerGameState.ShowGameOver();
            if (m_IsMissionChestCollected)
            {
                StartCoroutine(InventorySystem.I.AddNewChest());
            }
            if (null != InventorySystem.I.GetActiveChest())
                data.update_chest = 1;
            if (null != InventorySystem.I.GetActiveWeapon())
                data.update_weapon = 1;
            StartCoroutine(ShowGameResult());
            StartCoroutine(UpdateData());

#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                if (GameMode.Normal == GameplayDataManager.I.CurrentGameMode)
                {
                    WebBridgeUtils.PageAction($"{(int)PageActionType.EnterNormalResultScreen}", JsonUtility.ToJson(data));
                }
                else
                {
                    data.update_pass = 1;
                    WebBridgeUtils.PageAction($"{(int)PageActionType.EnterSurvivalStageResultScreen}", JsonUtility.ToJson(data));
                }
            }
#endif
            string stageType = string.Empty;
            if (GameMode.Normal == GameplayDataManager.I.CurrentGameMode)
                stageType = "Normal Stage";
            else
                stageType = "Survival Stage";
            EventLogManager.I.FinishGame(stageType, m_Score, m_Gold, m_IsMissionChestCollected, m_EnemyKilldedCount, m_BossKilledCount);
            SoundManager.I.MixGameOver();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateData()
        {
            yield return null;
            //List<PilotDataModel> pilotList = new List<PilotDataModel>();
            //List<SpaceshipDataModel> spaceshipList = new List<SpaceshipDataModel>();
            //List<SpecialWeaponDataModel> specialWeaponList = new List<SpecialWeaponDataModel>();
            //List<PassDataModel> passList = new List<PassDataModel>();
            //List<ChestDataModel> chestList = new List<ChestDataModel>();

            PilotDataModel pilot = InventorySystem.I.GetActivePilot();
            if (null != pilot)
            {
                if (pilot.highscore < m_Score)
                {
                    pilot.highscore = m_Score;
                }
                //pilotList.Add(pilot);
            }

            //SpaceshipDataModel spaceship = InventorySystem.I.GetActiveSpaceship();
            //if (null != spaceship)
            //{
            //    if (!string.IsNullOrEmpty(m_LastBossKilled))
            //    {
            //        spaceship.bosspart = m_LastBossKilled;
            //        spaceship.image = spaceship.GetImagePath();
            //        spaceshipList.Add(spaceship);
            //    }
            //}

            ChestDataModel chest = InventorySystem.I.GetActiveChest();
            if (null != chest)
            {
                if (chest.GetMaster().mission_type == "Play Count")
                {
                    chest.missionprogress += 1;
                }
                else if (chest.GetMaster().mission_type == "Kill Enemies")
                {
                    chest.missionprogress += m_EnemyKilldedCount;
                }

                m_IsEvolvedChest = chest.Evolve();
                if (m_IsEvolvedChest && 6 == PlayerMissionManager.I.PlayerMissionStep)
                {
                    StartCoroutine(UpdatePlayerGameplayMission(6));
                }
                //chestList.Add(chest);
            }

            //SpecialWeaponDataModel specialWeapon = InventorySystem.I.GetActiveWeapon();
            //if (null != specialWeapon)
            //{
            //    if (specialWeapon.GetMaster().evo_type == "Kill Enemies")
            //    {
            //        specialWeapon.missionprogress += m_EnemyKilldedCount;
            //    }

            //    m_IsEvolvedSpecialWeapon = specialWeapon.Evolve();
            //    if (m_IsEvolvedSpecialWeapon && 14 == PlayerMissionManager.I.PlayerMissionStep)
            //    {
            //        StartCoroutine(UpdatePlayerGameplayMission(14));
            //    }
            //    specialWeaponList.Add(specialWeapon);
            //}

            //if (GameMode.Survival == GameplayDataManager.I.CurrentGameMode)
            //{
            //    PassDataModel pass = InventorySystem.I.GetActivePass();
            //    if (null != pass)
            //    {
            //        if (pass.bestrecord < m_Score)
            //        {
            //            pass.bestrecord = (int)m_Score;
            //        }
            //        passList.Add(pass);
            //    }
            //}

            //CoroutineWithData api = new CoroutineWithData(this, InventorySystem.I.UpdateItem(pilotList,
            //    spaceshipList, specialWeaponList, passList, chestList));
            //yield return api.coroutine;
            //if ((bool)api.result)
            //{
            //    LayerSystem.I.HideSoftLoadingScreen();
            //}
            //else
            //{
            //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
            //}
        }
        /// <summary>
        /// Update player mission step
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdatePlayerGameplayMission(ushort mission)
        {
            CoroutineWithData api = new CoroutineWithData(this, PlayerMissionManager.I.MissionCompleted(mission));
            yield return api.coroutine;
            if (!(bool)api.result)
            {
                LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowGameResult()
        {
            PilotDataModel pilot = InventorySystem.I.GetActivePilot();
            uint finalGold = (uint)Mathf.RoundToInt(m_Gold + (m_Gold * pilot.goldbooster) / 100);
            InventorySystem.I.AddSoftCurrency1(finalGold);
            yield return InventorySystem.I.UpdateSoftCurrency1(InventorySystem.I.SoftCurrency1);
            m_LayerGameResult.SetData(m_Score, m_Gold, finalGold, m_EnemyKilldedCount, m_BossKilledCount, m_IsMissionChestCollected, m_IsEvolvedChest, m_IsEvolvedSpecialWeapon);
            yield return new WaitForSeconds(2f);
            m_CanvasControllerInput.gameObject.SetActive(false);
            m_LayerGameResult.ShowLayer();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnDoneResult()
        {
            StartCoroutine(ChangeToMainMenuAsync());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangeToMainMenuAsync()
        {
            LayerSystem.I.ShowLoadingScreen();
            ClearGameplayObject();
            yield return Shutdown();
            ControllerSystem.I.SceneChange("Scene/MainMenu");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Shutdown()
        {
            OnGameStateChanged = null;
            OnEnergyChanged = null;
            OnScoreChanged = null;
            OnGoldChanged = null;

            m_PlayerSpaceship.OnSpecialWeaponTimeChanged -= m_CanvasControllerInput.OnSpecialWeaponTimeChanged;
            m_PlayerSpaceship.OnSpecialFired -= m_CanvasControllerInput.OnSpecialFired;

            ObjectPoolManager.I.ReturnToPool(m_PlayerSpaceship, ObjectType.PlayerSpaceShip);

            m_LayerGameState.Delete();
            m_LayerGameTime.Delete();
            m_LayerBoss.Delete();
            m_LayerGameScore.Delete();
            m_LayerGameGold.Delete();
            m_LayerGameResult.Delete();
            yield return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="boss"></param>
        public void SetBoss(BossEnemy boss)
        {
            m_Boss = boss;
            m_BossCurrentHP = m_Boss.Health;
            OnSetBossHp += m_LayerBoss.SetHp;
            OnSetBossHp?.Invoke(m_BossCurrentHP);
            m_Boss.OnKilled += UnsubscribeBossEvent;
            m_Boss.OnHpChanged += m_LayerBoss.OnHpChanged;
            m_LayerBoss.ShowLayer();
        }
        /// <summary>
        /// Unsubscribe all boss event
        /// </summary>
        private void UnsubscribeBossEvent()
        {
            m_Boss.OnSetHp -= m_LayerBoss.SetHp;
            m_Boss.OnKilled -= UnsubscribeBossEvent;
            m_LayerBoss.HideLayer();
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearGameplayObject()
        {
            List<BaseEnemy> enemyList = m_EnemyDict.Values.ToList();
            for (int i = 0; i < enemyList.Count; i++)
            {
                Destroy(enemyList[i].gameObject);
            }
            m_EnemyDict.Clear();
            List<BaseEnemy> hazardList = m_HazardDict.Values.ToList();
            for (int i = 0; i < m_HazardDict.Count; i++)
            {
                Destroy(hazardList[i].gameObject);
            }
            m_HazardDict.Clear();
            List<BaseItem> itemList = m_ItemDict.Values.ToList();
            for (int i = 0; i < itemList.Count; i++)
            {
                Destroy(itemList[i].gameObject);
            }
            if (null != m_Boss)
            {
                m_Boss.OnHpChanged -= m_LayerBoss.OnHpChanged;
                Destroy(m_Boss.gameObject);
            }
            CurrentBulletId = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SetScaleCollider()
        {
            float ratio = (float)Screen.width / (float)Screen.height;
            m_StageCollider.GetComponent<BoxCollider2D>().size *= ratio;
        }
        /// <summary>
        /// Return a list of all enemies and boss transform
        /// </summary>
        /// <returns></returns>
        public List<Transform> GetAllEnemiesTransform()
        {
            List<Transform> result = new List<Transform>();
            foreach (BaseEnemy enemy in m_EnemyDict.Values)
            {
                result.Add(enemy.transform);
            }
            if (null != m_Boss) result.Add(m_Boss.transform);
            return result;
        }
    }
}
