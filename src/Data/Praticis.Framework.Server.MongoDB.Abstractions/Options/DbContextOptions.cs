
namespace Praticis.Framework.Server.Data.MongoDB.Abstractions.Options
{
    /// <summary>
    /// Represents a MongoDB settings
    /// </summary>
    public class DbContextOptions
    {
        /// <summary>
        /// The MongoDB connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The MongoDB database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Define the MongoDB database connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public void UseConnectionString(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Define the MongoDB database name.
        /// </summary>
        /// <param name="database">The name of database.</param>
        public void UseDatabase(string database)
        { this.Database = database; }
    }
}