
using System;

using MediatR;

namespace Praticis.Framework.Bus.Abstractions.Commands
{
    /// <summary>
    /// The command execution implementation abstraction.
    /// Defines the command that receives as input and the output type.
    /// </summary>
    /// <typeparam name="TCommand">The command that will carry the informations.</typeparam>
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, bool>, IDisposable
        where TCommand : ICommand
    {

    }

    /// <summary>
    /// The command execution implementation abstraction.
    /// </summary>
    /// <typeparam name="TCommand">The command that will carry the informations.</typeparam>
    /// <typeparam name="TResponse">The result type of command execution.</typeparam>
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>, IDisposable
        where TCommand : ICommand<TResponse>
    {

    }
}