using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PassMetadataRequestModel : BaseMetadataRequestModel
    {
        public AttributeRequestModel attributes;
        
        [Serializable]
        public class AttributeRequestModel
        {
            public int BESTRECORD;
            public int MAXPLAY;
            public int PLAYCOUNT;
        }
        /// <summary>
        /// 
        /// </summary>
        public PassMetadataRequestModel()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="attributes"></param>
        public PassMetadataRequestModel(string tokenId, string name, string description, AttributeRequestModel attributes)
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
        public PassMetadataRequestModel(PassDataModel model)
        {
            tokenId = model.token_id;
            name = model.name;
            description = model.description;
            image = model.image;
            attributes = new AttributeRequestModel
            {
                BESTRECORD = model.bestrecord,
                MAXPLAY = model.maxplay,
                PLAYCOUNT = model.playcount
            };
        }
    }
}
