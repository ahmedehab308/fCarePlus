using System.ComponentModel.DataAnnotations;

namespace fCarePlus.API.DTOs
{
    public class JournalVoucherInputModel
    {
        [Required(ErrorMessage = "Journal date is required")]
        public DateTime JournalDate { get; set; }


        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public required string JournalDescription { get; set; }


        [Range(0.01, double.MaxValue, ErrorMessage = "Total debit must be non-negative")]
        public decimal TotalDebit { get; set; }


        [Range(0.01, double.MaxValue, ErrorMessage = "Total credit must be non-negative")]
        public decimal TotalCredit { get; set; }


        [MinLength(1, ErrorMessage = "At least one detail is required")]
        public List<JournalDetailInputDto> Details { get; set; } = new List<JournalDetailInputDto>();
    }
}