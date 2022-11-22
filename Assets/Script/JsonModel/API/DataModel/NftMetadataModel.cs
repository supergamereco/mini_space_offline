using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class NftMetadataModel
    {
        public string token_id;
        public string name;
        public string image;
        public string description;
        public int status;
        public bool is_lock;
        public bool is_nft;
        public bool is_active;
        public bool is_minting;
        public bool is_open;
        public NftMetadataModel()
        {
        }

        public NftMetadataModel(string tokenId, string name)
        {
            token_id = tokenId;
            this.name = name;
            image = "";
            description = $"{name} #{tokenId}";
            status = 1;
            is_lock = false;
            is_nft = false;
            is_active = false;
            is_minting = false;
            is_open = false;
        }
    }
}
