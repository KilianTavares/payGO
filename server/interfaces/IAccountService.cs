using server.models.responses;

namespace server.interfaces
{
    public interface IAccountService
    {
        Task<string> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task RegisterAsync(string username, string password);
        Task<GetAccountResponse> GetAccountAsync(int accountId);
    }
}