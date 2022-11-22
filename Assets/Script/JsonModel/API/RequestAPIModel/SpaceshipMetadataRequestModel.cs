using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SpaceshipMetadataRequestModel : BaseMetadataRequestModel
    {
        public AttributeRequestModel attributes;

        [Serializable]
        public class AttributeRequestModel
        {
            public float DAMAGE;
            public ushort LEVEL;
            public float ARMOR;
            public string BOSSPART;
            public string RANK;

            public AttributeRequestModel()
            {
            }

            public AttributeRequestModel(float damage, float armor, string rank)
            {
                DAMAGE = damage;
                LEVEL = 1;
                ARMOR = armor;
                BOSSPART = "";
                RANK = rank;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="attributes"></param>
        public SpaceshipMetadataRequestModel(string tokenId, string name, string description, AttributeRequestModel attributes)
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
        public SpaceshipMetadataRequestModel(SpaceshipDataModel model)
        {
            tokenId = model.token_id;
            name = model.name;
            description = model.description;
            image = model.GetImagePath();
            attributes = new AttributeRequestModel
            {
                DAMAGE = model.damage,
                LEVEL = model.level,
                ARMOR = model.armor,
                BOSSPART = model.bosspart,
                RANK = model.rank
            };
        }
    }
}
