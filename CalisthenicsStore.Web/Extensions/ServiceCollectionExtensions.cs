using System.Reflection;
using CalisthenicsStore.Data.Models.ReCaptcha;
using CalisthenicsStore.Services;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.Web.Models;

namespace CalisthenicsStore.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserDefinedServices(this IServiceCollection serviceCollection, Assembly serviceAssembly)
        {
            Type[] serviceClasses = serviceAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Service") &&
                            !t.IsInterface)
                .ToArray();

            foreach (Type serviceClass in serviceClasses)
            {
                Type[] serviceInterfaces = serviceClass
                    .GetInterfaces();


                if (serviceInterfaces.Length == 1 &&
                    serviceInterfaces.First().Name.StartsWith("I") &&
                    serviceInterfaces.First().Name.EndsWith("Service"))
                {
                    Type serviceInterface = serviceInterfaces.First();

                    serviceCollection.AddScoped(serviceInterface, serviceClass);
                }
                
            }

            return serviceCollection;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection, Assembly repositoryAssembly)
        {
            Type[] repositoryClasses = repositoryAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Repository") &&
                            !t.IsInterface &&
                            !t.IsAbstract)
                .ToArray();

            foreach (Type repositoryClass in repositoryClasses)
            {
                Type[] repositoryInterfaces = repositoryClass
                    .GetInterfaces();

                Type? repositoryInterface = repositoryInterfaces
                    .FirstOrDefault(i => i.Name == $"I{repositoryClass.Name}");

                if (repositoryInterface != null)
                {
                    serviceCollection.AddScoped(repositoryInterface, repositoryClass);
                }

            }

            return serviceCollection;
        }

        public static IServiceCollection AddSupabase(this IServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.AddSingleton(_ =>
            {
                var url = config["Supabase:Url"];
                var key = config["Supabase:ServiceRoleKey"];
                var options = new Supabase.SupabaseOptions
                {
                    AutoConnectRealtime = false,
                };

                return new Supabase.Client(url!, key, options);
            });

            return serviceCollection;
        }

        public static IServiceCollection AddReCaptcha(this IServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.Configure<GoogleReCaptchaSettings>(config.GetSection("GoogleReCaptcha"));
            serviceCollection.AddHttpClient<IReCaptchaServ, ReCaptchaServ>();

            return serviceCollection;
        }

        public static IServiceCollection AddStripe(this IServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.Configure<StripeSettings>(config.GetSection("Stripe"));

            return serviceCollection;
        }

        public static IServiceCollection AddCartSession(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            return serviceCollection;
        }
    }
}
