using System.Collections;
using System.Net.Http;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using Script.JsonModel.API.DataModel;
//using Script.System;

namespace NFT1Forge.OSY.API
{
    public class ApiService : Singleton<ApiService>
    {
        ///// <summary>
        ///// Log In
        ///// </summary>
        ///// <param name="username"></param>
        ///// <param name="password"></param>
        ///// <param name="callback"></param>
        ///// <returns></returns>
        //public IEnumerator LogIn(string username, string password)
        //{
        //    string url = $"{APIConfig.ApiServerBaseUrl}/login/corporate";
        //    //CoroutineWithData getItem = new CoroutineWithData(this, NetworkSystem.I.SendApi<LoginResponseModel>(HttpMethod.Post.Method, url, new LoginRequestModel(username, password)));
        //    //yield return getItem.coroutine;
        //    //yield return getItem.result;
        //    yield return null;
        //}
        /// <summary>
        /// Update soft currency (Gold)
        /// </summary>
        /// <param name="gold"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator UpdateCurrency(uint gold)
        {
            //string url = $"{APIConfig.ApiServerBaseUrl}/api/gold";
            //UserCurrencyDataModel request = new UserCurrencyDataModel
            //{
            //    gold_amount = gold
            //};
            //CoroutineWithData api = new CoroutineWithData(this, NetworkSystem.I.SendApi<CurrencyResponseModel>(HttpMethod.Post.Method, url, request));
            //yield return api.coroutine;
            //yield return api.result;
            yield return null;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="walletAddress"></param>
        ///// <param name="type"></param>
        ///// <param name="callback"></param>
        ///// <returns></returns>
        //public IEnumerator GetItem()
        //{
        //    string url = $"{APIConfig.ApiServerBaseUrl}/api/items/list";
        //    //CoroutineWithData api = new CoroutineWithData(this, NetworkSystem.I.SendApi<GetItemResponseModel>(HttpMethod.Get.Method, url));
        //    //yield return api.coroutine;
        //    //yield return api.result;
        //    yield return null;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public IEnumerator AddItem(AddItemRequestModel request)
        //{
        //    string url = $"{APIConfig.ApiServerBaseUrl}/api/item/add";
        //    //CoroutineWithData api = new CoroutineWithData(this, NetworkSystem.I.SendApi<AddItemResponseModel>(HttpMethod.Post.Method, url, request));
        //    //yield return api.coroutine;
        //    //yield return api.result;
        //    yield return null;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public IEnumerator SetActiveItem(SetActiveItemRequestModel request)
        //{
        //    string url = $"{APIConfig.ApiServerBaseUrl}/api/item/active";
        //    //CoroutineWithData api = new CoroutineWithData(this, NetworkSystem.I.SendApi<BaseResponseModel>(HttpMethod.Post.Method, url, request));
        //    //yield return api.coroutine;
        //    //yield return api.result;
        //    yield return null;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public IEnumerator UpdateItem(UpdateItemRequestModel request)
        //{
        //    string url = $"{APIConfig.ApiServerBaseUrl}/api/item/update";
        //    //CoroutineWithData api = new CoroutineWithData(this, NetworkSystem.I.SendApi<BaseResponseModel>(HttpMethod.Post.Method, url, request));
        //    //yield return api.coroutine;
        //    //yield return api.result;
        //    yield return null;
        //}
        ///// <summary>
        ///// Update current mission as completed
        ///// </summary>
        ///// <param name="step">Current mission</param>
        ///// <returns></returns>
        //public IEnumerator UpdateMissionStep(ushort step)
        //{
        //    string url = $"{APIConfig.ApiServerBaseUrl}/api/mission";
        //    //CoroutineWithData api = new CoroutineWithData(this, NetworkSystem.I.SendApi<UpdateMissionResponseModel>(HttpMethod.Post.Method, url, new UpdateMissionRequestModel()
        //    //{
        //    //    step = step
        //    //}));
        //    //yield return api.coroutine;
        //    //yield return api.result;
        //    yield return null;
        //}
        ///// <summary>
        ///// Call api openbox
        ///// </summary>
        ///// <param name="chest"></param>
        ///// <returns></returns>
        //public IEnumerator OpenChest(NftMetadataModel chest)
        //{
        //    ChestMasterData chestMaster = DatabaseSystem.I.GetMetadata<ChestMaster>().chest_master.Find(a => a.name.Equals(chest.name));
        //    if (null == chestMaster)
        //    {
        //        yield return null;
        //    }
        //    else
        //    {
        //        string url = $"{APIConfig.ApiServerBaseUrl}/api/openbox";
        //        //CoroutineWithData api = new CoroutineWithData(this,
        //        //    NetworkSystem.I.SendApi<OpenChestResponseModel>(HttpMethod.Post.Method, url, new OpenChestRequestModel(chest.token_id, chestMaster.reward_id)));
        //        //yield return api.coroutine;
        //        //yield return api.result;
        //        yield return null;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="tokenID"></param>
        ///// <returns></returns>
        //public IEnumerator Minting(string tokenID)
        //{
        //    string url = $"{APIConfig.ApiServerBaseUrl}/api/minting";
        //    //CoroutineWithData api = new CoroutineWithData(this, NetworkSystem.I.SendApi<BaseResponseModel>(HttpMethod.Post.Method, url, new MintingRequestModel(tokenID)));
        //    //yield return api.coroutine;
        //    //yield return api.result;
        //    yield return null;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="tokenID"></param>
        ///// <returns></returns>
        //public IEnumerator CancelMinting(string tokenID)
        //{
        //    string url = $"{APIConfig.ApiServerBaseUrl}/api/minting/cancel";
        //    //CoroutineWithData api = new CoroutineWithData(this, NetworkSystem.I.SendApi<BaseResponseModel>(HttpMethod.Post.Method, url, new MintingRequestModel(tokenID)));
        //    //yield return api.coroutine;
        //    //yield return api.result;
        //    yield return null;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerator GetMintingStatus(string tokenID)
        //{
        //    string url = $"{APIConfig.ApiServerBaseUrl}/api/minting/status?token_id={tokenID}";
        //    //CoroutineWithData api = new CoroutineWithData(this, NetworkSystem.I.SendApi<GetMintingStatusResponseModel>(HttpMethod.Get.Method, url));
        //    //yield return api.coroutine;
        //    //yield return api.result;
        //    yield return null;
        //}
    }
}
