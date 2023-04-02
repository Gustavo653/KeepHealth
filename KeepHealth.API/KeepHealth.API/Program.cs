using Common.Functions;
using Hangfire;
using KeepHealth.Application;
using KeepHealth.Domain.Identity;
using KeepHealth.Persistence;
using KeepHealth.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            var optionsKeepHealth = builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<KeepHealthContext>>();

            Task.Run(() =>
            {
                using var dbContext = new KeepHealthContext(optionsKeepHealth);
                var model = dbContext.Model;
                dbContext.Database.Migrate();
            });

            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();

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

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "SO.API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header usando Bearer.
                                Entre com 'Bearer ' [espa�o] ent�o coloque seu token.
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
    }
}