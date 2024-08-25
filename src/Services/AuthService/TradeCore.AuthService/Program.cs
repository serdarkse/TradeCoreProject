using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using TradeCore.AuthService.Attribute;
using TradeCore.AuthService.Container.Modules;
using TradeCore.AuthService.Dependency;
using TradeCore.AuthService.Filters;
using TradeCore.AuthService.Helpers;
using TradeCore.AuthService.Helpers.HelperModels;
using TradeCore.AuthService.Repository;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerClaimRepositoryAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAuthRepositoryAggregate;
using TradeCore.AuthService.Repository.RepositoryAggregate.RepositoryappCustomerAggregate;
using TradeCore.AuthService.Repository.RepositoryAggregate.RepositoryAppOperationClaimAggregate;
using TradeCore.AuthService.Repository.RepositoryAggregate.RepositoryAuthAggregate;
using TradeCore.AuthService.Repository.RepositoryAggregate.ReposiyoryaApCustomerClaimAggregate;
using TradeCore.AuthService.Utilities.ElasticSearch;


var builder = WebApplication.CreateBuilder(args);


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MediatRModule()));
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new RepositoryModule()));
builder.Host.ConfigureLogging(logging =>{logging.ClearProviders();logging.SetMinimumLevel(LogLevel.Information);});

IConfiguration configuration = builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true).Build();

RepositoryModule.AddDbContext(builder.Services, configuration);

builder.Services.AddTransient<IAppCustomerRepository, AppCustomerRepository>();
builder.Services.AddTransient<IAppCustomerClaimRepository, AppCustomerClaimRepository>();
builder.Services.AddTransient<IAppOperationClaimRepository, AppOperationClaimRepository>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
builder.Services.AddTransient<IAccessToken, AccessToken>();
builder.Services.AddTransient<LdapConnection>();

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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });

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

// Serilog yapýlandýrmasý
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/authservice-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Serilog'u kullanacak þekilde yapýlandýrýn
builder.Host.UseSerilog(); 

builder.Services.ConfigurElasticLoggingService();
ServicesExtensions.ConfigurElasticLoggingService(builder.Services);

builder.Services.AddControllers(opts => opts.Filters.Add(new ValidateModelAttribute(Log.Logger)));

builder.Services.AddControllers(opts => opts.EnableEndpointRouting = true)
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                 options.JsonSerializerOptions.IgnoreNullValues = true;
             })
             .AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
             });

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new BasicAuthFilter());
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

// Set the default timeout configuration
builder.Services.Configure<TimeoutOptions>(configuration =>
{
    configuration.TimeoutSeconds = 60;
});


var origins = configuration.GetValue<string>("CorsOrigin").Split(";");

builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
policy =>{policy.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader();}));


DependencyModule.RegisterServices(builder.Services, configuration);

builder.Services.AddMvc().AddCookieTempDataProvider();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
{
    return memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.GetName();
};

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
    dbContext.Database.Migrate();
};

app.UseCors("NgOrigins");
app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("uat"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Make Turkish your default language. It shouldn't change according to the server.
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
