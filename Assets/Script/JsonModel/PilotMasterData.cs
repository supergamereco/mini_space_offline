using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PilotMasterData
    {
        public ushort id;
        public string name;
        public string description;
        public string rank;
        public string asset_path;
        public string profile_image_prefix;
        public string full_image_prefix;
    }
}