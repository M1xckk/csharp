using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.OpenApi;
using BuzzReel.Web.Models; 
using BuzzReel.Web.Hubs; 

var builder = WebApplication.CreateBuilder(args);

var PGHOST = Environment.GetEnvironmentVariable("ep-long-bar-a627t1vd.us-west-2.aws.neon.tech");
var PGDATABASE = Environment.GetEnvironmentVariable("neondb");
var PGUSER = Environment.GetEnvironmentVariable("neondb_owner");
var PGPASSWORD = Environment.GetEnvironmentVariable("YHRs6dkqoFS7");
var connectionString = $"Host={"ep-long-bar-a627t1vd.us-west-2.aws.neon.tech"};Database={"neondb"};Username={"neondb_owner"};Password={"YHRs6dkqoFS7"}";

builder.Services.AddDbContext<DatabaseContext>(
    opt =>
    {
        opt.UseNpgsql(connectionString);
        if (builder.Environment.IsDevelopment())
        {
            opt
              .LogTo(Console.WriteLine, LogLevel.Information)
              .EnableSensitiveDataLogging()
              .EnableDetailedErrors();
        }
    }
);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder done, let's build it
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/test", () => "Hello World!");
app.MapControllers();
app.MapHub<MovieHub>("/r/movieshub"); 

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();



