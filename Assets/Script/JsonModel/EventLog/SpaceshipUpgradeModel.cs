using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SpaceshipUpgradeModel : BaseJsonModel
    {
        public string spaceship_name;
        public ushort spaceship_level;
    }
}
