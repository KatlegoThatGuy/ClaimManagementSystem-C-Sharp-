namespace ClaimManagementSystem.Models
{
    public class UploadDocumentBase
    {
        private async Task<List<string>> UploadDocuments(IFormFile[]? supportingDocuments)
        {
            var documentPaths = new List<string>();

            // Check if there are no documents to upload
            if (supportingDocuments == null || supportingDocuments.Length == 0)
            {
                return documentPaths; // Return an empty list if no documents
            }

            // Define allowed file extensions
            var allowedExtensions = new[] { ".pdf", ".docx" };

            // Define the upload folder path
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var file in supportingDocuments)
            {
                // Check if the file is not null and has content
                if (file != null && file.Length > 0)
                {
                    // Validate file extension
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        throw new InvalidOperationException($"File type '{extension}' is not allowed. Only PDF and DOCX files are supported.");
                    }

                    // Create a unique filename to avoid conflicts
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Store the relative path for database storage
                    documentPaths.Add($"/uploads/{uniqueFileName}");
                }
            }

            return documentPaths;
        }
    }
}
 