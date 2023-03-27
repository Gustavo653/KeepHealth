using Common.Functions;
using Hangfire;
using KeepNotes.Application;
using KeepNotes.Domain.Identity;
using KeepNotes.Persistence;
using KeepNotes.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;


namespace KeepNotes.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            string databaseKeepNotes = Environment.GetEnvironmentVariable("KeepNotes") ?? configuration.GetConnectionString("KeepNotes");

            Console.WriteLine("Inicio parametros da aplicacao: \n");
            Console.WriteLine($"(KeepNotes) String de conexao com banco de dados para Hangfire: \n{databaseKeepNotes} \n");
            Console.WriteLine($"(KeepNotes) String de conexao com banco de dados para KeepNotes: \n{databaseKeepNotes} \n");
            Console.WriteLine("Fim parametros da aplicacao \n");

            builder.Services.AddDbContext<KeepNotesContext>(x =>
            {
                x.UseSqlServer(databaseKeepNotes);
                x.EnableSensitiveDataLogging();
                x.EnableDetailedErrors();
            });

            var optionsKeepNotes = builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<KeepNotesContext>>();

            Task.Run(() =>
            {
                using var dbContext = new KeepNotesContext(optionsKeepNotes);
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
            .AddEntityFrameworkStores<KeepNotesContext>()
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

            builder.Services.AddHangfire(x => x.UseSqlServerStorage(databaseKeepNotes));
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