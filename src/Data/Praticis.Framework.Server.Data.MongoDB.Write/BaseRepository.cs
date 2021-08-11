
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MongoDB.Driver;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Layers.Data.Abstractions;
using Praticis.Framework.Layers.Data.Abstractions.Filters;
using Praticis.Framework.Server.Data.MongoDB.Abstractions;
using Praticis.Framework.Layers.Domain.Abstractions.Objects;

namespace Praticis.Framework.Server.Data.MongoDB.Write
{
    public class BaseRepository<TModel, TId> : IBaseRepository<TModel, TId>
        where TModel : IdentifiedObject<TId>
    {
        protected readonly IServiceBus ServiceBus;
        protected IMongoCollection<TModel> Db { get; private set; }
        private IBaseReadRepository<TModel, TId> _readRepository { get; set; }

        public BaseRepository(DbContext context, IServiceBus serviceBus, IBaseReadRepository<TModel, TId> readRepository)
        {
            this.ServiceBus = serviceBus;
            this.Db = context.Set<TModel>();
            this._readRepository = readRepository;
        }

        public BaseRepository(DbContext context, IServiceBus serviceBus, IBaseReadRepository<TModel, TId> readRepository, string collection)
        {
            this.ServiceBus = serviceBus;
            this.Db = context.Set<TModel>(collection);
            this._readRepository = readRepository;
        }

        /// <summary>
        /// Insert a model in database.
        /// </summary>
        /// <param name="model">The model to insert in database.</param>
        /// <returns>
        /// Returns <strong>true</strong> if added or <strong>false</strong> if not added.
        /// </returns>
        public virtual async Task<bool> CreateAsync(TModel model)
        {
            try
            {
                await this.Db.InsertOneAsync(model);
                return true;
            }
            catch (Exception e)
            {
                var msg = $"An error occurred on add model.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, model, e));
            }

            return false;
        }

        /// <summary>
        /// Insert a model collection in database.
        /// </summary>
        /// <param name="models">The models to insert in database.</param>
        /// <returns>
        /// Returns a task with process running.
        /// </returns>
        public virtual async Task CreateAsync(IEnumerable<TModel> models)
        {
            try
            {
                await this.Db.InsertManyAsync(models);
            }
            catch (Exception e)
            {
                var msg = $"An error occurred on add model.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, models, e));
            }
        }

        /// <summary>
        /// Insert a model collection in database.
        /// </summary>
        /// <param name="models">The models to insert in database.</param>
        /// <returns>
        /// Returns a task with process running.
        /// </returns>
        public virtual Task CreateAsync(params TModel[] models)
            => this.CreateAsync(models.AsEnumerable());

        /// <summary>
        /// Update a model in database.
        /// </summary>
        /// <param name="model">The model to update in database.</param>
        /// <returns>
        /// Returns <strong>true</strong> if updated or <strong>false</strong> if not updated.
        /// </returns>
        public virtual async Task<bool> UpdateAsync(TModel model)
        {
            try
            {
                var result = await this.Db.ReplaceOneAsync(
                    Builders<TModel>.Filter.Eq(m => m.Id, model.Id),
                    model,
                    new ReplaceOptions
                    {
                        BypassDocumentValidation = true,
                        IsUpsert = false
                    }
                );

                return result.ModifiedCount > 0;
            }
            catch (Exception e)
            {
                var msg = $"An error occurred on update model.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, model, e));
            }

            return false;
        }

        /// <summary>
        /// Update a model collection in database.
        /// </summary>
        /// <param name="models">The models to update in database.</param>
        /// <returns>
        /// Returns a task with process running.
        /// </returns>
        public virtual async Task UpdateAsync(IEnumerable<TModel> models)
        {
            List<Task<ReplaceOneResult>> tasks = new List<Task<ReplaceOneResult>>();
            IEnumerable<ReplaceOneResult> results = null;

            try
            {
                foreach (var model in models)
                {
                    var task = this.Db.ReplaceOneAsync(
                        Builders<TModel>.Filter.Eq(m => m.Id, model.Id),
                        model,
                        new ReplaceOptions
                        {
                            BypassDocumentValidation = true,
                            IsUpsert = false,
                        }
                    );

                    tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray());

                results = tasks.Select(t => t.Result);

                if (results.Any(r => r.ModifiedCount < 1))
                {
                    var msg = $"Some models may not have been updated.";
                    await this.ServiceBus.PublishEvent(new Log(msg));
                }
            }
            catch (Exception e)
            {
                var msg = $"An error occurred on update models.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, models, e));
            }

            #region Disposing

            results = null;
            tasks?.ForEach(t => t.Dispose());
            tasks?.Clear();
            tasks = null;

            #endregion
        }

        /// <summary>
        /// Update a model collection in database.
        /// </summary>
        /// <param name="models">The models to update in database.</param>
        /// <returns>
        /// Returns a task with process running.
        /// </returns>
        public virtual Task UpdateAsync(params TModel[] models)
            => this.UpdateAsync(models.AsEnumerable());

        /// <summary>
        /// Execute basic logic to create or update model based on 
        /// <see cref="IBaseReadRepository{TModel, TKey}.Exists(TKey)"/> results.
        /// Calli <see cref="CreateAsync(TModel)"/> if model not exists or 
        /// <see cref="UpdateAsync(TModel)"/> if model already exists.
        /// </summary>
        /// <param name="model">The model to save.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<bool> SaveAsync(TModel model)
        {
            if (this.Exists(model))
                return this.UpdateAsync(model);
            else
                return this.CreateAsync(model);
        }

        /// <summary>
        /// Execute basic logic to create or update model based on 
        /// <see cref="IBaseReadRepository{TModel, TKey}.Exists(TKey)"/> results.
        /// Calli <see cref="CreateAsync(TModel)"/> if model not exists or 
        /// <see cref="UpdateAsync(TModel)"/> if model already exists.
        /// </summary>
        /// <param name="models">The models to save.</param>
        /// <returns>
        /// Returns a task with saving action running.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual async Task SaveRangeAsync(IEnumerable<TModel> models)
        {
            var idProjection = Builders<TModel>.Projection.Expression(m => m.Id);
            var query = Builders<TModel>.Filter.In(m => m.Id, models.Select(m => m.Id));

            var existingModelIds = await this.Db.Find(query)
                .Project(idProjection)
                .ToListAsync();

            var modelsToUpdate = models.Where(m => existingModelIds.Contains(m.Id));
            var modelsToInsert = models.Except(modelsToUpdate);

            var insertTask = this.CreateAsync(modelsToInsert);
            var updateTask = this.UpdateAsync(modelsToUpdate);

            Task.WaitAll(insertTask, updateTask);

            #region Disposing

            idProjection = null;
            query = null;
            existingModelIds?.Clear();

            modelsToInsert = null;
            modelsToUpdate = null;

            insertTask?.Dispose();
            insertTask = null;

            updateTask?.Dispose();
            updateTask = null;

            #endregion
        }

        /// <summary>
        /// Execute basic logic to create or update model based on 
        /// <see cref="IBaseReadRepository{TModel, TKey}.Exists(TKey)"/> results.
        /// Calli <see cref="CreateAsync(TModel)"/> if model not exists or 
        /// <see cref="UpdateAsync(TModel)"/> if model already exists.
        /// </summary>
        /// <param name="models">The models to save.</param>
        /// <returns>
        /// Returns a task with saving action running.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task SaveRangeAsync(params TModel[] models)
            => this.SaveRangeAsync(models.AsEnumerable());

        /// <summary>
        /// Remove a model from database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="id">The model id to find and remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual async Task<bool> RemoveAsync(TId id)
        {
            var result = await this.Db.DeleteManyAsync(Builders<TModel>.Filter.Eq(m => m.Id, id));

            return result.DeletedCount > 0;
        }

        /// <summary>
        /// Remove models from database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="ids">The model IDs to find and remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task RemoveAsync(params TId[] ids)
            => this.RemoveAsync(ids.AsEnumerable());

        /// <summary>
        /// Remove models from database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="ids">The model IDs to find and remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual async Task RemoveAsync(IEnumerable<TId> ids)
        {
            try
            {
                var result = await this.Db.DeleteManyAsync(Builders<TModel>.Filter.In(m => m.Id, ids));

                if (result.DeletedCount == ids.Count())
                    return;

                var msg = $"Some models may not have been removed.";
                await this.ServiceBus.PublishEvent(new Log(msg, ids));
            }
            catch (Exception e)
            {
                var msg = $"An error occurred on remove models of type {typeof(TModel).FullName} by collection id.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, ids, e));
            }
        }

        /// <summary>
        /// Remove a model from database.
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
                var result = await this.Db.DeleteOneAsync(Builders<TModel>.Filter.Eq(m => m.Id, model.Id));

                return result.DeletedCount > 0;
            }
            catch(Exception e)
            {
                var msg = $"An error occurred on remove model.";
                await this.ServiceBus.PublishEvent(new SystemError(msg, model, e));
            }

