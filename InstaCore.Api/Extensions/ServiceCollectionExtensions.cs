using System.Reflection;
using InstaCore.Core.Contracts;

namespace InstaCore.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserDefinedServices(this IServiceCollection services, Assembly serviceAssembly)
        {
            Type[] serviceClasses = serviceAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();

            foreach (Type serviceClass in serviceClasses)
            {
                Type[] serviceInterfaces = serviceClass.GetInterfaces();

                Type? serviceInterface = serviceInterfaces.FirstOrDefault(i => i.Name == "I" + serviceClass.Name);

                if (serviceInterface != null)
                {
                    services.AddScoped(serviceInterface, serviceClass);
                }
            }

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, Assembly repositoryAssembly)
        {
            Type[] repositoryClasses = repositoryAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Repository") && !t.IsInterface)
                .ToArray();

            foreach (Type repositoryClass in repositoryClasses)
            {
                Type[] repositoryInterfaces = repositoryClass.GetInterfaces();

                Type? repositoryInterface = repositoryInterfaces.FirstOrDefault(i => i.Name == "I" + repositoryClass.Name);

                if (repositoryInterface != null)
                {
                    services.AddScoped(repositoryInterface, repositoryClass);
                }
            }

            return services;
        }

    }
}
