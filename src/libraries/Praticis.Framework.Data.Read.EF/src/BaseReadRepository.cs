
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Praticis.Framework.Bus.Abstractions;

using Praticis.Framework.Layers.Data.Abstractions;
using Praticis.Framework.Layers.Data.Abstractions.Filters;
using Praticis.Framework.Layers.Domain.Abstractions;

namespace Praticis.Framework.Server.Data.Read.EF
{
    public class BaseReadRepository<TModel> : IBaseReadRepository<TModel>
        where TModel : class, IModel
    {
        /// <summary>
        /// The service bus.
        /// </summary>
        protected readonly IServiceBus ServiceBus;
        /// <summary>
        /// The entity framework context.
        /// </summary>
        protected DbContext Context { get; private set; }
        /// <summary>
        /// The entity framework DbSet of this model.
        /// </summary>
        protected DbSet<TModel> Db { get; private set; }
        
        /// <summary>
        /// Create a base repository for read operations.
        /// </summary>
        /// <param name="context">The entity framework context.</param>
        /// <param name="serviceBus">The service bus.</param>
        public BaseReadRepository(DbContext context, IServiceBus serviceBus)
        {
            this.ServiceBus = serviceBus;
            this.Context = context;
            this.Db = context.Set<TModel>();
        }

        /// <summary>
        /// Verify if a model exists by identification key.
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Returns <strong>True</strong> if model exists or  <strong>False</strong> if does not exist.
        /// </returns>
        public virtual bool Exists(Guid id)
            => this.SearchByIdAsync(id).GetAwaiter().GetResult() != null;

        /// <summary>
        /// Verify if a model exists by main identification properties. The default is the key.
        /// </summary>
        /// <param name="model">The model to find.</param>
        /// <returns>
        /// Returns <strong>True</strong> if model exists or <strong>False</strong> if does not exist.
        /// </returns>
        public virtual bool Exists(TModel model)
        {
            if (model is null)
            {
                this.ServiceBus.PublishEvent(new SystemError("Model can not be null"));
                return false;
            }

            return this.SearchByIdAsync(model.Id).GetAwaiter().GetResult() != null;
        }

        /// <summary>
        /// Find a model by identification key.
        /// Use var result = await SearchByIdAsync(id);
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Returns a model if found. Null will be returned if not found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual async Task<TModel> SearchByIdAsync(Guid id)
        {
            TModel model;

            try
            {
                model = await this.Db.Where(m => m.Id == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                model = default;
                var msg = string.Format($"An error ocurred during search {typeof(TModel).Name} model");
                await this.ServiceBus.PublishEvent(new SystemError(msg, id, e));
            }

            return model;
        }

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
        public virtual async Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate)
        {
            try
            {
                return await this.Db.Where(predicate)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                var msg = string.Format($"An error ocurred during find {typeof(TModel).Name} models");
                await this.ServiceBus.PublishEvent(new SystemError(msg, predicate, e));
            }

            return new List<TModel>();
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
        public virtual Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, Action<BasePaginationFilter> filter)
        {
            var options = new BasePaginationFilter();

            filter.Invoke(options);

            return this.FindAsync(predicate, options);
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
        public virtual async Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, BasePaginationFilter filter)
        {
            try
            {
                return await this.Db.Where(predicate)
                    .Skip(filter.PageNumber * filter.PageSize)
                    .Take(filter.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                var msg = string.Format($"An error ocurred during find ${typeof(TModel).Name} models");
                await this.ServiceBus.PublishEvent(new SystemError(msg, predicate, e));
            }

            return new List<TModel>();
        }

        /// <summary>
        /// Load all entities. Is not recommended for many data.
        /// Use <see cref="GetAllAsync(Action{BasePaginationFilter})"/> or <seealso cref="GetAllAsync(BasePaginationFilter)"/> to 
        /// filter and load on demand.
        /// </summary>
        /// <returns>
        /// Returns a model collection with all entities. Empty list will be returned if not existis entities.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        public virtual async Task<IList<TModel>> GetAllAsync()
        {
            IList<TModel> collection;

            try
            {
                collection = await this.Db.AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                collection = new List<TModel>();
                var msg = string.Format($"An error ocurred during search {typeof(TModel).Name} models");
                await this.ServiceBus.PublishEvent(new SystemError(msg, e));
            }

            return collection;
        }

        /// <summary>
        /// Load all entities. Use PageIndex and PageSize parameters to limit return length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<IList<TModel>> GetAllAsync(Action<BasePaginationFilter> filter)
        {
            var options = new BasePaginationFilter();

            filter.Invoke(options);

            return this.GetAllAsync(options);
        }

        /// <summary>
        /// Load all entities. Use PageIndex and PageSize parameters to limit return length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual async Task<IList<TModel>> GetAllAsync(BasePaginationFilter filter)
        {
            IList<TModel> collection;

            try
            {
                collection = await this.Db.Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                collection = new List<TModel>();
                var msg = string.Format($"An error ocurred during search {typeof(TModel).Name} models");
                await this.ServiceBus.PublishEvent(new SystemError(msg, filter, e.Message));
            }

            return collection;
        }

        /// <summary>
        /// Set null and dispose entity framework objects.
        /// </summary>
        public virtual void Dispose()
        {
            this.Context.Dispose();
            this.Context = null;
            this.Db = null;
        }
    }
}