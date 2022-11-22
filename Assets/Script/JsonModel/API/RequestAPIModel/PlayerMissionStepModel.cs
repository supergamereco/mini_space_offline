using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PlayerMissionStepModel : BaseRequestModel
    {
        public ushort player_mission_step_id;

        public PlayerMissionStepModel(ushort player_mission_step_id)
        {
            this.player_mission_step_id = player_mission_step_id;
        }

    }
}
