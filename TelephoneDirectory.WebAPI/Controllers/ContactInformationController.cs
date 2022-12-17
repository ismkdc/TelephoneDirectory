using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.WebAPI.Records;
using TelephoneDirectory.WebAPI.Services;

namespace TelephoneDirectory.WebAPI.Controllers;

[ApiController]
[Route("api/contactInformation")]
public class ContactInformationController : ControllerBase
{
    private readonly IContactInformationService _contactInformationService;

    public ContactInformationController(IContactInformationService contactInformationService)
    {
        _contactInformationService = contactInformationService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactInformation model)
    {
        await _contactInformationService.Create(model);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _contactInformationService.Delete(id);
        return Ok();
    }
}