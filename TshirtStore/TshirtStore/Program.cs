using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TshirtStore.Extentions;
using FluentValidation.AspNetCore;
using FluentValidation;
using TshirtStore.Middleware;
using TshirtStore.HealthChecks;
using MediatR;
using TshirtStore.BL.CommandHandlers;

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//serilog
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services
    .RegisterServices()
    .RegisterRepositories()
    .AddAutoMapper(typeof(Program));

// MediatR
builder.Services.AddMediatR(typeof(AddClientCommandHandler).Assembly);

// Fluen validation
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<CustomExeption>("Custom Check");
    //.AddUrlGroup(new Uri("https://google.com"), name: "Google Service");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Health Checks
app.RegisterHealthChecks();
app.MapHealthChecks("/healthz");

// Middleware
app.UseMiddleware<CustomMiddlewareErrorHandler>();

app.Run();
