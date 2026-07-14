using GymManagementSystem.BLL.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Attachments
{
    public interface IAttachmentService
    {
        Task<Result<string>> SaveAsync(IFormFile file, string category, CancellationToken cancellationToken = default);
        Task<Result<Stream>> GetAsync(string storagePath, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(string storagePath, CancellationToken cancellationToken = default);
    }
}
