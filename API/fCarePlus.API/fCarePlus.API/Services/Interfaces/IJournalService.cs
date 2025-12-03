using fCarePlus.API.DTOs;

namespace fCarePlus.API.Services.Interfaces
{
    public interface IJournalService
    {
        Task<bool> SaveJournalVoucherAsync(JournalVoucherInputModel model);
    }
}