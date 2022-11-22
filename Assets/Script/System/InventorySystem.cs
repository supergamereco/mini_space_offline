using System;
using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.API;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
//using Script.System;

namespace NFT1Forge.OSY.System
{
    public class InventorySystem : Singleton<InventorySystem>
    {
        public event Action<uint> OnSoftCurrency1Changed;
        public event Action<SpaceshipDataModel> OnSpaceShipUpgraded;
        public event Action<PilotDataModel> OnPilotUpgraded;
        public event Action<string, NFTCollectionType> OnItemActivated;
        public event Action<NftMetadataModel> OnItemUpdated;
        public event Action OnInventoryRefresh;

        public uint SoftCurrency1 { get; private set; }
        public bool IsApiFinished { get; private set; }
        public GetActiveObjectModel.ActiveObjectDataModel PostActiveObject = new GetActiveObjectModel.ActiveObjectDataModel();
        private readonly List<SpaceshipDataModel> m_SpaceshipList = new List<SpaceshipDataModel>();
        private readonly List<PilotDataModel> m_PilotList = new List<PilotDataModel>();
        private readonly List<ChestDataModel> m_ChestList = new List<ChestDataModel>();
        private readonly List<PassDataModel> m_PassList = new List<PassDataModel>();
        private readonly List<SpecialWeaponDataModel> m_SpecialWeaponList = new List<SpecialWeaponDataModel>();
        public bool IsMinting = false;

        public int SpaceshipCount => m_SpaceshipList.Count;
        public int PilotCount => m_PilotList.Count;
        public int ChestCount => m_ChestList.Count;
        public int PassCount => m_PassList.Count;
        public int WeaponCount => m_SpecialWeaponList.Count;

