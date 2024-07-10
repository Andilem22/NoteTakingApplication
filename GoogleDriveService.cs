using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace NoteTakingApplication
{
    public class GoogleDriveService
    {
        private static string[] Scopes = { DriveService.Scope.Drive };
        private static string ApplicationName = "Note Taking App";

        public static async Task<DriveService> AuthenticateAsync()
        {        
            try
            {
                // Adjust the path to credentials.json as needed
             
                GoogleCredential credential;

                using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(Scopes);
                }
                //// Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                 return service;

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: credentials.json not found. {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Error: Unauthorized access to credentials.json. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Unexpected error occurred. {ex.Message}");
            }

            return null;
        }
    }
}
