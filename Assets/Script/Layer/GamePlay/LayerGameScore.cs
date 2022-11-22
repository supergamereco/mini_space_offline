using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace NFT1Forge.OSY.Layer.Gameplay
{
    public class LayerGameScore : BaseLayer
    {
        [SerializeField] private TextMeshProUGUI m_Score;
        [SerializeField] private Image m_TextScore;
        private uint m_CurrentScore;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        public void OnScoreChanged(uint score)
        {
            if(m_CurrentScore == 0 && score >= 10)
            {
                Vector3 posx = new Vector3(-20, 0, 0);
                m_TextScore.transform.position = m_TextScore.transform.position + posx;
                m_CurrentScore = 1;
            }
            if (m_CurrentScore != 0 && score >= m_CurrentScore * 10)
            {
                Vector3 posx = new Vector3(-20, 0, 0);
                m_CurrentScore = m_CurrentScore * 10;
                m_TextScore.transform.position = m_TextScore.transform.position + posx;
            }
            m_Score.text = $" {score}";
        }
    }
}
