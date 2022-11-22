using System;
using NFT1Forge.OSY.Controller;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer
{
    public class InventorySlot : BaseObjectController
    {
        [SerializeField] private TextMeshProUGUI m_TextItemName;
        [SerializeField] private RawImage m_RawImageItem;
        [SerializeField] private Transform m_NftIconOn;
        [SerializeField] private Transform m_ActiveIconOn;
        [Header("BG Sprite")]
        [SerializeField] private Sprite m_SpriteNormal;
        [SerializeField] private Sprite m_SpriteSelected;

        public Action<string, NFTCollectionType> OnSelected;
        public string TokenId { get; private set; }
        public NFTCollectionType Type { get; private set; }

        private Image m_ImageBackground;

        /// <summary>
        /// Called on first frame
        /// </summary>
        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnSelectButton);
        }
        /// <summary>
        /// Setup data and subscript to InventorySystem event
        /// </summary>
        /// <param name="itemName"></param>
        public void Setup(string tokenId, string itemName, string fileName, bool isNFT, bool isActive, bool isSelected, Action<string, NFTCollectionType> selectedCallback, NFTCollectionType type)
        {
            TokenId = tokenId;
            Type = type;
            m_TextItemName.text = itemName;
            if (!string.IsNullOrEmpty(fileName))
                AssetCacheManager.I.SetTextureToRawImage(m_RawImageItem, fileName);
            OnSelected = selectedCallback;
            gameObject.SetActive(true);
            m_NftIconOn.SetActive(isNFT);
            m_ActiveIconOn.SetActive(isActive);
            if (null == m_ImageBackground)
                m_ImageBackground = GetComponent<Image>();
            m_ImageBackground.sprite = isSelected ? m_SpriteSelected : m_SpriteNormal;
            InventorySystem.I.OnItemActivated += OnItemActivated;
            InventorySystem.I.OnItemUpdated += OnItemUpdated;
        }
        /// <summary>
        /// On self clicked callback
        /// </summary>
        private void OnSelectButton()
        {
            OnSelected?.Invoke(TokenId, Type);
        }
        /// <summary>
        /// Update active status
        /// </summary>
        private void OnItemActivated(string tokenId, NFTCollectionType type)
        {
            if (Type == type)
                m_ActiveIconOn.SetActive(TokenId == tokenId);
        }
        /// <summary>
        /// Update icon status
        /// </summary>
        private void OnItemUpdated(NftMetadataModel item)
        {
            if (TokenId == item.token_id)
                m_NftIconOn.SetActive(item.is_nft);
        }
        /// <summary>
        /// Reset data
        /// </summary>
        public void Reset()
        {
            TokenId = string.Empty;
            m_TextItemName.text = string.Empty;
            OnSelected = null;
            m_NftIconOn.SetActive(false);
            m_ActiveIconOn.SetActive(false);
            InventorySystem.I.OnItemActivated -= OnItemActivated;
            InventorySystem.I.OnItemUpdated -= OnItemUpdated;
        }
    }
}