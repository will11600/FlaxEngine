using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace FlaxEngine;

/// <summary>
/// Provides static methods and extension methods for performing mathematical operations on vector types, including
/// conversions, arithmetic, interpolation, and geometric computations.
/// </summary>
public static class VectorMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<float> Modulus(in Vector128<float> left, in Vector128<float> right)
    {
        Vector128<float> quotient = left / right;
        Vector128<float> truncated = Vector128.Truncate(quotient);
        return left - (truncated * right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<double> Modulus(in Vector128<double> left, in Vector128<double> right)
    {
        Vector128<double> quotient = left / right;
        Vector128<double> truncated = Vector128.Truncate(quotient);
        return left - (truncated * right);
    }

    internal static Vector<T> CatmullRom<T>(in Vector<T> value1, in Vector<T> value2, in Vector<T> value3, in Vector<T> value4, T amount)
        where T : struct, IFloatingPoint<T>
    {
        T squared = amount * amount;
        T cubed = amount * squared;

        Vector<T> vT = Vector.Create(amount);
        Vector<T> vT2 = Vector.Create(squared);
        Vector<T> vT3 = Vector.Create(cubed);

        Vector<T> five = Vector.Create(T.CreateTruncating(5.0));
        Vector<T> two = Vector.Create(T.CreateTruncating(2.0));
        Vector<T> three = Vector.Create(T.CreateTruncating(3.0));
        Vector<T> four = Vector.Create(T.CreateTruncating(4.0));

        Vector<T> term0 = two * value2;
        Vector<T> term1 = (-value1 + value3) * vT;
        Vector<T> term2 = (two * value1 - five * value2 + four * value3 - value4) * vT2;
        Vector<T> term3 = (-value1 + three * value2 - three * value3 + value4) * vT3;

        Vector<T> half = Vector.Create(T.CreateTruncating(0.5));

        return half * (term0 + term1 + term2 + term3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<T> CatmullRom<T>(in Vector128<T> value1, in Vector128<T> value2, in Vector128<T> value3, in Vector128<T> value4, T amount)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = CatmullRom(value1.AsVector(), value2.AsVector(), value3.AsVector(), value4.AsVector(), amount);
        return result.AsVector128();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector256<T> CatmullRom<T>(in Vector256<T> value1, in Vector256<T> value2, in Vector256<T> value3, in Vector256<T> value4, T amount)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = CatmullRom(value1.AsVector(), value2.AsVector(), value3.AsVector(), value4.AsVector(), amount);
        return result.AsVector256();
    }

    internal static Vector<T> Hermite<T>(in Vector<T> value1, in Vector<T> tangent1, in Vector<T> value2, in Vector<T> tangent2, T amount)
        where T : struct, IFloatingPoint<T>
    {
        T squared = amount * amount;
        T cubed = amount * squared;

        T two = T.CreateTruncating(2.0);
        T three = T.CreateTruncating(3.0);

        Vector<T> vp1 = Vector.Create(two * cubed - three * squared + T.One);
        Vector<T> vp2 = Vector.Create(-two * cubed + three * squared);
        Vector<T> vp3 = Vector.Create(cubed - two * squared + amount);
        Vector<T> vp4 = Vector.Create(cubed - squared);

        return (value1 * vp1) + (value2 * vp2) + (tangent1 * vp3) + (tangent2 * vp4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<T> Hermite<T>(in Vector128<T> value1, in Vector128<T> tangent1, in Vector128<T> value2, in Vector128<T> tangent2, T amount)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = Hermite(value1.AsVector(), tangent1.AsVector(), value2.AsVector(), tangent2.AsVector(), amount);
        return result.AsVector128();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector256<T> Hermite<T>(in Vector256<T> value1, in Vector256<T> tangent1, in Vector256<T> value2, in Vector256<T> tangent2, T amount)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = Hermite(value1.AsVector(), tangent1.AsVector(), value2.AsVector(), tangent2.AsVector(), amount);
        return result.AsVector256();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T Barycentric<T>(in T value1, in T value2, in T value3, in T amount1, in T amount2)
        where T : struct, IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IMultiplyOperators<T, T, T>
    {
        return value1 + amount1 * (value2 - value1) + amount2 * (value3 - value1);
    }

    internal static Vector<T> Barycentric<T>(in Vector<T> value1, in Vector<T> value2, in Vector<T> value3, T amount1, T amount2)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> a1 = Vector.Create(amount1);
        Vector<T> a2 = Vector.Create(amount2);
        return value1 + (a1 * (value2 - value1)) + (a2 * (value3 - value1));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<T> Barycentric<T>(in Vector128<T> value1, in Vector128<T> value2, in Vector128<T> value3, T amount1, T amount2)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = Barycentric(value1.AsVector(), value2.AsVector(), value3.AsVector(), amount1, amount2);
        return result.AsVector128();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector256<T> Barycentric<T>(in Vector256<T> value1, in Vector256<T> value2, in Vector256<T> value3, T amount1, T amount2)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = Barycentric(value1.AsVector(), value2.AsVector(), value3.AsVector(), amount1, amount2);
        return result.AsVector256();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<float> Transform(in Vector128<float> vector, in Quaternion rotation)
    {
        float x = rotation.X + rotation.X;
        float y = rotation.Y + rotation.Y;
        float z = rotation.Z + rotation.Z;
        float wx = rotation.W * x, wy = rotation.W * y, wz = rotation.W * z;
        float xx = rotation.X * x, xy = rotation.X * y, xz = rotation.X * z;
        float yy = rotation.Y * y, yz = rotation.Y * z, zz = rotation.Z * z;

        Vector128<float> vX = Vector128.Create(vector.GetElement(0));
        Vector128<float> vY = Vector128.Create(vector.GetElement(1));
        Vector128<float> vZ = Vector128.Create(vector.GetElement(2));

        Vector128<float> col1 = Vector128.Create(1.0f - yy - zz, xy + wz, xz - wy, 0.0f);
        Vector128<float> col2 = Vector128.Create(xy - wz, 1.0f - xx - zz, yz + wx, 0.0f);
        Vector128<float> col3 = Vector128.Create(xz + wy, yz - wx, 1.0f - xx - yy, 0.0f);

        Vector128<float> vResult = (vX * col1) + (vY * col2) + (vZ * col3);

        return vResult.WithElement(3, vector.GetElement(3));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<float> Transform(in Vector128<float> vector, in Matrix transform)
    {
        Vector128<float> vX = Vector128.Create(vector.GetElement(0));
        Vector128<float> vY = Vector128.Create(vector.GetElement(1));
        Vector128<float> vZ = Vector128.Create(vector.GetElement(2));
        Vector128<float> vW = Vector128.Create(vector.GetElement(3));

        ref Vector128<float> row1 = ref Unsafe.As<float, Vector128<float>>(ref Unsafe.AsRef(in transform.M11));
        ref Vector128<float> row2 = ref Unsafe.As<float, Vector128<float>>(ref Unsafe.AsRef(in transform.M21));
        ref Vector128<float> row3 = ref Unsafe.As<float, Vector128<float>>(ref Unsafe.AsRef(in transform.M31));
        ref Vector128<float> row4 = ref Unsafe.As<float, Vector128<float>>(ref Unsafe.AsRef(in transform.M41));

        return (vX * row1) + (vY * row2) + (vZ * row3) + (vW * row4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref TComponent GetRef<TSelf, TComponent>(ref this TSelf vector, int index) 
        where TSelf : unmanaged, IVector<TSelf, TComponent> 
        where TComponent : struct, INumberBase<TComponent>
    {
        return ref Unsafe.Add(ref Unsafe.As<TSelf, TComponent>(ref vector), index);
    }

    extension<TSelf>(TSelf vector) where TSelf : unmanaged, IVector<TSelf>
    {
        /// <summary>
        /// Gets size of <typeparamref name="TSelf"/>, in bytes.
        /// </summary>
        public static unsafe int SizeInBytes => sizeof(TSelf);

        /// <summary>
        /// Gets a vector with values being absolute values of that vector.
        /// </summary>
        public TSelf Absolute => TSelf.Abs(vector);

        /// <summary>
        /// Gets a vector with values being opposite to values of that vector.
        /// </summary>
        public TSelf Negative => TSelf.Negate(vector);

        /// <param name="result">
        /// When the method completes, contains the sum of <paramref name="left"/> 
        /// and <paramref name="right"/>.
        /// </param>
        /// <returns/>
        /// <inheritdoc cref="IVector{TSelf}.Add(in TSelf, in TSelf)"/>
        /// <param name="left"/>
        /// <param name="right"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Add(in left, in right);

        /// <inheritdoc cref="IVector{TSelf}.Add(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator +(in TSelf left, in TSelf right) => TSelf.Add(in left, in right);

        /// <param name="result">When the method completes, contains the difference of the two vectors.</param>
        /// <returns/>
        /// <inheritdoc cref="IVector{TSelf}.Subtract(in TSelf, in TSelf)"/>
        /// <param name="left"/>
        /// <param name="right"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Subtract(in left, in right);

        /// <inheritdoc cref="IVector{TSelf}.Subtract(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator -(in TSelf left, in TSelf right) => TSelf.Subtract(in left, in right);

        /// <param name="result">
        /// When the method completes, contains the product of <paramref name="left"/> 
        /// multiplied by <paramref name="right"/>.
        /// </param>
        /// <returns/>
        /// <inheritdoc cref="IVector{TSelf}.Multiply(in TSelf, in TSelf)"/>
        /// <param name="left"/>
        /// <param name="right"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Multiply(in left, in right);

        /// <inheritdoc cref="IVector{TSelf}.Multiply(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator *(in TSelf left, in TSelf right) => TSelf.Multiply(in left, in right);

        /// <param name="result">
        /// When this method returns, contains the quotient of <paramref name="left"/> 
        /// divided by <paramref name="right"/>.
        /// </param>
        /// <returns/>
        /// <inheritdoc cref="IVector{TSelf}.Divide(in TSelf, in TSelf)"/>
        /// <param name="left"/>
        /// <param name="right"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Divide(in left, in right);

        /// <inheritdoc cref="IVector{TSelf}.Divide(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator /(in TSelf left, in TSelf right) => TSelf.Divide(in left, in right);

        /// <param name="result">When the method completes, contains a vector facing in the opposite direction.</param>
        /// <returns/>
        /// <inheritdoc cref="IVector{TSelf}.Negate(in TSelf)"/>
        /// <param name="value"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate(in TSelf value, out TSelf result) => result = TSelf.Negate(in value);

        /// <inheritdoc cref="IVector{TSelf}.Negate(in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator -(in TSelf value) => TSelf.Negate(in value);

        /// <summary>
        /// Assert a vector (return it unchanged).
        /// </summary>
        /// <param name="value">The vector to assert (unchanged).</param>
        /// <returns>The asserted (unchanged) vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator +(TSelf value) => value;

        /// <param name="result">
        /// When this method returns, contains the modulus or remainder of <paramref name="left"/> 
        /// divided by <paramref name="right"/>.
        /// </param>
        /// <returns/>
        /// <inheritdoc cref="IVector{TSelf}.Modulus(in TSelf, in TSelf)"/>
        /// <param name="left"/>
        /// <param name="right"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Modulus(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Modulus(in left, in right);

        /// <inheritdoc cref="IVector{TSelf}.Modulus(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator %(in TSelf left, in TSelf right) => TSelf.Modulus(in left, in right);

        /// <summary>
        /// Compares two <typeparamref name="TSelf"/> instances to determine equality.
        /// </summary>
        /// <param name="left">The value to compare with <paramref name="right"/>.</param>
        /// <param name="right">The value to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in TSelf left, in TSelf right) => left.Equals(in right);

        /// <summary>
        /// Compares two <typeparamref name="TSelf"/> instances to determine inequality.
        /// </summary>
        /// <param name="left">The value to compare with <paramref name="right"/>.</param>
        /// <param name="right">The value to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in TSelf left, in TSelf right) => !left.Equals(in right);

        /// <param name="result">When the method completes, contains the clamped value.</param>
        /// <inheritdoc cref="IVector{TSelf}.Clamp(in TSelf, in TSelf, in TSelf)"/>
        /// <param name="value"/>
        /// <param name="min"/>
        /// <param name="max"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp(in TSelf value, in TSelf min, in TSelf max, out TSelf result) => result = TSelf.Clamp(in value, in min, in max);
    }

    extension<TSelf, TResult>(TSelf) where TSelf : unmanaged, IVector<TSelf>, IMeasurableVector<TSelf, TResult> where TResult : struct, IFloatingPoint<TResult>
    {
        /// <remarks>
        /// <para>On x86/x64 hardware this may use the <c>RSQRTSS</c> instruction which has a maximum relative error of <c>1.5 * 2^-12</c>.</para>
        /// <para>On Arm64 hardware this may use the <c>FRSQRTE</c> instruction which has a maximum relative error of <c>1.5 * 2^-8</c>.</para>
        /// <para>On unsupported hardware this method will fall back to a precise normalization which does not use an estimate and has no error.</para>
        /// </remarks>
        /// <inheritdoc cref="PreciseDistance{TSelf, TResult}(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Distance(in TSelf value1, in TSelf value2)
        {
            TSelf result = TSelf.Abs(value1 - value2);
            return result.Length;
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <remarks>
        /// Only use this method if <see cref="Distance{TSelf, TResult}(in TSelf, in TSelf)"/> does not provide sufficient precision.
        /// </remarks>
        /// <returns>The distance between the two vectors.</returns>
        /// <inheritdoc cref="DistanceSquared{TSelf, TComponent}(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult PreciseDistance(in TSelf value1, in TSelf value2)
        {
            TSelf result = TSelf.Abs(value1 - value2);
            return result.PreciseLength;
        }

        /// <remarks>
        /// <para>Uses a fast approximation for the inverse square root, so the result may not be precise. 
        /// For a more accurate result, use <see cref="PreciseDistance{TSelf, TResult}(in TSelf, in TSelf, out TResult)" />.</para>
        /// <para>Consider using <see cref="DistanceSquared{TSelf, TComponent}(in TSelf, in TSelf, out TComponent)"/> when only relative 
        /// distance is required.</para>
        /// </remarks>
        /// <returns/>
        /// <param name="result">When the method completes, contains the distance between the two vectors.</param>
        /// <inheritdoc cref="Distance{TSelf, TResult}(in TSelf, in TSelf)"/>
        /// <param name="value1"/>
        /// <param name="value2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Distance(in TSelf value1, in TSelf value2, out TResult result) => result = Distance<TSelf, TResult>(in value1, in value2);

        /// <remarks>
        /// Only use this method if <see cref="Distance{TSelf, TResult}(in TSelf, in TSelf, out TResult)"/> does not provide sufficient precision.
        /// </remarks>
        /// <param name="result">When the method completes, contains the distance between the two vectors.</param>
        /// <returns/>
        /// <inheritdoc cref="PreciseDistance{TSelf, TResult}(in TSelf, in TSelf)"/>
        /// <param name="value1"/>
        /// <param name="value2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult PreciseDistance(in TSelf value1, in TSelf value2, out TResult result) => result = Distance<TSelf, TResult>(in value1, in value2);
    }

    extension<TSelf, TResult>(TSelf) where TSelf : unmanaged, ITrigonometricVector<TSelf, TResult> where TResult : struct, INumberBase<TResult>
    {
        /// <param name="result">When this method returns, contains the cross product.</param>
        /// <returns/>
        /// <inheritdoc cref="ITrigonometricVector{TSelf, TResult}.Cross(in TSelf, in TSelf)"/>
        /// <param name="left"/>
        /// <param name="right"/>
        public static void Cross(in TSelf left, in TSelf right, out TSelf result) => result = TSelf.Cross(in left, in right);
    }

    extension<TSelf, TComponent>(TSelf) where TSelf : unmanaged, IVector<TSelf, TComponent> where TComponent : INumberBase<TComponent>
    {
        /// <param name="result">When the method completes, contains the squared distance between the two vectors.</param>
        /// <returns/>
        /// <inheritdoc cref="DistanceSquared{TSelf, TComponent}(in TSelf, in TSelf)"/>
        /// <param name="value1"/>
        /// <param name="value2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TComponent DistanceSquared(in TSelf value1, in TSelf value2, out TComponent result)
        {
            return result = DistanceSquared<TSelf, TComponent>(in value1, in value2);
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between the two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TComponent DistanceSquared(in TSelf value1, in TSelf value2)
        {
            TSelf result = TSelf.Abs(value1 - value2);
            return result.LengthSquared;
        }

        #region Addition
        /// <inheritdoc cref="IVector{TSelf}.Add(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator +(in TSelf left, TComponent right) => TSelf.Add(in left, TSelf.Create(right));

        /// <inheritdoc cref="IVector{TSelf}.Add(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator +(TComponent left, in TSelf right) => TSelf.Add(TSelf.Create(left), in right);

        /// <inheritdoc cref="IVector{TSelf}.Add(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(in TSelf left, TComponent right) => TSelf.Add(in left, TSelf.Create(right));

        /// <inheritdoc cref="IVector{TSelf}.Add(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(TComponent left, in TSelf right) => TSelf.Add(TSelf.Create(left), in right);

        /// <inheritdoc cref="Add{TSelf}(in TSelf, in TSelf, out TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(in TSelf left, TComponent right, out TSelf result) => result = TSelf.Add(in left, TSelf.Create(right));

        /// <inheritdoc cref="Add{TSelf, TComponent}(in TSelf, TComponent, out TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(TComponent left, in TSelf right, out TSelf result) => result = TSelf.Add(TSelf.Create(left), in right);
        #endregion

        #region Subtraction
        /// <inheritdoc cref="IVector{TSelf}.Subtract(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator -(in TSelf left, TComponent right) => TSelf.Subtract(in left, TSelf.Create(right));

        /// <inheritdoc cref="IVector{TSelf}.Subtract(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator -(TComponent left, in TSelf right) => TSelf.Subtract(TSelf.Create(left), in right);

        /// <inheritdoc cref="IVector{TSelf}.Subtract(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(in TSelf left, TComponent right) => TSelf.Subtract(in left, TSelf.Create(right));

        /// <inheritdoc cref="IVector{TSelf}.Subtract(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(TComponent left, in TSelf right) => TSelf.Subtract(TSelf.Create(left), in right);

        /// <inheritdoc cref="Subtract{TSelf}(in TSelf, in TSelf, out TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(in TSelf left, TComponent right, out TSelf result) => result = TSelf.Subtract(in left, TSelf.Create(right));

        /// <inheritdoc cref="Subtract{TSelf, TComponent}(in TSelf, TComponent, out TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(TComponent left, in TSelf right, out TSelf result) => result = TSelf.Subtract(TSelf.Create(left), in right);
        #endregion

        #region Multiplication
        /// <inheritdoc cref="IVector{TSelf}.Multiply(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator *(in TSelf left, TComponent right) => TSelf.Multiply(in left, TSelf.Create(right));

        /// <inheritdoc cref="IVector{TSelf}.Multiply(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator *(TComponent left, in TSelf right) => TSelf.Multiply(TSelf.Create(left), in right);

        /// <inheritdoc cref="IVector{TSelf}.Multiply(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(in TSelf left, TComponent right) => TSelf.Multiply(in left, TSelf.Create(right));

        /// <inheritdoc cref="IVector{TSelf}.Multiply(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(TComponent left, in TSelf right) => TSelf.Multiply(TSelf.Create(left), in right);

        /// <inheritdoc cref="Multiply{TSelf}(in TSelf, in TSelf, out TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(in TSelf left, TComponent right, out TSelf result) => result = TSelf.Multiply(in left, TSelf.Create(right));

        /// <inheritdoc cref="Multiply{TSelf, TComponent}(in TSelf, TComponent, out TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(TComponent left, in TSelf right, out TSelf result) => result = TSelf.Multiply(TSelf.Create(left), in right);
        #endregion

        #region Division
        /// <inheritdoc cref="IVector{TSelf}.Divide(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator /(in TSelf left, TComponent right) => TSelf.Divide(in left, TSelf.Create(right));

        /// <inheritdoc cref="IVector{TSelf}.Divide(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator /(TComponent left, in TSelf right) => TSelf.Divide(TSelf.Create(left), in right);

        /// <inheritdoc cref="IVector{TSelf}.Divide(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(in TSelf left, TComponent right) => TSelf.Divide(in left, TSelf.Create(right));

        /// <inheritdoc cref="IVector{TSelf}.Divide(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(TComponent left, in TSelf right) => TSelf.Divide(TSelf.Create(left), in right);

        /// <inheritdoc cref="Divide{TSelf}(in TSelf, in TSelf, out TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(in TSelf left, TComponent right, out TSelf result) => result = TSelf.Divide(in left, TSelf.Create(right));

        /// <inheritdoc cref="Divide{TSelf, TComponent}(in TSelf, TComponent, out TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(TComponent left, in TSelf right, out TSelf result) => result = TSelf.Divide(TSelf.Create(left), in right);
        #endregion

        #region Modulus
        /// <inheritdoc cref="IVector{TSelf}.Modulus(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator %(in TSelf left, TComponent right) => TSelf.Modulus(in left, TSelf.Create(right));

        /// <inheritdoc cref="IVector{TSelf}.Modulus(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf operator %(TComponent left, in TSelf right) => TSelf.Modulus(TSelf.Create(left), in right);

        /// <inheritdoc cref="IVector{TSelf}.Modulus(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Modulus(in TSelf left, TComponent right) => TSelf.Modulus(in left, TSelf.Create(right));

        /// <inheritdoc cref="IVector{TSelf}.Modulus(in TSelf, in TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Modulus(TComponent left, in TSelf right) => TSelf.Modulus(TSelf.Create(left), in right);

        /// <inheritdoc cref="Modulus{TSelf}(in TSelf, in TSelf, out TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Modulus(in TSelf left, TComponent right, out TSelf result) => result = TSelf.Modulus(in left, TSelf.Create(right));

        /// <inheritdoc cref="Modulus{TSelf, TComponent}(in TSelf, TComponent, out TSelf)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Modulus(TComponent left, in TSelf right, out TSelf result) => result = TSelf.Modulus(TSelf.Create(left), in right);
        #endregion
    }

    extension<TSelf, TComponent>(TSelf) where TSelf : unmanaged, IVector2<TSelf, TComponent> where TComponent : struct, IFloatingPointIeee754<TComponent>
    {
        /// <param name="value">The vector to normalize.</param>
        /// <param name="result">When the method completes, contains the normalized vector.</param>
        /// <inheritdoc cref="IVector2{TSelf, TComponent}.Normalize"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(in TSelf value, out TSelf result) => result = Normalize<TSelf, TComponent>(value);

        /// <returns>The normalized vector.</returns>
        /// <inheritdoc cref="IVector2{TSelf, TComponent}.Normalize"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf Normalize(TSelf value)
        {
            value.Normalize();
            return value;
        }

        /// <param name="value">The vector to normalize.</param>
        /// <param name="result">When the method completes, contains the normalized vector.</param>
        /// <inheritdoc cref="IVector2{TSelf, TComponent}.NormalizePrecise"/>
        public static void NormalizePrecise(in TSelf value, out TSelf result) => result = NormalizePrecise<TSelf, TComponent>(value);

        /// <returns>The normalized vector.</returns>
        /// <inheritdoc cref="IVector2{TSelf, TComponent}.NormalizePrecise"/>
        public static TSelf NormalizePrecise(TSelf value)
        {
            value.NormalizePrecise();
            return value;
        }

        /// <returns>
        /// <see langword="true"/> if the absolute difference between <paramref name="left"/> 
        /// and <paramref name="right"/> is less than <c><typeparamref name="TComponent"/>.Epsilon</c>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <inheritdoc cref="IVector2{TSelf, TComponent}.NearEqual(in TSelf, in TSelf, TComponent)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NearEqual(in TSelf left, in TSelf right) => TSelf.NearEqual(in left, in right, TComponent.Epsilon);

        /// <summary>
        /// Clamps the length of the specified vector to a maximum length.
        /// </summary>
        /// <returns>
        /// The result of clamping <paramref name="vector"/> to the inclusive range of 
        /// <c><typeparamref name="TComponent"/>.Zero</c> and <paramref name="max"/>.
        /// </returns>
        /// <inheritdoc cref="IVector2{TSelf, TComponent}.ClampLength(in TSelf, TComponent, TComponent)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf ClampLength(in TSelf vector, TComponent max) => TSelf.ClampLength(in vector, TComponent.Zero, max);

        /// <param name="result">
        /// When this method returns, contains the result of clamping <paramref name="vector"/> to 
        /// the inclusive range of <paramref name="min"/> and <paramref name="max"/>.
        /// </param>
        /// <returns/>
        /// <inheritdoc cref="IVector2{TSelf, TComponent}.ClampLength(in TSelf, TComponent, TComponent)"/>
        /// <param name="vector"/>
        /// <param name="min"/>
        /// <param name="max"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampLength(in TSelf vector, TComponent min, TComponent max, out TSelf result) => result = TSelf.ClampLength(in vector, min, max);

        /// <param name="result">
        /// When this method returns, contains the result of clamping <paramref name="vector"/> to 
        /// the inclusive range of <c><typeparamref name="TComponent"/>.Zero</c> and <paramref name="max"/>.
        /// </param>
        /// <returns/>
        /// <inheritdoc cref="IVector2{TSelf, TComponent}.ClampLength(in TSelf, TComponent, TComponent)"/>
        /// <param name="vector"/>
        /// <param name="max"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampLength(in TSelf vector, TComponent max, out TSelf result) => result = TSelf.ClampLength(in vector, TComponent.Zero, max);

        /// <param name="result">When this method returns, contains the interpolated value.</param>
        /// <returns/>
        /// <inheritdoc cref="IVector2{TSelf, TComponent}.Lerp(in TSelf, in TSelf, TComponent)"/>
        /// <param name="start"/>
        /// <param name="end"/>
        /// <param name="amount"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Lerp(in TSelf start, in TSelf end, TComponent amount, out TSelf result) => result = TSelf.Lerp(in start, in end, amount);

        /// <param name="result">When the method completes, contains the interpolated value.</param>
        /// <returns/>
        /// <inheritdoc cref="IVector2{TSelf, TComponent}.SmoothStep(in TSelf, in TSelf, TComponent)"/>
        /// <param name="start"/>
        /// <param name="end"/>
        /// <param name="amount"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SmoothStep(in TSelf start, in TSelf end, TComponent amount, out TSelf result) => result = TSelf.SmoothStep(in start, in end, amount);
    }

    extension<TSelf, TComponent>(TSelf) where TSelf : unmanaged, IVector3<TSelf, TComponent> where TComponent : struct, IFloatingPointIeee754<TComponent>
    {
        /// <param name="result">When the method completes, contains the result of the Catmull-Rom interpolation.</param>
        /// <returns/>
        /// <inheritdoc cref="IVector3{TSelf, TComponent}.CatmullRom(in TSelf, in TSelf, in TSelf, in TSelf, TComponent)"/>
        /// <param name="value1"/>
        /// <param name="value2"/>
        /// <param name="value3"/>
        /// <param name="value4"/>
        /// <param name="amount"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CatmullRom(in TSelf value1, in TSelf value2, in TSelf value3, in TSelf value4, TComponent amount, out TSelf result)
        {
            result = TSelf.CatmullRom(in value1, in value2, in value3, in value4, amount);
        }

        /// <param name="result">When the method completes, contains the result of the Hermite spline interpolation.</param>
        /// <returns/>
        /// <inheritdoc cref="IVector3{TSelf, TComponent}.Hermite(in TSelf, in TSelf, in TSelf, in TSelf, TComponent)"/>
        /// <param name="value1"/>
        /// <param name="tangent1"/>
        /// <param name="value2"/>
        /// <param name="tangent2"/>
        /// <param name="amount"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Hermite(in TSelf value1, in TSelf tangent1, in TSelf value2, in TSelf tangent2, TComponent amount, out TSelf result)
        {
            result = TSelf.Hermite(in value1, in tangent1, in value2, in tangent2, amount);
        }

        /// <param name="result">When the method completes, contains the 4D Cartesian coordinates of the specified point.</param>
        /// <returns/>
        /// <inheritdoc cref="IVector3{TSelf, TComponent}.Barycentric(in TSelf, in TSelf, in TSelf, TComponent, TComponent)"/>
        /// <param name="value1"/>
        /// <param name="value2"/>
        /// <param name="value3"/>
        /// <param name="amount1"/>
        /// <param name="amount2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Barycentric(in TSelf value1, in TSelf value2, in TSelf value3, TComponent amount1, TComponent amount2, out TSelf result)
        {
            result = TSelf.Barycentric(in value1, in value2, in value3, amount1, amount2);
        }

        /// <param name="result">When the method completes, contains the transformed vector.</param>
        /// <returns/>
        /// <inheritdoc cref="IVector3{TSelf, TComponent}.Transform(in TSelf, in Quaternion)"/>
        /// <param name="vector"/>
        /// <param name="rotation"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(in TSelf vector, in Quaternion rotation, out TSelf result)
        {
            result = TSelf.Transform(in vector, in rotation);
        }

        /// <param name="result">When the method completes, contains the transformed vector.</param>
        /// <returns/>
        /// <inheritdoc cref="IVector3{TSelf, TComponent}.Transform(in TSelf, in Matrix)"/>
        /// <param name="vector"/>
        /// <param name="transform"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(in TSelf vector, in Matrix transform, out TSelf result)
        {
            result = TSelf.Transform(in vector, in transform);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the XY plane (ignoring Z).
        /// </summary>
        /// <inheritdoc cref="Distance{TSelf, TResult}(in TSelf, in TSelf)"/>
        public static TComponent DistanceXY(in TSelf value1, in TSelf value2)
        {
            TSelf v1 = value1 with { Z = TComponent.Zero };
            TSelf v2 = value2 with { Z = TComponent.Zero };
            return Distance<TSelf, TComponent>(in v1, in v2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the XY plane (ignoring Z).
        /// </summary>
        /// <inheritdoc cref="Distance{TSelf, TResult}(in TSelf, in TSelf, out TResult)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceXY(in TSelf value1, in TSelf value2, out TComponent result)
        {
            result = DistanceXY<TSelf, TComponent>(in value1, in value2);
        }

        /// <summary>
        /// Calculates the squared distance between two vectors on the XY plane (ignoring Z).
        /// </summary>
        /// <inheritdoc cref="PreciseDistance{TSelf, TComponent}(in TSelf, in TSelf)"/>
        public static TComponent DistanceXYPrecise(in TSelf value1, in TSelf value2)
        {
            TSelf v1 = value1 with { Z = TComponent.Zero };
            TSelf v2 = value2 with { Z = TComponent.Zero };
            return PreciseDistance<TSelf, TComponent>(in v1, in v2);
        }

        /// <summary>
        /// Calculates the squared distance between two vectors on the XY plane (ignoring Z).
        /// </summary>
        /// <inheritdoc cref="PreciseDistance{TSelf, TComponent}(in TSelf, in TSelf, out TComponent)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceXYPrecise(in TSelf value1, in TSelf value2, out TComponent result)
        {
            result = DistanceXYPrecise<TSelf, TComponent>(in value1, in value2);
        }

        /// <summary>
        /// Calculates the squared distance between two vectors on the XY plane (ignoring Z).
        /// </summary>
        /// <inheritdoc cref="DistanceSquared{TSelf, TComponent}(in TSelf, in TSelf)"/>
        public static TComponent DistanceXYSquared(in TSelf value1, in TSelf value2)
        {
            TSelf v1 = value1 with { Z = TComponent.Zero };
            TSelf v2 = value2 with { Z = TComponent.Zero };
            return DistanceSquared<TSelf, TComponent>(in v1, in v2);
        }

        /// <summary>
        /// Calculates the squared distance between two vectors on the XY plane (ignoring Z).
        /// </summary>
        /// <inheritdoc cref="DistanceSquared{TSelf, TComponent}(in TSelf, in TSelf, out TComponent)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceXYSquared(in TSelf value1, in TSelf value2, out TComponent result)
        {
            result = DistanceXYSquared<TSelf, TComponent>(in value1, in value2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the XZ plane (ignoring Y).
        /// </summary>
        /// <inheritdoc cref="Distance{TSelf, TResult}(in TSelf, in TSelf)"/>
        public static TComponent DistanceXZ(in TSelf value1, in TSelf value2)
        {
            TSelf v1 = value1 with { Y = TComponent.Zero };
            TSelf v2 = value2 with { Y = TComponent.Zero };
            return Distance<TSelf, TComponent>(in v1, in v2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the XZ plane (ignoring Y).
        /// </summary>
        /// <inheritdoc cref="Distance{TSelf, TResult}(in TSelf, in TSelf, out TResult)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceXZ(in TSelf value1, in TSelf value2, out TComponent result)
        {
            result = DistanceXY<TSelf, TComponent>(in value1, in value2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the XZ plane (ignoring Y).
        /// </summary>
        /// <inheritdoc cref="PreciseDistance{TSelf, TComponent}(in TSelf, in TSelf)"/>
        public static TComponent DistanceXZPrecise(in TSelf value1, in TSelf value2)
        {
            TSelf v1 = value1 with { Y = TComponent.Zero };
            TSelf v2 = value2 with { Y = TComponent.Zero };
            return PreciseDistance<TSelf, TComponent>(in v1, in v2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the XZ plane (ignoring Y).
        /// </summary>
        /// <inheritdoc cref="PreciseDistance{TSelf, TComponent}(in TSelf, in TSelf, out TComponent)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceXZPrecise(in TSelf value1, in TSelf value2, out TComponent result)
        {
            result = DistanceXYPrecise<TSelf, TComponent>(in value1, in value2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the XZ plane (ignoring Y).
        /// </summary>
        /// <inheritdoc cref="DistanceSquared{TSelf, TComponent}(in TSelf, in TSelf)"/>
        public static TComponent DistanceXZSquared(in TSelf value1, in TSelf value2)
        {
            TSelf v1 = value1 with { Y = TComponent.Zero };
            TSelf v2 = value2 with { Y = TComponent.Zero };
            return DistanceSquared<TSelf, TComponent>(in v1, in v2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the XZ plane (ignoring Y).
        /// </summary>
        /// <inheritdoc cref="DistanceSquared{TSelf, TComponent}(in TSelf, in TSelf, out TComponent)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceXZSquared(in TSelf value1, in TSelf value2, out TComponent result)
        {
            result = DistanceXYSquared<TSelf, TComponent>(in value1, in value2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the YZ plane (ignoring X).
        /// </summary>
        /// <inheritdoc cref="Distance{TSelf, TResult}(in TSelf, in TSelf)"/>
        public static TComponent DistanceYZ(in TSelf value1, in TSelf value2)
        {
            TSelf v1 = value1 with { X = TComponent.Zero };
            TSelf v2 = value2 with { X = TComponent.Zero };
            return Distance<TSelf, TComponent>(in v1, in v2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the YZ plane (ignoring X).
        /// </summary>
        /// <inheritdoc cref="Distance{TSelf, TResult}(in TSelf, in TSelf, out TResult)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceYZ(in TSelf value1, in TSelf value2, out TComponent result)
        {
            result = DistanceXY<TSelf, TComponent>(in value1, in value2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the YZ plane (ignoring X).
        /// </summary>
        /// <inheritdoc cref="PreciseDistance{TSelf, TComponent}(in TSelf, in TSelf)"/>
        public static TComponent DistanceYZPrecise(in TSelf value1, in TSelf value2)
        {
            TSelf v1 = value1 with { X = TComponent.Zero };
            TSelf v2 = value2 with { X = TComponent.Zero };
            return PreciseDistance<TSelf, TComponent>(in v1, in v2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the YZ plane (ignoring X).
        /// </summary>
        /// <inheritdoc cref="PreciseDistance{TSelf, TComponent}(in TSelf, in TSelf, out TComponent)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceYZPrecise(in TSelf value1, in TSelf value2, out TComponent result)
        {
            result = DistanceXYPrecise<TSelf, TComponent>(in value1, in value2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the YZ plane (ignoring X).
        /// </summary>
        /// <inheritdoc cref="DistanceSquared{TSelf, TComponent}(in TSelf, in TSelf)"/>
        public static TComponent DistanceYZSquared(in TSelf value1, in TSelf value2)
        {
            TSelf v1 = value1 with { X = TComponent.Zero };
            TSelf v2 = value2 with { X = TComponent.Zero };
            return DistanceSquared<TSelf, TComponent>(in v1, in v2);
        }

        /// <summary>
        /// Calculates the distance between two vectors on the YZ plane (ignoring X).
        /// </summary>
        /// <inheritdoc cref="DistanceSquared{TSelf, TComponent}(in TSelf, in TSelf, out TComponent)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceYZSquared(in TSelf value1, in TSelf value2, out TComponent result)
        {
            result = DistanceXYSquared<TSelf, TComponent>(in value1, in value2);
        }
    }
}
