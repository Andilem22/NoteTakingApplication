using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using System.IO;
using System.Threading.Tasks;
using File = Google.Apis.Drive.v3.Data.File;

namespace NoteTakingApplication
{
    public class NoteService
    {
        private DriveService _driveService;

        public NoteService(DriveService driveService)
        {
            _driveService = driveService;
        }

        public async Task<string> CreateNoteAsync(string title, string content)
        {
            var fileMetadata = new File()
            {
                Name = title,
                MimeType = "text/plain"
            };

            var byteArray = System.Text.Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(byteArray);

            FilesResource.CreateMediaUpload request;
            request = _driveService.Files.Create(fileMetadata, stream, "text/plain");
            request.Fields = "id";

            var uploadProgress = await request.UploadAsync();

            // After upload completes, get the file ID from the response
            if (uploadProgress.Status == UploadStatus.Completed)
            {
                var file = request.ResponseBody;
                return file.Id;
            }
            else
            {
                // Handle upload failure or cancellation
                return null;
            }
        }

        public async Task<string> UpdateNoteAsync(string fileId, string content)
        {
            var byteArray = System.Text.Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(byteArray);

            FilesResource.UpdateMediaUpload request;
            request = _driveService.Files.Update(null, fileId, stream, "text/plain");
            request.Fields = "id";

            var uploadProgress = await request.UploadAsync();

            // After update completes, get the file ID from the response
            if (uploadProgress.Status == UploadStatus.Completed)
            {
                var file = request.ResponseBody;
                return file.Id;
            }
            else
            {
                // Handle upload failure or cancellation
                return null;
            }
        }

        public async Task DeleteNoteAsync(string fileId)
        {
            await _driveService.Files.Delete(fileId).ExecuteAsync();
        }

        public async Task<string> GetNoteContentAsync(string fileId)
        {
            var request = _driveService.Files.Get(fileId);
            var response = await request.ExecuteAsync();

            using (var responseStream = await _driveService.HttpClient.GetStreamAsync(response.WebContentLink))
            using (var memoryStream = new MemoryStream())
            {
                await responseStream.CopyToAsync(memoryStream);
                return System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}
