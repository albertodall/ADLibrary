using System;

namespace AD.DomainModel
{
    [Serializable]
    public abstract class Entity<TIdentity> : IGenericEntity<TIdentity>
    {
        private int? _requestedHashCode;

        public abstract TIdentity Id { get; set; }

        /// <summary>                 
        /// Compare equality through Id                 
        /// </summary>                 
        /// <param name="other">Entity to compare.</param>                 
        /// <returns>true if are equals</returns>                 
        /// <remarks>                 
        /// Two entities are equals if they are of the same hierarchy tree/sub-tree                 
        /// and has same id.                 
        /// </remarks> 
        public virtual bool Equals(IGenericEntity<TIdentity> other)
        {
            if (other == null || !GetType().IsInstanceOfType(other))
            {
                return false;
            } 
            
            if (ReferenceEquals(this, other)) { return true; } 
            var otherIsTransient = Equals(other.Id, default(TIdentity)); 
            var thisIsTransient = IsTransient(); 
            
            if (otherIsTransient && thisIsTransient)
            {
                return ReferenceEquals(other, this);
            } 
            return other.Id.Equals(Id); 
        }

        protected bool IsTransient()
        {
            return Equals(Id, default(TIdentity));
        }

        public override bool Equals(object obj)
        {
            var that = obj as IGenericEntity<TIdentity>; 
            return Equals(that);
        }   
        
        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)
            {
                _requestedHashCode = IsTransient() ? base.GetHashCode() : Id.GetHashCode();
            } 
            return _requestedHashCode.Value;
        }

        public static bool operator == (Entity<TIdentity> left, Entity<TIdentity> right)
        {
            return Equals(left, right);
        }

        public static bool operator != (Entity<TIdentity> left, Entity<TIdentity> right)
        {
            return !Equals(left, right);
        }
    }
}
