using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add in-memory cache and session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Mantener sesión 8 horas (coincide con autenticación por cookie si se usa)
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // permite sesión aunque el usuario no acepte cookies opcionales
});

// Agregar autenticación por cookies (usada en controlador para SignOut)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Index";
        options.AccessDeniedPath = "/Acceso/Index";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.Lifetime.ApplicationStarted.Register(() =>
{
    logger.LogInformation("Tienda: Application started and listening.");
});
app.Lifetime.ApplicationStopping.Register(() =>
{
    logger.LogWarning("Tienda: Application is stopping...");
});

// Configure localization: usar 'es-NI' por defecto para que el model binder interprete decimales locales
var supportedCultures = new[] { new CultureInfo("es-NI"), new CultureInfo("es") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-NI"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Habilitar sesión y autenticación/authorization en el pipeline
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Index}/{id?}")
    .WithStaticAssets();

try
{
    app.Run();
}
catch (Exception ex)
{
    // Log unexpected exceptions during startup/run
    logger.LogCritical(ex, "Tienda: Host terminated unexpectedly");
    throw;
}