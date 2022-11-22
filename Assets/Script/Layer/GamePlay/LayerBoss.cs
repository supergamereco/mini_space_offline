using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer.Gameplay
{
    public class LayerBoss : BaseLayer
    {
        [SerializeField] private Image m_HPbar;
        private float m_MaxHP;
        private float m_CurrentHP;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hp"></param>
        public void SetHp(float hp)
        {
            m_HPbar.fillAmount = 1;
            m_MaxHP = hp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hp"></param>
        public void OnHpChanged(float hp)
        {
            m_CurrentHP = (hp / m_MaxHP);
            m_HPbar.fillAmount = m_CurrentHP;
        }
    }
}
