using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using SocialFeedAPI.Common.Middlewares.HttpClient;
using SocialFeedAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IRedditAPIService, RedditAPIService>();
builder.Services.AddTransient<RequestLimitHandler>();
builder.Services.AddHttpClient("RedditAPIService", c =>
{
    c.BaseAddress = new Uri("https://oauth.reddit.com");
    c.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "RedditAPIService");
}).AddHttpMessageHandler<RequestLimitHandler>();

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
