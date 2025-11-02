using System.Net.Http.Json;
using CalisthenicsStore.Data.Models.ReCaptcha;
using CalisthenicsStore.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CalisthenicsStore.Services
{
    public class ReCaptchaServ : IReCaptchaServ
    {
        private readonly HttpClient http;

        private readonly GoogleReCaptchaSettings captchaSett;

        public ReCaptchaServ(HttpClient http, IOptions<GoogleReCaptchaSettings> captchaSett)
        {
            this.http = http;
            this.captchaSett = captchaSett.Value;
        }

        private sealed class ReCaptchaResponse
        {
            public bool Success { get; set; }
            public List<string> ErrorCodes { get; set; } = new();
        }

        public async Task<bool> VerifyAsync(string? token, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;

            var url = $"https://www.google.com/recaptcha/api/siteverify?secret={captchaSett.SecretKey}&response={token}";
            using var res = await http.PostAsync(url, content: null, ct);
            if (!res.IsSuccessStatusCode) return false;

            var body = await res.Content.ReadFromJsonAsync<ReCaptchaResponse>(cancellationToken: ct);
            return body?.Success == true;
        }
    }
}
