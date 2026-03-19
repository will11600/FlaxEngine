using System.Numerics;

namespace FlaxEngine;

/// <summary>
/// Defines an interface for two-dimensional vectors with floating-point components, providing methods for vector
/// comparison, clamping, and interpolation operations.
/// </summary>
/// <inheritdoc/>
public interface IVector2<TSelf, TComponent> : IVector<TSelf, TComponent>
    where TSelf : unmanaged, IVector2<TSelf, TComponent>
    where TComponent : IFloatingPointIeee754<TComponent>
{
    /// <summary>
    /// Gets a <typeparamref name="TSelf"/> with all of its components set to half.
    /// </summary>
    static abstract TSelf Half { get; }

    /// <remarks>
    /// <para>Uses a fast approximation for the inverse square root, so the result may not be precise. 
    /// For a more accurate result, use <see cref="PreciseDistance(in TSelf, in TSelf)" />.</para>
    /// <para>Consider using <see cref="DistanceSquared(in TSelf, in TSelf)"/> when only relative 
    /// distance is required.</para>
    /// </remarks>
    /// <inheritdoc cref="PreciseDistance(in TSelf, in TSelf)"/>
    static abstract TComponent Distance(in TSelf value1, in TSelf value2);

    /// <summary>
    /// Calculates the distance between two vectors.
    /// </summary>
    /// <remarks>
    /// Only use this method if <see cref="Distance(in TSelf, in TSelf)"/> does not provide sufficient precision.
    /// </remarks>
    /// <returns>The distance between the two vectors.</returns>
    /// <inheritdoc cref="DistanceSquared(in TSelf, in TSelf)"/>
    static abstract TComponent PreciseDistance(in TSelf value1, in TSelf value2);

    /// <summary>
    /// Calculates the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The squared distance between the two vectors.</returns>
    static abstract TComponent DistanceSquared(in TSelf value1, in TSelf value2);

    /// <summary>
    /// Compares two values to determine approximate equality.
    /// </summary>
    /// <param name="left">The value to compare with <paramref name="right"/>.</param>
    /// <param name="right">The value to compare with <paramref name="left"/>.</param>
    /// <param name="tolerance">
    /// The maximum allowed difference between <paramref name="left"/> and 
    /// <paramref name="right"/> for them to be considered equal.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the absolute difference between <paramref name="left"/> 
    /// and <paramref name="right"/> is less than <paramref name="tolerance"/>; otherwise, <see langword="false"/>.
    /// </returns>
    static abstract bool NearEqual(in TSelf left, in TSelf right, TComponent tolerance);

    /// <summary>
    /// Clamps the length of the specified vector to an inclusive minimum and maximum value.
    /// </summary>
    /// <param name="vector">The vector to clamp.</param>
    /// <param name="min">The inclusive minimum to which <paramref name="vector"/> should clamp.</param>
    /// <param name="max">The inclusive maximum to which <paramref name="vector"/> should clamp.</param>
    /// <returns>
    /// The result of clamping <paramref name="vector"/> to the inclusive range 
    /// of <paramref name="min"/> and <paramref name="max"/>.
    /// </returns>
    static abstract TSelf ClampLength(in TSelf vector, TComponent min, TComponent max);

    /// <summary>
    /// Performs a linear interpolation between two values based on the given weight.
    /// </summary>
    /// <param name="start">The first value, which is intended to be the lower bound.</param>
    /// <param name="end">The second value, which is intended to be the upper bound.</param>
    /// <param name="amount">A value, intended to be between 0 and 1, that indicates the weight of the interpolation.</param>
    /// <returns>The interpolated value.</returns>
    static abstract TSelf Lerp(in TSelf start, in TSelf end, TComponent amount);

    /// <summary>
    /// Performs a cubic interpolation between two values based on the given weight.
    /// </summary>
    /// <returns>The interpolated value.</returns>
    /// <inheritdoc cref="Lerp(in TSelf, in TSelf, TComponent)"/>
    static abstract TSelf SmoothStep(in TSelf start, in TSelf end, TComponent amount);
}
