using EasyNetQ;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.Data.Enums;
using TelephoneDirectory.Data.Messages;
using TelephoneDirectory.ReportAPI.Records;

namespace TelephoneDirectory.ReportAPI.Services;

public interface IReportService
{
    Task Generate();
    Task<GetReport[]> GetAll();
    Task Complete(ReportMessage message);
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

    public async Task Generate()
    {
        var report = new Report
        {
            ReportStatus = ReportStatusEnum.Processing
        };

        _context.Reports.Add(report);
        await _context.SaveChangesAsync();

        await _bus.PubSub.PublishAsync(new ReportMessage(report.Id, ""));
    }

    public Task<GetReport[]> GetAll()
    {
        return _context
            .Reports
            .ProjectToType<GetReport>(_mapper.Config)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task Complete(ReportMessage message)
    {
        var report = _context
            .Reports
            .SingleOrDefault(x => x.Id == message.Id);

        if (report != null && !string.IsNullOrEmpty(message.FilePath))
        {
            report.FilePath = message.FilePath;
            report.ReportStatus = ReportStatusEnum.Completed;

            await _context.SaveChangesAsync();
        }
    }
}