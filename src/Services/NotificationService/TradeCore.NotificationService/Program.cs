using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;
using TradeCore.EventBus.Base;
using TradeCore.EventBus.Base.Abstraction;
using TradeCore.EventBus.Factory;
using TradeCore.EventBus.RabbitMQ;
using TradeCore.NotificationService.Attribute;
using TradeCore.NotificationService.Container.Modules;
using TradeCore.NotificationService.IntegrationEvents.EventHandlers;
using TradeCore.NotificationService.IntegrationEvents.Events;
using TradeCore.NotificationService.Sender;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MediatRModule()));
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new RepositoryModule()));

IConfiguration configuration = builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true).Build();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = configuration.GetValue<string>("Token:Issuer"),
        ValidAudience = configuration.GetValue<string>("Token:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(configuration.GetValue<string>("Token:SecretKey"))),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notification", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});

builder.Services.AddControllers(opts => opts.Filters.Add(new ValidateModelAttribute()));
builder.Services.AddControllers(opts => opts.EnableEndpointRouting = true)
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddLogging(configure =>
{
    configure.AddConsole();
    configure.AddDebug();

});

builder.Services.AddTransient<IMessageBrokerHelper, MqQueueHelper>();
builder.Services.AddTransient<IMessageConsumer, MqConsumerHelper>();
builder.Services.AddSingleton<IMailSender, MailSender>();

builder.Services.AddTransient<CommunicateSuccessIntegrationEventHandler>();

var _brokerOptions = configuration.GetSection("MessageBrokerOptions").Get<MessageBrokerOptions>();

builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetrycount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "NotificationService",
        EventBusType = EventBusType.RabbitMQ,
        Connection = new ConnectionFactory()
        {
            VirtualHost = _brokerOptions.VirtualHost,
            HostName = _brokerOptions.HostName,
            UserName = _brokerOptions.UserName,
            Password = _brokerOptions.Password,
            Port = _brokerOptions.Port
        }
    };

    return EventBusFactory.Create(config, sp, configuration);
});

var sp = builder.Services.BuildServiceProvider();
IEventBus eventBus = sp.GetRequiredService<IEventBus>();
eventBus.Subscribe<CommunicateEmailIntegrationEvent, CommunicateSuccessIntegrationEventHandler>();


var app = builder.Build();


if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("uat"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
