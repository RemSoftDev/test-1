using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test2.Models;
using Test2.Services;

namespace Test2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] FilterModel filter)
        {
            return Ok(await _transactionsService.GetAsync(filter));
        }
    }
}
