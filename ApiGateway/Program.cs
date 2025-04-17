using Microsoft.OpenApi.Models;
using SharedFunctionalities.DTOs.Address;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

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

var app = builder.Build();
 
app.Use(async (context, next) =>
{
    var hasToken = context.Request.Headers.TryGetValue("X-Authorization", out var token);
    Console.WriteLine($"Has Token: {hasToken}, Token: {token}");

    if (hasToken && !string.IsNullOrWhiteSpace(token))
    {
        context.Request.Headers["Authorization"] = $"Bearer {token}";
    }

    await next();
});
app.MapReverseProxy();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
