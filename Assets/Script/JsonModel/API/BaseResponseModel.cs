using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class BaseResponseModel
    {
        public bool success;
        public string message;
        public ErrorDataModel error;

        public BaseResponseModel()
        {
        }

        public BaseResponseModel(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }

        public BaseResponseModel(bool success, string message, long errorCode)
        {
            this.success = success;
            this.message = message;
            error = new ErrorDataModel(errorCode);
        }
    }
}
