using System;
using NFT1Forge.OSY.JsonModel;

public class GetActiveObjectModel : BaseResponseModel
{

    public ActiveObjectDataModel data;

    [Serializable]
    public class ActiveObjectDataModel
    {
        public string active_pilot;
        public string active_spaceship;
        public string active_weapon;
        public string active_chest;
        public string active_pass;
    }
}
