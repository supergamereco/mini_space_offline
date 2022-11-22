using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class OpenChestRequestModel : BaseRequestModel
    {
        public string token_id;
        public ushort reward_id;

        public OpenChestRequestModel(string tokenId, ushort rewardId)
        {
            token_id = tokenId;
            reward_id = rewardId;
        }
    }
}
