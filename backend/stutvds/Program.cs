using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using StopStatAuth_6_0.Entities;
using stutvds;
using stutvds.Consumers;
using stutvds.Controllers.MVC._Common;
using stutvds.DAL;
using stutvds.Data;
using stutvds.Integrations;
using stutvds.Logic;
using stutvds.Messages;
using stutvds.WebSocketHubs;
using stutvds.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

// ----- SERVICES -----

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));


var supportedCultures = StuConstants.Cultures.Select(x => new CultureInfo(x)).ToList();

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(StuConstants.DefaultCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

// 🔹 Вставляем RouteData provider первым, чтобы URL имел приоритет
var routeProvider = new RouteDataRequestCultureProvider()
{
    RouteDataStringKey = "culture",
    UIRouteDataStringKey = "culture"
};

localizationOptions.RequestCultureProviders.Insert(0, routeProvider);

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Localization";
});

builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
        {
            options.ViewLocationFormats.Add("/Controllers/MVC/{1}/{0}.cshtml"); // рядом с контроллером
            options.ViewLocationExpanders.Add(new LocalizationViewLocationExpander());
        }
    
    ).AddNewtonsoftJson();
builder.Services.AddRazorPages();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()        // <- разрешаем все заголовки
            .AllowCredentials();     // обязательно для SignalR
    });
});

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.LoginPath = "/auth/login";
    options.ExpireTimeSpan = TimeSpan.FromDays(365);
    options.SlidingExpiration = true;
    options.Cookie.MaxAge = TimeSpan.FromDays(365);
});


builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024 * 10; // 10 MB
    options.EnableDetailedErrors = true;
});

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<VoiceAnalysisResultConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration.GetValue<string>("RabbitMQ:Host");

        cfg.Host(host, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // 🔹 1. ТОЧКА ПУБЛИКАЦИИ ДЛЯ PYTHON
        cfg.Message<VoiceAnalysisRequested>(m =>
        {
            m.SetEntityName("voice-analysis"); // имя exchange
        });

        cfg.Publish<VoiceAnalysisRequested>(p =>
        {
            p.ExchangeType = ExchangeType.Fanout;
            p.Durable = true;
        });

        // 🔹 2. ОЧЕРЕДЬ ДЛЯ РЕЗУЛЬТАТОВ (Python → .NET)
        cfg.ReceiveEndpoint("voice-analysis-result-queue", e =>
        {
            e.UseRawJsonDeserializer();
            e.ConfigureConsumer<VoiceAnalysisResultConsumer>(context);
        });
    });
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<PollinationsIS>();
builder.Services.AddSingleton<ConnectionStringProvider>();
builder.Services.AddDataAccessLayer();
builder.Services.AddLogicLayer();

builder.Services.AddAutoMapper(
    typeof(AutoMapperProfile).Assembly,
    typeof(LogicMappingProfile).Assembly
);

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<ApplicationDbContext>()
    .SetApplicationName("StutVDS");

var app = builder.Build();

// ----- PIPELINE -----

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        if (exception is StuException stuException)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = stuException.Message, code =  stuException.Code});
        }
        else
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message , code = 123});
        }
    });
});

app.UseRouting();

app.UseRequestLocalization(localizationOptions);

app.UseMiddleware<CultureCaptureMiddleware>();

app.UseCors("AllowAngularDev");

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var culture = context.Features
        .Get<IRequestCultureFeature>()?
        .RequestCulture.Culture.Name;

    if (!string.IsNullOrEmpty(culture))
        context.Request.RouteValues["culture"] = culture;

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{culture=ru}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapHub<VoiceAnalysisHub>("/voice-analysis");
app.MapHub<ChatHub>("/chatHub");

//SEEDING
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    
    await DbInitializer.SeedAsync(userManager, roleManager);
    
    var installer = scope.ServiceProvider.GetRequiredService<IStoredProcedureInstaller>();
    await installer.InstallAsync(); 
}

app.Run();
