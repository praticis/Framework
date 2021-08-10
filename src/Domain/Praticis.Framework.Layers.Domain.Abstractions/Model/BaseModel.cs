using System;

using Praticis.Framework.Layers.Domain.Abstractions.Objects;

namespace Praticis.Framework.Layers.Domain.Abstractions
{
    /// <summary>
    /// An base model with equality and identified features.
    /// </summary>
    public abstract class BaseModel : BaseModel<Guid>
    {
        public BaseModel(bool generateId = true)
        {
            if (generateId)
                this.Id = Guid.NewGuid();
        }
    }

    /// <summary>
    /// An base model with equality and identified features.
    /// </summary>
    public abstract class BaseModel<TId> : IdentifiedObject<TId>
    {
        public BaseModel()
        { }

        public BaseModel(TId id)
            => base.Id = id;

        #region Comparer Overrides

        /// <summary>
        /// Verify by reference or ID whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>
        /// Returns <strong>true</strong> if the specified object is equal to the current object or
        /// <strong>false</strong> is do not equal.
        /// </returns>
        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseModel;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(BaseModel<TId> a, BaseModel<TId> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            
            return a.Equals(b);
        }

        public static bool operator != (BaseModel<TId> a, BaseModel<TId> b)
        { return !(a == b); }

        public override int GetHashCode()
            => (this.GetType().GetHashCode() ^ 93) + Id.GetHashCode();

        #endregion

        public override string ToString()
        { return this.GetType().Name + " [Id = " + this.Id + "]"; }
    }
}