using WebApplication;
using WebApplication.Installers;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

builder.Services.InstallServicesInAssembly<IApiAssemblyMarker>(builder.Configuration);

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

// API requests belong to the Python host and must never return the SPA HTML.
app.Map("/api/{**path}", () => Results.NotFound());

app.MapFallbackToFile("index.html");

app.Run();

