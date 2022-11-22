using UnityEngine;
using TMPro;

namespace NFT1Forge.OSY.Layer
{
    public class LayerLoading : BaseLayer
    {
        [SerializeField] private TextMeshProUGUI m_TextProgress;
        [SerializeField] private TextMeshProUGUI m_TextMessage;
        [SerializeField] private float m_ProgressSpeed = 2f;

        private float m_TargetProgress = 0f;
        private float m_CurrentProgress = 0f;
        private bool m_IsUpdate = false;

        #region getter
        public bool IsAnimationFinish => !m_IsUpdate;
        #endregion

        /// <summary>
        /// Reset progress and show layer
        /// </summary>
        public void Show()
        {
            m_TargetProgress = 0f;
            m_CurrentProgress = 0f;
            if (null != m_TextProgress)
                m_TextProgress.text = "00.00%";
            gameObject.SetActive(true);
        }
        /// <summary>
        /// Hide layer
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        /// <summary>
        /// Update loading progress
        /// </summary>
        /// <param name="progress"></param>
        public void UpdateProgress(float progress)
        {
            m_TargetProgress = progress;
            m_IsUpdate = true;
        }
        /// <summary>
        /// Show message on layer
        /// </summary>
        /// <param name="message"></param>
        public void SetMessage(string message)
        {
            m_TextMessage.text = message;
        }
        /// <summary>
        /// Show or hide progress number
        /// </summary>
        /// <param name="isShow"></param>
        public void ShowProgress(bool isShow)
        {
            m_TextProgress.gameObject.SetActive(isShow);
        }
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            if (!m_IsUpdate) return;

            m_CurrentProgress += (m_ProgressSpeed * Time.deltaTime);
            if (m_CurrentProgress > m_TargetProgress)
            {
                m_CurrentProgress = m_TargetProgress;
                m_IsUpdate = false;
            }
            if (null != m_TextProgress)
                m_TextProgress.text = $"{m_CurrentProgress:0.00} %";
        }
    }
}
