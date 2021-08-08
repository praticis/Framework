
using System;

namespace Praticis.Framework.Bus.Abstractions.Enums
{
    /// <summary>
    /// Represents a work type.
    /// </summary>
    [Flags]
    public enum WorkType
    {
        /// <summary>
        /// Represents a command work.
        /// Command works can be delivered to only one handler.
        /// </summary>
        Command = 1,

        /// <summary>
        /// Repreents a work event.
        /// Event works can be delivered to many handlers,
        /// use it to listening events from others contexts and execute some action.
        /// </summary>
        Event = 2
    }
}