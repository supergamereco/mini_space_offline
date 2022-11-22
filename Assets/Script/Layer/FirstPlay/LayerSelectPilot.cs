using System;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer
{
    public class LayerSelectPilot : BaseLayer
    {
        [SerializeField] private Button m_ButtonPilot1;
        [SerializeField] private Button m_ButtonPilot2;
        [SerializeField] private TextMeshProUGUI m_TextPilot1;
        [SerializeField] private TextMeshProUGUI m_TextPilot2;
        [SerializeField] private RawImage m_RawImagePilot1;
        [SerializeField] private RawImage m_RawImagePilot2;

        public Action OnSelectPilot1;
        public Action OnSelectPilot2;

        /// <summary>
        /// Initialize layer
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            m_ButtonPilot1.onClick.AddListener(OnButtonPilot1);
            m_ButtonPilot2.onClick.AddListener(OnButtonPilot2);

            //HARD CODE because we run out of time
            //Change it to scriptable or metadata later
            m_RawImagePilot1.texture = AssetCacheManager.I.GetTextureImmediately($"{SystemConfig.BaseAssetPath}rendered/pilot1_p_11.png");
            m_RawImagePilot2.texture = AssetCacheManager.I.GetTextureImmediately($"{SystemConfig.BaseAssetPath}rendered/pilot2_p_11.png");
            //StartCoroutine(AssetCacheManager.I.LoadTextureFromURL($"{SystemConfig.BaseAssetPath}rendered/pilot1_p_11.png", (texture) =>
            //{
            //    m_RawImagePilot1.texture = texture;
            //}
            //));
            //StartCoroutine(AssetCacheManager.I.LoadTextureFromURL($"{SystemConfig.BaseAssetPath}rendered/pilot2_p_11.png", (texture) =>
            //{
            //    m_RawImagePilot2.texture = texture;
            //}
            //));
            PilotMasterData pilot1 = DatabaseSystem.I.GetMetadata<PilotMaster>().pilot_master.Find(a => a.id.Equals(1));
            PilotMasterData pilot2 = DatabaseSystem.I.GetMetadata<PilotMaster>().pilot_master.Find(a => a.id.Equals(2));
            m_TextPilot1.text = pilot1.name;
            m_TextPilot2.text = pilot2.name;
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnButtonPilot1()
        {
            OnSelectPilot1?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnButtonPilot2()
        {
            OnSelectPilot2?.Invoke();
        }
    }
}