        /// <summary>
        /// Initialize system
        /// </summary>
        public void InitSystem()
        {
            //NetworkSystem.I.OnApiError += OnApiError;
        }
        /// <summary>
        /// Add all items to inventory
        /// </summary>
        /// <param name="data"></param>
        public void AddInventoryList(ObjectListModel data)
        {
            NftMetadataModel mintingItem = null;
            if (null != data.pilot)
            {
                for (int i = 0; i < data.pilot.Count; i++)
                {
                    if (!data.pilot[i].is_lock || data.pilot[i].is_open) continue;
                    PilotDataModel pilot = new PilotDataModel
                    {
                        token_id = data.pilot[i].token_id,
                        name = data.pilot[i].name,
                        image = data.pilot[i].image,
                        is_lock = data.pilot[i].is_lock,
                        is_nft = data.pilot[i].is_nft,
                        is_minting = data.pilot[i].is_minting,
                        is_open = data.pilot[i].is_open,
                        is_active = data.pilot[i].is_active,
                        status = data.pilot[i].status,
                        color2 = data.pilot[i].color2,
                        color1 = data.pilot[i].color1,
                        level = data.pilot[i].level,
                        highscore = data.pilot[i].highscore,
                    };
                    pilot.SyncMasterData();
                    AddPilot(pilot);
                    if (pilot.is_minting)
                        mintingItem = pilot;
                }
            }
            if (null != data.spaceship)
            {
                for (int i = 0; i < data.spaceship.Count; i++)
                {
                    if (!data.spaceship[i].is_lock || data.spaceship[i].is_open) continue;
                    SpaceshipDataModel spaceship = new SpaceshipDataModel
                    {
                        token_id = data.spaceship[i].token_id,
                        name = data.spaceship[i].name,
                        image = data.spaceship[i].image,
                        is_lock = data.spaceship[i].is_lock,
                        is_nft = data.spaceship[i].is_nft,
                        is_minting = data.spaceship[i].is_minting,
                        is_open = data.spaceship[i].is_open,
                        is_active = data.spaceship[i].is_active,
                        status = data.spaceship[i].status,
                        level = data.spaceship[i].level,
                        bosspart = data.spaceship[i].bosspart,
                    };
                    spaceship.SyncMasterData();
                    AddSpaceship(spaceship);
                    if (spaceship.is_minting)
                        mintingItem = spaceship;
                }
            }
            if (null != data.chest)
            {
                for (int i = 0; i < data.chest.Count; i++)
                {
                    if (!data.chest[i].is_lock || data.chest[i].is_open) continue;
                    ChestDataModel chest = new ChestDataModel
                    {
                        token_id = data.chest[i].token_id,
                        name = data.chest[i].name,
                        image = data.chest[i].image,
                        is_lock = data.chest[i].is_lock,
                        is_nft = data.chest[i].is_nft,
                        is_minting = data.chest[i].is_minting,
                        is_open = data.chest[i].is_open,
                        is_active = data.chest[i].is_active,
                        status = data.chest[i].status,
                        missionprogress = data.chest[i].missionprogress,
                        missiontype = data.chest[i].missiontype,
                    };
                    chest.SyncMasterData();
                    AddChest(chest);
                    if (chest.is_minting)
                        mintingItem = chest;
                }
            }
            if (null != data.survival_pass)
            {
                for (int i = 0; i < data.survival_pass.Count; i++)
                {
                    if (!data.survival_pass[i].is_lock || data.survival_pass[i].is_open) continue;
                    PassDataModel survivalPass = new PassDataModel
                    {
                        token_id = data.survival_pass[i].token_id,
                        name = data.survival_pass[i].name,
                        image = data.survival_pass[i].image,
                        is_lock = data.survival_pass[i].is_lock,
                        is_nft = data.survival_pass[i].is_nft,
                        is_minting = data.survival_pass[i].is_minting,
                        is_open = data.survival_pass[i].is_open,
                        is_active = data.survival_pass[i].is_active,
                        status = data.survival_pass[i].status,
                        bestrecord = data.survival_pass[i].bestrecord,
                        playcount = data.survival_pass[i].playcount,
                    };
                    survivalPass.SyncMasterData();
                    AddPass(survivalPass);
                    if (survivalPass.is_minting)
                        mintingItem = survivalPass;
                }
            }
            if (null != data.special_weapon)
            {
                for (int i = 0; i < data.special_weapon.Count; i++)
                {
                    if (!data.special_weapon[i].is_lock || data.special_weapon[i].is_open) continue;
                    SpecialWeaponDataModel weapon = new SpecialWeaponDataModel
                    {
                        token_id = data.special_weapon[i].token_id,
                        name = data.special_weapon[i].name,
                        image = data.special_weapon[i].image,
                        is_lock = data.special_weapon[i].is_lock,
                        is_nft = data.special_weapon[i].is_nft,
                        is_minting = data.special_weapon[i].is_minting,
                        is_open = data.special_weapon[i].is_open,
                        is_active = data.special_weapon[i].is_active,
                        status = data.special_weapon[i].status,
                        missionprogress = data.special_weapon[i].missionprogress,
                        evotype = data.special_weapon[i].evotype,
                    };
                    weapon.SyncMasterData();
                    AddSpecialWeapon(weapon);
                    if (weapon.is_minting)
                        mintingItem = weapon;
                }
            }
            if (null != mintingItem)
            {
                MintingSystem.I.ShowWaitingDialog(mintingItem);
            }
        }
        /// <summary>
        /// Replace current inventory items with all items from backend
        /// </summary>
        /// <returns></returns>
        //public IEnumerator RefreshInventory()
        //{
        //    CoroutineWithData api = new CoroutineWithData(this, ApiService.I.GetItem());
        //    yield return api.coroutine;
        //    GetItemResponseModel response = (GetItemResponseModel)api.result;
        //    if (response.success)
        //    {
        //        ClearInventory();
        //        AddInventoryList(response.data.object_list);
        //        OnInventoryRefresh?.Invoke();
        //    }
        //}
        /// <summary>
        /// Clear all inventory
        /// </summary>
        public void ClearInventory()
        {
            m_PilotList.Clear();
            m_SpaceshipList.Clear();
            m_ChestList.Clear();
            m_PassList.Clear();
            m_SpecialWeaponList.Clear();
        }
        #region pilot
        /// <summary>
        /// Create pilot data
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="pilotName"></param>
        /// <param name="level"></param>
        /// <param name="hairColor"></param>
        /// <param name="costumeColor"></param>
        /// <param name="highscore"></param>
        /// <returns></returns>
        public PilotDataModel CreatePilotData(string tokenId, string pilotName, ushort level = 1, HairColor hairColor = HairColor.Black, CostumeColor costumeColor = CostumeColor.Orange, ushort highscore = 0)
        {
            PilotDataModel model = new PilotDataModel
            {
                token_id = tokenId,
                name = pilotName,
                level = level,
                color1 = hairColor.ToString(),
                color2 = costumeColor.ToString(),
                highscore = highscore
            };
            model.SyncMasterData();

            return model;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddPilot(PilotDataModel item)
        {
            m_PilotList.Add(item);
        }
        /// <summary>
        /// For check Highest score
        /// </summary>
        /// <returns></returns>
        public PilotDataModel GetHighestScorePilot()
        {
            uint highScore = 0;
            for (int i = 0; i < m_PilotList.Count; i++)
            {
                uint score = m_PilotList[i].highscore;
                if (score > highScore)
                {
                    highScore = score;
                }
            }
            return m_PilotList.Find(a => a.highscore.Equals(highScore));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public PilotDataModel GetPilot(string tokenId)
        {
            PilotDataModel model = m_PilotList.Find(a => a.token_id.Equals(tokenId));
            return model ?? (m_PilotList.Count > 0 ? m_PilotList[0] : null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PilotDataModel GetPilotByIndex(int index)
        {
            if (index < m_PilotList.Count)
                return m_PilotList[index];
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PilotDataModel GetActivePilot()
        {
            PilotDataModel model = m_PilotList.Find(a => a.is_active);
            if (null != model)
            {
                return model;
            }
            else
            {
                StartCoroutine(SetActivePilot(m_PilotList[0].token_id));
                return m_PilotList[0];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="hairColorId"></param>
        /// <param name="costumeColorId"></param>
        public IEnumerator SetPilotColor(string tokenId, ushort hairColorId, ushort costumeColorId)
        {
            PilotDataModel pilot = GetPilot(tokenId);
            if (null != pilot)
            {
                pilot.color1 = ((HairColor)hairColorId).ToString();
                pilot.color2 = ((CostumeColor)costumeColorId).ToString();
                pilot.SyncMasterData();

                //CoroutineWithData api = new CoroutineWithData(this, UpdateItem(new List<PilotDataModel> { pilot }, null, null, null, null));
                //yield return api.coroutine;
                //if ((bool)api.result)
                //{
                //    LayerSystem.I.HideSoftLoadingScreen();
                //}
                //else
                //{
                //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
                //}
                yield return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pilotTokenId"></param>
        public IEnumerator SetActivePilot(string tokenId)
        {
            string name = string.Empty;
            ushort level = 0;
            if (string.IsNullOrEmpty(tokenId))
                tokenId = m_PilotList[0].token_id;
            for (int i = 0; i < m_PilotList.Count; i++)
            {
                if (tokenId == m_PilotList[i].token_id)
                {
                    if (m_PilotList[i].is_active)
                        yield break;
                    m_PilotList[i].is_active = true;
                    name = m_PilotList[i].name;
                    level = m_PilotList[i].level;
                }
                else
                {
                    m_PilotList[i].is_active = false;
                }
            }
            yield return SetActiveItem(tokenId, NFTCollectionType.Pilot);
            EventLogManager.I.ItemActivate("active_pilot_in_game", name, level);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="currentUpgradeCost"></param>
        public IEnumerator LevelupPilot(string tokenId, ushort currentUpgradeCost)
        {
            PilotDataModel pilot = GetPilot(tokenId);
            pilot.level = (ushort)(pilot.level + 1);
            pilot.SyncMasterData();
            //CoroutineWithData api = new CoroutineWithData(this, UpdateItem(new List<PilotDataModel> { pilot }, null, null, null, null));
            //yield return api.coroutine;
            //if ((bool)api.result)
            //{
            //    yield return UpdateSoftCurrency1(SoftCurrency1 - currentUpgradeCost);
            //    if (4 == PlayerMissionManager.I.PlayerMissionStep)
            //    {
            //        yield return UpdatePlayerGameplayMission(4);
            //    }
            //    OnPilotUpgraded(pilot);
            //    EventLogManager.I.PilotUpgrade(pilot);
            //}
            //else
            //{
            //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
            //}
            yield return null;
            yield return UpdateSoftCurrency1(SoftCurrency1 - currentUpgradeCost);
            OnPilotUpgraded(pilot);
            LayerSystem.I.HideSoftLoadingScreen();
        }
        #endregion

        #region spaceship
        /// <summary>
        /// Create spaceship data
        /// </summary>
        /// <param name="spaceshipTokenId"></param>
        /// <param name="spaceshipName"></param>
        /// <param name="level"></param>
        /// <param name="bosspart"></param>
        /// <returns></returns>
        public SpaceshipDataModel CreateSpaceshipData(string spaceshipTokenId, string spaceshipName, ushort level = 1, string bosspart = "NONE")
        {
            SpaceshipDataModel model = new SpaceshipDataModel
            {
                token_id = spaceshipTokenId,
                name = spaceshipName,
                level = level,
                bosspart = bosspart
            };
            model.SyncMasterData();

            return model;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddSpaceship(SpaceshipDataModel item)
        {
            m_SpaceshipList.Add(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public SpaceshipDataModel GetSpaceship(string tokenId)
        {
            SpaceshipDataModel model = m_SpaceshipList.Find(a => a.token_id.Equals(tokenId));
            return model ?? (m_SpaceshipList.Count > 0 ? m_SpaceshipList[0] : null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SpaceshipDataModel GetSpaceshipByIndex(int index)
        {
            if (index < m_SpaceshipList.Count)
                return m_SpaceshipList[index];
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SpaceshipDataModel GetActiveSpaceship()
        {
            SpaceshipDataModel model = m_SpaceshipList.Find(a => a.is_active);
            if (null != model)
            {
                return model;
            }
            else
            {
                StartCoroutine(SetActiveSpaceship(m_SpaceshipList[0].token_id));
                return m_SpaceshipList[0];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public IEnumerator SetActiveSpaceship(string tokenId)
        {
            string name = string.Empty;
            ushort level = 0;
            if (string.IsNullOrEmpty(tokenId))
                tokenId = m_SpaceshipList[0].token_id;
            for (int i = 0; i < m_SpaceshipList.Count; i++)
            {
                if (tokenId == m_SpaceshipList[i].token_id)
                {
                    if (m_SpaceshipList[i].is_active)
                        yield break;
                    m_SpaceshipList[i].is_active = true;
                    name = m_SpaceshipList[i].name;
                    level = m_SpaceshipList[i].level;
                }
                else
                {
                    m_SpaceshipList[i].is_active = false;
                }
            }
            yield return SetActiveItem(tokenId, NFTCollectionType.Spaceship);
            EventLogManager.I.ItemActivate("active_spaceship_in_game", name, level);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="currentUpgradeCost"></param>
        public IEnumerator LevelupSpaceship(string tokenId, ushort currentUpgradeCost)
        {
            SpaceshipDataModel spaceship = GetSpaceship(tokenId);
            spaceship.level = (ushort)(spaceship.level + 1);
            spaceship.SyncMasterData();
            //CoroutineWithData api = new CoroutineWithData(this, UpdateItem(null, new List<SpaceshipDataModel> { spaceship }, null, null, null));
            //yield return api.coroutine;
            //if ((bool)api.result)
            //{
            //    yield return UpdateSoftCurrency1(SoftCurrency1 - currentUpgradeCost);
            //    if (16 == PlayerMissionManager.I.PlayerMissionStep && spaceship.level >= 5)
            //    {
            //        yield return UpdatePlayerGameplayMission(16);
            //    }
            //    OnSpaceShipUpgraded(spaceship);
            //    EventLogManager.I.SpaceshipUpgrade(spaceship);
            //}
            //else
            //{
            //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
            //}
            
            yield return null;
            yield return UpdateSoftCurrency1(SoftCurrency1 - currentUpgradeCost);
            OnSpaceShipUpgraded(spaceship);
            LayerSystem.I.HideSoftLoadingScreen();
        }
        #endregion

        #region chest

        /// <summary>
        /// Add new chest into inventory
        /// </summary>
        /// <returns></returns>
        public IEnumerator AddNewChest()
        {
            AddItemRequestModel request = new AddItemRequestModel();
            request.item_list.Add(new AddItemRequestModel.ItemData((ushort)NFTCollectionType.Chest, 1));

            //CoroutineWithData api = new CoroutineWithData(this, ApiService.I.AddItem(request));
            //yield return api.coroutine;
            //AddItemResponseModel response = (AddItemResponseModel)api.result;
            //if (response.success)
            //{
            //    m_ChestList.AddRange(response.data.chest);
            //}
            //else
            //{
            //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_ADD_CHEST"));
            //}
            yield return null;
            AddChest(CreateChestData(Guid.NewGuid().ToString(), "name"));
        }
        /// <summary>
        /// Create chest data model
        /// </summary>
        /// <param name="chestTokenId"></param>
        /// <param name="chestName"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public ChestDataModel CreateChestData(string tokenId, string chestName, int progress = 0)
        {
            ChestDataModel model = new ChestDataModel
            {
                token_id = tokenId,
                name = chestName,
                missionprogress = progress
            };
            model.SyncMasterData();
            return model;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddChest(ChestDataModel item)
        {
            m_ChestList.Add(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public ChestDataModel GetChest(string tokenId)
        {
            return m_ChestList.Find(a => a.token_id.Equals(tokenId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ChestDataModel GetChestByIndex(int index)
        {
            if (index < m_ChestList.Count)
                return m_ChestList[index];
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ChestDataModel GetActiveChest()
        {
            int index = m_ChestList.FindIndex(a => a.is_active);
            if (-1 < index)
                return m_ChestList[index];
            else
                return null;
        }
        /// <summary>
        /// Remove chest by token id
        /// </summary>
        /// <param name="tokenId"></param>
        public void RemoveChest(string tokenId)
        {
            int index = m_ChestList.FindIndex(a => a.token_id.Equals(tokenId));
            if (-1 < index)
                m_ChestList.RemoveAt(index);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        public IEnumerator SetActiveChest(string tokenId)
        {
            string name = string.Empty;
            if (string.IsNullOrEmpty(tokenId))
                tokenId = m_ChestList[0].token_id;
            for (int i = 0; i < m_ChestList.Count; i++)
            {
                if (tokenId == m_ChestList[i].token_id)
                {
                    if (m_ChestList[i].is_active)
                        yield break;
                    m_ChestList[i].is_active = true;
                    name = m_ChestList[i].name;
                }
                else
                {
                    m_ChestList[i].is_active = false;
                }
            }
            yield return SetActiveItem(tokenId, NFTCollectionType.Chest);
            EventLogManager.I.ItemActivate("active_mission_chest_in_game", name);
        }
        #endregion

        #region pass

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passTokenId"></param>
        /// <param name="passName"></param>
        /// <param name="bestRecord"></param>
        /// <param name="playcount"></param>
        /// <returns></returns>
        public PassDataModel CreatePassData(string passTokenId, string passName, int bestRecord = 0)
        {
            PassDataModel model = new PassDataModel
            {
                token_id = passTokenId,
                name = passName,
                bestrecord = bestRecord
            };
            model.SyncMasterData();
            return model;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddPass(PassDataModel item)
        {
            m_PassList.Add(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public PassDataModel GetPass(string tokenId)
        {
            PassDataModel model = m_PassList.Find(a => a.token_id.Equals(tokenId));
            return model ?? (m_PassList.Count > 0 ? m_PassList[0] : null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PassDataModel GetPassByIndex(int index)
        {
            if (index < m_PassList.Count)
                return m_PassList[index];
            return null;
        }
        /// <summary>
        /// Increate play count then update to server
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="playCount"></param>
        public IEnumerator IncreaseSurvivalPassPlayCount(string tokenId)
        {
            int foundIndex = m_PassList.FindIndex(a => a.token_id.Equals(tokenId));
            if (-1 < foundIndex)
            {
                if (m_PassList[foundIndex].playcount < m_PassList[foundIndex].maxplay)
                {
                    m_PassList[foundIndex].playcount++;

                    //CoroutineWithData api = new CoroutineWithData(this, UpdateItem(null, null, null, new List<PassDataModel> { m_PassList[foundIndex] }, null));
                    //yield return api.coroutine;
                    //if ((bool)api.result)
                    //{
                    //    yield return true;
                    //    LayerSystem.I.HideSoftLoadingScreen();
                    //}
                    //else
                    //{
                    //    yield return false;
                    //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
                    //}
                    yield return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public IEnumerator SetActivePass(string tokenId)
        {
            string name = string.Empty;
            if (string.IsNullOrEmpty(tokenId))
                tokenId = m_PassList[0].token_id;
            for (int i = 0; i < m_PassList.Count; i++)
            {
                if (tokenId == m_PassList[i].token_id)
                {
                    if (m_PassList[i].is_active)
                        yield break;
                    m_PassList[i].is_active = true;
                    name = m_PassList[i].name;
                }
                else
                {
                    m_PassList[i].is_active = false;
                }
            }
            yield return SetActiveItem(tokenId, NFTCollectionType.Pass);
            EventLogManager.I.ItemActivate("active_pass_in_game", name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PassDataModel GetActivePass()
        {
            PassDataModel model = m_PassList.Find(a => a.is_active);
            if (null != model)
            {
                return model;
            }
            else
            {
                StartCoroutine(SetActivePass(m_PassList[0].token_id));
                return m_PassList[0];
            }
        }
        #endregion

        #region special weapon

        /// <summary>
        /// Create special weapon data model
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="weaponName"></param>
        /// <param name="missionProgress"></param>
        /// <returns></returns>
        public SpecialWeaponDataModel CreateSpecialWeaponData(string tokenId, string weaponName, int missionProgress = 0)
        {
            SpecialWeaponDataModel model = new SpecialWeaponDataModel
            {
                token_id = tokenId,
                name = weaponName,
                missionprogress = missionProgress
            };
            model.SyncMasterData();
            return model;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddSpecialWeapon(SpecialWeaponDataModel item)
        {
            m_SpecialWeaponList.Add(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public SpecialWeaponDataModel GetSpecialWeapon(string tokenId)
        {
            return m_SpecialWeaponList.Find(a => a.token_id.Equals(tokenId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SpecialWeaponDataModel GetWeaponByIndex(int index)
        {
            if (index < m_SpecialWeaponList.Count)
                return m_SpecialWeaponList[index];
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SpecialWeaponDataModel GetActiveWeapon()
        {
            int index = m_SpecialWeaponList.FindIndex(a => a.is_active);
            if (-1 < index)
                return m_SpecialWeaponList[index];
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        public IEnumerator SetActiveWeapon(string tokenId)
        {
            string name = string.Empty;
            if (string.IsNullOrEmpty(tokenId))
                tokenId = m_SpecialWeaponList[0].token_id;
            for (int i = 0; i < m_SpecialWeaponList.Count; i++)
            {
                if (tokenId == m_SpecialWeaponList[i].token_id)
                {
                    if (m_SpecialWeaponList[i].is_active)
                        yield break;
                    m_SpecialWeaponList[i].is_active = true;
                    name = m_SpecialWeaponList[i].name;
                }
                else
                {
                    m_SpecialWeaponList[i].is_active = false;
                }
            }
            yield return SetActiveItem(tokenId, NFTCollectionType.SpecialWeapon);
            EventLogManager.I.ItemActivate("active_special_weapon_in_game", name);
        }
        #endregion

        #region currency

        /// <summary>
        /// Update currency by adding amount
        /// </summary>
        /// <param name="amount"></param>
        public void AddSoftCurrency1(uint amount)
        {
            SoftCurrency1 += amount;
            OnSoftCurrency1Changed?.Invoke(SoftCurrency1);
        }
        /// <summary>
        /// Update currency by deducting amount
        /// </summary>
        /// <param name="amount"></param>
        public void DeductSoftCurrency1(uint amount)
        {
            SoftCurrency1 -= amount;
            OnSoftCurrency1Changed?.Invoke(SoftCurrency1);
        }
        /// <summary>
        /// Update currency by setting new amount
        /// </summary>
        /// <param name="amount"></param>
        public void SetSoftCurrency1(uint amount)
        {
            SoftCurrency1 = amount;
            OnSoftCurrency1Changed?.Invoke(SoftCurrency1);
        }
        /// <summary>
        /// Update Soft Currency1 (Gold) to server
        /// </summary>
        /// <param name="balance"></param>
        /// <returns></returns>
        public IEnumerator UpdateSoftCurrency1(uint balance)
        {
            //CoroutineWithData api = new CoroutineWithData(this, ApiService.I.UpdateCurrency(balance));
            //yield return api.coroutine;
            //CurrencyResponseModel response = (CurrencyResponseModel)api.result;
            LayerSystem.I.HideSoftLoadingScreen();
            //if (response.success)
            //{
            //    SetSoftCurrency1(response.data.gold_amount);
            //}
            //else
            //{
            //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetErrorMassage($"{response.error.status}"));
            //}
            SetSoftCurrency1(balance);
            yield return null;
        }

        #endregion

        #region other
        /// <summary>
        /// Send active item request API
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerator SetActiveItem(string tokenId, NFTCollectionType type)
        {
            yield return null;
            //CoroutineWithData api = new CoroutineWithData(this,
            //    ApiService.I.SetActiveItem(new SetActiveItemRequestModel(tokenId, (ushort)type)));
            //yield return api.coroutine;
            //BaseResponseModel response = (BaseResponseModel)api.result;
            //if (response.success)
            //{
            //    OnItemActivated?.Invoke(tokenId, type);
            //}
            //else
            //{
            //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetErrorMassage($"{response.error.status}"));
            //}
        }
        /// <summary>
        /// Send update item request API
        /// </summary>
        /// <param name="pilotList"></param>
        /// <param name="spaceshipList"></param>
        /// <param name="specialWeaponList"></param>
        /// <param name="passList"></param>
        /// <param name="chestList"></param>
        /// <returns></returns>
        //public IEnumerator UpdateItem(List<PilotDataModel> pilotList,
        //    List<SpaceshipDataModel> spaceshipList,
        //    List<SpecialWeaponDataModel> specialWeaponList,
        //    List<PassDataModel> passList,
        //    List<ChestDataModel> chestList)
        //{
        //    UpdateItemRequestModel request = new UpdateItemRequestModel();
        //    if (null != pilotList) request.update_list.pilot.AddRange(pilotList);
        //    if (null != spaceshipList) request.update_list.spaceship.AddRange(spaceshipList);
        //    if (null != specialWeaponList) request.update_list.special_weapon.AddRange(specialWeaponList);
        //    if (null != passList) request.update_list.survival_pass.AddRange(passList);
        //    if (null != chestList) request.update_list.chest.AddRange(chestList);

        //    CoroutineWithData api = new CoroutineWithData(this, ApiService.I.UpdateItem(request));
        //    yield return api.coroutine;
        //    BaseResponseModel response = (BaseResponseModel)api.result;
        //    if (!response.success)
        //    {
        //        LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetErrorMassage($"{response.error.status}"));
        //        yield return false;
        //    }
        //    else
        //    {
        //        yield return true;
        //    }
        //}
        /// <summary>
        /// Update List items when change langguage
        /// </summary>
        public void ChangedLanguageSyncMasterData()
        {
            if (null != m_PilotList)
            {
                for (int i = 0; i < m_PilotList.Count; i++)
                {
                    PilotDataModel pilot = m_PilotList[i];
                    pilot.SyncMasterData();
                    m_PilotList[i] = pilot;
                }
            }
            if (null != m_SpaceshipList)
            {
                for (int i = 0; i < m_SpaceshipList.Count; i++)
                {
                    SpaceshipDataModel spaceship = m_SpaceshipList[i];
                    spaceship.SyncMasterData();
                    m_SpaceshipList[i] = spaceship;
                }
            }
            if (null != m_ChestList)
            {
                for (int i = 0; i < m_ChestList.Count; i++)
                {
                    ChestDataModel chest = m_ChestList[i];
                    chest.SyncMasterData();
                    m_ChestList[i] = chest;
                }
            }
            if (null != m_PassList)
            {
                for (int i = 0; i < m_PassList.Count; i++)
                {
                    PassDataModel pass = m_PassList[i];
                    pass.SyncMasterData();
                    m_PassList[i] = pass;
                }
            }
            if (null != m_SpecialWeaponList)
            {
                for (int i = 0; i < m_SpecialWeaponList.Count; i++)
                {
                    SpecialWeaponDataModel specialWeapon = m_SpecialWeaponList[i];
                    specialWeapon.SyncMasterData();
                    m_SpecialWeaponList[i] = specialWeapon;
                }
            }
        }
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
        /// Change an item to NFT
        /// </summary>
        public void ChangeToNft(NftMetadataModel item)
        {
            item.is_nft = true;
            if (item is PilotDataModel)
            {
                GetPilot(item.token_id).is_nft = true;
            }
            else if (item is SpaceshipDataModel)
            {
                GetSpaceship(item.token_id).is_nft = true;
            }
            else if (item is ChestDataModel)
            {
                GetChest(item.token_id).is_nft = true;
            }
            else if (item is PassDataModel)
            {
                GetPass(item.token_id).is_nft = true;
            }
            else //special weapon
            {
                GetSpecialWeapon(item.token_id).is_nft = true;
            }
            OnItemUpdated?.Invoke(item);
        }
        #endregion

        #region API Event listener
        private void OnApiError(long statusCode)
        {
            //if (statusCode.Equals(1000139) || statusCode.Equals(1000132)
            //        || statusCode.Equals(1000108) || statusCode.Equals(1000119))
            //{
            //    StartCoroutine(RefreshInventory());
            //}
        }
        #endregion
    }
}
