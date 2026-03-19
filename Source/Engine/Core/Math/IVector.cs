using System;
using System.Numerics;

namespace FlaxEngine;

/// <summary>
/// Defines a vector type that support common vector operations.
/// </summary>
/// <typeparam name="TSelf">The type that implements the vector interface.</typeparam>
public interface IVector<TSelf> : IEquatable<TSelf>, IFormattable where TSelf : unmanaged, IVector<TSelf>
{
    /// <summary>
    /// Gets the number of elements stored in the vector.
    /// </summary>
    static abstract int Count { get; }

    /// <summary>
    /// Gets a value indicting whether this vector is zero
    /// </summary>
    bool IsZero { get; }

    /// <summary>
    /// Gets a value indicting whether this vector is one
    /// </summary>
    bool IsOne { get; }

    /// <inheritdoc cref="IEquatable{TSelf}.Equals(TSelf)" />
    bool Equals(in TSelf other);
    bool IEquatable<TSelf>.Equals(TSelf other) => Equals(in other);

    /// <summary>
    /// Adds two values together to compute their sum.
    /// </summary>
    /// <param name="left">The value to which <paramref name="right"/> is added.</param>
    /// <param name="right">The value that is added to <paramref name="left"/>.</param>
    /// <returns>The sum of sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
    static abstract TSelf Add(in TSelf left, in TSelf right);

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    /// <param name="left">The first vector to subtract.</param>
    /// <param name="right">The second vector to subtract.</param>
    /// <returns>The difference of the two vectors.</returns>
    static abstract TSelf Subtract(in TSelf left, in TSelf right);

    /// <summary>
    /// Multiplies two values together to compute their product.
    /// </summary>
    /// <param name="left">The value that <paramref name="right"/> multiplies.</param>
    /// <param name="right">The value that multiplies <paramref name="left"/>.</param>
    /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
    static abstract TSelf Multiply(in TSelf left, in TSelf right);

    /// <summary>
    /// Divides one value by another to compute their quotient.
    /// </summary>
    /// <param name="left">The value that <paramref name="right"/> divides.</param>
    /// <param name="right">The value that divides <paramref name="left"/>.</param>
    /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
    static abstract TSelf Divide(in TSelf left, in TSelf right);

    /// <summary>
    /// Divides two values together to compute their modulus or remainder.
    /// </summary>
    /// <param name="left">The value that <paramref name="right"/> divides.</param>
    /// <param name="right">The value that divides <paramref name="left"/>.</param>
    /// <returns>
    /// The modulus or remainder of <paramref name="left"/> divided by <paramref name="right"/>.
    /// </returns>
    static abstract TSelf Modulus(in TSelf left, in TSelf right);

    /// <summary>
    /// Reverses the direction of a given vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>A vector facing in the opposite direction.</returns>
    static abstract TSelf Negate(in TSelf value);

    /// <summary>
    /// Computes the absolute of a value.
    /// </summary>
    /// <param name="value">The value for which to get its absolute.</param>
    /// <returns>The absolute of value.</returns>
    static abstract TSelf Abs(in TSelf value);

    /// <summary>
    /// Restricts a value to be within a specified range.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped value.</returns>
    static abstract TSelf Clamp(in TSelf value, in TSelf min, in TSelf max);
}

