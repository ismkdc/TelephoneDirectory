using MapsterMapper;
using TelephoneDirectory.Data.Extensions;
using TelephoneDirectory.WebAPI;
using TelephoneDirectory.WebAPI.Middlewares;
using TelephoneDirectory.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency Injection

//register db context
builder.Services.AddDbContext(
    builder.Configuration.GetConnectionString("TelephoneDirectoryPostgresql")
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

//register middlewares
app.UseMiddleware<ExceptionMiddleware>();

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