
using System;
using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

using Praticis.Framework.Server.Data.MongoDB.Abstractions.Builders;
using Praticis.Framework.Layers.Domain.Abstractions;
using Praticis.Framework.Layers.Domain.Abstractions.Objects;

namespace Praticis.Framework.Server.Data.MongoDB.Abstractions.Mappings
{
    [ExcludeFromCodeCoverage]
    public class IdentifiedObjectMap : IEntityTypeConfiguration<IdentifiedObject<Guid>>
    {
        public virtual void Configure(EntityTypeBuilder<IdentifiedObject<Guid>> builder)
        {
            builder.AutoMap();

            builder.MapProperty(io => io.Id)
                .SetSerializer(new GuidSerializer(GuidRepresentation.Standard))
                .SetIdGenerator(CombGuidGenerator.Instance);
        }
    }

    [ExcludeFromCodeCoverage]
    public class IdentifiedObjectMap<TKey> : IEntityTypeConfiguration<IdentifiedObject<TKey>>
    {
        public virtual void Configure(EntityTypeBuilder<IdentifiedObject<TKey>> builder)
        {
            builder.AutoMap();
        }
    }
}