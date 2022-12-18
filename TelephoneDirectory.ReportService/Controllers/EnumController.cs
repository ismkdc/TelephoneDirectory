using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.ReportService.Controllers;

[ApiController]
[Route("api/enums")]
public class EnumController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var reportStatusEnum = Enum.GetValues(typeof(ReportStatusEnum))
            .Cast<ReportStatusEnum>()
            .ToDictionary(t => (int)t, t => t.ToString());

        return Ok(
            new { reportStatusEnum }
        );
    }
}