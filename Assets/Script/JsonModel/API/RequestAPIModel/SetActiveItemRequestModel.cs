using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SetActiveItemRequestModel : BaseRequestModel
    {
        public string token_id;
        public ushort type;

        public SetActiveItemRequestModel(string tokenId, ushort type)
        {
            token_id = tokenId;
            this.type = type;
        }
    }
}
