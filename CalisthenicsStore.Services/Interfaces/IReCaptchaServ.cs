namespace CalisthenicsStore.Services.Interfaces
{
    public interface IReCaptchaServ
    {
        Task<bool> VerifyAsync(string? token, CancellationToken ct = default);
    }
}