/// <typeparam name="TComponent">The numeric type of the vector's components.</typeparam>
/// <inheritdoc/>
/// <typeparam name="TSelf"/>
public interface IVector<TSelf, TComponent> : IVector<TSelf>
    where TSelf : unmanaged, IVector<TSelf, TComponent> 
    where TComponent : INumberBase<TComponent>
{
    /// <summary>
    /// A <typeparamref name="TSelf"/> with all of its components set to zero.
    /// </summary>
    static abstract TSelf Zero { get; }

    /// <summary>
    /// Gets a <typeparamref name="TSelf"/> with all of its components set to one.
    /// </summary>
    static abstract TSelf One { get; }

    /// <summary>
    /// Gets a <typeparamref name="TSelf"/> with all components equal to the smallest possible value of the component type.
    /// </summary>
    static abstract TSelf Minimum { get; }

    /// <summary>
    /// Gets a <typeparamref name="TSelf"/> with all components equal to the largest possible value of the component type.
    /// </summary>
    static abstract TSelf Maximum { get; }

    /// <summary>
    /// Gets a minimum component value
    /// </summary>
    TComponent MinValue { get; }

    /// <summary>
    /// Gets a maximum component value
    /// </summary>
    TComponent MaxValue { get; }

    /// <summary>
    /// Gets a sum of the component values.
    /// </summary>
    TComponent ValuesSum { get; }

    /// <summary>
    /// Gets or sets the component at the specified index.
    /// </summary>
    /// <returns>The value of the component at the specified index.</returns>
    TComponent this[int index] { get; set; }

    /// <summary>
    /// Gets the squared length of the vector.
    /// </summary>
    /// <remarks>
    /// This method may be preferred to <see cref="IMeasurableVector{TSelf, TResult}.Length" /> when only a relative 
    /// length is needed and speed is of the essence.
    /// </remarks>
    TComponent LengthSquared { get; }

    /// <summary>
    /// Creates a new <typeparamref name="TSelf"/> instance with all elements initialized to the specified value.
    /// </summary>
    /// <param name="value">The value that all elements will be initialized to.</param>
    /// <returns>A new <typeparamref name="TSelf"/> with all elements initialized to <paramref name="value"/>.</returns>
    static abstract TSelf Create(TComponent value);

    /// <summary>
    /// Creates a new <typeparamref name="TSelf"/> from a given readonly span.
    /// </summary>
    /// <param name="values">The readonly span from which the vector is created.</param>
    /// <returns>
    /// A new <typeparamref name="TSelf"/> with it's elements set to the first 
    /// <see cref="IVector{TSelf}.Count"/> elements from values.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <c><paramref name="values"/>.Length</c> does not equal 
    /// <see cref="IVector{TSelf}.Count"/>.
    /// </exception>
    static abstract TSelf Create(ReadOnlySpan<TComponent> values);

    /// <summary>
    /// Creates an array containing the elements of the vector.
    /// </summary>
    /// <returns>An array of <see cref="IVector{TSelf}.Count"/> elements.</returns>
    TComponent[] ToArray();

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <param name="result">When the method completes, contains the dot product of the two vectors.</param>
    static virtual void Dot(in TSelf left, in TSelf right, out TComponent result) => result = TSelf.Dot(in left, in right);

    /// <returns>The dot product of the two vectors.</returns>
    /// <inheritdoc cref="Dot(in TSelf, in TSelf, out TComponent)"/>
    static abstract TComponent Dot(in TSelf left, in TSelf right);

    /// <summary>
    /// Calculates a new vector whose elements are the maximum of each pair of elements in the two given vectors.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <param name="result">When the method completes, contains the maximum vector.</param>
    static virtual void Max(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Max(in left, in right);

    /// <returns>The maximum vector.</returns>
    /// <inheritdoc cref="Max(in TSelf, in TSelf, out TSelf)" />
    static abstract TSelf Max(in TSelf left, in TSelf right);

    /// <summary>
    /// Calculates a new vector whose elements are the minimum of each pair of elements in the two given vectors.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <param name="result">When the method completes, contains the minimum vector.</param>
    static virtual void Min(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Min(in left, in right);

    /// <returns>The minimum vector.</returns>
    /// <inheritdoc cref="Min(in TSelf, in TSelf, out TSelf)" />
    static abstract TSelf Min(in TSelf left, in TSelf right);

    /// <summary>
    /// Calculates the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The squared distance between the two vectors.</returns>
    static abstract TComponent DistanceSquared(in TSelf value1, in TSelf value2);
}
