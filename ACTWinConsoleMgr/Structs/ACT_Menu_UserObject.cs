using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager.Structs
{
    public class ACT_Menu_UserObject
    {
        public ACT_Menu_UserObject(Engine.ACT_Menu_Instance mnuInstance) { }

        public string UserID;
        public string UserName;
        public string Token;
        public string Email;
        public bool Authenticated;
        public Dictionary<string, string> UserData = new Dictionary<string, string>();

        public I_MenuPermissionProvider MenuPermissionProvider;

        public bool IsInGroup(string GroupName)
        {
            if (MenuPermissionProvider == null) { }
            else
            {

            }
        }
    }
}
