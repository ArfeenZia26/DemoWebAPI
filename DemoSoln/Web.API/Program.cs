using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Extensions.Logging;
using System.Net.Http.Headers;
using webApi.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ILoggerProvider>((sLog) =>
{
    Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .WriteTo.File(
                "log/webApiLog.txt",
                rollingInterval: RollingInterval.Day
            )
            .CreateLogger();

    return new SerilogLoggerProvider(Log.Logger, true);

});
builder.Services.AddControllers();
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = ApiVersion.Default;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new MediaTypeApiVersionReader("version"),
        new HeaderApiVersionReader("x-version")
    );
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WebApiDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
builder.Services.AddSingleton<WebApiInMemoryDB>();
var app = builder.Build();

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
