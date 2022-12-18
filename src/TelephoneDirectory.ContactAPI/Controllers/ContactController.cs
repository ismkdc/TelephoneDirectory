using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.ContactAPI.Records;
using TelephoneDirectory.ContactAPI.Services;

namespace TelephoneDirectory.ContactAPI.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await _contactService.Get(id);
        return Ok(result);
    }

    [HttpGet("getReportData")]
    public async Task<IActionResult> GetReportData()
    {
        var result = await _contactService.GetReportData();
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _contactService.GetAll();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContact model)
    {
        await _contactService.Create(model);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _contactService.Delete(id);
        return Ok();
    }
}