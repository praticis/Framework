
namespace Praticis.Framework.Server.Data.Abstractions
{
    /// <summary>
    /// Initialize database seeding data. Implements it in your DbContext.
    /// Configure Database Module, scan all DbContext in your assemblies and call this method to seed data.
    /// </summary>
    public interface IDatabaseInitializer
    {
        /// <summary>
        /// Seed data in database.
        /// </summary>
        void InitializeContext();
    }
}