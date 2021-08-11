
using System.Diagnostics.CodeAnalysis;

using MongoDB.Driver;

using Praticis.Framework.Server.Data.MongoDB.Abstractions.Builders;
using Praticis.Framework.Server.Data.MongoDB.Abstractions.Mappings;
using Praticis.Framework.Server.Data.MongoDB.Abstractions.Options;

namespace Praticis.Framework.Server.Data.MongoDB.Abstractions
{
    [ExcludeFromCodeCoverage]
    public class DbContext
    {
        private DbContextOptions _options { get; set; }
        private MongoClient _mongoClient { get; set; }
        protected IMongoDatabase Database { get; set; }
        private ModelBuilder _builder { get; set; }
        protected bool Initialized { get; set; }

        protected DbContext()
        { }

        public DbContext(DbContextOptions options)
        {
            this._options = options;
            this.Initialized = false;
            this._builder = new ModelBuilder();
        }

        protected virtual void Initialize()
        {
            this.OnModelCreating(this._builder);

            // need be the last to call because MongoDB static configurations
            this.Connect();
            this.Initialized = true;
        }

        protected virtual void Connect()
        {
            this._mongoClient = new MongoClient(this._options.ConnectionString);
            this.Database = this._mongoClient.GetDatabase(this._options.Database);
        }

        public virtual IMongoCollection<TModel> Set<TModel>(string collection)
            where TModel : class
        {
            if (!this.Initialized)
                this.Initialize();

            var collectionName = string.IsNullOrEmpty(collection) ? typeof(TModel).Name : collection;

            return this.Database.GetCollection<TModel>(collectionName);
        }

        public virtual IMongoCollection<TModel> Set<TModel>() where TModel : class
        {
            if (!this.Initialized)
                this.Initialize();

            return this.Database.GetCollection<TModel>(typeof(TModel).Name);
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention
        /// from the entity types exposed in <see cref="Set{TModel}"/> or <seealso cref="Set{TModel}(string)"/>
        /// on your derived context. The resulting model may be cached and re-used for subsequent
        /// instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context. Allow you to configure aspects of the 
        /// model that are specific to a given database.
        /// </param>
        public virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEntityMapping(new IdentifiedObjectMap());
            modelBuilder.ApplyObjectMapping(new ValidatableObjectMap());
        }
    }
}