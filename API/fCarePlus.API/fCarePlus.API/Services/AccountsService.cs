using fCarePlus.API.Data.Interfaces;
using fCarePlus.API.DTOs;
using fCarePlus.API.Services.Interfaces;

namespace fCarePlus.API.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AccountSearchDto>> SearchAccountsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                return new List<AccountSearchDto>();
            }

            string lowerQuery = query.ToLower();

            var accounts = await _unitOfWork.Accounts.FindAsync(
                a => a.AllowEntry == true && 
                     (a.NameAr.ToLower().Contains(lowerQuery) || 
                      a.Number.Contains(query)) 
            );

            var result = accounts.Select(a => new AccountSearchDto
            {
                Id = a.Id,
                NameAR = a.NameAr,
                Number = a.Number
            }).ToList();

            return result;
        }
    }
}