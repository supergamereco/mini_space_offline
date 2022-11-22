using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class GameConfig : BaseJsonModel
    {
        public List<GameConfigData> game_config;
    }
}