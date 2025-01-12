using CourseBundleSystem.Constants;
using CourseBundleSystem.Middlewares;
using CourseBundleSystem.Models;
using CourseBundleSystem.Services;
using CourseBundleSystem.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ProviderTopicsConfig>(options =>
{
    options.ProviderTopics = builder.Configuration.GetSection(ConstantValues.ProviderTopics).Get<Dictionary<string, string>>();
});

builder.Services.AddScoped<IQuotesService, QuotesService>();

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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
