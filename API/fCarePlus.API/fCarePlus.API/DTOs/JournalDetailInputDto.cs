
using System.ComponentModel.DataAnnotations;

namespace fCarePlus.API.DTOs
{

    public class JournalDetailInputDto
    {
        [Required(ErrorMessage = "Account is required")]
        public Guid AccountId { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Debit amount must be positive")]
        public decimal DebitAmount { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Credit  amount must be positive")]
        public decimal CreditAmount { get; set; }

        [StringLength(500, ErrorMessage = "Detail statement cannot exceed 500 characters")]
        public string? DetailStatement { get; set; }
    }
}