using System;
using System.Collections.Generic;
using System.Linq;
using ACT.Core.Extensions;

namespace ACT.Applications.ConsoleManager.Engine
{
    /// <summary>
    /// Variables Created By Markup and Menu System Engine
    /// </summary>
    public class ACT_Markup_Variables
    {
        Dictionary<string, string> GlobalVariables = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> Variables = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Set Variable Name and Value
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <exception cref="Exception"></exception>
        public void Set(string Name, string Value, string MarkupFileName, bool Global)
        {
            if (Global)
            {
                if (GlobalVariables.ContainsKey(Name)) { GlobalVariables[Name] = Value; }
                else { GlobalVariables.Add(Name, Value); }
            }

            if (MarkupFileName.NullOrEmpty()) { throw new Exception("No Menu Specified"); }

            if (Variables[MarkupFileName].ContainsKey(Name)) { Variables[MarkupFileName][Name] = Value; }
            else { Variables[MarkupFileName].Add(Name, Value); }


        }

        /// <summary>
        /// Get Variable Value
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>null if not found</returns>
        public string Get(string Name, string MarkupFileName)
        {
            if (MarkupFileName.NullOrEmpty()) { throw new Exception("No Menu Specified"); }

            if (Variables[MarkupFileName].ContainsKey(Name)) { return Variables[MarkupFileName][Name]; }
            else
            {
                if (GlobalVariables.ContainsKey(Name)) { return GlobalVariables[Name]; }
            }
            return null;
        }

        /// <summary>Returns All Markup Files Added</summary>
        public List<string> AllMarkupFilesAdded
        {
            get
            {
                if (Variables.ToList().Count > 0) { return Variables.Keys.ToList(); }
                return new List<string>();
            }
        }

        /// <summary>
        /// Save The Current Object State
        /// </summary>
        /// <returns>Base64Encoded Data</returns>
        public string SaveState()
        {
            Structs.Menu_Variables_SaveState _TmpSave = new Structs.Menu_Variables_SaveState();
            _TmpSave.Globals = new List<Structs.Variables>();
            _TmpSave.MarkupFileVariables = new List<Structs.Markup_File_Variables>();

            foreach (string k in Variables.Keys)
            {
                Structs.Markup_File_Variables _a = new Structs.Markup_File_Variables();
                _a.MarkupFileName = k;
                foreach (string kk in Variables[k].Keys)
                {
                    _a.Variables.Add(new Structs.Variables() { Name = kk, Value = Variables[k][kk] });
                }
            }

            foreach (string gvk in GlobalVariables.Keys)
            {
                _TmpSave.Globals.Add(new Structs.Variables() { Name = gvk, Value = GlobalVariables[gvk] });
            }

            return _TmpSave.ToJson();
        }

        /// <summary>
        /// Load a Saved State
        /// </summary>
        /// <param name="SavedState"></param>
        public void LoadState(string SavedState)
        {
            GlobalVariables = new Dictionary<string, string>();
            Variables = new Dictionary<string, Dictionary<string, string>>();

            Structs.Menu_Variables_SaveState _tmpSave = Structs.Menu_Variables_SaveState.FromJson(SavedState);
            foreach (var k in _tmpSave.Globals)
            {
                GlobalVariables.Add(k.Name, k.Value);
            }

            foreach (var mk in _tmpSave.MarkupFileVariables)
            {
                if (Variables.ContainsKey(mk.MarkupFileName) == false) { Variables.Add(mk.MarkupFileName, new Dictionary<string, string>()); }

                foreach (var kk in mk.Variables)
                {
                    Variables[mk.MarkupFileName].Add(kk.Name, kk.Value);
                }
            }
        }
    }
}
