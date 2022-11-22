using System;
using NFT1Forge.OSY.System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PassDataModel : NftMetadataModel
    {
        public int bestrecord;
        public ushort maxplay;
        public ushort playcount;

        public PassDataModel()
        {
        }

        public PassDataModel(string tokenId, string name, int bestrecord, ushort maxplay, ushort playcount) : base(tokenId, name)
        {
            this.bestrecord = bestrecord;
            this.maxplay = maxplay;
            this.playcount = playcount;
        }
        
        public string GetImagePath()
        {
            return $"{SystemConfig.BaseAssetPath}rendered/{GetMaster().image}";
        }

        public PassMasterData GetMaster()
        {
            return DatabaseSystem.I.GetMetadata<PassMaster>().pass_master.Find(
                a => a.name.Equals(name)
            ) ?? DatabaseSystem.I.GetMetadata<PassMaster>().pass_master[0];
        }
        
        public void SyncMasterData()
        {
            PassMasterData masterData = GetMaster();
            if (null != masterData)
            {
                description = masterData.description;
                maxplay = masterData.max_play;
                name = masterData.name;
                image = GetImagePath();
            }
        }
    }
}
