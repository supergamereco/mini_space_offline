using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.JsonModel;
using TMPro;
using UnityEngine;

namespace NFT1Forge.OSY.System
{
    public class LocalizationSystem : Singleton<LocalizationSystem>
    {
        private const string EN_LANGUEAGE_PATH = "Font/DinC_SDF";
        private const string TH_LANGUEAGE_PATH = "Font/IBMPlexSansThai-Regular_SDF";
        /// <summary>
        /// Enter the key to convert it into words.
        /// </summary>
        public string GetLocalizeValue(string key)
        {
            LocalizationData data = DatabaseSystem.I.GetMetadata<Localization>().localization.Find(a => a.key.Equals(key));
            if (data != null) return data.description;
            else return $"Error : {key}"; ;
        }
        /// <summary>
        /// Enter the key to convert it error massage.
        /// </summary>
        public string GetErrorMassage(string key)
        {
            ErrorMassageData data = DatabaseSystem.I.GetMetadata<ErrorMassage>().error_massage.Find(a => a.key.Equals(key));
            if (data != null) return data.description;
            else return $"Error : {key}"; ;
        }
        /// <summary>
        /// Get font asset by current language
        /// </summary>
        public TMP_FontAsset GetFont()
        {
            string currentlanguage = SystemConfig.LanguageCode;
            return currentlanguage switch
            {
                "en" => (TMP_FontAsset)Resources.Load(EN_LANGUEAGE_PATH),
                "th" => (TMP_FontAsset)Resources.Load(TH_LANGUEAGE_PATH),
                _ => null
            };
        }
    }
}
