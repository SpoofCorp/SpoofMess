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
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SpoofSettingsService.Models;
using SpoofSettingsService.ServiceRealizations;
using SpoofSettingsService.ServiceRealizations.MessageBrokers;
using SpoofSettingsService.ServiceRealizations.Repositories;
using SpoofSettingsService.ServiceRealizations.Validators;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<SpoofSettingsServiceContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSingleton<IProcessQueueTasksService, ProcessQueueTasksService>();
builder.Services.AddSingleton<ISerializer, JsonSerializerService>();
builder.Services.AddSingleton(sp =>
{
    var settings = new RabbitMQSettings();
    builder.Configuration.GetSection("RabbitMQSettings").Bind(settings);
    return settings;
});

builder.Services.AddScoped<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis")!);
    configuration.AbortOnConnectFail = false;
    configuration.ConnectTimeout = 5000;
    configuration.SyncTimeout = 5000;
    configuration.ReconnectRetryPolicy = new LinearRetry(1000);

    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddSingleton<ILoggerService>(provider =>
    new ConsoleLoggerService(
        minLogLevel: Enum.Parse<AdditionalHelpers.LogLevel>(builder.Configuration["Logging:LogLevel"] ?? "Information")
    )
);

builder.Services.AddSingleton<Microsoft.Extensions.Caching.Memory.IMemoryCache, Microsoft.Extensions.Caching.Memory.MemoryCache>();
builder.Services.AddSingleton<IMemoryCacheService, LocalCacheService>();

//redis
builder.Services.AddSingleton<IRedisService, BaseRedisCache>();


builder.Services.AddTransient<IUserMessageBrokerService, UserMessageService>();
builder.Services.AddTransient<IChatAvatarPublisherService, ChatAvatarPublisherService>();

builder.Services.AddHostedService<UserConsumerService>();
builder.Services.AddSingleton<IInjectionService, InjectionService>();
//multi cache(in-memory + redis)
builder.Services.AddSingleton<ICacheService, MultiCache>();

builder.Services.AddTransient<IChatAvatarValidator, ChatAvatarValidator>();
builder.Services.AddTransient<IRoleValidator, RoleValidator>();
builder.Services.AddTransient<IChatValidator, ChatValidator>();
builder.Services.AddTransient<IUserAvatarValidator, UserAvatarValidator>();
builder.Services.AddTransient<IStickerPackValidator, StickerPackValidator>();
builder.Services.AddTransient<IStickerValidator, StickerValidator>();
builder.Services.AddTransient<IUserValidator, UserValidator>();

builder.Services.AddScoped<IFileMetadatumRepository, FileMetadatumRepository>();
builder.Services.AddScoped<IChatAvatarRepository, ChatAvatarRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatUserRepository, ChatUserRepository>();
builder.Services.AddScoped<IStickerPackRepository, StickerPackRepository>();
builder.Services.AddScoped<IStickerRepository, StickerRepository>();
builder.Services.AddScoped<IUserAvatarRepository, UserAvatarRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatTypeRepository, ChatTypeRepository>();

builder.Services.AddScoped<IChatAvatarService, ChatAvatarService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatUserService, ChatUserService>();
builder.Services.AddScoped<IStickerPackService, StickerPackService>();
builder.Services.AddScoped<IStickerService, StickerService>();
builder.Services.AddScoped<IUserAvatarService, UserAvatarService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();