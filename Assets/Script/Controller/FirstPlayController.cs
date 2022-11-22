using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.API;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using UnityEngine;

namespace NFT1Forge.OSY.Controller
{
    public class FirstPlayController : BaseController
    {
        private readonly List<StartupRewardData> m_RewardList = new List<StartupRewardData>();

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Initialize()
        {
            yield return base.Initialize();
            if (AuthenticationSystem.I.isRegistered)
            {
                //yield return PostNftMetadata();
            }
            else
            {
                AddItemRequestModel request = new AddItemRequestModel();

                m_RewardList.AddRange(DatabaseSystem.I.GetMetadata<StartupReward>().startup_reward);
                for (int i = 0; i < m_RewardList.Count; i++)
                {
                    StartupRewardData data = m_RewardList[i];
                    int index = request.item_list.FindIndex(a => a.type.Equals(data.reward_type));
                    if (-1 < index)
                    {
                        request.item_list[index].value++;
                    }
                    else
                    {
                        request.item_list.Add(new AddItemRequestModel.ItemData(data.reward_type, 1));
                    }
                }
                //CoroutineWithData api = new CoroutineWithData(this, ApiService.I.AddItem(request));
                //yield return api.coroutine;
                //AddItemResponseModel response = (AddItemResponseModel)api.result;
                //if (response.success)
                //{
                //    yield return UpdateData(response.data);
                //}
                //else
                //{
                //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_ADD_ITEM"));
                //}
            }
        }
        private IEnumerator UpdateData(AddItemResponseModel.AddItemDataModel data)
        {
            yield return null;
            //List<PilotDataModel> pilotList = new List<PilotDataModel>();
            //List<SpaceshipDataModel> spaceshipList = new List<SpaceshipDataModel>();
            //List<SpecialWeaponDataModel> specialWeaponList = new List<SpecialWeaponDataModel>();
            //List<PassDataModel> passList = new List<PassDataModel>();
            //List<ChestDataModel> chestList = new List<ChestDataModel>();

            //for (int i = 0; i < data.pilot.Count; i++)
            //{
            //    int index = m_RewardList.FindIndex(a => a.reward_type == (ushort)NFTCollectionType.Pilot);
            //    if (-1 < index)
            //    {
            //        PilotMasterData pilotMaster = DatabaseSystem.I.GetMetadata<PilotMaster>().pilot_master.Find(
            //            a => a.id == m_RewardList[index].reward_type_id
            //            );
            //        if (null == pilotMaster) continue;
            //        PilotDataModel pilot = data.pilot[i];
            //        pilot.name = pilotMaster.name;
            //        pilot.SyncMasterData();
            //        pilotList.Add(pilot);
            //        m_RewardList.RemoveAt(index);
            //        InventorySystem.I.AddPilot(pilot);
            //    }
            //}
            //for (int i = 0; i < data.spaceship.Count; i++)
            //{
            //    int index = m_RewardList.FindIndex(a => a.reward_type == (ushort)NFTCollectionType.Spaceship);
            //    if (-1 < index)
            //    {
            //        SpaceshipMasterData spaceshipMaster = DatabaseSystem.I.GetMetadata<SpaceshipMaster>().spaceship_master.Find(
            //            a => a.id == m_RewardList[index].reward_type_id
            //            );
            //        if (null == spaceshipMaster) continue;
            //        SpaceshipDataModel spaceship = data.spaceship[i];
            //        spaceship.name = spaceshipMaster.name;
            //        spaceship.SyncMasterData();
            //        spaceshipList.Add(spaceship);
            //        m_RewardList.RemoveAt(index);
            //        InventorySystem.I.AddSpaceship(spaceship);
            //    }
            //}

            //CoroutineWithData api = new CoroutineWithData(this, InventorySystem.I.UpdateItem(pilotList,
            //    spaceshipList, specialWeaponList, passList, chestList));
            //yield return api.coroutine;
            //if ((bool)api.result)
            //{
            //    yield return ChangeToNextController();
            //}
            //else
            //{
            //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
            //}
        }
        /// <summary>
        /// Loading next scene
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangeToNextController()
        {
            yield return Shutdown();
            LayerSystem.I.HideSoftLoadingScreen();
            LayerSystem.I.ShowLoadingScreen();
            ControllerSystem.I.SceneChange("Scene/MainMenu");
        }
        /// <summary>
        /// Shutdown
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Shutdown()
        {
            yield return base.Shutdown();
        }
    }
}
