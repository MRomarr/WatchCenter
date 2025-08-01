using Microsoft.Extensions.DependencyInjection;

namespace WatchCenter.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            services.AddApplicationServices();  
            return services;
        }
        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<ISeriesService, SeriesService>();

            return services;
        }
    }
}
