using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SpaceshipRankLevelData
    {
        public ushort id;
        public string rank;
        public ushort level;
        public ushort upgrade_cost;
        public float damage;
        public ushort armor;
    }
}