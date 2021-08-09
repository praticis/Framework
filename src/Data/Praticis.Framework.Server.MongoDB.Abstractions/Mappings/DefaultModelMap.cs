
using System;
using System.Diagnostics.CodeAnalysis;

using Praticis.Framework.Server.Data.MongoDB.Abstractions.Builders;
using Praticis.Framework.Layers.Domain.Abstractions;
using Praticis.Framework.Layers.Domain.Abstractions.Objects;

namespace Praticis.Framework.Server.Data.MongoDB.Abstractions.Mappings
{
    [ExcludeFromCodeCoverage]
    public class DefaultModelMap<TModel> : DefaultModelMap<TModel, Guid>
        where TModel : IdentifiedObject<Guid>
    {
        public override void Configure(EntityTypeBuilder<TModel> builder)
            => base.Configure(builder);

    }

    [ExcludeFromCodeCoverage]
    public class DefaultModelMap<TModel, TKey> : IEntityTypeConfiguration<TModel>
        where TModel : IdentifiedObject<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TModel> builder)
        {
            builder.AutoMap();

            builder.SetDiscriminator(typeof(TModel).Name);
        }
    }
}