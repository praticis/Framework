
using System.Diagnostics.CodeAnalysis;

using Praticis.Framework.Server.Data.MongoDB.Abstractions.Builders;
using Praticis.Framework.Layers.Domain.Abstractions.Objects;

namespace Praticis.Framework.Server.Data.MongoDB.Abstractions.Mappings
{
    [ExcludeFromCodeCoverage]
    public class ValidatableObjectMap : IEntityTypeConfiguration<ValidatableObject>
    {
        public void Configure(EntityTypeBuilder<ValidatableObject> builder)
        {
            builder.AutoMap();

            builder.UnmapMember(vo => vo.IsValid);

            builder.UnmapMember(vo => vo.Notifications);
        }
    }
}