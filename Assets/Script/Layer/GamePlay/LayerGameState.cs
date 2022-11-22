using UnityEngine;
using TMPro;
using NFT1Forge.OSY.System;

namespace NFT1Forge.OSY.Layer.Gameplay
{
    public class LayerGameState : BaseLayer
    {
        [SerializeField] private Transform m_GetReady;
        [SerializeField] private TextMeshProUGUI m_TextGetReady;
        [SerializeField] private Transform m_GameOver;
        [SerializeField] private Transform m_BossWarning;

        /// <summary>
        /// 
        /// </summary>
        public void ShowGetReady(string levelName)
        {
            m_TextGetReady.text = $"{LocalizationSystem.I.GetLocalizeValue("GAME_STATE_SCREEN_ENTERING_MAP")} {levelName}";
            m_GetReady.SetActive(true);
            m_GameOver.SetActive(false);
            m_BossWarning.SetActive(false);
        }
        /// <summary>
        /// 
        /// </summary>
        public void ShowGameOver()
        {
            m_GetReady.SetActive(false);
            m_GameOver.SetActive(true);
            m_BossWarning.SetActive(false);
        }
        /// <summary>
        /// 
        /// </summary>
        public void ShowBossWarning()
        {
            m_GetReady.SetActive(false);
            m_GameOver.SetActive(false);
            m_BossWarning.SetActive(true);
        }
        /// <summary>
        /// 
        /// </summary>
        public void HideGameState()
        {
            m_GetReady.SetActive(false);
            m_GameOver.SetActive(false);
            m_BossWarning.SetActive(false);
        }
    }
}
