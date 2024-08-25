using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Serilog;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using TradeCore.EventBus.Base;
using TradeCore.EventBus.Base.Abstraction;
using TradeCore.EventBus.Factory;
using TradeCore.EventBus.RabbitMQ;
using TradeCore.OrderService.Attribute;
using TradeCore.OrderService.Container.Modules;
using TradeCore.OrderService.Dependency;
using TradeCore.OrderService.Helpers;
using TradeCore.OrderService.Helpers.HelperModels;
using TradeCore.OrderService.IntegrationEvents.EventHandlers;
using TradeCore.OrderService.IntegrationEvents.Events;
using TradeCore.OrderService.Repository;
using TradeCore.OrderService.Utilities.ElasticSearch;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new MediatRModule());
    builder.RegisterModule(new RepositoryModule());

});
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Information);
});

IConfiguration configuration = builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true).Build();

// Serilog yapýlandýrmasý
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/orderservice-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.ConfigurElasticLoggingService();
ServicesExtensions.ConfigurElasticLoggingService(builder.Services);

RepositoryModule.AddDbContext(builder.Services, configuration);

builder.Services.AddMvc().AddCookieTempDataProvider();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
});


#region Service Define

builder.Services.AddTransient<IDbContextFactory, DbContextFactory>();

#endregion


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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Trade Core", Version = "v1" });

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
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();


builder.Services.AddControllers(opts => opts.Filters.Add(new ValidateModelAttribute(Log.Logger)));

builder.Services.AddControllers(opts => opts.EnableEndpointRouting = true)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<MvcOptions>(options =>
{
    var cancellationTokenModelBinderProvider = options.ModelBinderProviders
        .FirstOrDefault(x => x is CancellationTokenModelBinderProvider);
    if (cancellationTokenModelBinderProvider != null)
    {
        options.ModelBinderProviders.Remove(cancellationTokenModelBinderProvider);
    }
    options.ModelBinderProviders.Insert(0, new TimeoutCancellationTokenModelBinderProvider());
});

builder.Services.Configure<TimeoutOptions>(configuration =>
{
    configuration.TimeoutSeconds = 60;
});

#region

builder.Services.AddTransient<CommunicateStartedIntegrationEventHandler>();

var _brokerOptions = configuration.GetSection("MessageBrokerOptions").Get<MessageBrokerOptions>();

builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetrycount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "TradeCore",
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
#endregion

var origins = configuration.GetValue<string>("CorsOrigin").Split(";");

builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
policy =>
{
    policy.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader();
}));

DependencyModule.RegisterServices(builder.Services, configuration);


builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});



var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<OrderDbContext>();
    dbContext.Database.Migrate();
};

app.UseCors("NgOrigins");
app.UseDeveloperExceptionPage();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("uat"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<CommunicateStartedIntegrationEvent, CommunicateStartedIntegrationEventHandler>();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr-TR"),

});

var cultureInfo = new CultureInfo("tr-TR");
cultureInfo.DateTimeFormat.ShortTimePattern = "HH:mm";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.UseStaticFiles();
app.UseExceptionHandler("/error");
app.MapControllers();
app.Run();

Console.WriteLine("Trade Core Web API Running");
