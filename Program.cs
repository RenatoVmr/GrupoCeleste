using GrupoCeleste.Data;
using GrupoCeleste.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de puerto solo para Render (producción)
if (builder.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
    
    // Limpiar configuración previa de puertos
    builder.Configuration["ASPNETCORE_URLS"] = $"http://0.0.0.0:{port}";
    builder.Configuration["HTTP_PORTS"] = "";
    builder.Configuration["HTTPS_PORTS"] = "";
}

// Configuración de archivos JSON opcionales
builder.Configuration.AddJsonFile("appsettings.MercadoPago.json", optional: true, reloadOnChange: true);

// Configuración de servicios
builder.Services.AddHttpClient();
builder.Services.AddScoped<GrupoCeleste.Services.MercadoPagoService>();
builder.Services.AddSingleton<GrupoCeleste.Services.RecommendationService>();

// Configuración de base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? "Data Source=/app/Data/GrupoCeleste.db";

// Configuración de base de datos con soporte para directorios
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);
    
    // En producción, asegurar que el directorio existe
    if (builder.Environment.IsProduction())
    {
        var dbPath = connectionString.Replace("Data Source=", "");
        var directory = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
});

builder.Services.AddIdentity<Usuario, IdentityRole>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configurar rutas de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Crear y migrar la base de datos automáticamente
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Asegurar que la base de datos existe y está migrada
        await context.Database.EnsureCreatedAsync();
        
        // Inicializar datos
        await SeedData.Initialize(services);
        // Crear usuario administrador
        await AdminSeeder.CreateAdminUser(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al inicializar la base de datos.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// En producción (Render), no usar HTTPS redirect ya que el proxy maneja SSL
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Health check endpoint para Docker/Render
app.MapGet("/health", () => "OK");

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
