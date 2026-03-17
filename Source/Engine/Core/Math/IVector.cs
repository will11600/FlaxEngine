using System;
using System.Numerics;
using System.Runtime.Intrinsics;

namespace FlaxEngine;

public interface IVector<TSelf, TComponent> : IEquatable<TSelf>, IFormattable
    where TSelf : unmanaged, IVector<TSelf, TComponent> 
    where TComponent : INumberBase<TComponent>, IMinMaxValue<TComponent>
{
    /// <summary>
    /// Gets size of the <typeparamref name="TSelf"/> type, in bytes.
    /// </summary>
    static unsafe virtual int SizeInBytes { get; } = sizeof(TSelf);

    /// <summary>
    /// Gets the number of elements stored in the vector.
    /// </summary>
    static abstract int Count { get; }

    /// <summary>
    /// A <typeparamref name="TSelf"/> with all of its components set to zero.
    /// </summary>
    static virtual TSelf Zero { get; } = TSelf.Create(TComponent.Zero);

    /// <summary>
    /// Gets a <typeparamref name="TSelf"/> with all of its components set to one.
    /// </summary>
    static virtual TSelf One { get; } = TSelf.Create(TComponent.One);

    /// <summary>
    /// Gets a <typeparamref name="TSelf"/> with all components equal to the smallest possible value of the component type.
    /// </summary>
    static virtual TSelf Minimum { get; } = TSelf.Create(TComponent.MinValue);

    /// <summary>
    /// Gets a <typeparamref name="TSelf"/> with all components equal to the largest possible value of the component type.
    /// </summary>
    static virtual TSelf Maximum { get; } = TSelf.Create(TComponent.MaxValue);

    /// <summary>
    /// Gets or sets the component at the specified index.
    /// </summary>
    /// <returns>The value of the component at the specified index.</returns>
    TComponent this[int index] { get; set; }

    /// <remarks>
    /// Uses a fast approximation for the inverse square root, so the result may not be precise. 
    /// For a more accurate result, use <see cref="PreciseLength" />.
    /// </remarks>
    /// <inheritdoc cref="PreciseLength" />
    float Length { get; }

    /// <summary>
    /// Gets the length of the vector.
    /// </summary>
    float PreciseLength { get; }

    /// <summary>
    /// Gets the squared length of the vector.
    /// </summary>
    /// <remarks>
    /// This method may be preferred to <see cref="Length" /> when only a relative 
    /// length is needed and speed is of the essence.
    /// </remarks>
    float LengthSquared { get; }

    /// <remarks>
    /// Uses a fast approximation for the inverse square root, so the result may not be precise. 
    /// For a more accurate result, use <see cref="NormalizePrecise" />.
    /// </remarks>
    /// <inheritdoc cref="NormalizePrecise" />
    void Normalize();

    /// <summary>
    /// Converts the vector into a unit vector with a length of 1.
    /// </summary>
    void NormalizePrecise();

    /// <summary>
    /// Creates a vector with the same direction as the specified vector, but with a length of one.
    /// </summary>
    /// <param name="value">The vector to normalize.</param>
    /// <param name="result">When the method completes, contains the normalized vector.</param>
    static virtual void Normalize(in TSelf value, out TSelf result) => result = TSelf.Normalize(in value);

    /// <returns>The normalized vector.</returns>
    /// <inheritdoc cref="Normalize(in TSelf, out TSelf)" />
    static virtual TSelf Normalize(in TSelf value)
    {
        TSelf result = value;
        result.Normalize();
        return result;
    }

    /// <summary>
    /// Creates an array containing the elements of the vector.
    /// </summary>
    /// <returns>An array of <see cref="Count"/> elements.</returns>
    float[] ToArray();

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
    /// <see cref="Count"/> elements from values.
    /// </returns>
    static abstract TSelf Create(ReadOnlySpan<TComponent> values);

    /// <summary>
    /// Adds two values together to compute their sum.
    /// </summary>
    /// <param name="left">The value to which <paramref name="right"/> is added.</param>
    /// <param name="right">The value that is added to <paramref name="left"/>.</param>
    /// <param name="result">
    /// When the method completes, contains the sum of sum of <paramref name="left"/> 
    /// and <paramref name="right"/>.
    /// </param>
    static virtual void Add(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Add(in left, in right);

    /// <returns>The sum of sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
    /// <inheritdoc cref="Add(in TSelf, in TSelf, out TSelf)" />
    static abstract TSelf Add(in TSelf left, in TSelf right);

    /// <inheritdoc cref="Add(in TSelf, in TSelf, out TSelf)" />
    static virtual void Add(in TSelf left, TComponent right, out TSelf result) => result = TSelf.Add(in left, right);

    /// <inheritdoc cref="Add(in TSelf, in TSelf)" />
    static virtual TSelf Add(in TSelf left, TComponent right) => TSelf.Add(in left, TSelf.Create(right));

    /// <inheritdoc cref="Add(in TSelf, in TSelf)" />
    static virtual TSelf operator +(TSelf left, TSelf right) => TSelf.Add(in left, right);

    /// <inheritdoc cref="Add(in TSelf, in TSelf)" />
    static virtual TSelf operator +(TSelf left, TComponent right) => TSelf.Add(in left, right);

    /// <inheritdoc cref="Add(in TSelf, in TSelf)" />
    static virtual TSelf operator +(TComponent left, TSelf right) => TSelf.Add(in right, left);

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    /// <param name="left">The first vector to subtract.</param>
    /// <param name="right">The second vector to subtract.</param>
    /// <param name="result">When the method completes, contains the difference of the two vectors.</param>
    static virtual void Subtract(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Subtract(in left, in right);

    /// <returns>The difference of the two vectors.</returns>
    /// <inheritdoc cref="Subtract(in TSelf, in TSelf, out TSelf)" />
    static abstract TSelf Subtract(in TSelf left, in TSelf right);

    /// <inheritdoc cref="Subtract(in TSelf, in TSelf, out TSelf)" />
    static virtual void Subtract(in TSelf left, TComponent right, out TSelf result) => result = TSelf.Subtract(in left, right);

    /// <inheritdoc cref="Subtract(in TSelf, in TSelf)" />
    static virtual TSelf Subtract(in TSelf left, TComponent right) => TSelf.Subtract(in left, TSelf.Create(right));

    /// <inheritdoc cref="Subtract(in TSelf, in TSelf, out TSelf)" />
    static virtual void Subtract(TComponent left, in TSelf right, out TSelf result) => result = TSelf.Subtract(left, in right);

    /// <inheritdoc cref="Subtract(in TSelf, in TSelf)" />
    static virtual TSelf Subtract(TComponent left, in TSelf right) => TSelf.Subtract(TSelf.Create(left), in right);

    /// <inheritdoc cref="Subtract(in TSelf, in TSelf)" />
    static virtual TSelf operator -(TSelf left, TSelf right) => TSelf.Subtract(in left, in right);

    /// <inheritdoc cref="Subtract(in TSelf, in TSelf)" />
    static virtual TSelf operator -(TComponent left, TSelf right) => TSelf.Subtract(left, in right);

    /// <inheritdoc cref="Subtract(in TSelf, in TSelf)" />
    static virtual TSelf operator -(TSelf left, TComponent right) => TSelf.Subtract(in left, right);

    /// <summary>
    /// Multiplies two values together to compute their product.
    /// </summary>
    /// <param name="left">The value that <paramref name="right"/> multiplies.</param>
    /// <param name="right">The value that multiplies <paramref name="left"/>.</param>
    /// <param name="result">
    /// When the method completes, contains the product of <paramref name="left"/> 
    /// multiplied by <paramref name="right"/>.
    /// </param>
    static virtual void Multiply(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Multiply(in left, in right);

    /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
    /// <inheritdoc cref="Multiply(in TSelf, in TSelf, out TSelf)" />
    static abstract TSelf Multiply(in TSelf left, in TSelf right);

    /// <inheritdoc cref="Multiply(in TSelf, in TSelf, out TSelf)" />
    static virtual void Multiply(in TSelf left, TComponent right, out TSelf result) => result = TSelf.Multiply(in left, right);

    /// <inheritdoc cref="Multiply(in TSelf, in TSelf)" />
    static virtual TSelf Multiply(in TSelf left, TComponent right) => TSelf.Multiply(in left, TSelf.Create(right));

    /// <inheritdoc cref="Multiply(in TSelf, in TSelf, out TSelf)" />
    static virtual void Multiply(TComponent left, in TSelf right, out TSelf result) => result = TSelf.Multiply(left, in right);

    /// <inheritdoc cref="Multiply(in TSelf, in TSelf)" />
    static virtual TSelf Multiply(TComponent left, in TSelf right) => TSelf.Multiply(TSelf.Create(left), in right);

    /// <inheritdoc cref="Multiply(in TSelf, in TSelf)" />
    static virtual TSelf operator *(TSelf left, TSelf right) => TSelf.Multiply(in left, in right);

    /// <inheritdoc cref="Multiply(in TSelf, in TSelf)" />
    static virtual TSelf operator *(TComponent left, TSelf right) => TSelf.Multiply(left, in right);

    /// <inheritdoc cref="Multiply(in TSelf, in TSelf)" />
    static virtual TSelf operator *(TSelf left, TComponent right) => TSelf.Multiply(in left, right);

    /// <summary>
    /// Divides one value by another to compute their quotient.
    /// </summary>
    /// <param name="left">The value that <paramref name="right"/> divides.</param>
    /// <param name="right">The value that divides <paramref name="left"/>.</param>
    /// <param name="result">
    /// When this method returns, contains the quotient of <paramref name="left"/> 
    /// divided by <paramref name="right"/>.
    /// </param>
    static virtual void Divide(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Divide(in left, in right);

    /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
    /// <inheritdoc cref="Divide(in TSelf, in TSelf, out TSelf)" />
    static abstract TSelf Divide(in TSelf left, in TSelf right);

    /// <inheritdoc cref="Divide(in TSelf, in TSelf, out TSelf)" />
    static virtual void Divide(in TSelf left, TComponent right, out TSelf result) => result = TSelf.Divide(in left, right);

    /// <inheritdoc cref="Divide(in TSelf, in TSelf)" />
    static virtual TSelf Divide(in TSelf left, TComponent right) => TSelf.Divide(in left, TSelf.Create(right));

    /// <inheritdoc cref="Divide(in TSelf, in TSelf, out TSelf)" />
    static virtual void Divide(TComponent left, in TSelf right, out TSelf result) => result = TSelf.Divide(left, in right);

    /// <inheritdoc cref="Divide(in TSelf, in TSelf)" />
    static virtual TSelf Divide(TComponent left, in TSelf right) => TSelf.Divide(TSelf.Create(left), in right);

    /// <inheritdoc cref="Divide(in TSelf, in TSelf)" />
    static virtual TSelf operator /(TSelf left, TSelf right) => TSelf.Divide(in left, in right);

    /// <inheritdoc cref="Divide(in TSelf, in TSelf)" />
    static virtual TSelf operator /(TComponent left, TSelf right) => TSelf.Divide(left, in right);

    /// <inheritdoc cref="Divide(in TSelf, in TSelf)" />
    static virtual TSelf operator /(TSelf left, TComponent right) => TSelf.Divide(in left, right);

    /// <summary>
    /// Divides two values together to compute their modulus or remainder.
    /// </summary>
    /// <param name="left">The value that <paramref name="right"/> divides.</param>
    /// <param name="right">The value that divides <paramref name="left"/>.</param>
    /// <param name="result">
    /// When this method returns, contains the modulus or remainder of <paramref name="left"/> 
    /// divided by <paramref name="right"/>.
    /// </param>
    static virtual void Modulus(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Modulus(in left, in right);

    /// <returns>
    /// The modulus or remainder of <paramref name="left"/> divided by <paramref name="right"/>.
    /// </returns>
    /// <inheritdoc cref="Modulus(in TSelf, in TSelf, out TSelf)" />
    static abstract TSelf Modulus(in TSelf left, in TSelf right);

    /// <inheritdoc cref="Modulus(in TSelf, in TSelf, out TSelf)" />
    static virtual void Modulus(in TSelf left, TComponent right, out TSelf result) => result = TSelf.Modulus(in left, right);

    /// <inheritdoc cref="Modulus(in TSelf, in TSelf)" />
    static virtual TSelf Modulus(in TSelf left, TComponent right) => TSelf.Modulus(in left, TSelf.Create(right));

    /// <inheritdoc cref="Modulus(in TSelf, in TSelf, out TSelf)" />
    static virtual void Modulus(TComponent left, in TSelf right, out TSelf result) => result = TSelf.Modulus(left, in right);

    /// <inheritdoc cref="Modulus(in TSelf, in TSelf)" />
    static virtual TSelf Modulus(TComponent left, in TSelf right) => TSelf.Modulus(TSelf.Create(left), in right);

    /// <inheritdoc cref="Modulus(in TSelf, in TSelf)" />
    static virtual TSelf operator %(TSelf left, TSelf right) => TSelf.Modulus(in left, in right);

    /// <inheritdoc cref="Modulus(in TSelf, in TSelf)" />
    static virtual TSelf operator %(TComponent left, TSelf right) => TSelf.Modulus(left, in right);

    /// <inheritdoc cref="Modulus(in TSelf, in TSelf)" />
    static virtual TSelf operator %(TSelf left, TComponent right) => TSelf.Modulus(in left, right);

    /// <summary>
    /// Reverses the direction of a given vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <param name="result">When the method completes, contains a vector facing in the opposite direction.</param>
    static virtual void Negate(ref TSelf value, out TSelf result) => result = TSelf.Negate(in value);

    /// <returns>A vector facing in the opposite direction.</returns>
    /// <inheritdoc cref="Negate(ref TSelf, out TSelf)" />
    static abstract TSelf Negate(in TSelf value);

    /// <inheritdoc cref="Negate(in TSelf)" />
    static virtual TSelf operator -(TSelf value) => TSelf.Negate(value);

    /// <summary>
    /// Assert a vector (return it unchanged).
    /// </summary>
    /// <param name="value">The vector to assert (unchanged).</param>
    /// <returns>The asserted (unchanged) vector.</returns>
    static virtual TSelf operator +(TSelf value) => value;

    /// <inheritdoc cref="IEquatable{TSelf}.Equals(TSelf)" />
    bool Equals(in TSelf other);

    bool IEquatable<TSelf>.Equals(TSelf other) => Equals(in other);

    /// <summary>
    /// Compares two <typeparamref name="TSelf"/> to determine equality.
    /// </summary>
    /// <param name="left">The value to compare with <paramref name="right"/>.</param>
    /// <param name="right">The value to compare with <paramref name="left"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    static virtual bool operator ==(TSelf left, TSelf right) => left.Equals(in right);

    /// <summary>
    /// Compares two <typeparamref name="TSelf"/> to determine inequality.
    /// </summary>
    /// <inheritdoc cref="operator ==(TSelf, TSelf)" />
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    static virtual bool operator !=(TSelf left, TSelf right) => !left.Equals(in right);

    /// <summary>
    /// Restricts a value to be within a specified range.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">When the method completes, contains the clamped value.</param>
    static virtual void Clamp(in TSelf value, in TSelf min, in TSelf max, out TSelf result) => result = TSelf.Clamp(in value, in min, in max);

    /// <returns>The clamped value.</returns>
    /// <inheritdoc cref="Clamp(in TSelf, in TSelf, in TSelf, out TSelf)" />
    static abstract TSelf Clamp(in TSelf value, in TSelf min, in TSelf max);

    /// <remarks>
    /// <para>Uses a fast approximation for the inverse square root, so the result may not be precise. 
    /// For a more accurate result, use <see cref="PreciseDistance(in TSelf, in TSelf, out TComponent)" />.</para>
    /// <para>Consider using <see cref="DistanceSquared(in TSelf, in TSelf, out TComponent)"/> when only relative 
    /// distance is required.</para>
    /// </remarks>
    /// <inheritdoc cref="PreciseDistance(in TSelf, in TSelf, out TComponent)" />
    static virtual void Distance(in TSelf value1, in TSelf value2, out TComponent result) => result = TSelf.Distance(in value1, in value2);

    /// <returns>The distance between the two vectors.</returns>
    /// <remarks>
    /// <para>Uses a fast approximation for the inverse square root, so the result may not be precise. 
    /// For a more accurate result, use <see cref="PreciseDistance(in TSelf, in TSelf)" />.</para>
    /// <para>Consider using <see cref="DistanceSquared(in TSelf, in TSelf)"/> when only relative 
    /// distance is required.</para>
    /// </remarks>
    /// <inheritdoc cref="Distance(in TSelf, in TSelf, out TComponent)" />
    static abstract TComponent Distance(in TSelf value1, in TSelf value2);

    /// <summary>
    /// Calculates the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">When the method completes, contains the distance between the two vectors.</param>
    /// <remarks>
    /// Only use this method if <see cref="Distance(in TSelf, in TSelf)"/> does not provide sufficient precision.
    /// </remarks>
    static virtual void PreciseDistance(in TSelf value1, in TSelf value2, out TComponent result) => result = TSelf.Distance(in value1, in value2);

    /// <returns>The distance between the two vectors.</returns>
    /// <remarks>
    /// Only use this method if <see cref="Distance(in TSelf, in TSelf)"/> does not provide sufficient precision.
    /// </remarks>
    /// <inheritdoc cref="Distance(in TSelf, in TSelf, out TComponent)" />
    static abstract TComponent PreciseDistance(in TSelf value1, in TSelf value2);

    /// <summary>
    /// Calculates the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">When the method completes, contains the squared distance between the two vectors.</param>
    static virtual void DistanceSquared(in TSelf value1, in TSelf value2, out TComponent result) => result = TSelf.DistanceSquared(in value1, in value2);

    /// <returns>The squared distance between the two vectors.</returns>
    /// <inheritdoc cref="DistanceSquared(in TSelf, in TSelf, out TComponent)" />
    static abstract TComponent DistanceSquared(in TSelf value1, in TSelf value2);

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
    /// Calculates a new vector whose elements are the absolute values of the given vector's elements.
    /// </summary>
    /// <param name="value">The source vector.</param>
    /// <returns>The absolute value vector.</returns>
    static abstract TSelf Abs(in TSelf value);
}
