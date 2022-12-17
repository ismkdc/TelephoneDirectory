using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.Data.Enums;
using TelephoneDirectory.Data.Messages;

#if DEBUG
var rabbitMqConnection = "host=localhost";
var postgresConnection =
    "Host=localhost;Database=phonedirectory_db;Username=phonedirectory_usr;Password=PZLqwVFf8YkwqRhq?PZLqwVFf8Y_dev";
#else
var rabbitMqConnection = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION");
var postgresConnection = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION");
#endif

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
            ContactCount = x.Count(),
            PhoneNumberCount = context
                .ContactInformation
                // Sum of all phone numbers for each location
                .Sum(y => y.ContactInformationType == ContactInformationTypeEnum.PhoneNumber &&
                          x.Select(c => c.ContactId).Contains(y.ContactId)
                    ? 1
                    : 0)
        })
        .AsNoTracking()
        .ToArrayAsync();

    report.ReportStatus = ReportStatusEnum.Completed;
    await context.SaveChangesAsync();

    Console.WriteLine("Report generated.");
}