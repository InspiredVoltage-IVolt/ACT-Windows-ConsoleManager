﻿using ACT.Core.Extensions;
using System;
using System.Collections.Generic;


namespace ACT.Applications.ConsoleManager
{
    public static class Core
    {
        public static string _BaseDirectory { get; private set; }
        public static string _MenuBaseDirectory { get; private set; }
        public static string _DefaultDataBaseDirectory { get; private set; }
        public static List<string> ErrorList_History = new List<string>();

        internal static string ActiveMenuName = null;

        /// <summary>
        /// Setup Base Directoy Paths
        /// </summary>
        static Core()
        {
            _BaseDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Applications\\ConsoleManager\\";
            _MenuBaseDirectory = _BaseDirectory + "Menus\\";
            _DefaultDataBaseDirectory = _DefaultDataBaseDirectory + "Default\\";
        }

        /// <summary>
        /// Init - Loads and Initalizes the Menu.
        /// </summary>
        /// <param name="MenuName">Name of the Menu</param>
        /// <param name="BaseDirectory">Custom Directory If Specified.</param>
        /// <returns>
        ///     -3 Unable to Load the Menu File
        ///     -2 Unable to Find Internal Base Directory
        ///     -1 Active Menu Already Running Use ChangeMenu Method Instead
        ///     0  Menu Already Loaded Use ResetMenu
        ///     1  Menu Loaded Successfully.
        /// </returns>
        public static int Init(string MenuName, string BaseDirectory = "")
        {
            if (ActiveMenuName == null) { ActiveMenuName = MenuName; }
            else if (ActiveMenuName == MenuName) { return 0; }

            if (BaseDirectory == "" || BaseDirectory == null)
            {
                BaseDirectory = _BaseDirectory;
            }

            // Ensure Directory Exists
            int _AttemptCount = 0;

        CheckMenu:
            _AttemptCount++;
            if (_AttemptCount > 2) { return -2; }

            if (System.IO.Directory.Exists(BaseDirectory) == false)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(BaseDirectory);
                    goto CheckMenu;
                }
                catch
                {
                    ErrorList_History.Add("Unable to locate the directory");
                    return -2;
                }
            }

        LoadMenuAtteempt:

            _AttemptCount++;
            if (_AttemptCount > 2) { return -3; }

            try
            {
                bool _MenuLoaded = false;
                _MenuLoaded = LoadMenu(MenuName, BaseDirectory);
            }
            catch
            {
                goto LoadMenuAtteempt;
            }

        }

        /// <summary>
        /// Menu Files should be places in the following location path
        ///    DEFAULT LOCATION = _MenuBaseDirectory\###MenuName###\###MenuName###.json
        ///    CUSTOM LOCATION = ###BaseDirectory\###MenuName###\###MenuName###.json
        /// </summary>
        /// <param name="MenuName">Name of the Folder and JSON File and Menu</param>
        /// <param name="BaseDirectory">OPTIONAL - Only use if you have defined locations elseware</param>
        /// <returns>true/false if the Menu was Found and Loaded
        ///     Exceptions if Not - ErrorList_History is AppendedAlso
        /// </returns>
        public static bool LoadMenu(string MenuName, string BaseDirectory = null)
        {
            // If Parameter is Null or Empty Set the Base Directory to the Default System Path
            if (BaseDirectory.NullOrEmpty()) { BaseDirectory = _MenuBaseDirectory; }

            // If the MenuFile Exists in the Correct Path
            // SEE Method Comments
            string _MenuFileFullPath = BaseDirectory.EnsureDirectoryFormat() + MenuName + "\\" + MenuName + ".json";

            if (_MenuFileFullPath.FileExists() == false)
            {
                if (_MenuFileFullPath.Replace(".json", ".acte").FileExists())
                {
                    _MenuFileFullPath = _MenuFileFullPath.Replace(".json", ".acte");
                }
            }
            else
            {
                if (_MenuFileFullPath.Replace(".json", ".acte").FileExists())
                {
                    // ARCHIVE THE MENU AND PROTECT

                    ACT.Applications.ConsoleManager._MenuFileFullPath
                }
            }
            if (_MenuFileFullPath.FileExists() == false) { ErrorList_History.Add("Menu Not Found"); return false; }
            return false;




        }

    }
}
