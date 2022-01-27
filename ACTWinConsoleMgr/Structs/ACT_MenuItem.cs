using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager.Structs
{
    public class Menu_Item
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("override_default")]
        public object OverrideDefault { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("keycode", NullValueHandling = NullValueHandling.Ignore)]
        public string Keycode { get; set; }

        [JsonProperty("methodname", NullValueHandling = NullValueHandling.Ignore)]
        public string Methodname { get; set; }

        [JsonProperty("plugin", NullValueHandling = NullValueHandling.Ignore)]
        public string Plugin { get; set; }

        [JsonProperty("action_display_children", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ActionDisplayChildren { get; set; }

        [JsonProperty("execute", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Execute { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<Menu_Item> Items { get; set; }
    }
}
