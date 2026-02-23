using CalisthenicsStore.Services.Admin.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CalisthenicsStore.Services.Admin
{
    public class SupabaseStorageService : IFileStorageService
    {
        private readonly Supabase.Client client;

        private const string BucketName = "product-images";


            
        public SupabaseStorageService(Supabase.Client client)
        {
            this.client = client;
        }

        public async Task<string?> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fullFileName = Path.GetFileName(file.Name);

            var ext = Path.GetExtension(fullFileName);
            var fileName = Path.GetFileNameWithoutExtension(fullFileName);
            var safeFileName = fileName.ToLower();

            var objectKey = $"products/{safeFileName}{ext}";

            using MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);

            await client.Storage
                .From(BucketName)
                .Upload(ms.ToArray(), objectKey);

            return client.Storage
                .From(BucketName)
                .GetPublicUrl(objectKey);
        }
    }
}
