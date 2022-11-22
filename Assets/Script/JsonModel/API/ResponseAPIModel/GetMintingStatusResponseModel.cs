using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class GetMintingStatusResponseModel : BaseResponseModel
    {
        public GetMintingStatusModel data;

        [Serializable]
        public class GetMintingStatusModel
        {
            public ushort status;
        }
    }
}
