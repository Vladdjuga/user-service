using System.Text;
using UI.Swagger;
using Chat;
using Infrastructure.DI;
using Infrastructure.Middleware;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using UI.gRPCClients;

var builder = WebApplication.CreateBuilder(args);
var config=builder.Configuration;
//Kestrel configuration
builder.WebHost.ConfigureKestrel((context,options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = config["JwtSettings:Issuer"],
                ValidAudiences = config.GetSection("JwtSettings:Audiences")
                    .Get<List<string>>(),
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true
            };
            options.MapInboundClaims = false;
        }
    );
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() 
    .WriteTo.Console() 
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) 
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddGrpc();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    // Apply migrations at startup
    // This will ensure that the database is created and migrations are applied
    var db = scope.ServiceProvider.GetRequiredService<MessengerDbContext>();
    db.Database.Migrate(); 
}
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapGet("/health", () => Results.Ok("OK"))
    .WithName("HealthCheck")
    .WithTags("HealthCheck");

app.MapControllers();
app.MapGrpcService<UI.gRPCClients.ChatService>();

app.Run();
