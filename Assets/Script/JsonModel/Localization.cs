using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class Localization : BaseJsonModel
    {
        public List<LocalizationData> localization;
    }
}
