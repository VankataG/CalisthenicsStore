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
    }
}
