using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using OpenAI.Examples;
using OpenAIServer.Data;
using System;
using static OpenAIServer.Data.OpenAIServerContext;
using Microsoft.Extensions.DependencyInjection;

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
builder.Services.AddScoped<AiServiceVectorStore>();

// Add IP Rate Limiting
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Add the rate limit middleware
builder.Services.AddOptions();
builder.Services.AddMemoryCache();

// Add Rate Limiting policies
builder.Services.Configure<IpRateLimitOptions>(_configuration.GetSection("IpRateLimiting"));

var app = builder.Build();

// 2) (Optional) redirect to HTTPS
app.UseHttpsRedirection();

// 3) Routing must come before MapControllers
app.UseStaticFiles();
app.UseRouting();

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

// 6) (Optional) static pages, razor, etc.
// Enable IP rate limiting
app.UseIpRateLimiting();

app.Run();

