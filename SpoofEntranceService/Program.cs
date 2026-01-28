using AdditionalHelpers.ServiceRealizations;
using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.ServiceRealizations;
using CommunicationLibrary.Services;
using DataSaveHelpers.ServiceRealizations;
using DataSaveHelpers.ServiceRealizations.Cache;
using DataSaveHelpers.ServiceRealizations.Cache.Memory;
using DataSaveHelpers.ServiceRealizations.Cache.Redis;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofEntranceService.Models;
using SpoofEntranceService.ServiceRealizations;
using SpoofEntranceService.ServiceRealizations.Repositories;
using SpoofEntranceService.ServiceRealizations.Validators;
using SpoofEntranceService.Services;
using SpoofEntranceService.Services.Repositories;
using SpoofEntranceService.Services.Validators;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

//swagger api documentation service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
//"Server=.;Database=SpoofEntranceService;Trusted_Connection=True;TrustServerCertificate=True"
//data services
builder.Services.AddDbContext<SpoofEntranceServiceContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(sp =>
{
    var settings = new RabbitMQSettings();
    builder.Configuration.GetSection("RabbitMQSettings").Bind(settings);
    return settings;
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis")!);
    configuration.AbortOnConnectFail = false;
    configuration.ConnectTimeout = 5000;
    configuration.SyncTimeout = 5000;
    configuration.ReconnectRetryPolicy = new LinearRetry(1000);

    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddSingleton<IProcessQueueTasksService, ProcessQueueTasksService>();

//in-memory cache
builder.Services.AddSingleton<Microsoft.Extensions.Caching.Memory.IMemoryCache, Microsoft.Extensions.Caching.Memory.MemoryCache>();
builder.Services.AddSingleton<IMemoryCacheService, LocalCacheService>();

//redis
builder.Services.AddSingleton<IRedisService, BaseRedisCache>();

//multi cache(in-memory + redis)
builder.Services.AddSingleton<ICacheService, MultiCache>();

builder.Services.AddTransient<ISessionRepository, SessionRepository>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
builder.Services.AddTransient<IUserEntryRepository, UserRepository>();

builder.Services.AddTransient<ITokenValidator, TokenValidator>();
builder.Services.AddTransient<ISessionValidator, SessionValidator>();
builder.Services.AddTransient<IUserEntryValidator, UserEntryValidator>();

//logic services
builder.Services.AddTransient<ISessionService, SessionService>();
builder.Services.AddTransient<IUserEntryService, UserEntryService>();
builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddSingleton<ISerializer, JsonSerializerService>();

builder.Services.AddTransient<IUserPublisherService, UserPublisherService>();
builder.Services.AddHostedService<UserConsumerService>();
builder.Services.AddSingleton<IInjectionService, InjectionService>();

builder.Services.AddSingleton<ILoggerService>(provider =>
    new ConsoleLoggerService(
        minLogLevel: Enum.Parse<AdditionalHelpers.LogLevel>(builder.Configuration["Logging:LogLevel"] ?? "Information")
    )
);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
