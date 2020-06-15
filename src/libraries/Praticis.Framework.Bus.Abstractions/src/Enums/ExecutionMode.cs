
using System;

namespace Praticis.Framework.Bus.Abstractions.Enums
{
    /// <summary>
    /// Represents the execution mode of a Command or Event.
    /// </summary>
    [Flags]
    public enum ExecutionMode
    {
        /// <summary>
        /// Is immediately executed when work has sent by service bus.
        /// </summary>
        WaitToClose = 1,

        /// <summary>
        /// Is not immediately executed. Will be enqueued in worker process and run after previous works was executed.
        /// It need of Praticis Worker module configured.
        /// </summary>
        Enqueue = 2,
    }
}