using ACT.Core.Extensions;
using System;
using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager.Engine
{
    public class ACT_Menu_Instance
    {
        public string MenuID { get; private set; }
        public string MenuName { get; private set; }
        public Structs.ACT_Menu MenuObject;
        public List<Structs.ACT_MarkupFile> MarkupFiles = new List<Structs.ACT_MarkupFile>();
        public bool IsDefaultMenu = false;
        public bool MenuLoaded { get { return _MenuLoaded; } }
        public List<string> ErrorList_History = new List<string>();
        public string MenuHomeDirectory = "";
        public Structs.ACT_Console_Menu_Permissions MenuPermissions = null;
        public ACT_Markup_Variables MarkupFileVariables = new ACT_Markup_Variables();

        bool _MenuLoaded = false;
        string _EncryptedData = null;

        public string MarkupDirectory { get { return MenuHomeDirectory.EnsureDirectoryFormat() + "displaymarkups\\"; } }
        public string CSharpCacheDirectory { get { return MenuHomeDirectory.EnsureDirectoryFormat() + "csharpcachedirectory\\"; } }
        public string SaveStatesDirectory { get { return MenuHomeDirectory.EnsureDirectoryFormat() + "savestates\\"; } }
        public string GetVariableValue(string Name, string MarkupFileName)
        {
            return MarkupFileVariables.Get(Name, MarkupFileName);
        }
        public void SetVariableValue(string Name, string Value, string MarkupFileName, bool Global)
        {
            MarkupFileVariables.Set(Name, Value, MarkupFileName, Global);
        }

        /// <summary>
        /// ACT Console Menu Constructor
        /// </summary>
        /// <param name="MenuName"></param>
        /// <param name="MenuPath"></param>
        public ACT_Menu_Instance(string MenuName, string MenuPath)
        {
            if (!Core.Initalized) { Core.Init(); }
            this.MenuID = MenuName.ToBase64();
            this.MenuName = MenuName;

            if (Init(MenuName, MenuPath) < 0) { throw new Exception("Unable To Load The Engine: Please Check Error Log"); }

            if (_MenuLoaded == false) { throw new Exception("Unable To Load The Engine: Please Check Error Log"); }
            if (MenuObject == null) { throw new Exception("Unable To Load The Engine: Please Check Error Log"); }
        }

        /// <summary>
        /// Locate the Menu File Specified
        /// </summary>
        /// <param name="MenuName"></param>
        /// <param name="BaseDirectory"></param>
        /// <param name="NonEncrypted"></param>
        /// <returns></returns>
        string LocateMenu(string MenuName, string BaseDirectory, bool NonEncrypted = true)
        {

            string _MenuFullPath = "";

            if (NonEncrypted)
            {
                _MenuFullPath = BaseDirectory.EnsureDirectoryFormat() + MenuName + "\\" + MenuName + ".json";
                if (_MenuFullPath.FileExists()) { return _MenuFullPath; }

                _MenuFullPath = BaseDirectory.FindFileReturnPath(MenuName + ".json", true);
                if (_MenuFullPath.NullOrEmpty() == false && _MenuFullPath.FileExists()) { return _MenuFullPath; }
            }
            else
            {
                _MenuFullPath = BaseDirectory.EnsureDirectoryFormat() + MenuName + "\\" + MenuName + ".enc";
                if (_MenuFullPath.FileExists()) { return _MenuFullPath; }

                _MenuFullPath = BaseDirectory.FindFileReturnPath(MenuName + ".enc", true);
                if (_MenuFullPath.NullOrEmpty() == false && _MenuFullPath.FileExists()) { return _MenuFullPath; }
            }

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
            _MenuLoaded = false;

            // Locate The Menu Full Path
            string _MenuFileFullPath = LocateMenu(MenuName, BaseDirectory);

            if (MenuName == "Default_Menu")
            {
                MenuObject = Structs.ACT_Menu.FromJson(_MenuFileFullPath.ReadAllText());

                if (MenuObject == null) { return false; }
                else if (MenuObject.Id.NullOrEmpty()) { return false; }
                else
                {
                    IsDefaultMenu = true;
                    return true;
                }
            }
            else
            {
                // Set The Encryption File Name
                string _MenuFileFullPathEncrypted = LocateMenu(MenuName, BaseDirectory, false);

                if (_MenuFileFullPath.FileExists() == false && _MenuFileFullPathEncrypted.FileExists() == false) { return false; }

                if (_MenuFileFullPath.FileExists()) { this.MenuHomeDirectory = _MenuFileFullPath.GetDirectoryFromFileLocation().EnsureDirectoryFormat(); }
                if (_MenuFileFullPathEncrypted.FileExists()) { this.MenuHomeDirectory = _MenuFileFullPathEncrypted.GetDirectoryFromFileLocation().EnsureDirectoryFormat(); }

                _EncryptedData = Helper.FileProtectionHelper.CleanupEncryptedFileLogic_ReturnEncryptedData(_MenuFileFullPath, _MenuFileFullPathEncrypted);

                if (_EncryptedData.NullOrEmpty()) { return false; }

                MenuObject = Structs.ACT_Menu.FromJson(ACT.Core.Security.ProtectData.UnProtectStringToString(_EncryptedData, true));

                if (MenuObject == null) { return false; }
                else if (MenuObject.Id.NullOrEmpty()) { return false; }
                else { return true; }
            }
        }
    }
}
