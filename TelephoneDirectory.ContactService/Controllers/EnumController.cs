using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.ContactService.Controllers;

[ApiController]
[Route("api/enums")]
public class EnumController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var contactInformationTypeEnum = Enum.GetValues(typeof(ContactInformationTypeEnum))
            .Cast<ContactInformationTypeEnum>()
            .ToDictionary(t => (int)t, t => t.ToString());

        return Ok(
            new { contactInformationTypeEnum }
        );
    }
}