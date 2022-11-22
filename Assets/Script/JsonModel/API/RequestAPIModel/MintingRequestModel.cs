using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class MintingRequestModel : BaseRequestModel
    {
        public string token_id;

        public MintingRequestModel(string tokenId)
        {
            token_id = tokenId;
        }
    }
}
