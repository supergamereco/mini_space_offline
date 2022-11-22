using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NFT1Forge.OSY.System;

namespace NFT1Forge.OSY.Layer
{
    public class LayerSetting : BaseLayer
    {
        [SerializeField] private Toggle m_BGMToggle;
        [SerializeField] private Toggle m_SFXToggle;
        [SerializeField] private TMP_Dropdown m_Language;
        [SerializeField] private Button m_ButtonClose;
        public Action OnClosed;
        public Action OnLangaugeChanged;
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            m_ButtonClose.onClick.AddListener(OnButtonClose);
            m_BGMToggle.onValueChanged.AddListener(OnBGMSwitch);
            m_SFXToggle.onValueChanged.AddListener(OnSFXSwitch);
            m_Language.onValueChanged.AddListener(OnLangueageChange);
            SetActiveToggleBGM(PlayerPrefs.GetInt("BGMVolume") != 0);
            SetActiveToggleSFX(PlayerPrefs.GetInt("SFXVolume") != 0);
            SetLanguageDropdown();

        }
        /// <summary>
        /// 
        /// </summary>
        private void OnLangueageChange(int index)
        {
            switch (index)
            {
                case 0:
                    SystemConfig.LanguageCode = "en";
                    break;
                case 1:
                    SystemConfig.LanguageCode = "th";
                    break;
            }
            PlayerPrefs.SetString("Language", SystemConfig.LanguageCode);
            OnLangaugeChanged?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnButtonClose()
        {
            OnClosed?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnBGMSwitch(bool isOn)
        {
            SoundManager.I.ToggleBGMVolume(isOn);
            PlayerPrefs.SetInt("BGMVolume", isOn ? 1 : 0);
            SetActiveToggleBGM(isOn);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnSFXSwitch(bool isOn)
        {
            SoundManager.I.ToggleSFXVolume(isOn);
            PlayerPrefs.SetInt("SFXVolume", isOn ? 1 : 0);
            SetActiveToggleSFX(isOn);
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetActiveToggleBGM(bool isOn)
        {
            m_BGMToggle.transform.Find("Toggle_On").gameObject.SetActive(isOn);
            m_BGMToggle.transform.Find("Toggle_Off").gameObject.SetActive(!isOn);
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetActiveToggleSFX(bool isOn)
        {
            m_SFXToggle.transform.Find("Toggle_On").gameObject.SetActive(isOn);
            m_SFXToggle.transform.Find("Toggle_Off").gameObject.SetActive(!isOn);
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetLanguageDropdown()
        {
            switch (SystemConfig.LanguageCode)
            {
                case "en":
                    m_Language.SetValueWithoutNotify(0);
                    break;
                case "th":
                    m_Language.SetValueWithoutNotify(1);
                    break;
            }
        }
    }
}
