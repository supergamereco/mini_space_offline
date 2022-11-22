using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SpecialWeaponMasterData
    {
        public int id;
        public string name;
        public string description;
        public string image;
        public string rank;
        public int firing_speed;
        public int damage;
        public string evo_type;
        public int evo_mission;
        public int evo_to;
        public float charge_time;
    }
}
