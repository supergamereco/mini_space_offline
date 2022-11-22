using System;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class LoginRequestModel : BaseRequestModel
    {
        public string username;
        public string password;

        public LoginRequestModel(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
