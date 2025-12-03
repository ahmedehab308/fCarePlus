using fCarePlus.API.DTOs;

namespace fCarePlus.API.Services.Interfaces
{
    public interface IAccountsService
    {
        Task<IEnumerable<AccountSearchDto>> SearchAccountsAsync(string query);
    }
}