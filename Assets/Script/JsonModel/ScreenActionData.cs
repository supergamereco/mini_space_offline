using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ScreenActionData
    {
        public int update_pilot = 1;
        public int update_spaceship = 1;
        public int update_weapon = 0;
        public int update_chest = 0;
        public int update_pass = 0;

        public ScreenActionData()
        {

        }

        public ScreenActionData(int update_pilot, int update_spaceship, int update_weapon, int update_chest, int update_pass)
        {
            this.update_pilot = update_pilot;
            this.update_spaceship = update_spaceship;
            this.update_weapon = update_weapon;
            this.update_chest = update_chest;
            this.update_pass = update_pass;
        }
    }
}