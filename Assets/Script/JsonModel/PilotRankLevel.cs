using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PilotRankLevel : BaseJsonModel
    {
        public List<PilotRankLevelData> pilot_rank_level;
    }
}
