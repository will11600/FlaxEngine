using System.Numerics;

namespace FlaxEngine;

public interface IVector3<TSelf, TComponent> : IVector2<TSelf, TComponent>
    where TSelf : unmanaged, IVector3<TSelf, TComponent>
    where TComponent : IFloatingPointIeee754<TComponent>
{
    /// <summary>
    /// Performs a Catmull-Rom interpolation using the specified positions.
    /// </summary>
    /// <param name="value1">The first position in the interpolation.</param>
    /// <param name="value2">The second position in the interpolation.</param>
    /// <param name="value3">The third position in the interpolation.</param>
    /// <param name="value4">The fourth position in the interpolation.</param>
    /// <inheritdoc cref="IVector2{TSelf, TComponent}.Lerp(in TSelf, in TSelf, TComponent)"/>
    /// <param name="amount"/>
    static abstract TSelf CatmullRom(in TSelf value1, in TSelf value2, in TSelf value3, in TSelf value4, TComponent amount);

    /// <summary>
    /// Performs a Hermite spline interpolation using the specified positions and tangents.
    /// </summary>
    /// <param name="value1">First source position vector.</param>
    /// <param name="tangent1">First source tangent vector.</param>
    /// <param name="value2">Second source position vector.</param>
    /// <param name="tangent2">Second source tangent vector.</param>
    /// <inheritdoc cref="IVector2{TSelf, TComponent}.Lerp(in TSelf, in TSelf, TComponent)"/>
    /// <param name="amount"/>
    static abstract TSelf Hermite(in TSelf value1, in TSelf tangent1, in TSelf value2, in TSelf tangent2, TComponent amount);
}
