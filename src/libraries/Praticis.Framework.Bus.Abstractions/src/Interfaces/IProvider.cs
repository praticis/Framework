
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// A wrapper implementation about IServiceProvider that provide same methods of 
    /// system service provider but nothing by extensions method. 
    /// This enable a readable constructor receiving only IProvider and get inside 
    /// the necessary instances, and mock tests.
    /// </summary>
    public interface IProvider : IServiceProvider
    {
        /// <summary>
        /// Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
        /// User <strong>CreateNewInstance</strong> parameter to obtains new instance, 
        /// still having existing scoped instance in DI stack.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="createNewInstance"> Set <strong>True</strong> to create new instance, still having existing scoped instance will be returned new object.</param>
        /// <returns>
        /// Returns a service object of type <typeparamref name="T"/> or null if there is no such service.
        /// </returns>
        T GetService<T>(bool createNewInstance = false);

        /// <summary>
        /// Get service of type <paramref name="serviceType"/> from the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType"/>.</returns>
        /// <exception cref="InvalidOperationException">There is no service of type <paramref name="serviceType"/>.</exception>
        object GetRequiredService(Type serviceType);

        /// <summary>
        /// Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <returns>A service object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException">There is no service of type <typeparamref name="T"/>.</exception>
        T GetRequiredService<T>();

        /// <summary>
        /// Get an enumeration of services of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <returns>An enumeration of services of type <typeparamref name="T"/>.</returns>
        IEnumerable<T> GetServices<T>();

        /// <summary>
        /// Get an enumeration of services of type <paramref name="serviceType"/> from the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>An enumeration of services of type <paramref name="serviceType"/>.</returns>
        IEnumerable<object> GetServices(Type serviceType);

        /// <summary>
        /// Creates a new <see cref="IServiceScope"/> that can be used to resolve scoped services.
        /// </summary>
        /// <returns>A <see cref="IServiceScope"/> that can be used to resolve scoped services.</returns>
        public IServiceScope CreateScope();
    }
}