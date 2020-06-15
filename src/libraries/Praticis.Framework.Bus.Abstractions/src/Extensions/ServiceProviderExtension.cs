
namespace System
{
    public static class ServiceProviderExtension
    {
        /// <summary>
        /// Obtains an object instance from dependency injection container.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <param name="provider">The dependency injection container.</param>
        /// <returns>
        /// Returns an instance of type or null if there is no service object of type service type.
        /// </returns>
        public static T GetInstance<T>(this IServiceProvider provider)
            => provider.GetService<T>();

        /// <summary>
        /// Obtains an object instance from dependency injection container.
        /// Use <see cref="GetInstance{T}(IServiceProvider)"/> if has ambiguous reference with 
        /// microsoft dependency injection extensions.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <param name="provider">The dependency injection container.</param>
        /// <returns>
        /// Returns an instance of type or null if there is no service object of type service type.
        /// </returns>
        public static T GetService<T>(this IServiceProvider provider)
            => (T)provider.GetService(typeof(T));
    }
}