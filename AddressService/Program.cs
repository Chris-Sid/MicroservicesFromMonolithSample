using AddressService;
using AddressService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedFunctionalities.DTOs.Address;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IAddressService, AddressServiceHub>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var baseUrl = "https://localhost:7093/swagger/index.html"; // Retrieve the value from appsettings.json or environment variables
                                                               // var requestHeaders = new AddressRequestHeaders(); // If headers are default or empty
    return new AddressServiceHub(provider.GetRequiredService<IHttpClientFactory>(), baseUrl);
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(7001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        listenOptions.UseHttps();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway API", Version = "v1" });
 
    // Optional: Customize schema for DmsIdentifier
    c.MapType<DmsIdentifier>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, Microsoft.OpenApi.Models.OpenApiSchema>
        {
            { "internalId", new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string" } },
            { "externalId", new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string" } }
        }
    });
    c.AddSecurityDefinition("X-Authorization", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "X-Authorization",
        In = ParameterLocation.Header,
        Description = "Enter your API token here"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "X-Authorization"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var config = builder.Configuration;
var jwtKey = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing");
var jwtIssuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is missing");
var jwtAudience = config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is missing");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddGrpc();
builder.Services.AddAuthorization();


var app = builder.Build();

app.Use(async (context, next) =>
{
    var authXHeader = context.Request.Headers["X-Authorization"].ToString();
    var authHeader = context.Request.Headers["Authorization"].ToString();
    var tokend = context.Request.Headers["Authorization"].FirstOrDefault();
    if (context.Request.Headers.TryGetValue("X-Authorization", out var token))
    {
        context.Request.Headers["Authorization"] = $"Bearer {token}";
    }
    await next();
});
app.UseHttpsRedirection();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGrpcService<AddressGrpcService>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
