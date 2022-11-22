using System;
using NFT1Forge.OSY.System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SpecialWeaponDataModel : NftMetadataModel
    {
        public string rank;
        public int firingspeed;
        public int damage;
        public string evotype;
        public int evomission;
        public int missionprogress;

        public SpecialWeaponDataModel()
        {
        }

        public SpecialWeaponDataModel(string tokenId, string name, string evoType, int damage,
            int firingSpeed, string rank, int evoMission, int missionProgress) : base(tokenId, name)
        {
            evotype = evoType;
            this.damage = damage;
            this.rank = rank;
            evomission = evoMission;
            firingspeed = firingSpeed;
            missionprogress = missionProgress;
        }
        
        public string GetImagePath()
        {
            return $"{SystemConfig.BaseAssetPath}rendered/{GetMaster().image}";
        }

        public SpecialWeaponMasterData GetMaster()
        {
            return DatabaseSystem.I.GetMetadata<SpecialWeaponMaster>().weapon_master.Find(
                a => a.name.Equals(name)
            ) ?? DatabaseSystem.I.GetMetadata<SpecialWeaponMaster>().weapon_master[0];
        }
        
        public void SyncMasterData()
        {
            SpecialWeaponMasterData masterData = GetMaster();
            if (null != masterData)
            {
                description = masterData.description;
                damage = masterData.damage;
                rank = masterData.rank;
                firingspeed = masterData.firing_speed;
                name = masterData.name;
                evotype = masterData.evo_type;
                evomission = masterData.evo_mission; //"" == masterData.evo_condition ? " " : masterData.evo_condition;
                image = GetImagePath();
            }
        }
        
        public bool Evolve()
        {
            if (evomission <= missionprogress && 0 != GetMaster().evo_to)
            {
                SpecialWeaponMasterData master = DatabaseSystem.I.GetMetadata<SpecialWeaponMaster>().weapon_master.Find(
                    a => a.id.Equals(GetMaster().evo_to));
                name = master.name;
                missionprogress = 0;
                SyncMasterData();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
