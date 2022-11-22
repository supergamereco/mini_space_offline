using System;
using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.API;
using NFT1Forge.OSY.JsonModel;

namespace NFT1Forge.OSY.System
{
    public class AuthenticationSystem : Singleton<AuthenticationSystem>
    {
        public string WalletAddress;
        public string AuthenticationToken { get; private set; }

        public bool isRegistered = false;
        public List<ulong> pilotTokenIdList;
        public List<ulong> spaceshipTokenIdList;

        private Action<bool> OnLoginCallback;

        /// <summary>
        /// Initialize system
        /// </summary>
        public void InitSystem()
        {
        }
        /// <summary>
        /// Login with corporate account
        /// </summary>
        //public IEnumerator LoginCorporate(string username, string password, Action<bool> callback)
        //{
        //    OnLoginCallback = callback;
        //    CoroutineWithData api = new CoroutineWithData(this, ApiService.I.LogIn(username, password));
        //    yield return api.coroutine;
        //    LoginResponseModel response = (LoginResponseModel)api.result;
        //    if (response.success)
        //    {
        //        AuthenticationToken = response.data;
        //        OnLoginCallback(true);
        //    }
        //    else
        //    {
        //        OnLoginCallback(false);
        //    }
        //}
    }
}
