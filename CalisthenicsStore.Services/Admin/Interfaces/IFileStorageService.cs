using Microsoft.AspNetCore.Http;

namespace CalisthenicsStore.Services.Admin.Interfaces
{
    public interface IFileStorageService
    {
        Task<string?> UploadImageAsync(IFormFile file);
    }
}
