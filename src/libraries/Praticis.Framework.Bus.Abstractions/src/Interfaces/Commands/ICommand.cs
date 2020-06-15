
using System;
using System.Collections.Generic;

using FluentValidation.Results;
using MediatR;

namespace Praticis.Framework.Bus.Abstractions.Commands
{
    /// <summary>
    /// An execution order abstraction that together the necessary information to be executed.
    /// </summary>
    public interface ICommand : ICommand<bool>, IWork
    {

    }

    /// <summary>
    /// An execution order abstraction that together the necessary information to be executed.
    /// </summary>
    /// <typeparam name="TResponse">The result of command execution.</typeparam>
    public interface ICommand<TResponse> : IRequest<TResponse>, IWork
    {
        /// <summary>
        /// The command Id.
        /// </summary>
        Guid CommandId { get; }

        /// <summary>
        /// The command name.
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// The creation command time.
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// The assembly context of the inherited command.
        /// </summary>
        Type ResourceType { get; }

        /// <summary>
        /// Verify if the work is valid. 
        /// Override <see cref="Validate"/> to implements validation.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Execute command validation.
        /// </summary>
        /// <returns>
        /// Returns a validation failure collection if has validation messages or
        /// an empty list if not has messages.
        /// </returns>
        IEnumerable<ValidationFailure> Validate();
    }
}