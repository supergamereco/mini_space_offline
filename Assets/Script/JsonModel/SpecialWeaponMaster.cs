using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SpecialWeaponMaster : BaseJsonModel
    {
        public List<SpecialWeaponMasterData> weapon_master;
    }
}
