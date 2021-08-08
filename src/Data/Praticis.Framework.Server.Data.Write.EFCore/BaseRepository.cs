﻿
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Praticis.Framework.Bus.Abstractions;

using Praticis.Framework.Layers.Data.Abstractions;
using Praticis.Framework.Layers.Data.Abstractions.Filters;
using Praticis.Framework.Layers.Domain.Abstractions;

namespace Praticis.Framework.Server.Data.Write.EFCore
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

        /// <summary>
        /// Save a model. If not exists it is created, if exists is updated.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="model">The model to save.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual async Task<bool> SaveAsync(TModel model)
        {
            bool saved;

            if (this.Exists(model))
                saved = await this.Update(model);
            else
                saved = await this.Add(model);

            return saved;
        }

        /// <summary>
        /// The models that do not exists will be created and existing entities will be updated.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="models">The models to save.</param>
        /// <returns>
        /// Returns a task with saving action running.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task SaveRangeAsync(IEnumerable<TModel> models)
        {
            List<Task> tasks = new List<Task>();

            foreach (var model in models)
                tasks.Add(this.SaveAsync(model));

            Task.WaitAll(tasks.ToArray());

            return Task.CompletedTask;
        }

        /// <summary>
        /// The models that do not exists will be created and existing entities will be updated.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="models">The models to save.</param>
        /// <returns>
        /// Returns a task with saving action running.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task SaveRangeAsync(params TModel[] models)
        {
            List<Task> tasks = new List<Task>();

            foreach (var model in models)
                tasks.Add(this.SaveAsync(model));

            Task.WaitAll(tasks.ToArray());

            return Task.CompletedTask;
        }

        /// <summary>
        /// Insert a model to the database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="model">The model to add on database.</param>
        /// <returns>
        /// Returns a task with saving action running.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
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

        /// <summary>
        /// Update a model on database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="model">The model to update on database.</param>
        /// <returns>
        /// Returns a task with saving action running.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
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

        /// <summary>
        /// Remove a model from database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="id">The model id to find and remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
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

        /// <summary>
        /// Remove models from database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="ids">The model IDs to find and remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task RemoveAsync(params Guid[] ids)
        {
            List<Task> tasks = new List<Task>();
            
            foreach (var id in ids)
                tasks.Add(this.RemoveAsync(id));

            Task.WaitAll(tasks.ToArray());

            return Task.CompletedTask;
        }

        /// <summary>
        /// Remove a model from database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="model">The model to remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
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

        /// <summary>
        /// Remove models from database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="models">The models to remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task RemoveAsync(params TModel[] models)
        {
            List<Task> tasks = new List<Task>();

            foreach (var model in models)
                tasks.Add(this.RemoveAsync(model));

            Task.WaitAll(tasks.ToArray());

            return Task.CompletedTask;
        }

        /// <summary>
        /// Save all changes in database.
        /// </summary>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
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

        /// <summary>
        /// Asynchronously save all changes in database.
        /// </summary>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
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

        /// <summary>
        /// Verify if a model exists by identification key.
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Returns <strong>True</strong> if model exists or  <strong>False</strong> if does not exist.
        /// </returns>
        public virtual bool Exists(Guid id) => this._readRepository.Exists(id);

        /// <summary>
        /// Verify if a model exists by main identification properties. The default is the key.
        /// </summary>
        /// <param name="model">The model to find.</param>
        /// <returns>
        /// Returns <strong>True</strong> if model exists or <strong>False</strong> if does not exist.
        /// </returns>
        public virtual bool Exists(TModel model) => this._readRepository.Exists(model);

        /// <summary>
        /// Find a model by identification key.
        /// Use var result = await SearchByIdAsync(id);
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Returns a model if found. Null will be returned if not found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<TModel> SearchByIdAsync(Guid id) => this._readRepository.SearchByIdAsync(id);

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
        public virtual Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate) 
            => this._readRepository.FindAsync(predicate);

        /// <summary>
        /// Filters a sequence of models based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <param name="filter">The filter and options of the search.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, BasePaginationFilter filter) 
            => this._readRepository.FindAsync(predicate, filter);

        /// <summary>
        /// Filters a sequence of models based on a predicate. Use filter to configure a parameterized search.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <param name="filter">The filter and options of the search.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<IList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, Action<BasePaginationFilter> filter)
            => this._readRepository.FindAsync(predicate, filter);

        /// <summary>
        /// Load all entities. Is not recommended for many data.
        /// Use <see cref="GetAllAsync(Action{BasePaginationFilter})"/> or <seealso cref="GetAllAsync(BasePaginationFilter)"/> to 
        /// filter and load on demand.
        /// </summary>
        /// <returns>
        /// Returns a model collection with all entities. Empty list will be returned if not exists entities.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        public virtual Task<IList<TModel>> GetAllAsync() => this._readRepository.GetAllAsync();

        /// <summary>
        /// Load all entities. Use PageIndex and PageSize parameters to limit return length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<IList<TModel>> GetAllAsync(BasePaginationFilter filter) => this._readRepository.GetAllAsync(filter);

        /// <summary>
        /// Load all entities. Use PageIndex and PageSize parameters to limit return length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<IList<TModel>> GetAllAsync(Action<BasePaginationFilter> filter) => this._readRepository.GetAllAsync(filter);

        #endregion

        /// <summary>
        /// Set null and dispose entity framework objects.
        /// </summary>
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