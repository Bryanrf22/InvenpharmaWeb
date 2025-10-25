using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
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
