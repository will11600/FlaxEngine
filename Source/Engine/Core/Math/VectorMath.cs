using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace FlaxEngine;

internal static class VectorMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> AsVector128(this Float4 float4)
    {
        return Unsafe.BitCast<Float4, Vector128<float>>(float4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float4 AsVector4(this Vector128<float> vector)
    {
        return Unsafe.BitCast<Vector128<float>, Float4>(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Vector128<float> AsVector128(ref Float4 float4)
    {
        return ref Unsafe.As<Float4, Vector128<float>>(ref float4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Float4 AsVector4(ref Vector128<float> vector)
    {
        return ref Unsafe.As<Vector128<float>, Float4>(ref vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector256<double> AsVector256(this Double4 double4)
    {
        return Unsafe.BitCast<Float4, Vector256<double>>(double4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double4 AsVector4(this Vector256<double> vector)
    {
        return Unsafe.BitCast<Vector256<double>, Float4>(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Vector256<double> AsVector256(ref Double4 double4)
    {
        return ref Unsafe.As<Double4, Vector256<double>>(ref double4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Double4 AsVector4(ref Vector256<double> vector)
    {
        return ref Unsafe.As<Vector256<double>, Double4>(ref vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector256<double> Widen(this Vector128<float> value)
    {
        (Vector128<double> lower, Vector128<double> upper) = Vector128.Widen(value);
        return Vector256.Create(lower, upper);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Narrow(this Vector256<double> value)
    {
        Vector256<float> narrowed = Vector256.Narrow(value, Vector256<double>.Zero);
        return narrowed.GetLower();
    }

    public static Vector<T> CatmullRom<T>(in Vector<T> value1, in Vector<T> value2, in Vector<T> value3, in Vector<T> value4, T amount)
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
    public static Vector128<T> CatmullRom<T>(in Vector128<T> value1, in Vector128<T> value2, in Vector128<T> value3, in Vector128<T> value4, T amount)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = CatmullRom(value1.AsVector(), value2.AsVector(), value3.AsVector(), value4.AsVector(), amount);
        return result.AsVector128();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector256<T> CatmullRom<T>(in Vector256<T> value1, in Vector256<T> value2, in Vector256<T> value3, in Vector256<T> value4, T amount)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = CatmullRom(value1.AsVector(), value2.AsVector(), value3.AsVector(), value4.AsVector(), amount);
        return result.AsVector256();
    }

    public static Vector<T> Hermite<T>(in Vector<T> value1, in Vector<T> tangent1, in Vector<T> value2, in Vector<T> tangent2, T amount)
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
    public static Vector128<T> Hermite<T>(in Vector128<T> value1, in Vector128<T> tangent1, in Vector128<T> value2, in Vector128<T> tangent2, T amount)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = Hermite(value1.AsVector(), tangent1.AsVector(), value2.AsVector(), tangent2.AsVector(), amount);
        return result.AsVector128();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector256<T> Hermite<T>(in Vector256<T> value1, in Vector256<T> tangent1, in Vector256<T> value2, in Vector256<T> tangent2, T amount)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = Hermite(value1.AsVector(), tangent1.AsVector(), value2.AsVector(), tangent2.AsVector(), amount);
        return result.AsVector256();
    }

    public static Vector<T> Barycentric<T>(in Vector<T> value1, in Vector<T> value2, in Vector<T> value3, T amount1, T amount2)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> a1 = Vector.Create(amount1);
        Vector<T> a2 = Vector.Create(amount2);
        return value1 + (a1 * (value2 - value1)) + (a2 * (value3 - value1));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<T> Barycentric<T>(in Vector128<T> value1, in Vector128<T> value2, in Vector128<T> value3, T amount1, T amount2)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = Barycentric(value1.AsVector(), value2.AsVector(), value3.AsVector(), amount1, amount2);
        return result.AsVector128();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector256<T> Barycentric<T>(in Vector256<T> value1, in Vector256<T> value2, in Vector256<T> value3, T amount1, T amount2)
        where T : struct, IFloatingPoint<T>
    {
        Vector<T> result = Barycentric(value1.AsVector(), value2.AsVector(), value3.AsVector(), amount1, amount2);
        return result.AsVector256();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Transform(in Vector128<float> vector, in Quaternion rotation)
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
    public static Vector128<float> Transform(in Vector128<float> vector, in Matrix transform)
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
}
