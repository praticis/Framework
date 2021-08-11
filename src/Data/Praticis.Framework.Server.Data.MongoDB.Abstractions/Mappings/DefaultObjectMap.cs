
using System.Diagnostics.CodeAnalysis;

using Praticis.Framework.Server.Data.MongoDB.Abstractions.Builders;

namespace Praticis.Framework.Server.Data.MongoDB.Abstractions.Mappings
{
    [ExcludeFromCodeCoverage]
    public class DefaultObjectMap<TObject> : IEntityTypeConfiguration<TObject>
        where TObject : class
    {
        public virtual void Configure(EntityTypeBuilder<TObject> builder)
        {
            builder.AutoMap();

            builder.SetDiscriminator(typeof(TObject).Name);
        }
    }
}