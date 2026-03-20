using System.Numerics;

namespace FlaxEngine;

public interface ITrigonometricVector<TSelf, TComponent> : IVector<TSelf, TComponent>
    where TSelf : unmanaged, ITrigonometricVector<TSelf, TComponent>
    where TComponent : INumberBase<TComponent>
{
    /// <summary>Computes the cross product of two vectors.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The cross product.</returns>
    static abstract TSelf Cross(in TSelf left, in TSelf right);
}
