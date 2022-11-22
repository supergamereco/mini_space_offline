using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class FinishGameEventModel : BaseJsonModel
    {
        public string stage_type;
        public uint score;
        public uint gold;
        public bool got_chest;
        public ushort enemy_killed;
        public ushort boss_killed;
    }
}
