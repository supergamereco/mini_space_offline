using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class PilotMetadataRequestModel : BaseMetadataRequestModel
    {
        public AttributeRequestModel attributes;

        [Serializable]
        public class AttributeRequestModel
        {
            public string COLOR1;
            public string COLOR2;
            public ushort LEVEL;
            public uint HIGHSCORE;
            public string RANK;
            public float GOLDBOOSTER;
            public float WEAPONCHARGESPEED;

            public AttributeRequestModel()
            {
            }

            public AttributeRequestModel(string rank, float goldbooster, float weaponchargespeed)
            {
                COLOR1 = "Green";
                COLOR2 = "Red";
                LEVEL = 1;
                HIGHSCORE = 1;
                RANK = rank;
                GOLDBOOSTER = goldbooster;
                WEAPONCHARGESPEED = weaponchargespeed;
            }
        }

        public PilotMetadataRequestModel()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="attributes"></param>
        public PilotMetadataRequestModel(string tokenId, string name, string description, AttributeRequestModel attributes)
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
        public PilotMetadataRequestModel(PilotDataModel model)
        {
            tokenId = model.token_id;
            name = model.name;
            description = model.description;
            image = model.GetProfileImagePath();
            attributes = new AttributeRequestModel
            {
                COLOR1 = model.color1,
                COLOR2 = model.color2,
                LEVEL = model.level,
                HIGHSCORE = model.highscore,
                RANK = model.rank,
                GOLDBOOSTER = model.goldbooster,
                WEAPONCHARGESPEED = model.weaponchargespeed
            };
        }
    }
}
