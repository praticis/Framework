
using System;

namespace Praticis.Framework.Layers.Domain.Abstractions
{
    public abstract class BaseModel : IModel
    {
        #region Custom Getters and Setters

        /// <summary>
        /// Obtains Identification Key of Entity.
        /// </summary>
        public Guid Id { get; protected set; }

        #endregion

        public BaseModel()
        {
            this.Id = Guid.NewGuid();
        }

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

        public static bool operator == (BaseModel a, BaseModel b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            
            return a.Equals(b);
        }

        public static bool operator != (BaseModel a, BaseModel b)
        { return !(a == b); }

        public override int GetHashCode()
            => (this.GetType().GetHashCode() ^ 93) + Id.GetHashCode();

        #endregion

        public override string ToString()
        { return this.GetType().Name + " [Id = " + this.Id + "]"; }
    }
}