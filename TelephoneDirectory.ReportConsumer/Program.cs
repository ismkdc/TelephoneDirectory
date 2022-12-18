using System.Net.Http.Json;
using ClosedXML.Excel;
using EasyNetQ;
using TelephoneDirectory.Data.Messages;
using TelephoneDirectory.Data.Records;

#region Configuration

#if DEBUG
const string contactServiceBaseUrl = "http://localhost:5164";
const string reportServiceBaseUrl = "http://localhost:5138";
const string reportDirectory = "/reports";

const string rabbitMqConnection = "host=localhost";
#else
const string contactServiceBaseUrl = Environment.GetEnvironmentVariable("CONTACT_SERVICE_BASE_URL");
const string reportServiceBaseUrl = Environment.GetEnvironmentVariable("REPORT_SERVICE_BASE_URL");
const string reportDirectory = Environment.GetEnvironmentVariable("REPORT_DIRECTORY");

const string rabbitMqConnection = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION");
#endif

#endregion

var bus = RabbitHutch.CreateBus(rabbitMqConnection,
    x => x.EnableSystemTextJson()
);

await bus.PubSub.SubscribeAsync<ReportMessage>(
    Environment.MachineName, GenerateReport
);

Console.ReadKey();

async Task GenerateReport(ReportMessage message)
{
    Console.WriteLine($"TASK: Generating report -- Date: ${DateTime.UtcNow} -- Id: ${message.Id}");

    using var httpClient = new HttpClient();
    var response =
        await httpClient.GetFromJsonAsync<GetReportContent[]>($"{contactServiceBaseUrl}/api/contacts/getReportData");

    var workbook = new XLWorkbook();
    var wsDetailedData = workbook.AddWorksheet("report");
    wsDetailedData.Cell(1, 1).InsertTable(response);

    var filePath = $"{reportDirectory}/{Guid.NewGuid()}.xlsx";
    workbook.SaveAs(filePath);

    await httpClient.PutAsJsonAsync($"{reportServiceBaseUrl}/api/reports", message with
    {
        FilePath = filePath
    });

    Console.WriteLine(
        $"TASK: Report generated -- Date: ${DateTime.UtcNow} -- Id: ${message.Id} -- FilePath: ${filePath}");
}