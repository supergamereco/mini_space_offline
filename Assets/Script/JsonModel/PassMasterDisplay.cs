using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PassMasterDisplay : BaseJsonModel
    {
        public List<PassMasterDisplayData> pass_master_display;
    }
}
