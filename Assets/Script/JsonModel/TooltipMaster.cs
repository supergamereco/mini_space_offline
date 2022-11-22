using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class TooltipMaster : BaseJsonModel
    {
        public List<TooltipMasterData> tooltips;
    }
}

