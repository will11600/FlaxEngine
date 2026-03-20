using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace FlaxEngine;

internal static class VectorExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector64<float> AsVector64(this Float2 int2)
    {
        return Unsafe.BitCast<Float2, Vector64<float>>(int2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float2 AsVector2(this Vector64<float> vector)
    {
        return Unsafe.BitCast<Vector64<float>, Float2>(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector64<int> AsVector64(this Int2 int2)
    {
        return Unsafe.BitCast<Int2, Vector64<int>>(int2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 AsVector2(this Vector64<int> vector)
    {
        return Unsafe.BitCast<Vector64<int>, Int2>(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<int> AsVector128Unsafe(this Int3 int3)
    {
        Unsafe.SkipInit(out Vector128<int> result);
        Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<int>, byte>(ref result), int3);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<int> AsVector128(this Int3 int3)
    {
        Vector128<int> result = default;
        Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<int>, byte>(ref result), int3);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int3 AsVector3(this Vector128<int> value)
    {
        ref byte address = ref Unsafe.As<Vector128<int>, byte>(ref value);
        return Unsafe.ReadUnaligned<Int3>(ref address);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> AsVector128Unsafe(this Float3 float3)
    {
        Unsafe.SkipInit(out Vector128<float> result);
        Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<float>, byte>(ref result), float3);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> AsVector128(this Float3 float3)
    {
        Vector128<float> result = default;
        Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<float>, byte>(ref result), float3);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float3 AsVector3(this Vector128<float> value)
    {
        ref byte address = ref Unsafe.As<Vector128<float>, byte>(ref value);
        return Unsafe.ReadUnaligned<Float3>(ref address);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector256<double> AsVector256(this Double3 double3)
    {
        Unsafe.SkipInit(out Vector256<double> result);
        Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<double>, byte>(ref result), double3);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double3 AsVector3(this Vector128<double> value)
    {
        ref byte address = ref Unsafe.As<Vector128<double>, byte>(ref value);
        return Unsafe.ReadUnaligned<Double3>(ref address);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<int> AsVector128(this Int4 int4)
    {
        return Unsafe.BitCast<Int4, Vector128<int>>(int4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 AsVector4(this Vector128<int> vector)
    {
        return Unsafe.BitCast<Vector128<int>, Int4>(vector);
    }

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
    public static Vector256<double> AsVector256(this Double4 double4)
    {
        return Unsafe.BitCast<Float4, Vector256<double>>(double4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double4 AsVector4(this Vector256<double> vector)
    {
        return Unsafe.BitCast<Vector256<double>, Float4>(vector);
    }

    extension<T>(T) where T : unmanaged, IVector<T>
    {
        [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfOutOfRange(int index, [CallerArgumentExpression(nameof(index))] string paramName = null)
        {
            if ((uint)index >= T.Count)
            {
                ThrowOutOfRangeException<T>(index, paramName);
            }
        }

        [DoesNotReturn]
        public static void ThrowOutOfRangeException(int index, [CallerArgumentExpression(nameof(index))] string paramName = null)
        {
            throw new ArgumentOutOfRangeException(paramName, T.ComponentIndexOutOfRangeMessage);
        }
    }
}
