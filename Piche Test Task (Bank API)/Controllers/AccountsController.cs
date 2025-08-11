using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _svc;
        public AccountsController(IAccountService svc) => _svc = svc;

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountDto dto, CancellationToken ct)
        {
            var result = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { accountNumber = result.AccountNumber }, result);
        }

        [HttpGet("{accountNumber}")]
        public async Task<IActionResult> Get(string accountNumber, CancellationToken ct)
        {
            var a = await _svc.GetAsync(accountNumber, ct);
            return a == null ? NotFound() : Ok(a);
        }

        [HttpGet]
        public async Task<IActionResult> List(CancellationToken ct)
        {
            var list = await _svc.ListAsync(ct);
            return Ok(list);
        }
    }
}
