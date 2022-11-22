using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class UpdateMissionRequestModel : BaseRequestModel
    {
        public ushort step;
    }
}