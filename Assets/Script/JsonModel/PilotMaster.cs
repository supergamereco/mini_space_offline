using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PilotMaster : BaseJsonModel
    {
        public List<PilotMasterData> pilot_master;
    }
}
