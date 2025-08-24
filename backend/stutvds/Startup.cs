using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using stutvds.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using stutvds.DAL;
using stutvds.Logic;

namespace stutvds
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("Database")));
            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddRazorPages();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDev", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .WithHeaders("Authorization", "Content-Type") // 👈 Authorization разрешён
                        .AllowCredentials();
                });
            });
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false, // если нужен issuer, указываем true
                        ValidateAudience = false, // если нужен audience, true
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("SuperSecretKeyHere123!")) // ключ, которым подписываете JWT
                    };
                });
            
            services.AddAuthorization();
            
            services.AddHttpContextAccessor();
            services.AddDataAccessLayer();
            services.AddLogicLayer();

            services.AddAutoMapper(typeof(MappingProfile).Assembly, typeof(LogicMappingProfile).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLocalization();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAngularDev");

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}