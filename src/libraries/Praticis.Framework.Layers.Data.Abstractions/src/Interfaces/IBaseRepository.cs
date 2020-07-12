
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Praticis.Framework.Layers.Domain.Abstractions;

namespace Praticis.Framework.Layers.Data.Abstractions
{
    public interface IBaseRepository<TEntity> : IBaseReadRepository<TEntity>, IDisposable
        where TEntity : class, IModel
    {
        /// <summary>
        /// Save a model. If not exists it is created, if exists is updated.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// </summary>
        /// <param name="model">The model to save.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<bool> SaveAsync(TEntity model);

        /// <summary>
        /// The models that do not exists will be created and existing entities will be updated.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="models">The models to save.</param>
        /// <returns>
        /// Returns a task with saving action running.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task SaveRangeAsync(IEnumerable<TEntity> models);

        /// <summary>
        /// The models that do not exists will be created and existing entities will be updated.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="models">The models to save.</param>
        /// <returns>
        /// Returns a task with saving action running.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task SaveRangeAsync(params TEntity[] models);

        /// <summary>
        /// Remove a model from database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="id">The model id to find and remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<bool> RemoveAsync(Guid id);

        /// <summary>
        /// Remove models from database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="ids">The model IDs find and remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task RemoveAsync(params Guid[] ids);

        /// <summary>
        /// Remove a model from database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="model">The model to remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<bool> RemoveAsync(TEntity model);

        /// <summary>
        /// Remove models from database.
        /// Use <see cref="Commit"/> or <seealso cref="CommitAsync"/> to confirm changes in database.
        /// Use NotificationStore on ServiceBus to verify if has notification errors.
        /// </summary>
        /// <param name="models">The models to remove from database.</param>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task RemoveAsync(params TEntity[] models);

        /// <summary>
        /// Save all changes in database.
        /// </summary>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        bool Commit();

        /// <summary>
        /// Asynchronously save all changes in database.
        /// </summary>
        /// <returns>
        /// Returns <strong>True</strong> when sucess or <strong>False</strong> when there are errors.
        /// See errors and notifications in service bus to verify if there was any problem.
        /// </returns>
        Task<bool> CommitAsync();
    }
}