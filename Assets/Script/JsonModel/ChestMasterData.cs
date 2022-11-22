using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ChestMasterData
    {
        public ushort id;
        public string name;
        public string description;
        public string rank;
        public ushort reward_id;
        public string mission_type;
        public short mission;
        public ushort evo_to;
        public string image;
    }
}
