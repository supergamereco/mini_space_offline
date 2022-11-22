using System;
using NFT1Forge.OSY.System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SpaceshipDataModel : NftMetadataModel
    {
        public float damage;
        public ushort level;
        public float armor;
        public string bosspart;
        public string rank;

        public SpaceshipDataModel()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="name"></param>
        /// <param name="damage"></param>
        /// <param name="armor"></param>
        /// <param name="rank"></param>
        /// <param name="level"></param>
        /// <param name="bosspart"></param>
        public SpaceshipDataModel(string tokenId, string name,
            float damage, float armor, string rank, ushort level, string bosspart = "NONE") : base(tokenId, name)
        {
            this.damage = damage;
            this.level = level;
            this.armor = armor;
            this.bosspart = bosspart;
            this.rank = rank;
        }

        public string GetImagePath()
        {
            if (string.IsNullOrWhiteSpace(bosspart) || bosspart.Equals("NONE") || string.IsNullOrEmpty(bosspart))
                return $"{SystemConfig.BaseAssetPath}rendered/{GetMaster().image}";
            else
            {
                string imagePrefix = $"{bosspart.ToLower().Replace(" ", "_")}";
                return $"{SystemConfig.BaseAssetPath}rendered/{imagePrefix}_{GetMaster().image}";
            }
        }

        public SpaceshipMasterData GetMaster()
        {
            return DatabaseSystem.I.GetMetadata<SpaceshipMaster>().spaceship_master.Find(
                a => a.name.Equals(name)
                ) ?? DatabaseSystem.I.GetMetadata<SpaceshipMaster>().spaceship_master[0];
        }
        
        public SpaceshipRankLevelData GetLevel()
        {
            return DatabaseSystem.I.GetMetadata<SpaceshipRankLevel>().spaceship_rank_level.Find(
                a => a.rank.Equals(rank) && a.level.Equals(level)
            ) ?? DatabaseSystem.I.GetMetadata<SpaceshipRankLevel>().spaceship_rank_level[0];
        }
        
        public void SyncMasterData()
        {
            SpaceshipMasterData masterData = GetMaster();
            if (null != masterData)
            {
                description = masterData.description;
                rank = masterData.rank;
                name = masterData.name;
                image = GetImagePath();
            }
            
            SpaceshipRankLevelData levelData = GetLevel();
            if (null != levelData)
            {
                damage = levelData.damage;
                armor = levelData.armor;
            }
        }
    }
}
