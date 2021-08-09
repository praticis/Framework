
using System;

namespace Praticis.Framework.Layers.Domain.Abstractions.Interfaces
{
    public interface IIdentity<TKey>
    {
        /// <summary>
        /// The identification key of the entity.
        /// </summary>
        TKey Id { get; }
    }
}