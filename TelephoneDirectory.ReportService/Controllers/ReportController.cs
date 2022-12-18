using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.Data.Messages;
using TelephoneDirectory.ReportService.Services;

namespace TelephoneDirectory.ReportService.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _reportService.GetAll();
        return Ok(result);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate()
    {
        await _reportService.Generate();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Complete([FromBody] ReportMessage message)
    {
        await _reportService.Complete(message);
        return Ok();
    }
}