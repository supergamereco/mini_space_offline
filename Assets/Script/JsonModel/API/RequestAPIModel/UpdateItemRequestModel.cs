using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class UpdateItemRequestModel : BaseRequestModel
    {
        public UpdateList update_list = new UpdateList();

        [Serializable]
        public class UpdateList
        {
            public List<PilotDataModel> pilot = new List<PilotDataModel>();
            public List<SpaceshipDataModel> spaceship = new List<SpaceshipDataModel>();
            public List<SpecialWeaponDataModel> special_weapon = new List<SpecialWeaponDataModel>();
            public List<PassDataModel> survival_pass = new List<PassDataModel>();
            public List<ChestDataModel> chest = new List<ChestDataModel>();
        }
    }
}
