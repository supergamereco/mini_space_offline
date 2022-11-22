#if UNITY_WEBGL
using System.Collections;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using UnityEngine;

namespace NFT1Forge.OSY.System
{
    public class WebBridgeSystem : Singleton<WebBridgeSystem>
    {
        private bool m_PageReady = false;
        public bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Startup system
        /// </summary>
        public void InitSystem()
        {
            StartCoroutine(Initialize());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator Initialize()
        {
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                CheckPageReady();
            }
            else
            {
                m_PageReady = true;
            }
            yield return new WaitUntil(() => { return m_PageReady; });
            IsInitialized = true;
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckPageReady()
        {
            if (WebBridgeUtils.IsPageReady())
            {
                m_PageReady = true;
            }
            else
            {
                Invoke("CheckPageReady", 1f);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public void SendEventToPage(int code, WebMessageModel message)
        {
            string messageStr = JsonUtility.ToJson(message);
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.SendMessageToPage(messageStr);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void ReceiveWebMessage(string jsonMessage)
        {
            WebMessageModel info = JsonUtility.FromJson<WebMessageModel>(jsonMessage);
            switch ((MessageType)info.code)
            {
                case MessageType.UpdateMissionStep:
                    PlayerMissionManager.I.ProcessUpdateMessage(info.message);
                    break;
            }
        }
    }
}
#endif
