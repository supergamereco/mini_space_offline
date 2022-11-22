using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ServerConfigData
    {
        public ushort id;
        public string name;
        public string base_1sync_url;
        public string base_api_url;
    }
}
