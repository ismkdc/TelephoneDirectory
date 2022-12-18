using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using TelephoneDirectory.ContactAPI.Records;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.Infrastructure.Errors;

namespace TelephoneDirectory.ContactAPI.Services;

public interface IContactInformationService
{
    public Task Create(CreateContactInformation model);
    public Task Delete(Guid id);
}

public class ContactInformationService : IContactInformationService
{
    private readonly TelephoneDirectoryContext _context;
    private readonly IMapper _mapper;

    public ContactInformationService(TelephoneDirectoryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task  Create(CreateContactInformation model)
    {
        if (string.IsNullOrWhiteSpace(model.Content)) throw new TelephoneDirectoryException(CustomErrors.E_103);

        var isContentExist = await _context.Contacts.AnyAsync(x => x.Id == model.ContactId);
        if (!isContentExist) throw new TelephoneDirectoryException(CustomErrors.E_102);

        var contactInformation = _mapper.Map<ContactInformation>(model);
        _context.ContactInformation.Add(contactInformation);

        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var entity = await _context
            .ContactInformation
            .SingleOrDefaultAsync(x => x.Id == id);

        if (entity == null) throw new TelephoneDirectoryException(CustomErrors.E_102);

        entity.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}