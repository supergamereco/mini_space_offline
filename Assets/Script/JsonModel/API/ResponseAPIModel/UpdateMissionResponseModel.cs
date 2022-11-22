using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class UpdateMissionResponseModel : BaseResponseModel
    {
        public NextMissionModel data;

        [Serializable]
        public class NextMissionModel
        {
            public ushort next_step;
        }
    }
}
