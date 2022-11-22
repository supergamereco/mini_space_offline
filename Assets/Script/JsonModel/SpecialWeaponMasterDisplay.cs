using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SpecialWeaponMasterDisplay : BaseJsonModel
    {
        public List<SpecialWeaponMasterDisplayData> weapon_master_display;
    }
}
