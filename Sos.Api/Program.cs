using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Sos.Api.Configurations;
using Sos.Api.Middlewares;
using Sos.Application;
using Sos.Domain.UserAggregate.Enums;
using Sos.Infrastructure;
using Sos.Infrastructure.Socket;
using Sos.Persistence;
using Sos.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new() { Title = "Sos API", Version = "v1" }
    );

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });

    options.DocumentFilter<ExcludeDocumentFilter>();
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add migrate database
using (var serviceScope = app.Services.CreateScope())
{

    AppDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Database.Migrate();

}

// Add middleware to the pipeline
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Add static files
var options = new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(UserRequirementEnum.StoragePath),
    RequestPath = "/images"
};

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(options => true)
);

app.UseStaticFiles(options);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () =>
{
    return Results.Redirect("/swagger");
}).ExcludeFromDescription();

app.MapHub<NotificationsHub>("/notifications-hub");

app.MapHub<WebRTCsHub>("/webRTCs-hub");

app.Run("http://*:6868");