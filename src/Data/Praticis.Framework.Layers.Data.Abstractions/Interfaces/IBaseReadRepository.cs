
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Praticis.Framework.Layers.Data.Abstractions.Filters;
using Praticis.Framework.Layers.Domain.Abstractions.Objects;

namespace Praticis.Framework.Layers.Data.Abstractions
{
    public interface IBaseReadRepository<TModel, TId> : IDisposable
        where TModel : IdentifiedObject<TId>
    {
        /// <summary>
        /// Verify if a model exists by identification key.
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Returns <strong>True</strong> if model exists or  <strong>False</strong> if does not exist.
        /// </returns>
        bool Exists(TId id);

        /// <summary>
        /// Verify if a model exists by main identification properties. The default is the key.
        /// </summary>
        /// <param name="model">The model to find.</param>
        /// <returns>
        /// Returns <strong>True</strong> if model exists or <strong>False</strong> if does not exist.
        /// </returns>
        bool Exists(TModel model);

        /// <summary>
        /// Verify if any model of repository satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <returns>
        /// Returns <strong>True</strong> if any model pass the test in the specified predicate or 
        /// <strong>False</strong> if does not pass.
        /// </returns>
        bool Exists(Expression<Func<TModel, bool>> predicate);

        /// <summary>
        /// Find a model by <see cref="TId"/>.
        /// Use var result = await SearchByIdAsync(id);
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Returns a model if found. Null will be returned if not found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        Task<TModel> FindByIdAsync(TId id);

        /// <summary>
        /// Filters a sequence of models based on a predicate.
        /// Use <see cref="FindAsync(Expression{Func{TModel, bool}}, BasePaginationFilter)"/> or 
        /// <seealso cref="FindAsync(Expression{Func{TModel, bool}}, Action{BasePaginationFilter})"/> to 
        /// configure filters and load on demand.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate);

        /// <summary>
        /// Filters a sequence of models based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <param name="filter">The filter and options of the search.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, BasePaginationFilter filter);

        /// <summary>
        /// Filters a sequence of models based on a predicate. Use <paramref name="filter"/> to configure a parameterized search.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <param name="filter">The filter and options of the search.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, Action<BasePaginationFilter> filter);

        /// <summary>
        /// Create a LINQ queryable of model.
        /// </summary>
        /// <returns>
        /// Returns a queryable of model.
        /// </returns>
        IQueryable<TModel> Query();

        /// <summary>
        /// Count total existing models in database.
        /// </summary>
        /// <returns>Returns quantity of items in database or 0 if nothing exists.</returns>
        Task<long> CountAsync();

        /// <summary>
        /// Count total existing models in database based on predicate.
        /// </summary>
        /// <returns>
        /// Returns quantity of models in database matching with predicate or 0 if nothing match.
        /// </returns>
        Task<long> CountAsync(Expression<Func<TModel, bool>> predicate);

        /// <summary>
        /// Load all entities. Is not recommended for many data.
        /// Use <see cref="GetAllAsync(Action{BasePaginationFilter})"/> or <seealso cref="GetAllAsync(BasePaginationFilter)"/> to 
        /// filter and load on demand.
        /// </summary>
        /// <returns>
        /// Returns a collection with all models. Empty list will be returned if not exists entities.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<IEnumerable<TModel>> GetAllAsync();

        /// <summary>
        /// Load all models. Use <see cref="BasePaginationFilter.PageNumber"/> and <seealso cref="BasePaginationFilter.PageSize"/> 
        /// parameters to limit return length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        Task<IEnumerable<TModel>> GetAllAsync(BasePaginationFilter filter);

        /// <summary>
        /// Load all entities. Use PageIndex and PageSize parameters to limit return length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        Task<IEnumerable<TModel>> GetAllAsync(Action<BasePaginationFilter> filter);
    }
}