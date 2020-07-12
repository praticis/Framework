
namespace Praticis.Framework.Layers.Data.Abstractions
{
    /// <summary>
    /// Initialize database seeding data. Implements it in your DbContext.
    /// Using Praticis Framework Configure Database Module, 
    /// scan all DbContext in your assemblies and call this method to seed data.
    /// </summary>
    public interface IDatabaseInitializer
    {
        void InitializeContext();
    }
}