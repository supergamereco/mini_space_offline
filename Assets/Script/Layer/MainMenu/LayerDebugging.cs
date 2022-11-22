using System;
using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.API;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer
{
    public class LayerDebugging : BaseLayer
    {
        [SerializeField] private Button m_ButtonAddSpaceship1;
        [SerializeField] private Button m_ButtonAddSpaceship2;
        [SerializeField] private Button m_ButtonAddSpaceship3;
        [SerializeField] private Button m_ButtonAddSpaceship4;
        [SerializeField] private Button m_ButtonAddSpaceship5;

        [SerializeField] private Button m_ButtonAddPilot1;
        [SerializeField] private Button m_ButtonAddPilot2;
        [SerializeField] private Button m_ButtonAddPilot3;
        [SerializeField] private Button m_ButtonAddPilot4;
        [SerializeField] private Button m_ButtonAddPilot5;

        [SerializeField] private Button m_ButtonAddChest1;
        [SerializeField] private Button m_ButtonAddChest2;

        [SerializeField] private Button m_ButtonAddPass1;
        [SerializeField] private Button m_ButtonAddPass2;

        [SerializeField] private Button m_ButtonAddWeapon1;
        [SerializeField] private Button m_ButtonAddWeapon2;
        [SerializeField] private Button m_ButtonAddWeapon3;

        [SerializeField] private Button m_ButtonClose;
        [SerializeField] private Button m_ButtonTestJava;
        [SerializeField] private Button m_ButtonAddGold;
        public Action OnClosed;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override void Initialize()
        {
            //m_ButtonAddPilot1.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Pilot, 1, AfterAddPilot)); });
            //m_ButtonAddPilot2.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Pilot, 2, AfterAddPilot)); });
            //m_ButtonAddPilot3.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Pilot, 3, AfterAddPilot)); });
            //m_ButtonAddPilot4.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Pilot, 4, AfterAddPilot)); });
            //m_ButtonAddPilot5.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Pilot, 5, AfterAddPilot)); });

            //m_ButtonAddSpaceship1.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Spaceship, 1, AfterAddSpaceship)); });
            //m_ButtonAddSpaceship2.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Spaceship, 2, AfterAddSpaceship)); });
            //m_ButtonAddSpaceship3.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Spaceship, 3, AfterAddSpaceship)); });
            //m_ButtonAddSpaceship4.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Spaceship, 4, AfterAddSpaceship)); });
            //m_ButtonAddSpaceship5.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Spaceship, 5, AfterAddSpaceship)); });

            //m_ButtonAddChest1.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Chest, 1, AfterAddChest)); });
            //m_ButtonAddChest2.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Chest, 2, AfterAddChest)); });

            //m_ButtonAddPass1.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Pass, 1, AfterAddPass)); });
            //m_ButtonAddPass2.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.Pass, 2, AfterAddPass)); });

            //m_ButtonAddWeapon1.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.SpecialWeapon, 1, AfterAddSpecialWeapon)); });
            //m_ButtonAddWeapon2.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.SpecialWeapon, 3, AfterAddSpecialWeapon)); });
            //m_ButtonAddWeapon3.onClick.AddListener(() => { StartCoroutine(AddInventory(NFTCollectionType.SpecialWeapon, 5, AfterAddSpecialWeapon)); });


            m_ButtonClose.onClick.AddListener(OnCloseButton);
            //m_ButtonTestJava.onClick.AddListener(() => { OnButtonTestJava(); });
            m_ButtonAddGold.onClick.AddListener(OnAddGoldButton);
            m_ButtonAddPilot1.onClick.AddListener(() => HardAddPilot(0));
            m_ButtonAddPilot2.onClick.AddListener(() => HardAddPilot(1));
            m_ButtonAddPilot3.onClick.AddListener(() => HardAddPilot(2));
            m_ButtonAddPilot4.onClick.AddListener(() => HardAddPilot(3));
            m_ButtonAddPilot5.onClick.AddListener(() => HardAddPilot(4));

            m_ButtonAddSpaceship1.onClick.AddListener(() => HardAddSpaceship(0));
            m_ButtonAddSpaceship2.onClick.AddListener(() => HardAddSpaceship(1));
            m_ButtonAddSpaceship3.onClick.AddListener(() => HardAddSpaceship(2));
            m_ButtonAddSpaceship4.onClick.AddListener(() => HardAddSpaceship(3));
            m_ButtonAddSpaceship5.onClick.AddListener(() => HardAddSpaceship(4));

            m_ButtonAddChest1.onClick.AddListener(() => HardAddChest(0));
            m_ButtonAddChest2.onClick.AddListener(() => HardAddChest(1));

            m_ButtonAddPass1.onClick.AddListener(() => HardAddPass(0));
            m_ButtonAddPass2.onClick.AddListener(() => HardAddPass(1));

            m_ButtonAddWeapon1.onClick.AddListener(() => HardAddWeapon(0));
            m_ButtonAddWeapon2.onClick.AddListener(() => HardAddWeapon(2));
            m_ButtonAddWeapon3.onClick.AddListener(() => HardAddWeapon(4));
        }
        private void HardAddPilot(int index)
        {
            PilotMasterData pilotMaster = DatabaseSystem.I.GetMetadata<PilotMaster>().pilot_master[index];
            PilotDataModel pilot = new PilotDataModel();
            pilot.token_id = Guid.NewGuid().ToString();
            pilot.name = pilotMaster.name;
            pilot.level = 1;
            pilot.SyncMasterData();
            InventorySystem.I.AddPilot(pilot);
        }
        private void HardAddSpaceship(int index)
        {
            SpaceshipMasterData spaceshipMaster = DatabaseSystem.I.GetMetadata<SpaceshipMaster>().spaceship_master[index];
            SpaceshipDataModel spaceship = new SpaceshipDataModel();
            spaceship.token_id = Guid.NewGuid().ToString();
            spaceship.name = spaceshipMaster.name;
            spaceship.level = 1;
            spaceship.SyncMasterData();
            InventorySystem.I.AddSpaceship(spaceship);
        }
        private void HardAddChest(int index)
        {
            ChestMasterData chestMaster = DatabaseSystem.I.GetMetadata<ChestMaster>().chest_master[index];
            ChestDataModel chest = new ChestDataModel();
            chest.token_id = Guid.NewGuid().ToString();
            chest.name = chestMaster.name;
            chest.SyncMasterData();
            InventorySystem.I.AddChest(chest);
        }
        private void HardAddWeapon(int index)
        {
            SpecialWeaponMasterData specialWeaponMaster = DatabaseSystem.I.GetMetadata<SpecialWeaponMaster>().weapon_master[index];
            SpecialWeaponDataModel specialWeapon = new SpecialWeaponDataModel();
            specialWeapon.token_id = Guid.NewGuid().ToString();
            specialWeapon.name = specialWeaponMaster.name;
            specialWeapon.SyncMasterData();
            InventorySystem.I.AddSpecialWeapon(specialWeapon);
        }
        private void HardAddPass(int index)
        {
            PassMasterData passMaster = DatabaseSystem.I.GetMetadata<PassMaster>().pass_master[index];
            PassDataModel pass = new PassDataModel();
            pass.token_id = Guid.NewGuid().ToString();
            pass.name = passMaster.name;
            pass.SyncMasterData();
            InventorySystem.I.AddPass(pass);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnCloseButton()
        {
            OnClosed?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator AddInventory(NFTCollectionType type, ushort id, Action<List<BaseMetadataRequestModel>> callback)
        {
            LayerSystem.I.ShowSoftLoadingScreen();
            AddItemRequestModel request = new AddItemRequestModel();
            request.item_list.Add(new AddItemRequestModel.ItemData((ushort)type, 1));
            //CoroutineWithData api = new CoroutineWithData(this, ApiService.I.AddItem(request));
            //yield return api.coroutine;
            //AddItemResponseModel response = (AddItemResponseModel)api.result;
            //if (response.success)
            //{
            //    yield return UpdateItemData(type, id, response.data);
            //}
            //else
            //{
            //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_ADD_ITEM"));
            //}
            yield return null;

            LayerSystem.I.HideSoftLoadingScreen();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerator UpdateItemData(NFTCollectionType type, ushort id, AddItemResponseModel.AddItemDataModel rawData)
        {
            List<PilotDataModel> pilotList = new List<PilotDataModel>();
            List<SpaceshipDataModel> spaceshipList = new List<SpaceshipDataModel>();
            List<SpecialWeaponDataModel> specialWeaponList = new List<SpecialWeaponDataModel>();
            List<PassDataModel> passList = new List<PassDataModel>();
            List<ChestDataModel> chestList = new List<ChestDataModel>();

            switch (type)
            {
                case NFTCollectionType.Pilot:
                    PilotMasterData pilotMaster = DatabaseSystem.I.GetMetadata<PilotMaster>().pilot_master.Find(
                                    a => a.id.Equals(id)
                                    );
                    PilotDataModel pilot = rawData.pilot[0];
                    pilot.name = pilotMaster.name;
                    pilot.SyncMasterData();
                    pilotList.Add(pilot);
                    InventorySystem.I.AddPilot(pilot);
                    break;

                case NFTCollectionType.Spaceship:
                    SpaceshipMasterData spaceshipMaster = DatabaseSystem.I.GetMetadata<SpaceshipMaster>().spaceship_master.Find(
                                    a => a.id.Equals(id)
                                    );
                    SpaceshipDataModel spaceship = rawData.spaceship[0];
                    spaceship.name = spaceshipMaster.name;
                    spaceship.SyncMasterData();
                    spaceshipList.Add(spaceship);
                    InventorySystem.I.AddSpaceship(spaceship);
                    break;

                case NFTCollectionType.Chest:
                    ChestMasterData chestMaster = DatabaseSystem.I.GetMetadata<ChestMaster>().chest_master.Find(
                                    a => a.id.Equals(id)
                                    );
                    ChestDataModel chest = rawData.chest[0];
                    chest.name = chestMaster.name;
                    chest.SyncMasterData();
                    chestList.Add(chest);
                    InventorySystem.I.AddChest(chest);
                    break;

                case NFTCollectionType.Pass:
                    PassMasterData passMaster = DatabaseSystem.I.GetMetadata<PassMaster>().pass_master.Find(
                                    a => a.id.Equals(id)
                                    );
                    PassDataModel pass = rawData.survival_pass[0];
                    pass.name = passMaster.name;
                    pass.SyncMasterData();
                    passList.Add(pass);
                    InventorySystem.I.AddPass(pass);
                    break;

                case NFTCollectionType.SpecialWeapon:
                    SpecialWeaponMasterData specialWeaponMaster = DatabaseSystem.I.GetMetadata<SpecialWeaponMaster>().weapon_master.Find(
                                    a => a.id.Equals(id)
                                    );
                    SpecialWeaponDataModel specialWeapon = rawData.special_weapon[0];
                    specialWeapon.name = specialWeaponMaster.name;
                    specialWeapon.SyncMasterData();
                    specialWeaponList.Add(specialWeapon);
                    InventorySystem.I.AddSpecialWeapon(specialWeapon);
                    break;
            }
            //CoroutineWithData api = new CoroutineWithData(this, InventorySystem.I.UpdateItem(pilotList,
            //spaceshipList, specialWeaponList, passList, chestList));
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
        /// <summary>
        /// 
        /// </summary>
        private void OnButtonTestJava()
        {
#if UNITY_WEBGL
            WebBridgeUtils.IsLogIn("1", "1");
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnAddGoldButton()
        {
            StartCoroutine(InventorySystem.I.UpdateSoftCurrency1(InventorySystem.I.SoftCurrency1 + 10000));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        private void AfterAddPilot(List<BaseMetadataRequestModel> rawData)
        {
            PilotMetadataRequestModel data = (PilotMetadataRequestModel)rawData[0];
            InventorySystem.I.AddPilot(InventorySystem.I.CreatePilotData(data.tokenId, data.name));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        private void AfterAddSpaceship(List<BaseMetadataRequestModel> rawData)
        {
            SpaceshipMetadataRequestModel data = (SpaceshipMetadataRequestModel)rawData[0];
            InventorySystem.I.AddSpaceship(InventorySystem.I.CreateSpaceshipData(data.tokenId, data.name));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        private void AfterAddChest(List<BaseMetadataRequestModel> rawData)
        {
            ChestMetadataRequestModel data = (ChestMetadataRequestModel)rawData[0];
            InventorySystem.I.AddChest(InventorySystem.I.CreateChestData(data.tokenId, data.name));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        private void AfterAddPass(List<BaseMetadataRequestModel> rawData)
        {
            PassMetadataRequestModel data = (PassMetadataRequestModel)rawData[0];
            InventorySystem.I.AddPass(InventorySystem.I.CreatePassData(data.tokenId, data.name));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        private void AfterAddSpecialWeapon(List<BaseMetadataRequestModel> rawData)
        {
            SpecialWeaponMetadataRequestModel data = (SpecialWeaponMetadataRequestModel)rawData[0];
            InventorySystem.I.AddSpecialWeapon(InventorySystem.I.CreateSpecialWeaponData(data.tokenId, data.name));
        }
    }
}
