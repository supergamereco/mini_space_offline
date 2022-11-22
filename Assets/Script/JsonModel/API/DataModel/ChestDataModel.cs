using System;
using NFT1Forge.OSY.System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ChestDataModel : NftMetadataModel
    {
        public string missiontype;
        public string rank;
        public int mission;
        public int missionprogress;

        public ChestDataModel()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="name"></param>
        /// <param name="missionType"></param>
        /// <param name="rank"></param>
        /// <param name="mission"></param>
        /// <param name="missionProgress"></param>
        public ChestDataModel(string tokenId, string name, string missionType, string rank, int mission, int missionProgress) : base(tokenId, name)
        {
            missiontype = missionType;
            this.rank = rank;
            this.mission = mission;
            missionprogress = missionProgress;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetImagePath()
        {
            return $"{SystemConfig.BaseAssetPath}rendered/{GetMaster().image}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ChestMasterData GetMaster()
        {
            return DatabaseSystem.I.GetMetadata<ChestMaster>().chest_master.Find(
                a => a.name.Equals(name)
            ) ?? DatabaseSystem.I.GetMetadata<ChestMaster>().chest_master[0];
        }
        /// <summary>
        /// 
        /// </summary>
        public void SyncMasterData()
        {
            ChestMasterData masterData = GetMaster();
            if (null != masterData)
            {
                description = masterData.description;
                rank = masterData.rank;
                name = masterData.name;
                mission = masterData.mission;
                image = GetImagePath();
                missiontype = "" == masterData.mission_type ? " " : masterData.mission_type;
            }
        }
        /// <summary>
        /// Evolve chest
        /// </summary>
        public bool Evolve()
        {
            if (mission <= missionprogress && 0 != GetMaster().evo_to)
            {
                ChestMasterData master = DatabaseSystem.I.GetMetadata<ChestMaster>().chest_master.Find(
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
