using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class OpenChestEventModel : BaseJsonModel
    {
        public string nft_type;
        public string nft_name;
    }
}
