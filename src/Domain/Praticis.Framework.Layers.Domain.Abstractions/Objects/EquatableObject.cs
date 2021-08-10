
namespace Praticis.Framework.Layers.Domain.Abstractions.Objects
{
    /// <summary>
    /// An base object with basic equality features.
    /// </summary>
    public abstract class EquatableObject
    {
        #region Comparer Overrides

        /// <summary>
        /// Verify by reference whether the specified object is equals to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>
        /// Returns <strong>true</strong> if the specified object is equals to the current object or
        /// <strong>false</strong> is do not equal.
        /// </returns>
        public override bool Equals(object obj)
        {
            var compareTo = obj as EquatableObject;

            if (ReferenceEquals(null, compareTo)) return false;

            return ReferenceEquals(this, compareTo);
        }

        public static bool operator ==(EquatableObject a, EquatableObject b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EquatableObject a, EquatableObject b)
        { return !(a == b); }

        public override int GetHashCode() => this.GetType().GetHashCode();

        #endregion
    }
}