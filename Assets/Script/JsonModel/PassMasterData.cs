using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PassMasterData
    {
        public ushort id;
        public string name;
        public string description;
        public string type;
        public ushort max_play;
        public string unlock_condition;
        public short unlock_value;
        public string image;
    }
}