            return false;
        }

        /// <summary>
        /// Remove models from database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="models">The models to remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task RemoveAsync(params TModel[] models)
            => this.RemoveAsync(models.Select(m => m.Id));

        /// <summary>
        /// Save all changes in database.
        /// </summary>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual bool Commit() => true;

        /// <summary>
        /// Asynchronously save all changes in database.
        /// </summary>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<bool> CommitAsync() => Task.FromResult(true);

        /// <summary>
        /// Verify if a model exists by identification key.
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Returns <strong>True</strong> if model exists or  <strong>False</strong> if does not exist.
        /// </returns>
        public virtual bool Exists(TId id)
            => this._readRepository.Exists(id);

        /// <summary>
        /// Verify if a model exists by main identification properties. The default is the key.
        /// </summary>
        /// <param name="model">The model to find.</param>
        /// <returns>
        /// Returns <strong>True</strong> if model exists or <strong>False</strong> if does not exist.
        /// </returns>
        public virtual bool Exists(TModel model)
            => this._readRepository.Exists(model);

        /// <summary>
        /// Verify if any model of repository satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <returns>
        /// Returns <strong>True</strong> if any model pass the test in the specified predicate or 
        /// <strong>False</strong> if does not pass.
        /// </returns>
        public virtual bool Exists(Expression<Func<TModel, bool>> predicate)
            => this._readRepository.Exists(predicate);

        /// <summary>
        /// Find a model by <see cref="TId"/>.
        /// Use var result = await SearchByIdAsync(id);
        /// </summary>
        /// <param name="id">The identification key of model.</param>
        /// <returns>
        /// Returns a model if found. Null will be returned if not found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<TModel> FindByIdAsync(TId id)
            => this._readRepository.FindByIdAsync(id);

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
        public virtual Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate)
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
        public virtual Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, BasePaginationFilter filter)
            => this._readRepository.FindAsync(predicate, filter);

        /// <summary>
        /// Filters a sequence of models based on a predicate. Use <paramref name="filter"/> to configure a parameterized search.
        /// </summary>
        /// <param name="predicate">A function to test each model for a condition.</param>
        /// <param name="filter">The filter and options of the search.</param>
        /// <returns>
        /// Returns a model collection that match with predicate. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, Action<BasePaginationFilter> filter)
            => this._readRepository.FindAsync(predicate, filter);

        /// <summary>
        /// Create a LINQ queryable of model.
        /// </summary>
        /// <returns>
        /// Returns a queryable of model.
        /// </returns>
        public virtual IQueryable<TModel> Query()
            => this._readRepository.Query();

        /// <summary>
        /// Count total existing models in database.
        /// </summary>
        /// <returns>Returns quantity of models in database or 0 if nothing exists.</returns>
        public virtual Task<long> CountAsync()
            => this._readRepository.CountAsync();

        /// <summary>
        /// Count total existing models in database based on predicate.
        /// </summary>
        /// <returns>
        /// Returns quantity of models in database matching with predicate or 0 if nothing match.
        /// </returns>
        public virtual Task<long> CountAsync(Expression<Func<TModel, bool>> predicate)
            => this._readRepository.CountAsync(predicate);

        /// <summary>
        /// Load all entities. Is not recommended for many data.
        /// Use <see cref="GetAllAsync(Action{BasePaginationFilter})"/> or <seealso cref="GetAllAsync(BasePaginationFilter)"/> to 
        /// filter and load on demand.
        /// </summary>
        /// <returns>
        /// Returns collection with all models. Empty list will be returned if not exists entities.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        public virtual Task<IEnumerable<TModel>> GetAllAsync()
            => this._readRepository.GetAllAsync();

        /// <summary>
        /// Load all models. Use <see cref="BasePaginationFilter.PageNumber"/> and <seealso cref="BasePaginationFilter.PageSize"/> 
        /// to limit return length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<IEnumerable<TModel>> GetAllAsync(BasePaginationFilter filter)
            => this._readRepository.GetAllAsync(filter);

        /// <summary>
        /// Load all models. Use <see cref="BasePaginationFilter.PageNumber"/> and <seealso cref="BasePaginationFilter.PageSize"/>
        /// to limit return length.
        /// </summary>
        /// <param name="filter">The pagination parameters</param>
        /// <returns>
        /// Returns a model collection. An empty list will be returned if nothing found.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        public virtual Task<IEnumerable<TModel>> GetAllAsync(Action<BasePaginationFilter> filter)
            => this._readRepository.GetAllAsync(filter);

        public virtual void Dispose()
        {

        }
    }
}