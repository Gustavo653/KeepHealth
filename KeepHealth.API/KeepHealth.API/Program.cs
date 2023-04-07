using Common.Functions;
using Hangfire;
using KeepHealth.Application;
using KeepHealth.Application.Interface;
using KeepHealth.Domain.Enum;
using KeepHealth.Domain.Identity;
using KeepHealth.Persistence;
using KeepHealth.Service;
using KeepHealth.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;


namespace KeepHealth.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            string databaseKeepHealth = Environment.GetEnvironmentVariable("KeepHealth") ?? configuration.GetConnectionString("KeepHealth");

            Console.WriteLine("Inicio parametros da aplicacao: \n");
            Console.WriteLine($"(KeepHealth) String de conexao com banco de dados para Hangfire: \n{databaseKeepHealth} \n");
            Console.WriteLine($"(KeepHealth) String de conexao com banco de dados para KeepHealth: \n{databaseKeepHealth} \n");
            Console.WriteLine("Fim parametros da aplicacao \n");

            builder.Services.AddDbContext<KeepHealthContext>(x =>
            {
                x.UseSqlServer(databaseKeepHealth);
                x.EnableSensitiveDataLogging();
                x.EnableDetailedErrors();
            });

            builder.Services.AddIdentity<User, Role>()
                            .AddEntityFrameworkStores<KeepHealthContext>()
                            .AddDefaultTokenProviders();

            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IMedicalConditionRepository, MedicalConditionRepository>();
            builder.Services.AddTransient<IPatientService, PatientService>();
            builder.Services.AddTransient<IPatientRepository, PatientRepository>();
            builder.Services.AddTransient<IPatient_MedicalConditionRepository, Patient_MedicalConditionRepository>();
            builder.Services.AddTransient<RoleManager<Role>>();
            builder.Services.AddTransient<UserManager<User>>();

            Task.Run(() =>
            {
                using (var serviceProvider = builder.Services.BuildServiceProvider())
                {
                    var dbContext = serviceProvider.GetService<KeepHealthContext>();
                    dbContext.Database.Migrate();
                    SeedRoles(serviceProvider).Wait();
                    SeedAdminUser(serviceProvider).Wait();
                }
            });

            builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddRoleValidator<RoleValidator<Role>>()
            .AddEntityFrameworkStores<KeepHealthContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("TokenKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole(RoleName.Admin.ToString()));
                options.AddPolicy("RequireDoctorRole", policy => policy.RequireRole(RoleName.Doctor.ToString()));
                options.AddPolicy("RequirePatientRole", policy => policy.RequireRole(RoleName.Patient.ToString()));
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
                    )
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "KeepHealth.API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header usando Bearer.
                                Entre com 'Bearer ' [espaço] então coloque seu token.
                                Exemplo: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddCors();

            builder.Services.AddHangfire(x => x.UseSqlServerStorage(databaseKeepHealth));
            builder.Services.AddHangfireServer(x => x.WorkerCount = 1);

            builder.Services.AddMvc();
            builder.Services.AddRouting();

            var app = builder.Build();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyMethod()
                       .AllowAnyOrigin()
                       .AllowAnyHeader();
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var roles = new List<string>() { RoleName.Patient.ToString(), RoleName.Doctor.ToString(), RoleName.Admin.ToString() };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role { Name = role });
                }
            }
        }

        private static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var adminEmail = "admin@admin.com";
            var adminPassword = "admin";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            var user = new User { Name = adminPassword, Email = adminEmail, UserName = adminPassword };
            if (adminUser == null)
            {
                await userManager.CreateAsync(user, adminPassword);
            }
            if (!await userManager.IsInRoleAsync(adminUser ?? user, RoleName.Admin.ToString()))
                await userManager.AddToRoleAsync(adminUser ?? user, RoleName.Admin.ToString());
        }
    }
}