using Microsoft.Net.Http.Headers;
using Shared.Lib.ExceptionHandling;
using WebApp.Admin.HttpHandlers;
using WebApp.Admin.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddControllersWithViews();

//Add AccessControl API
builder.Services.AddTransient<AuthenticationDelegatingHandler>();

builder.Services.AddHttpClient<IAccessControlService, AccessControlService>("AccessCotrolApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:AccessCotrolApi"]);
    c.DefaultRequestHeaders.Clear();
    c.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// global error handler
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
