using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.JsonModel;
using Script.System;
using UnityEngine;

namespace NFT1Forge.OSY.System
{
    public class DatabaseSystem : Singleton<DatabaseSystem>
    {
        private readonly Dictionary<string, BaseJsonModel> m_MetadataDict = new Dictionary<string, BaseJsonModel>();
        private int m_RegisteredCount = 0;
        private int m_LoadedCount = 0;

        #region getter
        public bool IsMetadataReady => (m_RegisteredCount == m_LoadedCount);
        #endregion

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
            yield return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void LoadMetadata<T>(string metadataName) where T : BaseJsonModel
        {
            m_RegisteredCount++;
            StartCoroutine(NetworkSystem.I.GetRequest($"{SystemConfig.MetadataUrl}{SystemConfig.LanguageCode}/{metadataName}.json", (json) =>
            {
                if (!string.IsNullOrEmpty(json))
                {
                    m_LoadedCount++;
                    m_MetadataDict[typeof(T).Name] = JsonUtility.FromJson<T>(json);
                }
            }));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetMetadata<T>() where T : BaseJsonModel
        {
            if (m_MetadataDict.ContainsKey(typeof(T).Name))
            {
                return m_MetadataDict[typeof(T).Name] as T;
            }
            else
            {
                return null;
            }
        }

    }
}
