using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ItemMaster : BaseJsonModel
    {
        public List<ItemMasterData> item_master;
    }
}
