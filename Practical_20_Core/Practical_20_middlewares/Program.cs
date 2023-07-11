using Microsoft.AspNetCore.Mvc;
using Practical_20_middlewares.Middlewares;
using Serilog;
using UnitOfWork.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var tableName = builder.Configuration["AppSettings:LogsTableName"];
var schemaName = builder.Configuration["AppSettings:LogsSchemaName"];


// configs for serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.Console()
    .WriteTo.File("logs/DailyLogs.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(connectionString, sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions { TableName = tableName, SchemaName = schemaName, AutoCreateSqlTable = true })
    .CreateLogger();

// Use Serilog service
builder.Host.UseSerilog();

builder.Services
    .AddControllers(options =>
    {
        options.ReturnHttpNotAcceptable = true;
    })
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddDIServices(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();
app.MapControllers();
app.Run();
