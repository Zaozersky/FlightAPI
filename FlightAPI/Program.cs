using FlightAPI.Common;
using FlightAPI.Profiles;
using FlightAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Configuration.AddJsonFile("appsettings.json");

var dataSources = builder.Configuration.GetSection("AppSettings").GetValue<string>("DataSources");
var namesApi = new List<string>();

ParseSettings(builder, dataSources, namesApi);

var externAPIs = string.Join(", ", namesApi.ToArray());

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Flights API",
        Description = $"## Aggregation service to search flights from extern API: {externAPIs}",
        Version = "v1",
    });
    var filePath = Path.Combine(AppContext.BaseDirectory, "./FlightAPI.xml");
    options.IncludeXmlComments(filePath);
});

builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(FlightProfile));


builder.Services.AddScoped<IAggregatedFlightService, AggregatedFlightService>();
builder.Services.AddOptions();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {

    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void ParseSettings(WebApplicationBuilder builder, string? dataSources, List<string> namesApi)
{
    if (!string.IsNullOrEmpty(dataSources))
    {
        foreach (string ds in dataSources.Split(";", StringSplitOptions.RemoveEmptyEntries))
        {
            var pairNameWithAddress = ds.Split(": ", StringSplitOptions.RemoveEmptyEntries);
            var nameAPI = pairNameWithAddress[0].Trim();
            var address = pairNameWithAddress[1].Trim();
            namesApi.Add(nameAPI);

            builder.Services.AddHttpClient(nameAPI, c =>
            {
                c.BaseAddress = new Uri(address);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}