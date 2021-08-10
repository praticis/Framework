
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Praticis.Framework.Layers.Domain.Abstractions.Objects;

namespace Praticis.Framework.Layers.Data.Abstractions
{
    public interface IBaseRepository<TModel, TId> : IBaseReadRepository<TModel, TId>, IDisposable
        where TModel : IdentifiedObject<TId>
    {
        /// <summary>
        /// Insert a model in database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="model">The model to insert in database.</param>
        /// <returns>
        /// Returns <strong>true</strong> if added or <strong>false</strong> if not added.
        /// </returns>
        Task<bool> CreateAsync(TModel model);

        /// <summary>
        /// Insert a model collection in database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="models">The models to insert in database.</param>
        /// <returns>
        /// Returns a task with process running.
        /// </returns>
        Task CreateAsync(IEnumerable<TModel> models);

        /// <summary>
        /// Insert a model collection in database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="models">The models to insert in database.</param>
        /// <returns>
        /// Returns a task with process running.
        /// </returns>
        Task CreateAsync(params TModel[] models);

        /// <summary>
        /// Update a model in database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="model">The model to update in database.</param>
        /// <returns>
        /// Returns <strong>true</strong> if updated or <strong>false</strong> if not updated.
        /// </returns>
        Task<bool> UpdateAsync(TModel model);

        /// <summary>
        /// Update a model collection in database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="models">The models to update in database.</param>
        /// <returns>
        /// Returns a task with process running.
        /// </returns>
        Task UpdateAsync(IEnumerable<TModel> models);

        /// <summary>
        /// Update a model collection in database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="models">The models to update in database.</param>
        /// <returns>
        /// Returns a task with process running.
        /// </returns>
        Task UpdateAsync(params TModel[] models);

        /// <summary>
        /// Execute basic logic to create or update model based on 
        /// <see cref="IBaseReadRepository{TModel, TId}.Exists(TId)"/> results.
        /// Calli <see cref="CreateAsync(TModel)"/> if model not exists or 
        /// <see cref="UpdateAsync(TModel)"/> if model already exists.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="model">The model to save.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        Task<bool> SaveAsync(TModel model);

        /// <summary>
        /// Execute basic logic to create or update model based on 
        /// <see cref="IBaseReadRepository{TModel, TId}.Exists(TId)"/> results.
        /// Calli <see cref="CreateAsync(TModel)"/> if model not exists or 
        /// <see cref="UpdateAsync(TModel)"/> if model already exists.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="models">The models to save.</param>
        /// <returns>
        /// Returns a task with saving action running.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        Task SaveRangeAsync(IEnumerable<TModel> models);

        /// <summary>
        /// Execute basic logic to create or update model based on 
        /// <see cref="IBaseReadRepository{TModel, TId}.Exists(TId)"/> results.
        /// Calli <see cref="CreateAsync(TModel)"/> if model not exists or 
        /// <see cref="UpdateAsync(TModel)"/> if model already exists.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="models">The models to save.</param>
        /// <returns>
        /// Returns a task with saving action running.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        Task SaveRangeAsync(params TModel[] models);

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
        Task<bool> RemoveAsync(TId id);

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
        Task RemoveAsync(params TId[] ids);

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
        Task RemoveAsync(IEnumerable<TId> ids);

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
        Task<bool> RemoveAsync(TModel model);

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
        Task RemoveAsync(params TModel[] models);

        /// <summary>
        /// Save all changes in database.
        /// </summary>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        bool Commit();

        /// <summary>
        /// Asynchronously save all changes in database.
        /// </summary>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors.
        /// See errors and notifications in service bus notification store to verify if there was any problem.
        /// </returns>
        Task<bool> CommitAsync();
    }
}