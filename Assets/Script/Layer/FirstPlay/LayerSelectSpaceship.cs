using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;

namespace NFT1Forge.OSY.Layer
{
    public class LayerSelectSpaceship : BaseLayer
    {
        [SerializeField] private Button m_ButtonSpaceship1;
        [SerializeField] private Button m_ButtonSpaceship2;
        [SerializeField] private TextMeshProUGUI m_TextSpaceship1;
        [SerializeField] private TextMeshProUGUI m_TextSpaceship2;
        [SerializeField] private RawImage m_RawImageSpaceship1;
        [SerializeField] private RawImage m_RawImageSpaceship2;

        public Action OnSelectSpaceship1;
        public Action OnSelectSpaceship2;

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            m_ButtonSpaceship1.onClick.AddListener(OnButtonSpaceship1);
            m_ButtonSpaceship2.onClick.AddListener(OnButtonSpaceship2);

            //HARD CODE because we run out of time
            //Change it to scriptable or metadata later
            m_RawImageSpaceship1.texture = AssetCacheManager.I.GetTextureImmediately($"{SystemConfig.BaseAssetPath}rendered/spaceship_01.png");
            m_RawImageSpaceship2.texture = AssetCacheManager.I.GetTextureImmediately($"{SystemConfig.BaseAssetPath}rendered/spaceship_02.png");
            //StartCoroutine(AssetCacheManager.I.LoadTextureFromURL($"{SystemConfig.BaseAssetPath}rendered/spaceship_01.png", (texture) =>
            //{
            //    m_RawImageSpaceship1.texture = texture;
            //}
            //));
            //StartCoroutine(AssetCacheManager.I.LoadTextureFromURL($"{SystemConfig.BaseAssetPath}rendered/spaceship_02.png", (texture) =>
            //{
            //    m_RawImageSpaceship2.texture = texture;
            //}
            //));
            SpaceshipMasterData spaceship1 = DatabaseSystem.I.GetMetadata<SpaceshipMaster>().spaceship_master.Find(a => a.id.Equals(1));
            SpaceshipMasterData spaceship2 = DatabaseSystem.I.GetMetadata<SpaceshipMaster>().spaceship_master.Find(a => a.id.Equals(2));
            m_TextSpaceship1.text = spaceship1.name;
            m_TextSpaceship2.text = spaceship2.name;
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnButtonSpaceship1()
        {
            OnSelectSpaceship1?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnButtonSpaceship2()
        {
            OnSelectSpaceship2?.Invoke();
        }
    }
}
