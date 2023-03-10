using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using TelephoneDirectory.ContactAPI.Records;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.Data.Enums;
using TelephoneDirectory.Data.Records;
using TelephoneDirectory.Infrastructure.Errors;

namespace TelephoneDirectory.ContactAPI.Services;

public interface IContactService
{
    public Task<GetContactDetail?> Get(Guid id);
    public Task<GetContact[]> GetAll();
    Task<GetReportContent[]> GetReportData();
    public Task Create(CreateContact model);
    public Task Delete(Guid id);
}

public class ContactService : IContactService
{
    private readonly TelephoneDirectoryContext _context;
    private readonly IMapper _mapper;

    public ContactService(TelephoneDirectoryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<GetContactDetail?> Get(Guid id)
    {
        return _context
            .Contacts
            .Include(x => x.ContactInformation)
            .Where(x => x.Id == id)
            .ProjectToType<GetContactDetail>(_mapper.Config)
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }

    public Task<GetContact[]> GetAll()
    {
        return _context
            .Contacts
            .ProjectToType<GetContact>(_mapper.Config)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public Task<GetReportContent[]> GetReportData()
    {
        return _context.ContactInformation
            .Where(x => x.ContactInformationType == ContactInformationTypeEnum.Location)
            .GroupBy(x => x.Content)
            .Select(x => new GetReportContent
            (
                x.Key,
                x.Count(),
                _context
                    .ContactInformation
                    // Count of all phone numbers for each location
                    .Count(y => y.ContactInformationType == ContactInformationTypeEnum.PhoneNumber &&
                                x.Select(c => c.ContactId).Contains(y.ContactId))
            ))
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task Create(CreateContact model)
    {
        if (string.IsNullOrWhiteSpace(model.Name)) throw new TelephoneDirectoryException(CustomErrors.E_101);

        var entity = _mapper.Map<Contact>(model);
        _context.Contacts.Add(entity);

        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var entity = await _context
            .Contacts
            .SingleOrDefaultAsync(x => x.Id == id);

        if (entity == null) throw new TelephoneDirectoryException(CustomErrors.E_102);

        await _context
            .ContactInformation
            .Where(x => x.Contact == entity)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.DeletedAt, DateTime.UtcNow));

        entity.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}