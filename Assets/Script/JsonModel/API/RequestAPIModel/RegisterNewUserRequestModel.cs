using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class RegisterNewUserRequestModel : BaseRequestModel
    {
        public string wallet_address;
        public string type;

        public RegisterNewUserRequestModel(string walletAddress, ushort nftType)
        {
            wallet_address = walletAddress;
            type = nftType.ToString();
        }
    }
}
