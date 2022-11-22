using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.System
{
    public class NetworkSystem : Singleton<NetworkSystem>
    {
        public delegate void ApiError(long statusCode);
        public event ApiError OnApiError;

        /// <summary>
        /// Send api to game server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public IEnumerator SendApi<T>(string httpMethod, string url,
            BaseRequestModel data = null) where T : BaseResponseModel
        {
            UnityWebRequest request = new UnityWebRequest(url, httpMethod);
            if (httpMethod != HttpMethod.Get.Method)
            {
                if (data != null)
                {
                    string modelString = JsonUtility.ToJson(data);
                    byte[] bytePostData = Encoding.UTF8.GetBytes(modelString);
                    request.uploadHandler = new UploadHandlerRaw(bytePostData);
                }
            }
            if (!string.IsNullOrEmpty(AuthenticationSystem.I.AuthenticationToken))
            {
                request.SetRequestHeader("Authorization", $"Bearer {AuthenticationSystem.I.AuthenticationToken}");
            }
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            T response;
            if (request.downloadHandler.text != "" && request.downloadHandler != null)
            {
                response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                if (!response.success)
                    OnApiError?.Invoke(response.error.status);
            }
            else
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    response = new BaseResponseModel(true, "success") as T;
                }
                else
                {
                    response = new BaseResponseModel(false, request.error, request.responseCode) as T;
                }
            }
            yield return response;
        }
        /* NOTE keep this comment because we might use it for calling 1sync server later */
        /// <summary>
        /// Send api to 1sync server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        //public IEnumerator SendApiOneSync<T>(string httpMethod, string url,
        //    Action<T> callback, string requestModel = null) where T : BaseResponseModel
        //{
        //    var request = new UnityWebRequest(url, httpMethod);
        //    if (httpMethod != HttpMethod.Get.Method)
        //    {
        //        if (requestModel != null)
        //        {
        //            var bytePostData = Encoding.UTF8.GetBytes(requestModel);
        //            request.uploadHandler = new UploadHandlerRaw(bytePostData);
        //        }
        //    }
        //    request.downloadHandler = new DownloadHandlerBuffer();

        //    request.SetRequestHeader("Content-Type", "application/json");
        //    request.SetRequestHeader("Accept", "application/json");
        //    request.SetRequestHeader(APIConfig.OneSyncAppSecretKey, APIConfig.AppSecret);
        //    yield return request.SendWebRequest();

        //    T responseModel;
        //    if (request.downloadHandler.text != "" && request.downloadHandler != null)
        //    {
        //        responseModel = JsonUtility.FromJson<T>(request.downloadHandler.text);
        //    }
        //    else
        //    {
        //        if (request.result == UnityWebRequest.Result.Success)
        //        {
        //            responseModel = new BaseResponseModel(true, "success") as T;
        //        }
        //        else
        //        {
        //            responseModel = new BaseResponseModel(false, request.error, request.responseCode) as T;
        //        }
        //    }
        //    callback?.Invoke(responseModel);
        //}
        /// <summary>
        /// Get HTTP request and callback with string content
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator GetRequest(string url, Action<string> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                string[] pages = url.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        callback?.Invoke(null);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        callback?.Invoke(null);
                        break;
                    case UnityWebRequest.Result.Success:
                        callback?.Invoke(webRequest.downloadHandler.text);
                        break;
                }
            }
        }
    }
}