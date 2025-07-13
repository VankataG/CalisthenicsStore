using System.Reflection;

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
    }
}
