using CommunicationLibrary;
using SecurityLibrary;
using SecurityLibrary.Tokens;
using SettingsHelper;
using SpoofFileInfo;
using SpoofFileService;
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

builder.SetBaseSettingsWithFactory<SpoofFileServiceContext>();
builder.Services.Configure<FileSettings>(
    builder.Configuration.GetSection("FileSettings"));

builder.Services.AddTransient<IFileValidator, FileValidator>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IFileClassifier, FileClassifier>();
builder.Services.AddTransient<IExtensionService, ExtensionService>();
builder.Services.AddTransient<IExtensionValidator, ExtensionValidator>();
builder.Services.AddTransient<IExtensionRepository, ExtensionRepository>();
builder.Services.AddTransient<IFileWorkerService, LocalFileWorkerService>();
builder.Services.AddTransient<IFingerprintService, FingerprintService>();
builder.Services.AddTransient<IFileTokenService, FileTokenService>();

builder.Services.AddSingleton(
    builder.Configuration.GetSection("TokenHeader")
    .Get<TokenHeaderCover>()!);

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
