using System;
using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Manager;
using NFT1Forge.OSY.System;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer
{
    public class LayerGameMode : BaseLayer
    {
        [SerializeField] private Transform m_PanelGameMode;
        [SerializeField] private Transform m_PanelSurvivalPass;
        [SerializeField] private RectTransform m_PassContainer;
        [SerializeField] private Button m_ButtonClose;
        [SerializeField] private Button m_ButtonCloseSurvivalPass;
        [SerializeField] private Button m_ButtonNormalStage;
        [SerializeField] private Button m_ButtonSurvivalStage;
        [SerializeField] private Button m_ButtonPlay;
        [SerializeField] private Button m_NextButton;
        [SerializeField] private Button m_PreviousButton;
        [SerializeField] private Image m_ImageNormalMode;
        [SerializeField] private Image m_ImageSurivalMode;
        private AudioSource m_Sfx;

        [Header("SFX")]
        [SerializeField] private AudioClip m_SfxBack;

        public Action OnClosed;
        public Action OnPlayNormalStage;
        public Action OnPlaySurvivalStage;

        private string m_SelectedTokenId;

        private bool m_IsScrollRight;
        private bool m_IsScrollLeft;
        private float m_ScrollPosX = 0;

        private readonly List<SurvivalPassSlot> m_SlotList = new List<SurvivalPassSlot>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override void Initialize()
        {
            m_Sfx = GetComponent<AudioSource>();
            m_ButtonClose.onClick.AddListener(OnCloseButton);
            m_ButtonCloseSurvivalPass.onClick.AddListener(OnCloseSurvivalStageButton);
            m_ButtonNormalStage.onClick.AddListener(OnNormalStageButton);
            m_ButtonSurvivalStage.onClick.AddListener(OnSurvivalStageButton);
            m_ButtonPlay.onClick.AddListener(() => StartCoroutine(OnPlaySurvival()));
            m_NextButton.onClick.AddListener(OnNextButton);
            m_PreviousButton.onClick.AddListener(OnPreviousButton);
            ShowPanelGameMode();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnCloseButton()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxBack);
            OnClosed?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void ShowPanelGameMode()
        {
            AssetCacheManager.I.SetUISprite("NORMAL_STAGE", "ui", m_ImageNormalMode);
            AssetCacheManager.I.SetUISprite("SURVIVAL_STAGE", "ui", m_ImageSurivalMode);
            m_PanelGameMode.SetActive(true);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnNormalStageButton()
        {
            OnPlayNormalStage?.Invoke();
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.PageAction(((int)PageActionType.EnterGameplayScreen).ToString(), "");
            }
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnSurvivalStageButton()
        {
            m_PanelSurvivalPass.SetActive(true);
            m_PanelGameMode.SetActive(false);
            for (int i = 0; i < InventorySystem.I.PassCount; i++)
            {
                PassDataModel pass = InventorySystem.I.GetPassByIndex(i);
                if (pass.playcount < pass.maxplay)
                {
                    CreatePassSlot(pass);
                }
            }

#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.PageAction(((int)PageActionType.EnterSurvivalModeSceen).ToString(), "");
            }
#endif
        }
        /// <summary>
        /// Create a survival pass slot
        /// </summary>
        private void CreatePassSlot(PassDataModel pass)
        {
            SurvivalPassSlot slot = ObjectPoolManager.I.GetObject<SurvivalPassSlot>(ObjectType.UI, "Layer/SurvivalPassSlot", m_PassContainer);
            slot.transform.localScale = Vector3.one;
            slot.Setup(pass, OnPassSelected);
            m_SlotList.Add(slot);
        }
        /// <summary>
        /// Close survival panel
        /// </summary>
        private void OnCloseSurvivalStageButton()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxBack);
            m_PanelGameMode.SetActive(true);
            m_PanelSurvivalPass.SetActive(false);
            for (int i = 0; i < m_SlotList.Count; i++)
            {
                ObjectPoolManager.I.ReturnToPool(m_SlotList[i], ObjectType.UI);
            }
            m_SlotList.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        private void OnPassSelected(string tokenId)
        {
            m_SelectedTokenId = tokenId;
            for (int i = 0; i < m_SlotList.Count; i++)
            {
                m_SlotList[i].SetSelect(tokenId);
                StartCoroutine(InventorySystem.I.SetActivePass(tokenId));
            }
            m_ButtonPlay.interactable = true;
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.PageAction(((int)PageActionType.SelectSurvivalPass).ToString(), "");
            }
#endif
        }
        /// <summary>
        /// CloseWardrobe
        /// </summary>
        private IEnumerator OnPlaySurvival()
        {
            LayerSystem.I.ShowSoftLoadingScreen();
            CoroutineWithData api = new CoroutineWithData(this, InventorySystem.I.IncreaseSurvivalPassPlayCount(m_SelectedTokenId));
            yield return api.coroutine;
            if ((bool)api.result)
            {
#if UNITY_WEBGL
                if (BuildType.WebPlayer == SystemConfig.BuildType)
                {
                    WebBridgeUtils.PageAction(((int)PageActionType.EnterGameplayScreen).ToString(), "");
                }
#endif
                m_PanelSurvivalPass.SetActive(false);
                OnPlaySurvivalStage?.Invoke();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnNextButton()
        {
            m_IsScrollRight = true;
            m_IsScrollLeft = false;
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnPreviousButton()
        {
            m_IsScrollLeft = true;
            m_IsScrollRight = false;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ScrollRight(float posx)
        {
            m_PassContainer.anchoredPosition = new Vector2(m_PassContainer.anchoredPosition.x + posx, m_PassContainer.anchoredPosition.y);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ScrollLeft(float posx)
        {
            m_PassContainer.anchoredPosition = new Vector2(m_PassContainer.anchoredPosition.x + posx, m_PassContainer.anchoredPosition.y);
        }
        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (m_IsScrollRight)
            {
                float posx = 0;
                if (m_PassContainer.anchoredPosition.x <= m_ScrollPosX - 200)
                {
                    m_IsScrollRight = false;
                    m_ScrollPosX = m_PassContainer.anchoredPosition.x;
                }
                posx = (posx - Time.deltaTime) * 500;
                ScrollRight(posx);
            }
            else if (m_IsScrollLeft)
            {
                float posx = 0;
                if (m_PassContainer.anchoredPosition.x >= m_ScrollPosX + 200)
                {
                    m_IsScrollLeft = false;
                    m_ScrollPosX = m_PassContainer.anchoredPosition.x;
                }
                posx = (posx + Time.deltaTime) * 500;
                ScrollLeft(posx);
            }
        }
    }
}
