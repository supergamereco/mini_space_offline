using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class GetItemResponseModel : BaseResponseModel
    {
        public GetItemDataModel data;

        [Serializable]
        public class GetItemDataModel
        {
            public string account_id;
            public uint gold_amount;
            public ushort current_step;
            public ObjectListModel object_list;
        }
    }
}
