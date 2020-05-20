using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test2.Models;
using Test2.Services;

namespace Test2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IParseService _parseService;
        private readonly ITransactionsService _transactionsService;

        public UploadController(IParseService parse, ITransactionsService transactions)
        {
            _parseService = parse;
            _transactionsService = transactions;
        }

        [HttpPost]
        public async Task<ActionResult<ICollection<ReportItem>>> Post()
        {
            var fileToUpload = HttpContext.Request.Form.Files.FirstOrDefault();
            if (fileToUpload == null) return BadRequest();

            var result = await _parseService.Parse(fileToUpload.FileName, fileToUpload.OpenReadStream());
            if (result.Report.Status == ReportStatus.Done)
            {
                await _transactionsService.Upload(result.Transactions);
            }

            if (result.Report.Status == ReportStatus.Done)
                return Ok(result.Report.Items);
            return BadRequest(result.Report.Items);
        }
    }
}