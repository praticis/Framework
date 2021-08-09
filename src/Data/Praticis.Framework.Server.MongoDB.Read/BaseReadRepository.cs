
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MongoDB.Driver;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Server.Data.Abstractions;
using Praticis.Framework.Server.Data.Abstractions.Filters;
using Praticis.Framework.Layers.Domain.Abstractions.Objects;
using Praticis.Framework.Server.Data.MongoDB.Abstractions;

namespace Praticis.Framework.Server.Data.MongoDB.Read
{
    public abstract class BaseReadRepository<TModel, TId> : IBaseReadRepository<TModel, TId>
        where TModel : IdentifiedObject<TId>
    {
        protected IMongoCollection<TModel> Db { get; private set; }
        protected IServiceBus ServiceBus { get; private set; }

        #region Contructors

        public BaseReadRepository(DbContext context, IServiceBus serviceBus)
        {
            this.Db = context.Set<TModel>();
            this.ServiceBus = serviceBus;
        }

        public BaseReadRepository(DbContext context, IServiceBus serviceBus, string collection)
        {
            this.Db = context.Set<TModel>(collection);
            this.ServiceBus = serviceBus;
        }

        #endregion

        /// <summary>
        /// Verify if a model exists by identification key.
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Returns <strong>True</strong> if model exists or  <strong>False</strong> if does not exist.
        /// </returns>
        public virtual bool Exists(TId id)
            => this.Db.CountDocumentsAsync(e => e.Id.Equals(id)).GetAwaiter().GetResult() > 0;

        /// <summary>
        /// Verify if a model exists by main identification properties. The default is the key.
        /// </summary>
        /// <param name="model">The model to find.</param>
        /// <returns>
        /// Returns <strong>True</strong> if model exists or <strong>False</strong> if does not exist.
        /// </returns>
        public virtual bool Exists(TModel model)
            => this.Exists(model.Id);

        /// <summary>
        /// Verify if any model of repository satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <returns>
        /// Returns <strong>True</strong> if any model pass the test in the specified predicate or 
        /// <strong>False</strong> if does not pass.
        /// </returns>
        public virtual bool Exists(Expression<Func<TModel, bool>> predicate)
            => this.Db.CountDocumentsAsync(predicate).GetAwaiter().GetResult() > 0;

        /// <summary>
        /// Find a model by identification key.
        /// Use var result = await SearchByIdAsync(id);
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Returns a model if found. Null will be returned if not found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual async Task<TModel> FindByIdAsync(TId id)
        {
            try
            {
                var findResult = await this.Db.FindAsync(Builders<TModel>.Filter.Eq(m => m.Id, id));

                return await findResult.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                var msg = $"An error occurred on find a model of type {typeof(TModel).FullName} by id.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, id, e));
            }

            return default(TModel);
        }

        /// <summary>
        /// Counts the number of models in the database.
        /// </summary>
        /// <returns>Returns the number of models if exists or 0 if is empty.</returns>
        public virtual Task<long> CountAsync()
            => this.Db.CountDocumentsAsync(Builders<TModel>.Filter.Empty);

        /// <summary>
        /// Counts the number of models in database that satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <returns>
        /// Returns a model if found. Null will be returned if not found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<long> CountAsync(Expression<Func<TModel, bool>> predicate)
            => this.Db.CountDocumentsAsync(predicate);

        /// <summary>
        /// Create a LINQ queryable of model.
        /// </summary>
        /// <returns>
        /// Returns a queryable of model.
        /// </returns>
        public virtual IQueryable<TModel> Query() => this.Db.AsQueryable();

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
        public virtual async Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate)
        {
            try
            {
                var findResult = await this.Db.FindAsync(predicate);

                return await findResult.ToListAsync();
            }
            catch (Exception e)
            {
                var msg = $"An error occurred on find models of type {typeof(TModel).FullName}.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, e));
            }

            return new List<TModel>();
        }

        /// <summary>
        /// Filters a sequence of models based on a predicate. Use filter to configure a parameterized search.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <param name="filter">The filter and options of the search.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, Action<BasePaginationFilter> filter)
        {
            var filterOptions = new BasePaginationFilter();
            filter.Invoke(filterOptions);

            return this.FindAsync(predicate, filterOptions);
        }

        /// <summary>
        /// Filters a sequence of models based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <param name="filter">The filter and options of the search.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual async Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, BasePaginationFilter filter)
        {
            try
            {
                var findResult = await this.Db.FindAsync(predicate, new FindOptions<TModel, TModel>
                {
                    Skip = filter.SkipLength,
                    Limit = filter.PageSize,
                });

                return await findResult.ToListAsync();
            }
            catch (Exception e)
            {
                var msg = $"An error occurred on find models of type {typeof(TModel).FullName}.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, filter, e));
            }

            return new List<TModel>();
        }

        /// <summary>
        /// Load all entities. Use PageIndex and PageSize parameters to limit return length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<IEnumerable<TModel>> GetAllAsync(Action<BasePaginationFilter> filter)
        {
            var filterOptions = new BasePaginationFilter();

            filter.Invoke(filterOptions);

            return this.GetAllAsync(filterOptions);
        }

        /// <summary>
        /// Load all entities. Use PageIndex and PageSize parameters to limit return length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual async Task<IEnumerable<TModel>> GetAllAsync(BasePaginationFilter filter)
        {
            try
            {
                var collection = await this.Db.FindAsync(Builders<TModel>.Filter.Empty, new FindOptions<TModel, TModel>
                {
                    Skip = filter.SkipLength,
                    Limit = filter.PageSize
                });

                return await collection.ToListAsync();
            }
            catch (Exception e)
            {
                var msg = $"An error occurred on get all models of type {typeof(TModel).FullName}.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, filter, e));
            }

            return new List<TModel>();
        }

        /// <summary>
        /// Load all entities. Is not recommended for many data.
        /// Use <see cref="GetAllAsync(Action{BasePaginationFilter})"/> or <seealso cref="GetAllAsync(BasePaginationFilter)"/> to 
        /// filter and load on demand.
        /// </summary>
        /// <returns>
        /// Returns a model collection with all entities. Empty list will be returned if not exists entities.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
            try
            {
                var collection = await this.Db.FindAsync<TModel>(Builders<TModel>.Filter.Empty);

                return await collection.ToListAsync();
            }
            catch (Exception e)
            {
                var msg = $"An error occurred on get all models of type {typeof(TModel).FullName}.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, e));
            }

            return new List<TModel>();
        }

        public virtual void Dispose()
        {

        }
    }
}