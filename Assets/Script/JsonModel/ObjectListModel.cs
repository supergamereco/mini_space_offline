using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ObjectListModel
    {
        public List<PilotDataModel> pilot;
        public List<SpaceshipDataModel> spaceship;
        public List<PassDataModel> survival_pass;
        public List<SpecialWeaponDataModel> special_weapon;
        public List<ChestDataModel> chest;
    }
}
