using System.Numerics;

namespace FlaxEngine;

/// <summary>
/// Defines a vector type that supports measurement operations such as calculating length, average value, and distance
/// between vectors.
/// </summary>
/// <remarks>This interface provides both fast approximate and precise methods for measuring vector properties.
/// Use the precise methods when accuracy is critical, as the approximate methods may use optimizations that sacrifice
/// precision for performance.</remarks>
/// <typeparam name="TResult">The floating-point type used for measurement results.</typeparam>
/// <inheritdoc cref="IVector{TSelf}"/>
/// <typeparam name="TSelf"/>
public interface IMeasurableVector<TSelf, TResult> where TSelf : unmanaged, IMeasurableVector<TSelf, TResult> where TResult : IFloatingPoint<TResult>
{
    /// <remarks>
    /// <para>On x86/x64 hardware this may use the <c>RSQRTSS</c> instruction which has a maximum relative error of <c>1.5 * 2^-12</c>.</para>
    /// <para>On Arm64 hardware this may use the <c>FRSQRTE</c> instruction which has a maximum relative error of <c>1.5 * 2^-8</c>.</para>
    /// <para>On unsupported hardware this method will fall back to a precise normalization which does not use an estimate and has no error.</para>
    /// </remarks>
    /// <inheritdoc cref="PreciseLength" />
    TResult Length { get; }

    /// <summary>
    /// Gets the length of the vector.
    /// </summary>
    /// <remarks>Only use this property if <see cref="Length"/> does not provide sufficient precision.</remarks>
    TResult PreciseLength { get; }

    /// <summary>
    /// Gets an arithmetic average value of all vector components.
    /// </summary>
    TResult AvgValue { get; }
}
