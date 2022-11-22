using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class SpecialWeaponMetadataRequestModel : BaseMetadataRequestModel
    {
        public AttributeRequestModel attributes;

        [Serializable]
        public class AttributeRequestModel
        {
            public string EVOTYPE;
            public int DAMAGE;
            public string RANK;
            public int EVOMISSION;
            public int FIRINGSPEED;
            public int MISSIONPROGRESS;
        }
        /// <summary>
        /// 
        /// </summary>
        public SpecialWeaponMetadataRequestModel()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="attributes"></param>
        public SpecialWeaponMetadataRequestModel(string tokenId, string name, string description, AttributeRequestModel attributes)
        {
            this.tokenId = tokenId;
            this.name = name;
            this.description = description;
            this.attributes = attributes;
        }

        public SpecialWeaponMetadataRequestModel(SpecialWeaponDataModel model)
        {
            tokenId = model.token_id;
            name = model.name;
            description = model.description;
            image = model.GetImagePath();
            attributes = new AttributeRequestModel
            {
                EVOTYPE = model.evotype,
                DAMAGE = model.damage,
                RANK = model.rank,
                EVOMISSION = model.evomission,
                FIRINGSPEED = model.firingspeed,
                MISSIONPROGRESS = model.missionprogress
            };
        }
    }
}
