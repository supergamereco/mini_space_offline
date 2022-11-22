using System;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer.Gameplay
{
    public class LayerHowToControl : BaseLayer
    {
        [SerializeField] private Button m_ButtonClose;

        public Action OnClosed;

        /// <summary>
        /// Called right after the layer was created
        /// </summary>
        public override void Initialize()
        {
            Debug.Log("HOW TO INIT");
            base.Initialize();
            m_ButtonClose.onClick.AddListener(OnButtonClose);
        }
        /// <summary>
        /// ButtonClose clicked callback
        /// </summary>
        private void OnButtonClose()
        {
            OnClosed?.Invoke();
            HideLayer();
        }
    }
}
