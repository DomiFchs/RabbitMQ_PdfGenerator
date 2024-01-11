using Domain.Repositories.Implementations;
using Domain.Repositories.Interfaces;
using MessageQueueService.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Model.Configurations;
using Model.Entities;
using View.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<IRepository<ToDo>,ToDoRepository>();
builder.Services.AddScoped<MessageService>();

builder.Services.AddDbContextFactory<ToDoDbContext>(
    options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 31))
        ).UseLoggerFactory(new NullLoggerFactory()),
    ServiceLifetime.Transient
);



var app = builder.Build();

app.Urls.Add("http://0.0.0.0:80");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();  
var connected = false;  

while (!connected)  
{  
    if (!dbContext.Database.CanConnect())  
    {        
        await Task.Delay(1_000);  
        continue;  
    }
	  
    dbContext.Database.Migrate();  
    connected = true;  
}

app.Run();