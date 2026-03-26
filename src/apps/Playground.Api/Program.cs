using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Playground.Api;
using Playground.Api.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<JwtService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("book", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Book API",
        Version = "v1"
    });

    c.SwaggerDoc("product", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Product API",
        Version = "v1"
    });
    c.SwaggerDoc("FileUpload", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Document API",
        Version = "v1"
    });
    c.SwaggerDoc("Document", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Authentication API",
        Version = "v1"
    });
    c.DocInclusionPredicate((doc_name, api_desc) =>
    {
        var group_name = api_desc.ActionDescriptor.EndpointMetadata
            .OfType<ApiExplorerSettingsAttribute>()
            .FirstOrDefault()?.GroupName;
        return group_name == doc_name;
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    c.DocInclusionPredicate((doc_name, api_desc) =>
    {
        var group_name = api_desc.ActionDescriptor.EndpointMetadata
                                .OfType<ApiExplorerSettingsAttribute>()
                                .FirstOrDefault()?.GroupName;

        Console.WriteLine($"doc={doc_name} | path={api_desc.RelativePath} | group={group_name ?? "NULL"}");

        return group_name == doc_name;
    });
});
//builder.Services.AddOpenApi();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/book/swagger.json", "Book API");
    c.SwaggerEndpoint("/swagger/product/swagger.json", "Product API");
    c.SwaggerEndpoint("/swagger/FileUpload/swagger.json", "Document API");
    c.SwaggerEndpoint("/swagger/Document/swagger.json", "Auth API");

    c.RoutePrefix = string.Empty;
});

//if(app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();