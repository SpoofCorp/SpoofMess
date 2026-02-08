using Microsoft.EntityFrameworkCore;
using SettingsHelper;
using SpoofFileService.Models;
using SpoofFileService.ServiceRealizations;
using SpoofFileService.ServiceRealizations.Repositories;
using SpoofFileService.ServiceRealizations.Validators;
using SpoofFileService.Services;
using SpoofFileService.Services.Repositories;
using SpoofFileService.Services.Validators;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.SetBaseSettings<SpoofFileServiceContext>();

builder.Services.AddTransient<IFileValidator, FileValidator>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IFileWorkerService, LocalFileWorkerService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
    app.UseSwagger();
    app.MapSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
