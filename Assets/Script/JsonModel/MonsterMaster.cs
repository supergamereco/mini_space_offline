using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class MonsterMaster : BaseJsonModel
    {
        public List<MonsterMasterData> monster_master;
    }
}
