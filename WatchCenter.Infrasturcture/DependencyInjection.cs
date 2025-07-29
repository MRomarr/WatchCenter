using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WatchCenter.Infrasturcture.Services.FormFileService;

namespace WatchCenter.Infrasturcture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddIdentity();
            services.AddJWT(configuration);
            services.AddEmailServices(configuration);
            services.AddRepositores();

            services.AddScoped<IFormFileService, FormFileService>();
            return services;
        }
        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection String Not Found");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            return services;
        }

        private static IServiceCollection AddJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    var jwtSettings = configuration.GetSection(JwtSettings.Section).Get<JwtSettings>();
                    if (jwtSettings == null)
                    {
                        throw new InvalidOperationException("JWT settings not found in configuration.");
                    }
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ClockSkew = TimeSpan.Zero // Optional: Set clock skew to zero for immediate expiration
                    };
                });

            services.AddScoped<ITokenProvider, JwtTokeProvider>();
            
            return services;
        }
        private static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.Section));

            var emailSettings = configuration.GetSection(EmailSettings.Section).Get<EmailSettings>()
                ?? throw new InvalidOperationException("Email settings are not configured properly.");

            if (emailSettings.UseSmtp4Dev)
            {
                emailSettings.Host = "localhost";
                emailSettings.Port = 25;
                emailSettings.FromEmail = "noreply@localhost";
                services.AddFluentEmail(emailSettings.FromEmail)
                .AddSmtpSender(emailSettings.Host, emailSettings.Port);
            }
            else
            {
                services.AddFluentEmail(emailSettings.FromEmail, emailSettings.FromName)
                    .AddSmtpSender(emailSettings.Host, emailSettings.Port, emailSettings.Username, emailSettings.Password);
            }

            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
        private static IServiceCollection AddRepositores(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
