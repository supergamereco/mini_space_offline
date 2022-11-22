using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PassMaster : BaseJsonModel
    {
        public List<PassMasterData> pass_master;
    }
}
