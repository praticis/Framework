
using System.Collections.Generic;

using FluentValidation.Results;

namespace Praticis.Framework.Layers.Domain.Abstractions
{
    public interface IValidatable
    {
        /// <summary>
        /// The validation notification messages.
        /// </summary>
        IEnumerable<ValidationFailure> Notifications { get; }

        /// <summary>
        /// Call <see cref="Validate"/> to obtains information 
        /// about validation status and returns <strong>true</strong> if is valid or 
        /// <strong>false</strong> if is not valid.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Execute the validate process.
        /// </summary>
        /// <returns>
        /// Returns <strong>true</strong> if is valid or
        /// <strong>false</strong> if is not valid.
        /// </returns>
        bool Validate();
    }
}