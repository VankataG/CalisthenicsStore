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

            var ext = Path.GetExtension(file.Name);
            var fileName = $"{Guid.NewGuid}{ext}";

            using MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);

            await client.Storage
                .From(BucketName)
                .Upload(ms.ToArray(), fileName);

            return client.Storage
                .From(BucketName)
                .GetPublicUrl(fileName);
        }
    }
}
