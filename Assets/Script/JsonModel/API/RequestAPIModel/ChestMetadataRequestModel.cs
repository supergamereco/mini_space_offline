using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ChestMetadataRequestModel : BaseMetadataRequestModel
    {
        public AttributeRequestModel attributes;

        [Serializable]
        public class AttributeRequestModel
        {
            public string MISSIONTYPE;
            public string RANK;
            public int MISSION;
            public int MISSIONPROGRESS;
        }
        /// <summary>
        /// 
        /// </summary>
        public ChestMetadataRequestModel()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="attributes"></param>
        public ChestMetadataRequestModel(string tokenId, string name, string description, AttributeRequestModel attributes)
        {
            this.tokenId = tokenId;
            this.name = name;
            this.description = description;
            this.attributes = attributes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public ChestMetadataRequestModel(ChestDataModel model)
        {
            tokenId = model.token_id;
            name = model.name;
            description = model.description;
            image = model.image;
            attributes = new AttributeRequestModel
            {
                MISSIONTYPE = model.missiontype,
                RANK = model.rank,
                MISSION = model.mission,
                MISSIONPROGRESS = model.missionprogress
            };
        }
    }
}
