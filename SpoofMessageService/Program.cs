using RuleRoleHelper.ServiceRealizations;
using RuleRoleHelper.Services;
using SettingsHelper;
using SpoofMessageService.Models;
using SpoofMessageService.ServiceRealizations;
using SpoofMessageService.ServiceRealizations.Repositories;
using SpoofMessageService.ServiceRealizations.Validators;
using SpoofMessageService.Services;
using SpoofMessageService.Services.Repositories;
using SpoofMessageService.Services.Validators;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.SetBaseSettings<SpoofMessageServiceContext>();

builder.Services.AddHostedService<ChatUserConsumerService>();

builder.Services.AddScoped<IChatUserService, ChatUserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();

builder.Services.AddScoped<IMessageValidator, MessageValidator>();
builder.Services.AddScoped<IChatUserValidator, ChatUserValidator>();
builder.Services.AddScoped<IRuleParserService, RuleParserService>();

builder.Services.AddScoped<IRuleService, RuleService>();

builder.Services.AddScoped<IChatUserRepository, ChatUserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
