using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class BaseMetadataRequestModel
    {
        public string tokenId;
        public string name;
        public string description;
        public string image;
    }
}