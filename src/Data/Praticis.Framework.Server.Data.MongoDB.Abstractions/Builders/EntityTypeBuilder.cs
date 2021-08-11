
using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson.Serialization;

namespace Praticis.Framework.Server.Data.MongoDB.Abstractions.Builders
{
    [ExcludeFromCodeCoverage]
    public class EntityTypeBuilder<TModel> : BsonClassMap<TModel>
        where TModel : class
    {

    }
}