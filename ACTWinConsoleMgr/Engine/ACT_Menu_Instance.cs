using ACT.Core.Extensions;
using System;
using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager.Engine
{
    public class ACT_Menu_Instance
    {
        public string MenuID { get; private set; }
        public Structs.ACT_Menu MenuObject;
        public List<string> ErrorList_History = new List<string>();

        bool _MenuLoaded = false;
        string _EncryptedData = null;

        /// <summary>
        /// ACT Console Menu Constructor
        /// </summary>
        /// <param name="MenuName"></param>
        /// <param name="MenuPath"></param>
        public ACT_Menu_Instance(string MenuName, string MenuPath)
        {
            if (!Core.Initalized) { Core.Init(); }

            if (Init(MenuName, MenuPath) < 0) { throw new Exception("Unable To Load The Engine: Please Check Error Log"); }
        }

        string LocateMenu(string MenuName, string BaseDirectory)
        {
            string _MenuFullPath = BaseDirectory.EnsureDirectoryFormat() + MenuName + "\\" + MenuName + ".json";
            if (_MenuFullPath.FileExists()) { return _MenuFullPath; }

            _MenuFullPath = BaseDirectory.FindFileReturnPath(MenuName + ".json", true);
            if (_MenuFullPath.NullOrEmpty() == false && _MenuFullPath.FileExists()) { return _MenuFullPath; }

            ErrorList_History.Add("Menu Not Found");
            return "";
        }

        /// <summary>
        /// Init - Loads and Initalizes the Menu.
        /// </summary>
        /// <param name="MenuName">Name of the Menu</param>
        /// <param name="BaseDirectory">Custom Directory If Specified.</param>
        /// <returns>
        ///     -3, -4 Unable to Load the Menu File
        ///     -2 Unable to Find Internal Base Directory
        ///     -1 Active Menu Already Running Use ChangeMenu Method Instead
        ///     0  Menu Already Loaded Use ResetMenu
        ///     1  Menu Loaded Successfully.
        /// </returns>
        int Init(string MenuName, string BaseDirectory = "")
        {
            if (BaseDirectory == "" || BaseDirectory == null) { BaseDirectory = Core.MenuBaseDirectory; }

            // Ensure Directory Exists
            if (System.IO.Directory.Exists(BaseDirectory) == false)
            {
                ErrorList_History.Add("Unable to locate the directory");
                return -2;
            }

            try
            {
                _MenuLoaded = LoadMenu(MenuName, BaseDirectory);
            }
            catch (Exception ex)
            {
                ErrorList_History.Add(ex.Message);
                return -4;
            }

            if (_MenuLoaded == false) { return -3; }

            return 1;
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
        bool LoadMenu(string MenuName, string BaseDirectory)
        {
            // Locate The Menu Full Path
            string _MenuFileFullPath = LocateMenu(MenuName, BaseDirectory);
            // Set The Encryption File Name
            string _MenuFileFullPathEncrypted = BaseDirectory.EnsureDirectoryFormat() + MenuName + "\\" + MenuName + ".json";

            _EncryptedData = Helper.FileProtectionHelper.GetEncryptedString(_MenuFileFullPath, _MenuFileFullPathEncrypted);

            if (_EncryptedData == null)
            {
                _MenuLoaded = false;
                return false;
            }

            Structs.ACT_Menu.FromJson(ACT.Core.Security.ProtectData.UnProtectString(_EncryptedData, true);



            return false;




        }
    }
}
