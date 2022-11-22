using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class AddItemRequestModel : BaseRequestModel
    {
        public List<ItemData> item_list = new List<ItemData>();

        [Serializable]
        public class ItemData
        {
            public ushort type;
            public ushort value;

            public ItemData(ushort type, ushort value)
            {
                this.type = type;
                this.value = value;
            }
        }
    }
}
