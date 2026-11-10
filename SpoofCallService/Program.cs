using SettingsHelper;
using SpoofCallService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//swagger api documentation service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.SetBaseSettingsWithFactory<SpoofCallServiceContext>();

var app = builder.Build();

app.MapHealthChecks("/healthz").ShortCircuit();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
