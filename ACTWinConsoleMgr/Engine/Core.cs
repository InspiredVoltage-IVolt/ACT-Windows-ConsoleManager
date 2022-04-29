using System;
using System.Collections.Generic;
using System.Linq;
using ACT.Core.Extensions;

namespace ACT.Applications.ConsoleManager.Engine
{


    public static class Core
    {
        /// TODO Replace To ACT Extensions Method        
        public static ConsoleColor PSC(string ConsoleColorValue) { try { return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConsoleColorValue); } catch { throw new Exception(ConsoleColorValue + " Is Not A Valid Console Color."); } }

        public static ACT_Menu_Instance Active_MenuObject = null;
        public static bool Initalized = false;
        public static string BaseDirectory { get; internal set; }
        public static string MenuBaseDirectory { get; internal set; }
        public static string DefaultDataBaseDirectory { get; internal set; }
        public static string ActiveMenuID;
        public static List<ACT_Menu_Instance> LoadedMenus = new List<ACT_Menu_Instance>();



        #region Internal and Private Methods
        /// <summary>
        /// Initalize the Console Manager Engine
        /// </summary>
        internal static void Init()
        {
            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Applications\\ConsoleManager\\";
            MenuBaseDirectory = BaseDirectory + "Menus\\";
            DefaultDataBaseDirectory = BaseDirectory + "Default\\";

            string _DefaultMenuFile = DefaultDataBaseDirectory += "Default_Menu.json";
            var _tmpDefaultMnu = new ACT_Menu_Instance("Default_Menu", DefaultDataBaseDirectory);

            if (_tmpDefaultMnu.MenuLoaded == false)
            {
                LoadedMenus.Add(_tmpDefaultMnu);
                // Ignore Default Menu Configuration
                LoadMenuMarkupFiles(_tmpDefaultMnu);
                try { LoadPermissionFile(_tmpDefaultMnu); }
                catch
                {
                    // TODO DECIDE WTHAT TO DO AND LOG
                }
            }

            Initalized = true;
        }

        /// <summary>
        /// Load Markup File
        /// </summary>
        /// <param name="MenuID"></param>
        /// <param name="MarkupFileFullPath"></param>
        static void LoadMarkupFile(ACT_Menu_Instance MenuRef, string MarkupFileFullPath)
        {
            if (MenuRef == null) { throw new ArgumentNullException("Invalid Argument - MenuRef is Null"); } // TODO LOG ERROR

            Structs.ACT_MarkupFile _newFile = new Structs.ACT_MarkupFile(MenuRef.MenuName, MarkupFileFullPath, false);
            if (_newFile == null)
            { //TODO LOG ERROR
                throw new Exception("Error Loading The Markup File");
            }
            if (MenuRef.MarkupFiles.Exists(x => x.MarkupFile_ID == _newFile.MarkupFile_ID))
            {
                MenuRef.MarkupFiles.Add(_newFile);
            }
        }

        /// <summary>
        /// Start The Menu Engine By Loading All Data
        /// </summary>
        /// <param name="MenuID"></param>
        static void StartMenuEngine(string MenuID)
        {
            if (Initalized == false) { Init(); }
            if (Initalized == false) { throw new Exception("Serious Error!!!"); }

            var _tmpMenuRef = GetMenuByID(MenuID);

            if (_tmpMenuRef == null) { throw new Exception("Menu Does Not Exist"); }

            ActiveMenuID = MenuID;
            Active_MenuObject = _tmpMenuRef;

            // Run Startup Markup File If Defined
            var _startupMarkupFile = Active_MenuObject.MenuObject.StartupMarkupFile;
            if (_startupMarkupFile.NullOrEmpty() == false)
            {
                _startupMarkupFile = Active_MenuObject.MarkupDirectory + _startupMarkupFile;
                if (_startupMarkupFile.FileExists()) { LoadMarkupFile(Active_MenuObject, _startupMarkupFile); }
            }

        }

        static void ProcessAllMarkupFiles(ACT_Menu_Instance ActiveMnu)
        {
            if (ActiveMnu == null) { throw new Exception("ActiveMnu Argument Missing or Null"); }
            if (ActiveMnu.MenuLoaded == false) { throw new Exception("ActiveMnu Is Not Loaded Properly"); }

            Dictionary<string, (bool, string)> _MarkupFiles = new Dictionary<string, (bool, string)>();



        }

