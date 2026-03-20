using System.Numerics;

namespace FlaxEngine;

/// <summary>
/// Defines a contract for a three-dimensional vector with floating-point components.
/// </summary>
/// <inheritdoc/>
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

    /// <summary>
    /// Computes a vector containing the Cartesian coordinates of a point specified in Barycentric coordinates relative to a triangle.
    /// </summary>
    /// <param name="value1">A vector containing the Cartesian coordinates of vertex 1 of the triangle.</param>
    /// <param name="value2">A vector containing the Cartesian coordinates of vertex 2 of the triangle.</param>
    /// <param name="value3">A vector containing the Cartesian coordinates of vertex 3 of the triangle.</param>
    /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2" />).</param>
    /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3" />).</param>
    /// <returns>A new vector containing the Cartesian coordinates of the specified point.</returns>
    static abstract TSelf Barycentric(in TSelf value1, in TSelf value2, in TSelf value3, TComponent amount1, TComponent amount2);

    /// <summary>
    /// Transforms a vector by the given <see cref="Quaternion" /> rotation.
    /// </summary>
    /// <param name="vector">The vector to rotate.</param>
    /// <param name="rotation">The <see cref="Quaternion" /> rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    static abstract TSelf Transform(in TSelf vector, in Quaternion rotation);

    /// <summary>
    /// Transforms a vector by the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <returns>The transformed vector.</returns>
    static abstract TSelf Transform(in TSelf vector, in Matrix transform);
}
