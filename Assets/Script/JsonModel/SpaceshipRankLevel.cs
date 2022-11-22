using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SpaceshipRankLevel : BaseJsonModel
    {
        public List<SpaceshipRankLevelData> spaceship_rank_level;
    }
}
