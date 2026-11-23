using System.Text;
using App_NetCoreAngular.Server.Services.Token;
using App_NetCoreAngular.Server.EntityFramework;
using App_NetCoreAngular.Server.Repository.AuthRep;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using App_NetCoreAngular.Server.Repository.UsuarioRep;
using App_NetCoreAngular.Server.Repository.UsuariorRep;
using App_NetCoreAngular.Server.Repository.ProductoRep;
using App_NetCoreAngular.Server.Repository.ClienteRep;
using App_NetCoreAngular.Server.Services.Storage;
using App_NetCoreAngular.Server.Providers.FileProvider;
using App_NetCoreAngular.Server.Policies;
using Microsoft.AspNetCore.Authorization;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;
using App_NetCoreAngular.SendGrid;
using App_NetCoreAngular.Server.Services.SignalR;
using Microsoft.AspNetCore.SignalR;
using Stripe;

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
        
        // Configuracion hub
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // Valida si reques es del hub
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/api/hub")))
                {
                    // Obtiene el token del reques  y lo asigna al context
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Authorization - Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserActive", policy => policy.Requirements.Add(new StatusUserRequirement(Estado.Activo.ToString("D"))));
    options.AddPolicy("RolAdmin", policy => policy.Requirements.Add(new RolRequirement(RolUsuario.Admin.ToString("D"))));
});

// Context SQL Server - EntityFramework
builder.Services.AddDbContext<AppDataContext>(options => options.UseSqlServer(builder.Configuration["SqlServer:DefaultConnection"]));

// Dependency injection
// Authorization policies handle
builder.Services.AddTransient<IAuthorizationHandler, StatusUserHandler>();
builder.Services.AddTransient<IAuthorizationHandler, RolHandler>();

// Repositorios
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IProductoRepository, ProductoRepository>();
builder.Services.AddTransient<IClienteRepository, ClienteRepository>();

// Servicios
builder.Services.AddScoped<App_NetCoreAngular.Server.Services.Token.TokenService>();
builder.Services.AddTransient<BlobStorage>();
builder.Services.AddTransient<FileLocal>();
builder.Services.AddTransient<MailSendGrid>();

// Agregar SignalR al contenedor de servicios
builder.Services.AddSignalR();
builder.Services.AddScoped<NotificacionSignalR>();
builder.Services.AddSingleton<IUserIdProvider, QueryStringUserIdProvider>();

// Agregar Stripe
builder.Services.AddSingleton(new StripeClient(builder.Configuration["StripeApi"]));
builder.Services.AddTransient<StripeService>();


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

//  Archivos locales
if (string.IsNullOrWhiteSpace(builder.Environment.WebRootPath))
{
    builder.Environment.WebRootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
}

// Creamos aplicativo
var app = builder.Build();

// Permitir mapping de archivos
app.UseDefaultFiles();
app.MapStaticAssets();

// Generamos json para swagger
app.UseSwagger();

// Habilitamos archivos estáticos desde wwwroot directory
app.UseStaticFiles();

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
app.UseAuthentication();
app.UseAuthorization();

// Mapeamos controladores
app.MapControllers();

// Ruta para los hubs de signal
app.MapHub<MainHub>("/api/hub");

// Ruta de inicio
app.MapFallbackToFile("/index.html");

// Iniciamos app
app.Run();
