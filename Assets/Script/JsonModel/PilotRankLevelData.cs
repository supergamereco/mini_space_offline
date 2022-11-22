using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PilotRankLevelData
    {
        public ushort id;
        public string rank;
        public ushort level;
        public ushort upgrade_cost;
        public float gold_booster;
        public float weapon_charge_speed;
    }
}
