using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class GameLevelData
    {
        public int level;
        public float enemy_attack;
        public float enemy_armor;
        public float gold_multiplier;
        public float score_multiplier;
        public int score_per_second;
    }
}
