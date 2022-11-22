using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PilotUpgradeModel : BaseJsonModel
    {
        public string pilot_name;
        public ushort pilot_level;
    }
}
