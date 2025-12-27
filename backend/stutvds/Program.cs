using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
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
using stutvds.Logic;
using stutvds.Messages;
using stutvds.WebSocketHubs;

var builder = WebApplication.CreateBuilder(args);

// ----- SERVICES -----

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
        var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtIssuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
        
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // Если запрос к SignalR hub и есть access_token — используем его
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/voice-analysis"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddSignalR();

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
builder.Services.AddDataAccessLayer();
builder.Services.AddLogicLayer();

builder.Services.AddAutoMapper(
    typeof(MappingProfile).Assembly,
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
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAngularDev");

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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
