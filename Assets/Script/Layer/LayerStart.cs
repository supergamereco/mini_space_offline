using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer
{
    public class LayerStart : BaseLayer
    {
        [SerializeField] private TextMeshProUGUI m_TextPressStart;
        [SerializeField] private Button m_ButtonPressStart;
        [SerializeField] private TextMeshProUGUI m_TextMessage;

        public Action OnPressStart;

        /// <summary>
        /// Called right after the layer was created
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            m_ButtonPressStart.onClick.AddListener(OnButtonPressStart);
        }
        /// <summary>
        /// Press anywhere on screen
        /// </summary>
        private void OnButtonPressStart()
        {
            OnPressStart?.Invoke();
        }
        /// <summary>
        /// Show message on layer
        /// </summary>
        public void SetMessage(string message)
        {
            m_TextMessage.text = message;
        }
        /// <summary>
        /// Ready to go
        /// </summary>
        public void Ready()
        {
            m_TextPressStart.gameObject.SetActive(true);
            m_ButtonPressStart.gameObject.SetActive(true);
            m_TextMessage.gameObject.SetActive(false);
        }
    }
}