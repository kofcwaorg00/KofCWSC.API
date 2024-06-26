using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KofCWSC.API.Data;
using KofCWSC.API.Controllers;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .WriteTo.File("logs/MyAppLog.txt", retainedFileCountLimit: 21, rollingInterval: RollingInterval.Day)
    .CreateLogger();

//Log.Information("Serilog is Initialized");

//******************************************************************************************************************************
// 6/6/2024 Tim Philomneo
//  Getting the connect string.  We have switched from the local appsettings.json to using KeyVault.
//  we have 2 vaults, PROD and DEV (see the KV section in appsettings.json  Let's keep the local connection
//  but to use it you will need to comment out the next 4 lines and uncoment the 5th one.
//  Securtiy to KeyVault is handled by the DefaultAzureCredential.  It will use your VS login if you are running
//  in Visual Studio or the Azure Application Identity when published.
//******************************************************************************************************************************
try
{
    var kvURL = builder.Configuration.GetSection("KV").GetValue(typeof(string), "KVDev");
    var client = new SecretClient(new Uri((string)kvURL), new DefaultAzureCredential());
    var cnString = client.GetSecret("AZDEV").Value;
    string connectionString = cnString.Value;

    //------------------------------------------------------------------------------------------------------------------------------
    //////////////////var connectionString = builder.Configuration.GetConnectionString("DASPDEVConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    Log.Information("Using: " + connectionString);
    //------------------------------------------------------------------------------------------------------------------------------
    // make sure we have a value from KeyVault. if not throw an exception
    if (connectionString.IsNullOrEmpty()) throw new Exception("APIURL is not defined");
    //------------------------------------------------------------------------------------------------------------------------------

    //builder.Services.AddDbContext<KofCWSCAPIDBContext>(options =>
    //    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection") ?? throw new InvalidOperationException("Connection string 'KofCWSCAPIDBContext' not found.")));
    builder.Services.AddDbContext<KofCWSCAPIDBContext>(options =>
        options.UseSqlServer(connectionString));

}
catch (Exception ex)
{
    Log.Error(ex.Message);
    throw new Exception(ex.Message); ;
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
//////if (app.Environment.IsDevelopment())
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
