using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer.Gameplay
{
    public class LayerGameGold : BaseLayer
    {
        [SerializeField] private TextMeshProUGUI m_Gold;
        [SerializeField] private Image m_TextGold;
        private uint m_CurrentGold;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gold"></param>
        public void OnGoldChanged(uint gold)
        {
            if (m_CurrentGold == 0 && gold >= 10)
            {
                Vector3 posx = new Vector3(-24, 0, 0);
                m_TextGold.transform.position = m_TextGold.transform.position + posx;
                m_CurrentGold = 1;
            }
            if (m_CurrentGold != 0 && gold >= m_CurrentGold * 10)
            {
                Vector3 posx = new Vector3(-24, 0, 0);
                m_CurrentGold = m_CurrentGold * 10;
                m_TextGold.transform.position = m_TextGold.transform.position + posx;
            }
            m_Gold.text = $" {gold}";
        }
    }
}
