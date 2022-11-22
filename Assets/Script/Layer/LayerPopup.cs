using System;
using NFT1Forge.OSY.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer
{
    public class LayerPopup : BaseLayer
    {
        [SerializeField] private Image m_ShowImage;
        [SerializeField] private TextMeshProUGUI m_TextMessage;
        [SerializeField] private Button m_ButtonOk;
        [SerializeField] private TextMeshProUGUI m_TextButton;

        public Action OnClosed;

        /// <summary>
        /// Initializing layer
        /// </summary>
        /// <returns></returns>
        public override void Initialize()
        {
            m_ButtonOk.onClick.AddListener(OnButtonCloseClicked);
        }
        /// <summary>
        /// Set up
        /// </summary>
        /// <param name="message"></param>
        public void Setup(string message, string urlImage = null, string TextButton = null)
        {
            if (null == TextButton)
                TextButton = LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_OK");

            if (null != urlImage)
                AssetCacheManager.I.SetSprite(m_ShowImage, urlImage);

            m_TextMessage.font = LocalizationSystem.I.GetFont();
            m_TextButton.font = LocalizationSystem.I.GetFont();
            m_TextMessage.text = message;
            m_TextButton.text = TextButton;
            m_ShowImage.gameObject.SetActive(null != urlImage);
        }
        /// <summary>
        /// 
        /// </summary>
        public void OnButtonCloseClicked()
        {
            OnClosed?.Invoke();
        }
    }
}
