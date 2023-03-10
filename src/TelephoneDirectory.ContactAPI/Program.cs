using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using TelephoneDirectory.ContactAPI;
using TelephoneDirectory.ContactAPI.Services;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.Data.Extensions;
using TelephoneDirectory.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency Injection

//register db context
builder.Services.AddDbContext(
    builder.Configuration.GetConnectionString("POSTGRESQL_CONNECTION")
);

//register mapster
var config = MappingConfiguration.Generate();

builder.Services.AddSingleton(config);
builder.Services.AddSingleton<IMapper, ServiceMapper>();

//register our services
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IContactInformationService, ContactInformationService>();

#endregion

var app = builder.Build();

// Ensure db is created!
using var serviceScope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<TelephoneDirectoryContext>();
context.Database.Migrate();

//register middlewares
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();