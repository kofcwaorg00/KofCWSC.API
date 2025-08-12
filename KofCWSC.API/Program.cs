using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KofCWSC.API.Data;
using KofCWSC.API.Controllers;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Net;
using KofCWSC.API.Utils;
using KofCWSC.API.Services;

var builder = WebApplication.CreateBuilder(args);

var kvURLAZ = builder.Configuration.GetSection("KV").GetValue(typeof(string), "KVPROD");
var kvclient = new SecretClient(new Uri((string)kvURLAZ), new DefaultAzureCredential());
var vConnString = kvclient.GetSecret("AZEmailConnString").Value;
string azureConnectionString = vConnString.Value;


// Read the Azure connection string from configuration
//string azureConnectionString = builder.Configuration["Azure:CommunicationService:ConnectionString"];
string fromEmail = builder.Configuration["Azure:CommunicationService:FromEmail"];
string toEmail = builder.Configuration["Azure:CommunicationService:ToEmail"];


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .WriteTo.File("logs/MyAppLog.txt", retainedFileCountLimit: 21, rollingInterval: RollingInterval.Day)
    .WriteTo.Sink(new AzureCommunicationEmailSink(azureConnectionString, fromEmail, toEmail))
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
    object kvURL = null;
    ///////////////////////SecretClient secretClient = null;
    KeyVaultSecret cnString = null;
    Log.Information("Using " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") + " Environment");
    kvURL = builder.Configuration.GetSection("KV").GetValue(typeof(string), "KVPROD");
    /////////////////////////////secretClient = new SecretClient(new Uri((string)kvURL), new DefaultAzureCredential());
    //**************************************************************************************************
    // Secrets for sql server db connect strings
    // DBCONN = KofCWSC sql server KofCWSCWeb
    // AZPROD = KofCWSC sql server KofCWSCWeb
    // AZDEV = KofCWSC sql server KofCWSCWebDev
    // DBCONNLOC = Tim's local sql server KofCWSCWebSite
    // DBCONNLOCMARC = Marcus' local sql server KofCWeb
    //---------------------------------------------------------------------------------------------------
    string myEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();
    switch (myEnv)
    {
        case "production":
            cnString = kvclient.GetSecret("AZPROD").Value;
            break;
        case "development":
            cnString = kvclient.GetSecret("DBCONNLOC").Value;
            break;
        case "test":
            cnString = kvclient.GetSecret("AZDEV").Value;
            break;
        default:
            cnString = kvclient.GetSecret("AZPROD").Value;
            break;
    }
    

    string connectionString = cnString.Value;
    //------------------------------------------------------------------------------------------------------------------------------
    //////////////////var connectionString = builder.Configuration.GetConnectionString("DASPDEVConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    string ipAddress = Helper.GetIPAddress(Dns.GetHostName());
    //Log.Information("Using: " + connectionString);
    Log.Information("Local ip Address is: " + ipAddress);
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

builder.Services.AddScoped<HttpClient, HttpClient>();

//------------------------------------------------------------
// 8/7/2025 Tim Philomeno
// this is needed to support cross origin isses
// the WithOrigins needs to include any development urls too
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "https://localhost:7213",
            "https://kofc-wa.org") // Replace with your frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
        //.AllowCredentials(); // Optional: only needed if using cookies or auth
    });
});
//------------------------------------------------------------


var app = builder.Build();

// Configure the HTTP request pipeline.
//////if (app.Environment.IsDevelopment())
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// added this based on chatgpt recommendation
app.UseRouting(); // add this explicitly

app.UseCors("AllowFrontend"); // Apply the CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
