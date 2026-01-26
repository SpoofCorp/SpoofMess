using AdditionalHelpers.ServiceRealizations;
using AdditionalHelpers.Services;
using CommunicationLibrary;
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


builder.Services.AddTransient<IProcessQueueTasksService, ProcessQueueTasksService>();
builder.Services.AddTransient<ISerializer, JsonSerializerService>();
builder.Services.AddTransient(sp =>
{
    var settings = new RabbitMQSettings();
    builder.Configuration.GetSection("RabbitMQSettings").Bind(settings);
    return settings;
});

builder.Services.AddTransient<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis")!);
    configuration.AbortOnConnectFail = false;
    configuration.ConnectTimeout = 5000;
    configuration.SyncTimeout = 5000;
    configuration.ReconnectRetryPolicy = new LinearRetry(1000);

    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddTransient<ILoggerService>(provider =>
    new ConsoleLoggerService(
        minLogLevel: Enum.Parse<AdditionalHelpers.LogLevel>(builder.Configuration["Logging:LogLevel"] ?? "Information")
    )
);

builder.Services.AddTransient<Microsoft.Extensions.Caching.Memory.IMemoryCache, Microsoft.Extensions.Caching.Memory.MemoryCache>();
builder.Services.AddTransient<IMemoryCacheService, LocalCacheService>();

//redis
builder.Services.AddTransient<IRedisService, BaseRedisCache>();


builder.Services.AddTransient<IUserMessageBrokerService, UserMessageService>();
builder.Services.AddTransient<IChatAvatarPublisherService, ChatAvatarPublisherService>();

builder.Services.AddHostedService<UserConsumerService>();
//multi cache(in-memory + redis)
builder.Services.AddTransient<ICacheService, MultiCache>();

builder.Services.AddTransient<IChatAvatarValidator, ChatAvatarValidator>();
builder.Services.AddTransient<IRoleValidator, RoleValidator>();
builder.Services.AddTransient<IChatValidator, ChatValidator>();
builder.Services.AddTransient<IUserAvatarValidator, UserAvatarValidator>();
builder.Services.AddTransient<IStickerPackValidator, StickerPackValidator>();
builder.Services.AddTransient<IStickerValidator, StickerValidator>();
builder.Services.AddTransient<IUserValidator, UserValidator>();

builder.Services.AddTransient<IFileMetadatumRepository, FileMetadatumRepository>();
builder.Services.AddTransient<IChatAvatarRepository, ChatAvatarRepository>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();
builder.Services.AddTransient<IChatUserRepository, ChatUserRepository>();
builder.Services.AddTransient<IStickerPackRepository, StickerPackRepository>();
builder.Services.AddTransient<IStickerRepository, StickerRepository>();
builder.Services.AddTransient<IUserAvatarRepository, UserAvatarRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IChatTypeRepository, ChatTypeRepository>();

builder.Services.AddTransient<IChatAvatarService, ChatAvatarService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IChatUserService, ChatUserService>();
builder.Services.AddTransient<IStickerPackService, StickerPackService>();
builder.Services.AddTransient<IStickerService, StickerService>();
builder.Services.AddTransient<IUserAvatarService, UserAvatarService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
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