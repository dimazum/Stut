using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using stutvds;
using stutvds.Consumers;
using stutvds.DAL;
using stutvds.Data;
using stutvds.Integrations;
using stutvds.Logic;
using stutvds.Messages;
using stutvds.WebSocketHubs;

var builder = WebApplication.CreateBuilder(args);

// ----- SERVICES -----

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// builder.Services
//     .AddDefaultIdentity<IdentityUser>(options =>
//     {
//         options.SignIn.RequireConfirmedAccount = false;
//         options.SignIn.RequireConfirmedEmail = false;
//         options.SignIn.RequireConfirmedPhoneNumber = false;
//     })
//     .AddRoles<IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
        {
            options.ViewLocationFormats.Add("/Controllers/MVC/{1}/{0}.cshtml"); // рядом с контроллером
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
    .AddIdentity<IdentityUser, IdentityRole>(options =>
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
});

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

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
builder.Services.AddDataAccessLayer();
builder.Services.AddLogicLayer();

builder.Services.AddAutoMapper(
    typeof(AutoMapperProfile).Assembly,
    typeof(LogicMappingProfile).Assembly
);

var app = builder.Build();

// ----- PIPELINE -----

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRequestLocalization();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


app.UseRouting();
app.UseCors("AllowAngularDev");

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { controller = "Home", action = "Index" }
    );

app.MapRazorPages();

app.MapHub<VoiceAnalysisHub>("/voice-analysis");

    //SEEDING
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    
    await DbInitializer.SeedAsync(userManager, roleManager);
}

app.Run();
