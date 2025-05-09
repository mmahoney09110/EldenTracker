using OpenAI.Examples;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddRazorPages();
// 1) Register your services
builder.Services.AddControllers();
builder.Services.AddScoped<AiServiceVectorStore>();

var app = builder.Build();

// 2) (Optional) redirect to HTTPS
app.UseHttpsRedirection();

// 3) Routing must come before MapControllers
app.UseRouting();

// 4) auth goes here
//app.UseAuthorization();

// 5) Map API controllers
app.MapControllers();

// 6) (Optional) static pages, razor, etc.
//app.MapStaticAssets();
//app.MapRazorPages().WithStaticAssets();

app.Run();
