using System;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PilotDataModel : NftMetadataModel
    {
        public string color1;
        public string color2;
        public ushort level;
        public uint highscore;
        public string rank;
        public float goldbooster;
        public float weaponchargespeed;


        public PilotDataModel()
        {
        }

        public PilotDataModel(string tokenId, HairColor color1, CostumeColor color2, string name,
            string rank, ushort level, float goldbooster, float weaponchargespeed) : base(tokenId, name)
        {
            this.description = GetMaster().description;
            this.color1 = color1.ToString();
            this.color2 = color2.ToString();
            this.level = level;
            this.highscore = 0;
            this.rank = rank;
            this.goldbooster = goldbooster;
            this.weaponchargespeed = weaponchargespeed;
        }
        
        public string GetProfileImagePath()
        {
            Enum.TryParse(this.color1, out HairColor color1);
            Enum.TryParse(this.color2, out CostumeColor color2);
            if (HairColor.None == color1)
                color1 = HairColor.Black;
            if (CostumeColor.None == color2)
                color2 = CostumeColor.Orange;
            return $"{SystemConfig.BaseAssetPath}rendered/{GetMaster().profile_image_prefix}{(int)color1}{(int)color2}.png";
        }

        public string GetFullImagePath()
        {

            Enum.TryParse(this.color1, out HairColor color1);
            Enum.TryParse(this.color2, out CostumeColor color2);
            if (HairColor.None == color1)
                color1 = HairColor.Black;
            if (CostumeColor.None == color2)
                color2 = CostumeColor.Orange;
            return $"{SystemConfig.BaseAssetPath}rendered/{GetMaster().full_image_prefix}{(int)color1}{(int)color2}.png";
        }

        public PilotMasterData GetMaster()
        {
            return DatabaseSystem.I.GetMetadata<PilotMaster>().pilot_master.Find(
                a => a.name.Equals(name)
                ) ?? DatabaseSystem.I.GetMetadata<PilotMaster>().pilot_master[0];
        }
        
        public PilotRankLevelData GetLevel()
        {
            return DatabaseSystem.I.GetMetadata<PilotRankLevel>().pilot_rank_level.Find(
                a => a.rank.Equals(rank) && a.level.Equals(level)
            ) ?? DatabaseSystem.I.GetMetadata<PilotRankLevel>().pilot_rank_level[0];
        }

        public void SyncMasterData()
        {
            PilotMasterData masterData = GetMaster();
            if (null != masterData)
            {
                description = masterData.description;
                rank = masterData.rank;
                name = masterData.name;
                image = GetProfileImagePath();
            }
            
            PilotRankLevelData levelData = GetLevel();
            if (null != levelData)
            {
                goldbooster = levelData.gold_booster;
                weaponchargespeed = levelData.weapon_charge_speed;
            }
        }
    }
}
