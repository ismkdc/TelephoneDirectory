using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.Data.Enums;
using TelephoneDirectory.Data.Messages;

var rabbitMqConnection = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION");
var postgresConnection = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION");

var contextOptions = new DbContextOptionsBuilder<TelephoneDirectoryContext>()
    .UseNpgsql(postgresConnection)
    .Options;

var bus = RabbitHutch.CreateBus(rabbitMqConnection,
    x => x.EnableSystemTextJson()
);
var context = new TelephoneDirectoryContext(contextOptions);

await bus.PubSub.SubscribeAsync<ReportMessage>(
    Environment.MachineName, _ => GenerateReport()
);

Console.ReadKey();

async Task GenerateReport()
{
    Console.WriteLine("Generating report...");
    var report = new Report
    {
        ReportStatus = ReportStatusEnum.Processing
    };

    context.Reports.Add(report);
    await context.SaveChangesAsync();

    report.Content = await context.ContactInformation
        .Where(x => x.ContactInformationType == ContactInformationTypeEnum.Location)
        .GroupBy(x => x.Content)
        .Select(x => new ReportContent
        {
            Location = x.Key,
            Contacts = x.Select(y => new ReportContentDetail { Name = y.Contact.Name, Surname = y.Contact.Surname })
        })
        .AsNoTracking()
        .ToArrayAsync();

    report.ReportStatus = ReportStatusEnum.Completed;
    await context.SaveChangesAsync();

    Console.WriteLine("Report generated.");
}