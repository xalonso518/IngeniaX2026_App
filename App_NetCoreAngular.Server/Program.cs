using System.Text;
using App_NetCoreAngular.Server.Services.Token;
using App_NetCoreAngular.Server.EntityFramework;
using App_NetCoreAngular.Server.Repository.AuthRep;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using App_NetCoreAngular.Server.Repository.UsuarioRep;
using App_NetCoreAngular.Server.Repository.UsuariorRep;

// Builder para la aplicación
var builder = WebApplication.CreateBuilder(args);

// Indicamos que usará controllers
builder.Services.AddControllers();

// Indicamos que usará open API
builder.Services.AddOpenApi();

// Configuraciones para Open Api
builder.Services.AddEndpointsApiExplorer(); // Permitir que herramientas descubran y documenten automáticamente los endpoints, 
builder.Services.AddSwaggerGen(); // registra el generador en el ID, será el encargador de revisar los controladores

// Authentication - JWT Bearer
var secretToken = builder.Configuration["TokenSettings:Secret"] ?? "ZXC2026";
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        // Configuracion api
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["TokenSettings:Issuer"],
            ValidAudience = builder.Configuration["TokenSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretToken))
        };
    });

// Context SQL Server - EntityFramework
builder.Services.AddDbContext<AppDataContext>(options => options.UseSqlServer(builder.Configuration["SqlServer:DefaultConnection"]));

// Dependency injection
// Repositorios
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

// Servicios
builder.Services.AddScoped<TokenService>();


// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// CORS service - DEV //(Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:4200") // URL SPA
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Cookies/auth headers
    });
});

// Creamos aplicativo
var app = builder.Build();

// Permitir mapping de archivos
app.UseDefaultFiles();
app.MapStaticAssets();

// Generamos json para swagger
app.UseSwagger();

// Configurar HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Abre endpoint para ver open api
    //https://localhost:5044/openapi/v1.json
    app.MapOpenApi();

    // Abre endpoint para ver swagger
    //http://localhost:5044/swagger/index.html
    app.UseSwaggerUI(options =>
    {
        // Point Swagger UI para cargar datos .NET OpenAPI JSON
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

// Redireccionamiento https
app.UseHttpsRedirection();

// Aceptamos uso de CORS
app.UseCors("FrontendPolicy");

// Authorization
app.UseAuthorization();

// Mapeamos controladores
app.MapControllers();

// Ruta de inicio
app.MapFallbackToFile("/index.html");

// Iniciamos app
app.Run();
