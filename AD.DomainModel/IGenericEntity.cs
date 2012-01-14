using System;

namespace AD.DomainModel
{
    public interface IGenericEntity<TIdentity> : IEquatable<IGenericEntity<TIdentity>>
    {
        TIdentity Id { get; }
    }
}
