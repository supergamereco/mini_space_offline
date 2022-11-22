using System;
using NFT1Forge.OSY.JsonModel;
using Script.JsonModel.API.DataModel;

namespace Script.JsonModel.API
{
    [Serializable]
    public class GetUserCurrencyDataModel: BaseResponseModel
    {
        public UserCurrencyDataModel data;
    }
}