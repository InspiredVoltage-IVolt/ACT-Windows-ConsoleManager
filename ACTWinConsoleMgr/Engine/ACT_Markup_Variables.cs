using ACT.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ACT.Applications.ConsoleManager.Engine
{
    /// <summary>
    /// Variables Created By Markup and Menu System Engine
    /// </summary>
    public static class ACT_Markup_Variables
    {
        static Dictionary<string, string> GlobalVariables = new Dictionary<string, string>();
        static Dictionary<string, Dictionary<string, string>> Variables = new Dictionary<string, Dictionary<string, string>>();
        static string ActiveMenu = "";

        /// <summary>
        /// Set Active Menu
        /// </summary>
        /// <param name="MenuName"></param>
        public static void SetActiveMenu(string MenuName)
        {
            ActiveMenu = MenuName;
            if (Variables.ContainsKey(MenuName) == false) { Variables.Add(MenuName, new Dictionary<string, string>()); }
        }

        /// <summary>
        /// Set Variable Name and Value
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <exception cref="Exception"></exception>
        public static void Set(string Name, string Value, bool Global)
        {

            if (ActiveMenu.NullOrEmpty()) { throw new Exception("No Active Menu Specified"); }

            if (Variables[ActiveMenu].ContainsKey(Name)) { Variables[ActiveMenu][Name] = Value; }
            else { Variables[ActiveMenu].Add(Name, Value); }

            if (Global) { GlobalVariables.TryAdd(Name, Value); }
        }

        /// <summary>
        /// Get Variable Value
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>null if not found</returns>
        public static string Get(string Name)
        {
            if (ActiveMenu.NullOrEmpty()) { throw new Exception("No Active Menu Specified"); }

            if (Variables[ActiveMenu].ContainsKey(Name)) { return Variables[ActiveMenu][Name]; }
            else
            {
                if (GlobalVariables.ContainsKey(Name)) { return GlobalVariables[Name]; }
            }
            return null;
        }

        /// <summary>Returns All All Menus Added</summary>
        public static List<string> AllMenusAdded
        {
            get
            {
                if (Variables.ToList().Count > 0) { return Variables.Keys.ToList(); }
                return new List<string>();
            }
        }
    }
}
