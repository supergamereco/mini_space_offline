using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ChestMaster : BaseJsonModel
    {
        public List<ChestMasterData> chest_master;
    }
}
