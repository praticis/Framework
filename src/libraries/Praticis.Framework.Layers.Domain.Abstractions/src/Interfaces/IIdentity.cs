
using System;

namespace Praticis.Framework.Layers.Domain.Abstractions
{
    public interface IIdentity
    {
        /// <summary>
        /// Obtains Identification Key of Entity.
        /// </summary>
        /// <returns>
        /// Return the Identification Key of Entity.
        /// </returns>
        Guid Id { get; }
    }
}