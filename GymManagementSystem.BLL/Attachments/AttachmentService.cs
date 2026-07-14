using GymManagementSystem.BLL.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string _root;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly string[] _allowedMimeTypes = { "image/jpeg", "image/jpg", "image/png" };

        public AttachmentService(string rootPath)
        {
            _root = rootPath;
        }

        public async Task<Result<string>> SaveAsync(IFormFile file, string category, CancellationToken cancellationToken = default)
        {
            var validation = await ValidateImageAsync(file, cancellationToken);
            if (!validation.Success) return Result<string>.Fail(validation.Error);

            var ext = NormalizedExtension(Path.GetExtension(file.FileName));
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var dir = Path.Combine(_root, category);

            Directory.CreateDirectory(dir);

            await using var stream = new FileStream(Path.Combine(dir, fileName), FileMode.CreateNew);
            await file.CopyToAsync(stream, cancellationToken);

            return Result<string>.Ok($"{category}/{fileName}");
        }

        public string ToFullPath(string storagePath)
        {
            return Path.Combine(_root, storagePath.Replace('/', Path.DirectorySeparatorChar));
        }

        private string NormalizedExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension)) return string.Empty;
            var cleanedExt = extension.ToLowerInvariant();
            return cleanedExt == ".jpeg" ? ".jpg" : cleanedExt;
        }

        public async Task<Result> DeleteAsync(string storagePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(storagePath))
                return Result.Fail("Storage path is empty.");

            var fullPath = ToFullPath(storagePath);

            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return Result.Ok();
                }
                return Result.Fail("File not found on server.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Delete failed: {ex.Message}");
            }
        }

        public async Task<Result<Stream>> GetAsync(string storagePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(storagePath))
                return Result<Stream>.Fail("Storage path is empty.");

            var fullPath = ToFullPath(storagePath);

            if (!File.Exists(fullPath))
                return Result<Stream>.Fail("File not found.");

            try
            {
                var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return Result<Stream>.Ok(stream);
            }
            catch (Exception ex)
            {
                return Result<Stream>.Fail($"Could not read file: {ex.Message}");
            }
        }


        private async Task<Result> ValidateImageAsync(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return Result.Fail("Image file is empty.");
            }


            if (file.Length > AttachmentRules.MaxBytes)
            {
                return Result.Fail("Image file size exceeds the maximum allowed size.");
            }


            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(ext))
            {
                return Result.Fail("Invalid file extension. Only .jpg, .jpeg, and .png are allowed.");
            }


            if (!_allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                return Result.Fail("Invalid image content type.");
            }


            return await Task.FromResult(Result.Ok());
        }
    }
}