        static void LoadMenuMarkupFiles(ACT_Menu_Instance MnuInstance)
        {
            string _StartupMk = MnuInstance.MenuObject.StartupMarkupFile;
            string _menu_header_markupfile = MnuInstance.MenuObject.MenuHeaderMarkupfile;
            string _menu_footer_markupfile = MnuInstance.MenuObject.MenuFooterMarkupfile;
            string _login_menu_display_markupfile = MnuInstance.MenuObject.LoginMenuDisplayMarkupfile;
            string _admin_header_display_markupfile = MnuInstance.MenuObject.AdminHeaderDisplayMarkupfile;
            string _admin_menu_option_markupfile = MnuInstance.MenuObject.AdminMenuOptionMarkupfile;

            Structs.ACT_MarkupFile _tmp = null;

            try
            {
                _tmp = new Structs.ACT_MarkupFile(MnuInstance.MenuName, MnuInstance.MarkupDirectory + _StartupMk, false);
                MnuInstance.MarkupFiles.Add(_tmp);
            }
            catch (Exception ex) { if (MnuInstance.IsDefaultMenu == false) { throw; } }

            try
            {
                _tmp = new Structs.ACT_MarkupFile(MnuInstance.MenuName, MnuInstance.MarkupDirectory + _menu_header_markupfile, false);
                MnuInstance.MarkupFiles.Add(_tmp);
            }
            catch (Exception ex) { if (MnuInstance.IsDefaultMenu == false) { throw; } }

            try
            {
                _tmp = new Structs.ACT_MarkupFile(MnuInstance.MenuName, MnuInstance.MarkupDirectory + _menu_footer_markupfile, false);
                MnuInstance.MarkupFiles.Add(_tmp);
            }
            catch (Exception ex) { if (MnuInstance.IsDefaultMenu == false) { throw; } }

            try
            {
                _tmp = new Structs.ACT_MarkupFile(MnuInstance.MenuName, MnuInstance.MarkupDirectory + _login_menu_display_markupfile, false);
                MnuInstance.MarkupFiles.Add(_tmp);
            }
            catch (Exception ex) { if (MnuInstance.IsDefaultMenu == false) { throw; } }

            try
            {
                _tmp = new Structs.ACT_MarkupFile(MnuInstance.MenuName, MnuInstance.MarkupDirectory + _admin_header_display_markupfile, false);
                MnuInstance.MarkupFiles.Add(_tmp);
            }
            catch (Exception ex) { if (MnuInstance.IsDefaultMenu == false) { throw; } }

            try
            {
                _tmp = new Structs.ACT_MarkupFile(MnuInstance.MenuName, MnuInstance.MarkupDirectory + _admin_menu_option_markupfile, false);
                MnuInstance.MarkupFiles.Add(_tmp);
            }
            catch (Exception ex) { if (MnuInstance.IsDefaultMenu == false) { throw; } }

        }

        static void LoadPermissionFile(ACT_Menu_Instance MnuInstance)
        {
            try
            {
                string _permissions_file = MnuInstance.MenuObject.PermissionsFile;

                string _filePath = MnuInstance.MenuHomeDirectory + _permissions_file;
                if (_filePath.FileExists())
                {
                    var _PermissionFile = Structs.ACT_Console_Menu_Permissions.FromJson(_filePath.ReadAllText());
                    if (_PermissionFile != null)
                    {
                        MnuInstance.MenuPermissions = _PermissionFile;
                    }
                }
            }
            catch
            {
                // TODO Log Error
                throw new Exception("Error Processing Permissions File");
            }
        }

        #endregion

        /// <summary>
        /// Get Menu By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static ACT_Menu_Instance GetMenuByID(string ID)
        {
            return LoadedMenus.Exists(x => x.MenuID == ID) ? LoadedMenus.First(x => x.MenuID == ID) : null;
        }

        /// <summary>
        /// Run Menu
        /// </summary>
        /// <param name="MenuName"></param>
        public static void RunMenu(string MenuName) { RunMenu(MenuName, MenuBaseDirectory); }

        /// <summary>
        /// Run Menu
        /// </summary>
        /// <param name="MenuName"></param>
        /// <param name="LocationBasePath"></param>
        /// <exception cref="Exception"></exception>
        public static void RunMenu(string MenuName, string LocationBasePath)
        {
            if (LoadedMenus.Exists(x => x.MenuID == MenuName.ToBase64()) == false)
            {
                ACT_Menu_Instance _Menu = new ACT_Menu_Instance(MenuName, LocationBasePath);
                if (_Menu != null) { LoadedMenus.Add(_Menu); }
                else { throw new Exception("Unable to locate Menu"); }
            }

            Active_MenuObject = LoadedMenus.First(x => x.MenuID == ActiveMenuID);

            // RUN THIS MENU BABY
            StartMenuEngine(MenuName.ToBase64());
        }



        public static class ConsoleHelperMethods
        {

            /// <summary>
            /// Generate Current Markup and Save as MarkupText
            /// </summary>
            /// <returns>ACT ConsoleMarkup</returns>
            public static string SaveConsoleStateToMarkup()
            {
                string _tmpReturn = "";

                _tmpReturn += "bg=" + Console.BackgroundColor.ToString() + Environment.NewLine;
                _tmpReturn += "fg=" + Console.ForegroundColor.ToString() + Environment.NewLine;
                _tmpReturn += "cursorsize=" + Console.CursorSize.ToString() + Environment.NewLine;
#if WINDOWS
#pragma warning disable CA1416 // Validate platform compatibility
                _tmpReturn += "cursorvisible=" + Console.CursorVisible.ToString() + Environment.NewLine;
#pragma warning restore CA1416 // Validate platform compatibility
#endif

                return _tmpReturn;
            }
        }
    }
}
