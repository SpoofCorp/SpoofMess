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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using StackExchange.Redis;

namespace SettingsHelper;

public static class ServerSettingsService
{
    public static void AddDbContext<TContext>(this WebApplicationBuilder builder) where TContext : DbContext =>
        builder.Services.AddDbContext<TContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("Sql")));
    public static void AddDbContextFactory<TContext>(this WebApplicationBuilder builder) where TContext : DbContext =>
        builder.Services.AddDbContextFactory<TContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("Sql")));

    public static void AddJwtAuthentification(this WebApplicationBuilder builder) =>
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = SecurityLibrary.AuthOptions.AUDIENCE,
                ValidIssuer = SecurityLibrary.AuthOptions.ISSUER,
                IssuerSigningKey = SecurityLibrary.AuthOptions.GetSecurityKey()
            };
        });

    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ILoggerService>(provider =>
            new ConsoleLoggerService(
                minLogLevel: Enum.Parse<AdditionalHelpers.LogLevel>(builder.Configuration["Logging:LogLevel"] ?? "Information")
            )
        );
    }

    public static void AddRabbitMQ(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(sp =>
        {
            RabbitMQSettings settings = new();
            builder.Configuration.GetSection("RabbitMQSettings").Bind(settings);
            return settings;
        });
    }

    public static void AddJsonSerializer(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISerializer, JsonSerializerService>();
    }

    public static void AddInjectionService(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IInjectionService, InjectionService>();
    }

    public static void AddMemoryCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<Microsoft.Extensions.Caching.Memory.IMemoryCache, Microsoft.Extensions.Caching.Memory.MemoryCache>();
        builder.Services.AddSingleton<IMemoryCacheService, LocalCacheService>();
    }

    public static void AddRedis(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            ConfigurationOptions configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis")!);
            configuration.AbortOnConnectFail = false;
            configuration.ConnectTimeout = 5000;
            configuration.SyncTimeout = 5000;
            configuration.ReconnectRetryPolicy = new LinearRetry(1000);

            return ConnectionMultiplexer.Connect(configuration);
        });
        builder.Services.AddSingleton<IRedisService, BaseRedisCache>();
    }

    public static void AddMultiCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ICacheService, MultiCache>();
        builder.AddProccessQueueTasksSevice();
    }

    public static void AddRedisAndMemoryCaches(this WebApplicationBuilder builder)
    {
        builder.AddMemoryCache();
        builder.AddRedis();
        builder.AddMultiCache();
    }

    public static void AddProccessQueueTasksSevice(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IProcessQueueTasksService, ProcessQueueTasksService>();
    }
    public static void AddSwaggerGenWithAccess(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new("bearer", document)] = []
            });
        });
    }

    public static void SetBaseSettings<TContext>(this WebApplicationBuilder builder) where TContext : DbContext
    {
        builder.AddDbContext<TContext>();
        builder.AddJwtAuthentification();
        builder.AddLogging();
        builder.AddRabbitMQ();
        builder.AddRedisAndMemoryCaches();
        builder.AddJsonSerializer();
        builder.AddSwaggerGenWithAccess();
        builder.AddInjectionService();
    }
    public static void SetBaseSettings(this WebApplicationBuilder builder)
    {
        builder.AddJwtAuthentification();
        builder.AddLogging();
        builder.AddRabbitMQ();
        builder.AddRedisAndMemoryCaches();
        builder.AddJsonSerializer();
        builder.AddSwaggerGenWithAccess();
        builder.AddInjectionService();
    }
    public static void SetBaseSettingsWithFactory<TContext>(this WebApplicationBuilder builder) where TContext : DbContext
    {
        builder.AddDbContextFactory<TContext>();
        builder.AddJwtAuthentification();
        builder.AddLogging();
        builder.AddRabbitMQ();
        builder.AddRedisAndMemoryCaches();
        builder.AddJsonSerializer();
        builder.AddSwaggerGenWithAccess();
        builder.AddInjectionService();
    }
}
