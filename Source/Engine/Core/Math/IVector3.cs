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
    /// <param name="amount">Weighting factor.</param>
    /// <param name="result">When the method completes, contains the result of the Catmull-Rom interpolation.</param>
    static virtual void CatmullRom(in TSelf value1, in TSelf value2, in TSelf value3, in TSelf value4, TComponent amount, out TSelf result)
    {
        result = TSelf.CatmullRom(in value1, in value2, in value3, in value4, amount);
    }

    /// <inheritdoc cref="CatmullRom(in TSelf, in TSelf, in TSelf, in TSelf, TComponent, out TSelf)"/>
    /// <returns>A vector that is the result of the Catmull-Rom interpolation.</returns>
    static abstract TSelf CatmullRom(in TSelf value1, in TSelf value2, in TSelf value3, in TSelf value4, TComponent amount);

    /// <summary>
    /// Performs a Hermite spline interpolation.
    /// </summary>
    /// <param name="value1">First source position vector.</param>
    /// <param name="tangent1">First source tangent vector.</param>
    /// <param name="value2">Second source position vector.</param>
    /// <param name="tangent2">Second source tangent vector.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <param name="result">When the method completes, contains the result of the Hermite spline interpolation.</param>
    static virtual void Hermite(in TSelf value1, in TSelf tangent1, in TSelf value2, in TSelf tangent2, TComponent amount, out TSelf result)
    {
        result = TSelf.Hermite(in value1, in tangent1, in value2, in tangent2, amount);
    }

    /// <inheritdoc cref="Hermite(in TSelf, in TSelf, in TSelf, in TSelf, TComponent, out TSelf)"/>
    /// <returns>The result of the Hermite spline interpolation.</returns>
    static abstract TSelf Hermite(in TSelf value1, in TSelf tangent1, in TSelf value2, in TSelf tangent2, TComponent amount);
}
