using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class StartupReward : BaseJsonModel
    {
        public List<StartupRewardData> startup_reward;
    }
}
