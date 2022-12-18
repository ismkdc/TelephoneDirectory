using EasyNetQ;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.Data.Extensions;
using TelephoneDirectory.Infrastructure.Middlewares;
using TelephoneDirectory.ReportService;
using TelephoneDirectory.ReportService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency Injection

// Register db context
builder.Services.AddDbContext(
    builder.Configuration.GetConnectionString("POSTGRESQL_CONNECTION")
);

// Register rabbitmq
builder.Services.RegisterEasyNetQ(
    builder.Configuration.GetConnectionString("RABBITMQ_CONNECTION"),
    x => x.EnableSystemTextJson()
);

// Register mapster
var config = MappingConfiguration.Generate();

builder.Services.AddSingleton(config);
builder.Services.AddSingleton<IMapper, ServiceMapper>();

// Register our services
builder.Services.AddScoped<IReportService, ReportService>();

#endregion

var app = builder.Build();

// Ensure db is created!
using var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<TelephoneDirectoryContext>();
context.Database.Migrate();

// Register middlewares
app.UseMiddleware<ExceptionMiddleware>();

// Expose file path
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(app.Configuration.GetValue<string>("ReportPath")),
    RequestPath = "/Reports"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();