using fCarePlus.API.DTOs;
using fCarePlus.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fCarePlus.API.Controllers
{
    [ApiController]
    [Route("api/journal")] 
    public class JournalVoucherController : ControllerBase
    {
        private readonly IJournalService _journalService;

        public JournalVoucherController(IJournalService journalService)
        {
            _journalService = journalService;
        }

        [HttpPost("save")]
        
        public async Task<IActionResult> SaveVoucher([FromBody] JournalVoucherInputModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                var errors = ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage)
               .ToList();

                return BadRequest(new { Message = "The voucher data is invalid", Errors = errors });
            }

            bool success = await _journalService.SaveJournalVoucherAsync(model);

            if (!success)
            {
                return BadRequest(" Failed to save the voucher. Please ensure that the total debit equals the total credit, and that at least one of them has a value");
            }

            return Ok(new { Message = "Voucher saved successfully" });
        }
    }
}