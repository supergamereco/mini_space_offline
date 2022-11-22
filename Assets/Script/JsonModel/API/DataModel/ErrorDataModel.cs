using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ErrorDataModel
    {
        public long status;

        public ErrorDataModel()
        {
        }

        public ErrorDataModel(long status)
        {
            this.status = status;
        }
    }
}
