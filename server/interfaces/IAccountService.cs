using server.models;
using server.models.responses;

namespace server.interfaces
{
    public interface IAccountService
    {
        Task<string> LoginAsync(AccountLoginDTO request);
        Task LogoutAsync();
        Task RegisterAsync(AccountRegistrationDTO request);
        Task<GetAccountResponse> GetAccountAsync(int accountId);
    }
}
