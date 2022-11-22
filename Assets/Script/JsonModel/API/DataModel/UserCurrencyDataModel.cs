using System;
using NFT1Forge.OSY.JsonModel;

namespace Script.JsonModel.API.DataModel
{
    [Serializable]
    public class UserCurrencyDataModel: BaseRequestModel
    {
        public uint gold_amount;
    }
}
