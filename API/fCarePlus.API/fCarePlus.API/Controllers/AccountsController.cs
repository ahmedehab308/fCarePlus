using Microsoft.AspNetCore.Http;
using fCarePlus.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fCarePlus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var accounts = await _accountsService.SearchAccountsAsync(query);

            return Ok(accounts);
        }
    }
}
