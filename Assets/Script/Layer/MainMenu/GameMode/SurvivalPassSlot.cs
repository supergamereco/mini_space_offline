using System;
using NFT1Forge.OSY.Controller;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer
{
    public class SurvivalPassSlot : BaseObjectController
    {
        [SerializeField] private TextMeshProUGUI m_TextPassName;
        [SerializeField] private TextMeshProUGUI m_TextDescription;
        [SerializeField] private TextMeshProUGUI m_TextPlayCount;
        [SerializeField] private Transform m_ImageSelected;
        [SerializeField] private Image m_ImagePass;
        [SerializeField] private Image m_ImageProgress;
        public Action<string> OnSelected;

        private string m_TokenId;

        /// <summary>
        /// Setup data
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="selectedCallback"></param>
        public void Setup(PassDataModel pass, Action<string> selectedCallback)
        {
            //transform.localScale = Vector3.one;
            PassMasterDisplayData passMasterDisplay = DatabaseSystem.I.GetMetadata<PassMasterDisplay>().pass_master_display.Find(a => a.name.Equals(pass.name));
            m_TokenId = pass.token_id;
            m_TextPassName.text = pass.name;
            m_TextDescription.text = passMasterDisplay.description;
            m_TextPlayCount.text = $"{pass.playcount}/{pass.maxplay}";
            float progress = pass.playcount * 100 / pass.maxplay;
            m_ImageProgress.fillAmount = progress * 0.01f;
            OnSelected = selectedCallback;
            AssetCacheManager.I.SetSprite(m_ImagePass, pass.GetImagePath());
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnClicked()
        {
            OnSelected?.Invoke(m_TokenId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSelect"></param>
        public void SetSelect(string tokenId)
        {
            m_ImageSelected.SetActive(m_TokenId == tokenId);
        }
        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
        }
    }
}