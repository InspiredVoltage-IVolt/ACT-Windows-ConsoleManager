using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;


namespace ACT.Applications.ConsoleManager.Structs
{


    public class Menu_Variables_SaveState
    {
        [JsonProperty("globals", NullValueHandling = NullValueHandling.Ignore)]
        public List<Variables> Globals { get; set; }

        [JsonProperty("markupfile_variables", NullValueHandling = NullValueHandling.Ignore)]
        public List<Markup_File_Variables> MarkupFileVariables { get; set; }

        public static Menu_Variables_SaveState FromJson(string json) => JsonConvert.DeserializeObject<Menu_Variables_SaveState>(json, JSON_Helper.Converter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, JSON_Helper.Converter.Settings);
    }

    public class Variables
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }

    public class Markup_File_Variables
    {
        [JsonProperty("markup_file_name", NullValueHandling = NullValueHandling.Ignore)]
        public string MarkupFileName { get; set; }

        [JsonProperty("variables", NullValueHandling = NullValueHandling.Ignore)]
        public List<Variables> Variables { get; set; }
    }

}
