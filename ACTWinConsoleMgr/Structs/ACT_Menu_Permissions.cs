using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager.Structs
{

    public class ACT_Console_Menu_Permissions
    {
        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public List<ACT_Console_Menu_Permissions_User> Users { get; set; }

        public static ACT_Console_Menu_Permissions FromJson(string json) => JsonConvert.DeserializeObject<ACT_Console_Menu_Permissions>(json, JSON_Helper.Converter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, JSON_Helper.Converter.Settings);
    }

    public class ACT_Console_Menu_Permissions_User
    {
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        [JsonProperty("groups", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Groups { get; set; }

        [JsonProperty("additional_info", NullValueHandling = NullValueHandling.Ignore)]
        public List<ACT_Console_Menu_Permissions_User_AdditionalInfo> AdditionalInfo { get; set; }
    }

    public class ACT_Console_Menu_Permissions_User_AdditionalInfo
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }
}
