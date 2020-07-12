
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Praticis.Framework.Bus.Abstractions;

using Praticis.Framework.Layers.Data.Abstractions;
using Praticis.Framework.Layers.Data.Abstractions.Filters;
using Praticis.Framework.Layers.Domain.Abstractions;

namespace Praticis.Framework.Data.Write.EF
{
    public class BaseRepository<TModel> : IBaseRepository<TModel>
        where TModel : class, IModel
    {
        private readonly IServiceBus _serviceBus;
        protected DbContext Context { get; private set; }
        protected DbSet<TModel> Db { get; private set; }
        private IBaseReadRepository<TModel> _readRepository { get; set; }

        public BaseRepository(DbContext context, IBaseReadRepository<TModel> readRepository, IServiceBus serviceBus)
        {
            this.Context = context;
            this.Db = context.Set<TModel>();
            this._readRepository = readRepository;
            this._serviceBus = serviceBus;
        }

        public virtual async Task<bool> SaveAsync(TModel model)
        {
            bool saved;

            if (this.Exists(model))
                saved = await this.Update(model);
            else
                saved = await this.Add(model);

            return saved;
        }

        public virtual Task SaveRangeAsync(IEnumerable<TModel> models)
        {
            List<Task> tasks = new List<Task>();

            foreach (var model in models)
                tasks.Add(this.SaveAsync(model));

            Task.WaitAll(tasks.ToArray());

            return Task.CompletedTask;
        }

        public virtual Task SaveRangeAsync(params TModel[] models)
        {
            List<Task> tasks = new List<Task>();

            foreach (var model in models)
                tasks.Add(this.SaveAsync(model));

            Task.WaitAll(tasks.ToArray());

            return Task.CompletedTask;
        }

        protected virtual async Task<bool> Add(TModel model)
        {
            try
            {
                this.Db.Add(model);

                return true;
            }
            catch (Exception e)
            {
                var msg = $"An error ocurred during add {typeof(TModel).Name} model";
                await this._serviceBus.PublishEvent(new SystemError(msg, model, e));

                return false;
            }
        }

        protected virtual async Task<bool> Update(TModel model)
        {
            try
            {
                this.Db.Update(model);

                return true;
            }
            catch (Exception e)
            {
                var msg = $"An error ocurred during update {typeof(TModel).Name} model";
                await this._serviceBus.PublishEvent(new SystemError(msg, model, e));

                return false;
            }
        }

        public virtual async Task<bool> RemoveAsync(Guid id)
        {
            try
            {
                var model = await this.SearchByIdAsync(id);

                if (model is null)
                    return false;

                this.Db.Remove(model);
                model = null;

                return true;
            }
            catch (Exception e)
            {
                var msg = string.Format($"An error ocurred during remove {typeof(TModel).Name} model");
                await this._serviceBus.PublishEvent(new SystemError(msg, id, e));

                return false;
            }
        }

        public virtual Task RemoveAsync(params Guid[] ids)
        {
            List<Task> tasks = new List<Task>();
            
            foreach (var id in ids)
                tasks.Add(this.RemoveAsync(id));

            Task.WaitAll(tasks.ToArray());

            return Task.CompletedTask;
        }

        public virtual async Task<bool> RemoveAsync(TModel model)
        {
            try
            {
                this.Db.Remove(model);

                return true;
            }
            catch (Exception e)
            {
                var msg = string.Format($"An error ocurred during remove {typeof(TModel).Name} model");
                await this._serviceBus.PublishEvent(new SystemError(msg, model, e));

                return false;
            }
        }

        public virtual Task RemoveAsync(params TModel[] models)
        {
            List<Task> tasks = new List<Task>();

            foreach (var model in models)
                tasks.Add(this.RemoveAsync(model));

            Task.WaitAll(tasks.ToArray());

            return Task.CompletedTask;
        }

        public virtual bool Commit()
        {
            try
            {
                return this.Context.SaveChanges() > 0;
            }
            catch(Exception e)
            {
                var msg = string.Format($"An error ocurred during save changes of {typeof(TModel).Name} model");
                this._serviceBus.PublishEvent(new SystemError(msg, e)).GetAwaiter().GetResult();

                return false;
            }
        }

        public virtual async Task<bool> CommitAsync()
        {
            try
            {
                var affectedRows = await this.Context.SaveChangesAsync();

                return affectedRows > 0;
            }
            catch (Exception e)
            {
                var msg = string.Format($"An error ocurred during save changes of {typeof(TModel).Name} model");
                this._serviceBus.PublishEvent(new SystemError(msg, e)).GetAwaiter().GetResult();

                return false;
            }
        }

        #region Read Repository Calls

        public virtual bool Exists(Guid id) => this._readRepository.Exists(id);

        public virtual bool Exists(TModel model) => this._readRepository.Exists(model);

        public virtual Task<TModel> SearchByIdAsync(Guid id) => this._readRepository.SearchByIdAsync(id);

        public virtual Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate) 
            => this._readRepository.FindAsync(predicate);

        public virtual Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, BasePaginationFilter filter) 
            => this._readRepository.FindAsync(predicate, filter);

        public virtual Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, Action<BasePaginationFilter> filter)
            => this._readRepository.FindAsync(predicate, filter);

        public virtual Task<IList<TModel>> GetAllAsync() => this._readRepository.GetAllAsync();

        public virtual Task<IList<TModel>> GetAllAsync(BasePaginationFilter filter) => this._readRepository.GetAllAsync(filter);

        public virtual Task<IList<TModel>> GetAllAsync(Action<BasePaginationFilter> filter) => this._readRepository.GetAllAsync(filter);

        #endregion

        public virtual void Dispose()
        {
            this.Context?.Dispose();
            this.Context = null;
            this.Db = null;
            this._readRepository?.Dispose();
            this._readRepository = null;
        }
    }
}