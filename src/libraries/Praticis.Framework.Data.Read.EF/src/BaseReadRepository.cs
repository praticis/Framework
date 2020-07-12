
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

namespace Praticis.Framework.Data.Read.EF
{
    public class BaseReadRepository<TModel> : IBaseReadRepository<TModel>
        where TModel : class, IModel
    {
        private readonly IServiceBus ServiceBus;
        protected DbContext Context { get; private set; }
        protected DbSet<TModel> Db { get; private set; }
        
        public BaseReadRepository(DbContext context, IServiceBus serviceBus)
        {
            this.ServiceBus = serviceBus;
            this.Context = context;
            this.Db = context.Set<TModel>();
        }

        public virtual bool Exists(Guid id)
            => this.SearchByIdAsync(id).GetAwaiter().GetResult() != null;

        public virtual bool Exists(TModel model)
        {
            if (model is null)
            {
                this.ServiceBus.PublishEvent(new SystemError("Model can not be null"));
                return false;
            }

            return this.SearchByIdAsync(model.Id).GetAwaiter().GetResult() != null;
        }

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

        public virtual Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, Action<BasePaginationFilter> filter)
        {
            var options = new BasePaginationFilter();

            filter.Invoke(options);

            return this.FindAsync(predicate, options);
        }

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

        public virtual Task<IList<TModel>> GetAllAsync(Action<BasePaginationFilter> filter)
        {
            var options = new BasePaginationFilter();

            filter.Invoke(options);

            return this.GetAllAsync(options);
        }

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

        public virtual void Dispose()
        {
            this.Context.Dispose();
            this.Context = null;
            this.Db = null;
        }
    }
}