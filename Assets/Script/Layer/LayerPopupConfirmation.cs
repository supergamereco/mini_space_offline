using System;
using NFT1Forge.OSY.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer
{
    public class LayerPopupConfirmation : BaseLayer
    {
        [SerializeField] private TextMeshProUGUI m_TextMessage;
        [SerializeField] private Button m_ButtonYes;
        [SerializeField] private Button m_ButtonNo;

        public Action OnClickedYes;
        public Action OnClickedNo;

        /// <summary>
        /// Initializing layer
        /// </summary>
        /// <returns></returns>
        public override void Initialize()
        {
            m_ButtonYes.onClick.AddListener(OnButtonYesClicked);
            m_ButtonNo.onClick.AddListener(OnButtonNoClicked);
        }
        /// <summary>
        /// Set up
        /// </summary>
        /// <param name="message"></param>
        public void Setup(string message)
        {
            m_TextMessage.text = message;
            m_TextMessage.font = LocalizationSystem.I.GetFont();
        }
        /// <summary>
        /// 
        /// </summary>
        public void OnButtonNoClicked()
        {
            OnClickedNo?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        public void OnButtonYesClicked()
        {
            OnClickedYes?.Invoke();
        }
    }
}
