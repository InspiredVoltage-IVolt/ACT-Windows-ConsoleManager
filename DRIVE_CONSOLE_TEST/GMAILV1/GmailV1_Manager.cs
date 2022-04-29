using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACT.APIS.GOOGLE.DRIVE.GMAILV1
{
    public static class GmailV1_Manager
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/gmail-dotnet-quickstart.json
        static string[] Scopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailLabels };
        static string ApplicationName = GoogleConsoleEntry._ProjectName;

        public static void RunGet_Labels_Test()
        {
            GoogleCredential credential;
            
            using (var stream = new FileStream(GoogleConsoleEntry.GetPath(GoogleConsoleEntry.PathList.GMAILSERVICEACCOUNTCREDENTIALS), FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created automatically when the authorization flow completes for the first time.
                string credPath = GoogleConsoleEntry.GetPath(GoogleConsoleEntry.PathList.GMAIL_AUTHTOKENPATH);
                credential = GoogleCredential.FromServiceAccountCredential(ServiceAccountCredential.FromServiceAccountData(stream));          //...Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)); ; ;.Result;
               // credential = credential.CreateWithUser("mark@inspiredvoltage.com");

                Console.WriteLine("Credential file saved to: " + credPath);
            }
            
            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApiKey = GoogleConsoleEntry._APIKey
            });

            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("mark@inspiredvoltage.com");

            // List labels.
            IList<Label> labels = request.Execute().Labels;

            Console.WriteLine("Labels:");
            
            if (labels != null && labels.Count > 0)
            {
                foreach (var labelItem in labels)
                {
                    Console.WriteLine("{0}", labelItem.Name);
                }
            }
            else
            {
                Console.WriteLine("No labels found.");
            }

            Console.Read();
        }
    }
}



