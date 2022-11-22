using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer.Gameplay
{

    public class LayerGameTime : BaseLayer
    {
        [SerializeField] private TextMeshProUGUI m_TextPilotName;
        [SerializeField] private Image m_HPbar;
        [SerializeField] private Image m_PlayerAvatar;
        [SerializeField] private Image m_RapidFireIcon;
        [SerializeField] private Image m_MagnetIcon;
        private float m_MaxHP;
        private float m_CurrentHP;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="energy"></param>
        public void SetMaxTime(float energy)
        {
            m_MaxHP = energy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="energy"></param>
        public void OnTimeChanged(float energy)
        {
            m_CurrentHP = (energy /m_MaxHP);
            m_HPbar.fillAmount = m_CurrentHP;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isMagnet"></param>
        public void OnMagnetBuff(bool isMagnet, float duration)
        {
            m_MagnetIcon.gameObject.SetActive(isMagnet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isRapidFire"></param>
        public void OnRapidFireBuff(bool isRapidFire, float duration)
        {
            m_RapidFireIcon.gameObject.SetActive(isRapidFire);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="energy"></param>
        public void LoadPilotAndSpaceship()
        {
            PilotDataModel pilot = InventorySystem.I.GetActivePilot();
            SpaceshipDataModel spaceship = InventorySystem.I.GetActiveSpaceship();
            AssetCacheManager.I.SetSprite(m_PlayerAvatar, pilot.GetProfileImagePath());
            m_TextPilotName.text = $"{pilot.name} : {spaceship.name}";
        }
    }
}
