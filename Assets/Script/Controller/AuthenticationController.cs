using System.Collections;
using NFT1Forge.OSY.API;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Layer;
using NFT1Forge.OSY.System;
using UnityEngine;

namespace NFT1Forge.OSY.Controller
{
    public class AuthenticationController : BaseController
    {
        private LayerLogin m_LayerLogin;

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Initialize()
        {
            yield return base.Initialize();
#if UNITY_WEBGL
            yield return new WaitUntil(() => { return WebBridgeSystem.I.IsInitialized; });
#endif
            AuthenticationSystem.I.InitSystem();

            LayerSystem.I.CreateLayer<LayerLogin>("LayerLogin", LayerType.NormalLayer, (layer) =>
            {
                m_LayerLogin = layer;
                //m_LayerLogin.OnLoginSuccess = () => { StartCoroutine(LoginSuccess()); };
                m_LayerLogin.OnLoginSuccess = () => { TempFakeLogin(); };
            });
        }
        /// <summary>
        /// Authentication system has successfully log in
        /// </summary>
        //private IEnumerator LoginSuccess()
        //{
        //#if UNITY_ANDROID && !UNITY_EDITOR
        //            Firebase.Analytics.FirebaseAnalytics
        //              .LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin);
        //#endif
        //yield return GetItemList();
        //}
        private void TempFakeLogin()
        {
            InventorySystem.I.SetSoftCurrency1(5000);
            AuthenticationSystem.I.WalletAddress = "0x056F93678893737AD88EE80F56";
            InventorySystem.I.AddPilot(InventorySystem.I.CreatePilotData("1", "name"));
            InventorySystem.I.AddSpaceship(InventorySystem.I.CreateSpaceshipData("2", "name"));
            StartCoroutine(ChangeSceneToMainMenu());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        //        private IEnumerator GetItemList()
        //        {
        //            CoroutineWithData api = new CoroutineWithData(this, ApiService.I.GetItem());
        //            yield return api.coroutine;
        //            GetItemResponseModel response = (GetItemResponseModel)api.result;

        //            if (response.success)
        //            {
        //                AuthenticationSystem.I.WalletAddress = response.data.account_id;
        //                InventorySystem.I.SetSoftCurrency1(response.data.gold_amount);
        //                PlayerMissionManager.I.UpdateMission(response.data.current_step);
        //#if UNITY_WEBGL
        //                if (BuildType.WebPlayer == SystemConfig.BuildType)
        //                {
        //                    WebBridgeUtils.IsLogIn("1",$"{response.data.current_step}");
        //                }
        //#endif
        //                if (0 == response.data.object_list.pilot.Count || 0 == response.data.object_list.spaceship.Count)
        //                {
        //                    StartCoroutine(ChangeSceneToFirstPlay());
        //                }
        //                else
        //                {
        //                    InventorySystem.I.AddInventoryList(response.data.object_list);
        //                    StartCoroutine(ChangeSceneToMainMenu());
        //                }
        //            }
        //            else
        //            {
        //                LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetErrorMassage($"{response.error.status}"));
        //            }
        //        }













        /*
         * OLD CODE
         * WILL BE REMOVED AFTER VERSION 16xx.00
         */




        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private IEnumerator GetUserRegisterData(string userID)
        {
            yield return null;
            //yield return ApiService.GetNewUserRegisteredData(userID, (responseModel) =>
            //{
            //    if (responseModel.success)
            //    {
            //        //Debug.Log($"GetUserRegisterData {JsonUtility.ToJson(responseModel)}");  //keep it for debugging
            //        foreach (var dataModel in responseModel.data)
            //        {
            //            AuthenticationSystem.I.isRegistered = true;
            //            if (dataModel.type == (int)NFTCollectionType.Pilot)
            //            {
            //                AuthenticationSystem.I.pilotTokenIdList.Add(dataModel.tokenId);
            //            }
            //            else if (dataModel.type == (int)NFTCollectionType.Spaceship)
            //            {
            //                AuthenticationSystem.I.spaceshipTokenIdList.Add(dataModel.tokenId);
            //            }
            //        }

            //        StartCoroutine(ChangeSceneToFirstPlay());
            //    }
            //    else
            //    {
            //        LayerSystem.I.ShowPopupDialog($"{responseModel.message},{responseModel.error.status}");
            //    }
            //});
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangeSceneToFirstPlay()
        {
            yield return Shutdown();
            ControllerSystem.I.SceneChange("Scene/FirstPlay");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangeSceneToMainMenu()
        {
            yield return Shutdown();
            LayerSystem.I.ShowLoadingScreen();
            ControllerSystem.I.SceneChange("Scene/MainMenu");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Shutdown()
        {
            yield return base.Shutdown();
            m_LayerLogin.Delete();
        }
    }
}
