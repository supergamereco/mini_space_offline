using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ImagePath : BaseJsonModel
    {
        public List<ImagePathData> image_path;
    }
}