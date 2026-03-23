using Microsoft.AspNetCore.Mvc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

    c.RoutePrefix = string.Empty;
});

//if(app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();