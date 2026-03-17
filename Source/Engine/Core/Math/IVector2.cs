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
    static virtual TSelf Half { get; } = TSelf.Create(TComponent.CreateTruncating(0.5));

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

    /// <returns>
    /// <see langword="true"/> if the absolute difference between <paramref name="left"/> 
    /// and <paramref name="right"/> is less than <c><typeparamref name="TComponent"/>.Epsilon</c>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <inheritdoc cref="NearEqual(in TSelf, in TSelf, TComponent)"/>
    static virtual bool NearEqual(in TSelf left, in TSelf right) => TSelf.NearEqual(in left, in right, TComponent.Epsilon);

    /// <returns>
    /// The result of clamping <paramref name="vector"/> to the inclusive range of 
    /// <c><typeparamref name="TComponent"/>.Zero</c> and <paramref name="max"/>.
    /// </returns>
    /// <inheritdoc cref="ClampLength(in TSelf, TComponent, TComponent, out TSelf)"/>
    static virtual TSelf ClampLength(in TSelf vector, TComponent max) => TSelf.ClampLength(in vector, TComponent.Zero, max);

    /// <returns>
    /// The result of clamping <paramref name="vector"/> to the inclusive range 
    /// of <paramref name="min"/> and <paramref name="max"/>.
    /// </returns>
    /// <inheritdoc cref="ClampLength(in TSelf, TComponent)"/>
    static abstract TSelf ClampLength(in TSelf vector, TComponent min, TComponent max);

    /// <summary>
    /// Clamps the length of the specified vector to an inclusive minimum and maximum value.
    /// </summary>
    /// <param name="vector">The vector to clamp.</param>
    /// <param name="min">The inclusive minimum to which <paramref name="vector"/> should clamp.</param>
    /// <param name="max">The inclusive maximum to which <paramref name="vector"/> should clamp.</param>
    /// <param name="result">
    /// When this method returns, contains the result of clamping <paramref name="vector"/> to 
    /// the inclusive range of <paramref name="min"/> and <paramref name="max"/>.
    /// </param>
    static virtual void ClampLength(in TSelf vector, TComponent min, TComponent max, out TSelf result) => result = TSelf.ClampLength(in vector, min, max);

    /// <summary>
    /// Performs a linear interpolation between two values based on the given weight.
    /// </summary>
    /// <remarks>
    /// Passing <paramref name="amount" /> a value of 0 will cause <paramref name="start" /> to be returned; 
    /// a value of 1 will cause <paramref name="end" /> to be returned.
    /// </remarks>
    /// <param name="start">The first value, which is intended to be the lower bound.</param>
    /// <param name="end">The second value, which is intended to be the upper bound.</param>
    /// <param name="amount">A value, intended to be between 0 and 1, that indicates the weight of the interpolation.</param>
    /// <param name="result">When the method completes, contains the interpolated value.</param>
    static virtual void Lerp(in TSelf start, in TSelf end, TComponent amount, out TSelf result) => result = TSelf.Lerp(in start, in end, amount);

    /// <returns>The interpolated value.</returns>
    /// <inheritdoc cref="Lerp(in TSelf, in TSelf, TComponent, out TSelf)"/>
    static abstract TSelf Lerp(in TSelf start, in TSelf end, TComponent amount);

    /// <summary>
    /// Performs a cubic interpolation between two values based on the given weight.
    /// </summary>
    /// <remarks>
    /// Passing <paramref name="amount" /> a value of 0 will cause <paramref name="start" /> to be returned; 
    /// a value of 1 will cause <paramref name="end" /> to be returned.
    /// </remarks>
    /// <param name="start">The first value, which is intended to be the lower bound.</param>
    /// <param name="end">The second value, which is intended to be the upper bound.</param>
    /// <param name="amount">A value, intended to be between 0 and 1, that indicates the weight of the interpolation.</param>
    /// <param name="result">When the method completes, contains the interpolated value.</param>
    static virtual void SmoothStep(in TSelf start, in TSelf end, TComponent amount, out TSelf result) => result = TSelf.SmoothStep(in start, in end, amount);

    /// <returns>The interpolated value.</returns>
    /// <inheritdoc cref="SmoothStep(in TSelf, in TSelf, TComponent, out TSelf)"/>
    static abstract TSelf SmoothStep(in TSelf start, in TSelf end, TComponent amount);
}
