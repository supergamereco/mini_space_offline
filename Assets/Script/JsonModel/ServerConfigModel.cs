using System;
using System.Collections.Generic;

namespace NFT1Forge.OSY.JsonModel
{
    [Serializable]
    public class ServerConfigModel
    {
        public string environment_short_name;
        public string metadata_url;
        public string base_asset_path;
        public string app_id;
        public string app_secret;
        public string pilot_contract_address;
        public string spaceship_contract_address;
        public string survival_pass_contract_address;
        public string special_weapon_contract_address;
        public string chest_contract_address;
        public List<ServerConfigData> server_list;
    }
}
