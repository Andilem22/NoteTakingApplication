using NoteTakingApplication;
using System;
using System.Threading.Tasks;

namespace NoteTakingApplication
{
    class Program
    {
        private static NoteService _noteService;

        [Obsolete]
        static async Task Main(string[] args)
        {
            var driveService = await GoogleDriveService.AuthenticateAsync();
            _noteService = new NoteService(driveService);

            while (true)
            {
                Console.WriteLine("Note Taking App Menu:");
                Console.WriteLine("1. Create a new note");
                Console.WriteLine("2. Update a note");
                Console.WriteLine("3. Delete a note");
                Console.WriteLine("4. Exit");

                Console.Write("Enter your choice (1-4): ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CreateNote();
                        break;
                    case "2":
                        await UpdateNote();
                        break;
                    case "3":
                        await DeleteNote();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number from 1 to 4.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private static async Task CreateNote()
        {
            Console.Write("Enter note title: ");
            var title = Console.ReadLine();

            Console.Write("Enter note content: ");
            var content = Console.ReadLine();

            var fileId = await _noteService.CreateNoteAsync(title, content);
            Console.WriteLine($"Note created successfully with ID: {fileId}");
        }

        private static async Task UpdateNote()
        {
            Console.Write("Enter note ID to update: ");
            var fileId = Console.ReadLine();

            Console.Write("Enter updated note content: ");
            var content = Console.ReadLine();

            var updatedFileId = await _noteService.UpdateNoteAsync(fileId, content);
            Console.WriteLine($"Note updated successfully with ID: {updatedFileId}");
        }

        private static async Task DeleteNote()
        {
            Console.Write("Enter note ID to delete: ");
            var fileId = Console.ReadLine();

            await _noteService.DeleteNoteAsync(fileId);
            Console.WriteLine($"Note with ID {fileId} deleted successfully");
        }
    }
}
