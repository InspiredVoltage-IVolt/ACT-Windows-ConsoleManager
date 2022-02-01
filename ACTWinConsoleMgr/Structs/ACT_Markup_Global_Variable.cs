using ACT.Core.Extensions;
using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager.Structs
{
    public class ACT_Markup_Global_Variables
    {
        SortedDictionary<string, (string, string)> VariableData = new SortedDictionary<string, (string, string)>();

        public void AddVariable(string MenuID, string Key, string Value)
        {
            if (VariableData.ContainsKey(Key) == false) { VariableData.Add(MenuID, (MenuID, Value)); }
            else { VariableData[Key] = (MenuID, Value); }

        }

        public string GetVariable(string Key, string MenuID = "")
        {
            if (MenuID.NullOrEmpty())
            {
                if (VariableData.ContainsKey(Key))
                {
                    string _D = VariableData[Key].Item2;
                    return _D;
                }
                else { return null; }
            }
            else
            {
                if (VariableData.ContainsKey(Key))
                {
                    if (VariableData[Key].Item1 == MenuID) { return VariableData[Key].Item1; }
                }
            }
            return null;
        }

        public void RemoveVariable(string Key)
        {
            VariableData.Remove(Key);
        }


        public string ToJSON()
        {
            string x = Newtonsoft.Json.JsonConvert.SerializeObject(VariableData, Newtonsoft.Json.Formatting.Indented);
            return x;
        }

        public ACT_Markup_Global_Variables()
        {
        }
        public ACT_Markup_Global_Variables(string JSONdata)
        {
            VariableData = (SortedDictionary<string, (string, string)>)Newtonsoft.Json.JsonConvert.DeserializeObject(JSONdata, typeof(SortedDictionary<string, (string, string)>));
        }

    }
}
