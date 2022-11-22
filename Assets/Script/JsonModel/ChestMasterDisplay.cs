using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ChestMasterDisplay : BaseJsonModel
    {
        public List<ChestMasterDisplayData> chest_master_display;
    }
}
