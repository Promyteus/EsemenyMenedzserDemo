namespace EsemenyMenedzser.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool IsSuccess, string Message, string? Email)> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
