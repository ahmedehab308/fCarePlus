using fCarePlus.API.Data.Interfaces;
using fCarePlus.API.DTOs;
using fCarePlus.API.Services.Interfaces;
using fCarePlus.API.Data.Entities;

namespace fCarePlus.API.Services
{
    public class JournalService : IJournalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public JournalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SaveJournalVoucherAsync(JournalVoucherInputModel model)
        {
            decimal calculatedDebit = model.Details.Sum(d => d.DebitAmount);
            decimal calculatedCredit = model.Details.Sum(d => d.CreditAmount);

            if (calculatedDebit != calculatedCredit || calculatedDebit <= 0)
            {
                return false;
            }

            var header = new JournalHeader
            {
                JournalDate = DateOnly.FromDateTime(model.JournalDate),
                JournalDescription = model.JournalDescription,
                TotalDebit = calculatedDebit,
                TotalCredit = calculatedCredit,
                IsDeleted = false
            };

            await _unitOfWork.Repository<JournalHeader>().AddAsync(header);

            foreach (var detailDto in model.Details)
            {
                bool debitNonZero = detailDto.DebitAmount != 0;
                bool creditNonZero = detailDto.CreditAmount != 0;

                if ((debitNonZero && creditNonZero) || (!debitNonZero && !creditNonZero))
                {
                    return false; 
                }

                var detail = new JournalDetail
                {
                    AccountId = detailDto.AccountId,
                    DebitAmount = detailDto.DebitAmount,
                    CreditAmount = detailDto.CreditAmount,
                    DetailStatement = detailDto.DetailStatement,
                    IsDeleted = false,
                    Journal = header
                };
                await _unitOfWork.Repository<JournalDetail>().AddAsync(detail);
            }

            int changes = await _unitOfWork.CompleteAsync();

            return changes > 0;
        }
    }
}