
using System;

namespace Praticis.Framework.Layers.Domain.Abstractions
{
    public interface IIdentity : IIdentity<Guid>
    {

    }

    public interface IIdentity<TKey>
    {
        /// <summary>
        /// The identification key.
        /// </summary>
        TKey Id { get; }
    }
}