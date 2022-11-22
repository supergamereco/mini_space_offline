using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class GameLevel : BaseJsonModel
    {
        public List<GameLevelData> game_level;
    }
}