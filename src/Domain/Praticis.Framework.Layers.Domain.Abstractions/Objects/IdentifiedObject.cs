
namespace Praticis.Framework.Layers.Domain.Abstractions.Objects
{
    /// <summary>
    /// An identified object with improvements in equality comparer based on <see cref="Id"/> 
    /// in equality operators.
    /// </summary>
    /// <typeparam name="TId">The identification key of object.</typeparam>
    public abstract class IdentifiedObject<TId> : EquatableObject, IIdentity<TId>
    {
        /// <summary>
        /// The identification key of entity.
        /// </summary>
        public TId Id { get; protected set; }

        protected IdentifiedObject()
        { }

        protected IdentifiedObject(TId id)
        { this.Id = id; }


        #region Comparer Overrides

        /// <summary>
        /// Verify by reference or <see cref="Id"/> whether the specified object is equals to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>
        /// Returns <strong>true</strong> if the specified object is equals to the current object or
        /// <strong>false</strong> is do not equal.
        /// </returns>
        public override bool Equals(object obj)
        {
            var compareTo = obj as IdentifiedObject<TId>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public override int GetHashCode()
            => (this.GetType().GetHashCode() ^ 93) + Id.GetHashCode();

        #endregion

        public override string ToString()
        { return this.GetType().Name + " [Id = " + this.Id + "]"; }
    }
}