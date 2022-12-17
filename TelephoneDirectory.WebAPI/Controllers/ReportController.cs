using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.WebAPI.Services;

namespace TelephoneDirectory.WebAPI.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await _reportService.Get(id);
        return Ok(result);
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
}