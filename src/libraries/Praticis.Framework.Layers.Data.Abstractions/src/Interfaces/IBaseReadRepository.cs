
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Praticis.Framework.Layers.Data.Abstractions.Filters;
using Praticis.Framework.Layers.Domain.Abstractions;

namespace Praticis.Framework.Layers.Data.Abstractions
{
    public interface IBaseReadRepository<TModel> : IDisposable
        where TModel : class, IModel
    {
        /// <summary>
        /// Verify if a model exists by identification key.
        /// </summary>
        /// <param name="id">Identification key of model.</param>
        /// <returns>
        /// Return 'True' if model exists or 'False' does not exist.
        /// </returns>
        bool Exists(Guid id);

        /// <summary>
        /// Verify if a model exists by main identification properties. Per default, is the key.
        /// </summary>
        /// <param name="model">The model to find.</param>
        /// <returns>
        /// Return 'True' if model exists or 'False' does not exist.
        /// </returns>
        bool Exists(TModel model);

        /// <summary>
        /// Find a model by identification key.
        /// Use var result = await SearchByIdAsync(id);
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Return a model if found. Null will be returned if entity not found.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<TModel> SearchByIdAsync(Guid id);

        /// <summary>
        /// Filters a sequence of models based on a predicate.
        /// Use <see cref="FindAsync(Expression{Func{TModel, bool}}, BasePaginationFilter)"/> or 
        /// <seealso cref="FindAsync(Expression{Func{TModel, bool}}, Action{BasePaginationFilter})"/> to 
        /// configure filters and load on demand.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate);

        /// <summary>
        /// Filters a sequence of models based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <param name="filter">The filter and options of the search.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, BasePaginationFilter filter);

        /// <summary>
        /// Filters a sequence of models based on a predicate. Use filter to configure a parameterized search.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <param name="filter">The filter and options of the search.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, Action<BasePaginationFilter> filter);

        /// <summary>
        /// Load all entities. Is not recommended for many data.
        /// Use <see cref=""/> to filter or <seealso cref="GetAllAsync(BasePaginationFilter)"/> to load on demand.
        /// </summary>
        /// <returns>
        /// Returns a model collection with all entities. Empty list will be returned if not existis entities.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<IList<TModel>> GetAllAsync();

        /// <summary>
        /// Load all entities. Use PageIndex and PageSize parameters to limit reeturn length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<IList<TModel>> GetAllAsync(BasePaginationFilter filter);

        /// <summary>
        /// Load all entities. Use PageIndex and PageSize parameters to limit reeturn length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<IList<TModel>> GetAllAsync(Action<BasePaginationFilter> filter);
    }
}