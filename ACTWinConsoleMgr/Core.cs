using ACT.Core.Extensions;
using System;
using System.Collections.Generic;


namespace ACT.Applications.WinConsoleMgr
{
    public static class Core
    {
        public static string _BaseDirectory { get; private set; }
        public static string _MenuBaseDirectory { get; private set; }
        static string ActiveMenuName = null;
        public static List<string> ErrorList = new List<string>();

        static Core()
        {
            _BaseDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\ACTWinConsoleMgr\\";
            _MenuBaseDirectory = _BaseDirectory + "Menus\\";
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
                    ErrorList.Add("Unable to locate the directory");
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

        public static bool LoadMenu(string MenuName, string BaseDirectory = null)
        {
            if (BaseDirectory.NullOrEmpty()) { BaseDirectory = _MenuBaseDirectory; }
            string _MenuJSON = BaseDirectory.EnsureDirectoryFormat() + MenuName.EnsureEndsWith(".json");
            if (_MenuJSON.FileExists() == false) { ErrorList.Add("Menu Not Found"); return false; }
            return false;




        }

    }
}
