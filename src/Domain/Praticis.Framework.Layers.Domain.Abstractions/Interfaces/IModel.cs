
using System;

namespace Praticis.Framework.Layers.Domain.Abstractions
{
    public interface IModel : IModel<Guid>
    {
        
    }

    public interface IModel<TId> : IIdentity<TId>
    {

    }
}