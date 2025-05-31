using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using OpenAI.Examples;
using OpenAIServer.Data;
using System;
using static OpenAIServer.Data.OpenAIServerContext;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using System.Net.Http.Headers;

var _configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OpenAIServerContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

// 1) Register your services
builder.Services.AddControllers();

builder.Services.AddHttpClient("OpenAI", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/");
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", builder.Configuration["OpenAI:APIKey"]);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped<AiServiceVectorStore>();


// Add IP Rate Limiting
// Load configuration
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddOptions();

var app = builder.Build();

// 2) (Optional) redirect to HTTPS
app.UseHttpsRedirection();

// 3) Routing must come before MapControllers
app.UseStaticFiles();
app.UseRouting();

// Enable IP rate limiting
app.UseIpRateLimiting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=ERStats}/{action=Index}/{id?}");
});

// 4) auth goes here
//app.UseAuthorization();

// 5) Map API controllers
app.MapControllers();


app.Run();

