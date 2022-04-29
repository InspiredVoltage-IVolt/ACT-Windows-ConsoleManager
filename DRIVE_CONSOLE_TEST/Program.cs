using System;
using System.Collections.Generic;
using System.Linq;

namespace ACT.APIS.GOOGLE.DRIVE
{
    public class GoogleConsoleEntry
    {
        internal static string _ProjectNumber = "822928842213";
        internal static string _ProjectID = "act-core-gmail";
        internal static string _ProjectName = "ACT-CORE-GMAIL";
        internal static string _APIKey = "AIzaSyDmKITNJx5ZHuCepc8iAqqLvz9tjseY22I";
        
        internal static string _CredentialsPAth = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Applications\GoogleAPI\";
        internal static string _GmailAuthTokensPAth = _CredentialsPAth + "GmailAuthTokens\\";
        internal static string _ServiceGmailCredentialFile = "act-core-gmail-614853f39961.json";
        internal static string _AuthTokenFile = "gmail_token.json";
        internal static string _OAuthJSONFile = "client_secret_822928842213-i1tgggrluur2a5of0mj4ippm3oukqjj6.apps.googleusercontent.com.json";

        public enum PathList
        {
            GMAILSERVICEACCOUNTCREDENTIALS,
            GMAIL_AUTHTOKENPATH
        }
        public static string GetPath(PathList pathToGet)
        {
            if (pathToGet == PathList.GMAILSERVICEACCOUNTCREDENTIALS)
            {
                return _CredentialsPAth + _ServiceGmailCredentialFile;
            }
            else if (pathToGet == PathList.GMAIL_AUTHTOKENPATH)
            {
                return _GmailAuthTokensPAth + _AuthTokenFile;
            }
            return null;
        }

        public static void Main(string[] args)
        {
            var _RL = Console.ReadLine();

            if (_RL.ToLower() == "labels_test")
            {
                GMAILV1.GmailV1_Manager.RunGet_Labels_Test();
            }

            Console.ReadKey();
        }
    }
}

