using System;
using System.Collections;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.Layer;
using UnityEngine;

namespace NFT1Forge.OSY.System
{
    public class LayerSystem : Singleton<LayerSystem>
    {
        public bool IsInitialized { get; private set; } = false;
        private Canvas m_Canvas;
        private Transform m_NormalLayer;
        private Transform m_ImportantLayer;
        private Transform m_TopLayer;
        private LayerPopup m_LayerPopup;
        private LayerPopupConfirmation m_LayerPopupConfirmationDialogue;
        private LayerLoading m_LayerLoading;
        private LayerSoftLoading m_LayerSoftLoading;
        private AudioSource m_Sfx;
        private Action m_PopupClosedCallback;
        private Action m_PopupCancleDialogueCallback;
        private Action m_PopupConfirmationClickedYes;
        private Action m_PopupConfirmationClickedNo;
        private AudioClip m_SfxBack;

        /// <summary>
        /// Initialize system
        /// </summary>
        public IEnumerator Initialize(GameObject canvasObject)
        {
            yield return null;
            GameObject canvas = Instantiate(canvasObject, transform);
            m_Canvas = canvas.GetComponent<Canvas>();
            m_SfxBack = Resources.Load<AudioClip>("SFX/button-back");
            m_Sfx = GetComponentInChildren<Canvas>().GetComponent<AudioSource>();
            if (null != m_Canvas)
            {
                m_NormalLayer = m_Canvas.transform.Find("NormalLayer");
                m_ImportantLayer = m_Canvas.transform.Find("ImportantLayer");
                m_TopLayer = m_Canvas.transform.Find("TopLayer");
                if (null != m_NormalLayer && null != m_ImportantLayer && null != m_TopLayer)
                    IsInitialized = true;
            }
        }
        /// <summary>
        /// Show pop up dialog, create new one if not exist
        /// </summary>
        /// <param name="message"></param>
        public void ShowPopupDialog(string message, Action closeCallback = null, string urlImage = null, string TextButton = null)
        {
            if (null == m_LayerPopup)
                CreatePopupDialog();
            m_LayerPopup.Setup(message, urlImage,TextButton);
            m_LayerPopup.ShowLayer();
            m_PopupClosedCallback = closeCallback ?? null;

        }
        /// <summary>
        /// Create LayerPopup
        /// </summary>
        private void CreatePopupDialog()
        {
            LayerPopup prefab = Resources.Load<LayerPopup>("Layer/LayerPopup");
            m_LayerPopup = Instantiate(prefab, m_TopLayer);
            m_LayerPopup.Initialize();
            m_LayerPopup.OnClosed = HidePopupDialog;
        }
        /// <summary>
        /// LayerPopup Onclosed callback
        /// </summary>
        public void HidePopupDialog()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxBack);
            if (null != m_LayerPopup)
            {
                m_LayerPopup.HideLayer();
                m_PopupClosedCallback?.Invoke();
                m_PopupClosedCallback = null;
            }
        }
        /// <summary>
        /// Show pop up dialog, create new one if not exist
        /// </summary>
        /// <param name="message"></param>
        public void ShowPopupConfirmationDialog(string message, Action clickedYesCallback = null, Action clickedNoCallback = null)
        {
            if (null == m_LayerPopupConfirmationDialogue)
                CreatePopupConfirmationDialog();
            m_LayerPopupConfirmationDialogue.Setup(message);
            m_LayerPopupConfirmationDialogue.ShowLayer();
            if (null != clickedNoCallback)
                m_PopupConfirmationClickedNo = clickedNoCallback;
            if (null != clickedYesCallback)
                m_PopupConfirmationClickedYes = clickedYesCallback;
        }
        /// <summary>
        /// Create LayerPopup
        /// </summary>
        private void CreatePopupConfirmationDialog()
        {
            LayerPopupConfirmation prefab = Resources.Load<LayerPopupConfirmation>("Layer/LayerPopupConfirmation");
            m_LayerPopupConfirmationDialogue = Instantiate(prefab, m_TopLayer);
            m_LayerPopupConfirmationDialogue.Initialize();
            m_LayerPopupConfirmationDialogue.OnClickedNo = CancelPopupConfirmationDialog;
            m_LayerPopupConfirmationDialogue.OnClickedYes = ConfirmPopupConfirmationDialog;
        }
        /// <summary>
        /// LayerPopup OnClieckedNo callback
        /// </summary>
        public void CancelPopupConfirmationDialog()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxBack);
            if (null != m_LayerPopupConfirmationDialogue)
            {
                m_LayerPopupConfirmationDialogue.HideLayer();
                m_PopupConfirmationClickedNo?.Invoke();
                m_PopupConfirmationClickedNo = null;
            }
        }
        /// <summary>
        /// LayerPopup OnClickedYes callback
        /// </summary>
        public void ConfirmPopupConfirmationDialog()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxBack);
            if (null != m_LayerPopupConfirmationDialogue)
            {
                m_LayerPopupConfirmationDialogue.HideLayer();
                m_PopupConfirmationClickedYes?.Invoke();
                m_PopupConfirmationClickedYes = null;
            }
        }
        /// <summary>
        /// A big screen that block everything behide and show loading progress
        /// </summary>
        public void ShowLoadingScreen(bool isShowProgress = true)
        {
            if (null == m_LayerLoading)
                CreateLoadingScreen();
            m_LayerLoading.ShowProgress(isShowProgress);
            m_LayerLoading.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        private void CreateLoadingScreen()
        {
            LayerLoading prefab = Resources.Load<LayerLoading>("Layer/LayerLoading");
            m_LayerLoading = Instantiate(prefab, m_TopLayer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public LayerLoading GetLoadingScreen()
        {
            if (null == m_LayerLoading)
                CreateLoadingScreen();
            return m_LayerLoading;
        }
        /// <summary>
        /// Hide loading screen
        /// </summary>
        public void HideLoadingScreen()
        {
            if (null != m_LayerLoading)
                m_LayerLoading.Hide();
        }
        /// <summary>
        /// A big screen that block everything behide and show loading progress
        /// </summary>
        public void ShowSoftLoadingScreen()
        {
            if (null == m_LayerSoftLoading)
                CreateSoftLoadingScreen();
            m_LayerSoftLoading.ShowLayer();
        }
        /// <summary>
        /// 
        /// </summary>
        private void CreateSoftLoadingScreen()
        {
            LayerSoftLoading prefab = Resources.Load<LayerSoftLoading>("Layer/LayerSoftLoading");
            m_LayerSoftLoading = Instantiate(prefab, m_TopLayer);
        }
        /// <summary>
        /// Hide loading screen
        /// </summary>
        public void HideSoftLoadingScreen()
        {
            if (null != m_LayerSoftLoading)
                m_LayerSoftLoading.HideLayer();
        }
        /// <summary>
        /// Create UI layer as child object of ui holder transform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public T CreateLayer<T>(string layerName, LayerType layerType, Action<T> callback) where T : BaseLayer
        {
            T layer = null;
            switch (layerType)
            {
                case LayerType.NormalLayer:
                    layer = CreateLayer<T>(layerName, m_NormalLayer);
                    break;
                case LayerType.ImportantLayer:
                    layer = CreateLayer<T>(layerName, m_ImportantLayer);
                    break;
                case LayerType.TopLayer:
                    layer = CreateLayer<T>(layerName, m_TopLayer);
                    break;
            }
            callback?.Invoke(layer);
            return layer;
        }
        /// <summary>
        /// Instantiate UI prefab
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layerName"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private T CreateLayer<T>(string layerName, Transform parent) where T : BaseLayer
        {
            T res = Resources.Load<T>($"Layer/{layerName}");
            T layer = Instantiate(res, parent);
            layer.Initialize();
            return layer;
        }
    }
}