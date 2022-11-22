using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ItemActivateModel : BaseJsonModel
    {
        public string name;
        public ushort level;
    }
}
