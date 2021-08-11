
using Praticis.Framework.Server.Data.MongoDB.Abstractions.Builders;

namespace Praticis.Framework.Server.Data.MongoDB.Abstractions
{
    public interface IEntityTypeConfiguration<TModel>
        where TModel : class
    {
        void Configure(EntityTypeBuilder<TModel> builder);
    }
}