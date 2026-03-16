using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace FlaxEngine;

internal static class VectorExtensions
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
}
