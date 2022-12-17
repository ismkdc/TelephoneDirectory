using EasyNetQ;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.Data.Enums;
using TelephoneDirectory.Data.Errors;
using TelephoneDirectory.Data.Messages;
using TelephoneDirectory.ReportService.Records;

namespace TelephoneDirectory.ReportService.Services;

public interface IReportService
{
    Task<GetReportDetail> Get(Guid id);
    Task Generate();
    Task<GetReport[]> GetAll();
}

public class ReportService : IReportService
{
    private readonly IBus _bus;
    private readonly TelephoneDirectoryContext _context;
    private readonly IMapper _mapper;

    public ReportService(TelephoneDirectoryContext context, IMapper mapper, IBus bus)
    {
        _context = context;
        _mapper = mapper;
        _bus = bus;
    }

    public async Task<GetReportDetail> Get(Guid id)
    {
        var entity = await _context
            .Reports
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id && x.ReportStatus == ReportStatusEnum.Completed);

        if (entity == null) throw new TelephoneDirectoryException(CustomErrors.E_102);

        return _mapper.Map<GetReportDetail>(entity.Content);
    }

    public Task Generate()
    {
        return _bus.PubSub.PublishAsync(new ReportMessage());
    }

    public Task<GetReport[]> GetAll()
    {
        return _context
            .Contacts
            .ProjectToType<GetReport>(_mapper.Config)
            .AsNoTracking()
            .ToArrayAsync();
    }
}