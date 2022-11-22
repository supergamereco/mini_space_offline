using NFT1Forge.OSY.JsonModel;
using System;

public class PlayerMissionStepResponseModel : BaseResponseModel
{
    public PlayerMissionStepDataModel data;

    [Serializable]
    public class PlayerMissionStepDataModel
    {
        public ushort next_step;
    }
}
