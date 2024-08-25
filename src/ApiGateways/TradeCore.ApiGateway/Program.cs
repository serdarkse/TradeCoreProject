using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var environmentName = builder.Environment.EnvironmentName;
var environmentNamePath = environmentName == "-" ? "" : environmentName + ".";

IConfiguration configuration = builder.Configuration.AddJsonFile($"appsettings.{environmentNamePath}json", true, true).Build();

var origins = configuration.GetValue<string>("CorsOrigin").Split(";");

builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
policy => { policy.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader(); }));

builder.Services.AddOcelot(configuration);
builder.Services.AddSwaggerForOcelot(configuration);

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


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        corsBuilder =>
        {
            corsBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Configuration.AddJsonFile($"ocelot.{environmentNamePath}json", optional: false, reloadOnChange: true);

builder.Services.Configure<IISServerOptions>(options => {
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});

var app = builder.Build();


app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});


app.UseCors("NgOrigins");
app.UseOcelot().Wait();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

