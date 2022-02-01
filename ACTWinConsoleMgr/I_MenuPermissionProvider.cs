using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager
{
    public interface I_MenuPermissionProvider
    {

        bool Login(string username, string password, Dictionary<string, string> AdditionalData = null);
        bool Login(string Token);
        bool Login(string CertificateData, string Password);

        bool UserIsInGroup(string GroupName);
        bool UserIsAdmin();

    }
}
