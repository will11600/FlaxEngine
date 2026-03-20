using System.Numerics;

namespace FlaxEngine;

/// <summary>
/// Defines a contract for a four-dimensional vector with floating-point components.
/// </summary>
/// <inheritdoc/>
public interface IVector4<TSelf, TComponent> : IVector3<TSelf, TComponent>
    where TSelf : unmanaged, IVector4<TSelf, TComponent>
    where TComponent : INumberBase<TComponent>
{
    /// <summary>
    /// Gets or sets the W component of the vector.
    /// </summary>
    TComponent W { get; set; }
}
