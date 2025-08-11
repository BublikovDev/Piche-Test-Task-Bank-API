using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IAccountService _svc;
        public TransactionsController(IAccountService svc) => _svc = svc;

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositDto dto, CancellationToken ct)
        {
            await _svc.DepositAsync(dto, ct);
            return NoContent();
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawDto dto, CancellationToken ct)
        {
            await _svc.WithdrawAsync(dto, ct);
            return NoContent();
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferDto dto, CancellationToken ct)
        {
            await _svc.TransferAsync(dto, ct);
            return NoContent();
        }
    }
}
