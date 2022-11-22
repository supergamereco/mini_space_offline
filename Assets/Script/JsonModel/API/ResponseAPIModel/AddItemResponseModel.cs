using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class AddItemResponseModel : BaseResponseModel
    {
        public AddItemDataModel data;

        [Serializable]
        public class AddItemDataModel
        {
            public List<PilotDataModel> pilot;
            public List<SpaceshipDataModel> spaceship;
            public List<PassDataModel> survival_pass;
            public List<SpecialWeaponDataModel> special_weapon;
            public List<ChestDataModel> chest;
        }
    }
}
