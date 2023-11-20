using Microsoft.EntityFrameworkCore;
using OllamaTot.Models;
using OllamaTot.Utilities;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OllamaTot");
builder.Services.AddDbContext<OllamaTotContext>(options =>
  options.UseNpgsql(connectionString));

builder.Services.AddHttpClient();
builder.Services.AddTransient<Tot>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
