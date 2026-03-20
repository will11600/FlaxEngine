// Copyright (c) Wojciech Figat. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace FlaxEngine;

/// <summary>
/// Represents a two dimensional mathematical vector (signed integers).
/// </summary>
[Serializable]
#if FLAX_EDITOR
[System.ComponentModel.TypeConverter(typeof(TypeConverters.Int2Converter))]
#endif
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "Implemented as extension members in VectorMath.")]
partial struct Int2 : IVector<Int2, int>, IMeasurableVector<Int2, float>, Json.ICustomValueEquals
{
    private static readonly string _formatString = "X:{0} Y:{1}";

    /// <inheritdoc/>
    public static Int2 Zero { get; } = new Int2();

    /// <summary>
    /// The X unit <see cref="Int2" /> (1, 0).
    /// </summary>
    public static Int2 UnitX { get; } = new(1, 0);

    /// <summary>
    /// The Y unit <see cref="Int2" /> (0, 1).
    /// </summary>
    public static Int2 UnitY { get; } = new(0, 1);

    /// <inheritdoc/>
    public static Int2 One { get; } = new(1, 1);

    /// <inheritdoc/>
    public static Int2 Minimum { get; } = new(int.MinValue);

    /// <inheritdoc/>
    public static Int2 Maximum { get; } = new(int.MaxValue);

    /// <summary>
    /// Initializes a new instance of the <see cref="Int2" /> struct.
    /// </summary>
    /// <returns/>
    /// <inheritdoc cref="IVector{TSelf, TComponent}.Create(TComponent)"/>
    public Int2(int value)
    {
        X = value;
        Y = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Int2" /> struct.
    /// </summary>
    /// <param name="x">Initial value for the X component of the vector.</param>
    /// <param name="y">Initial value for the Y component of the vector.</param>
    public Int2(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Int2" /> struct.
    /// </summary>
    /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
    public Int2(Int3 value)
    {
        X = value.X;
        Y = value.Y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Int2" /> struct.
    /// </summary>
    /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
    public Int2(Int4 value)
    {
        X = value.X;
        Y = value.Y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Int2" /> struct.
    /// </summary>
    /// <returns/>
    /// <inheritdoc cref="IVector{TSelf, TComponent}.Create(ReadOnlySpan{TComponent})"/>
    public Int2(ReadOnlySpan<int> values)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(values.Length, Count, nameof(values));
        X = values[0];
        Y = values[1];
    }

    /// <inheritdoc/>
    public static int Count => 2;

    /// <inheritdoc/>
    public static Int2 Create(int value) => new(value);

    /// <inheritdoc/>
    public static Int2 Create(ReadOnlySpan<int> values) => new(values);

    /// <inheritdoc/>
    public readonly bool IsZero => Vector64.All(this.AsVector64(), 0);

    /// <inheritdoc/>
    public readonly int MinValue => Mathf.Min(X, Y);

    /// <inheritdoc/>
    public readonly int MaxValue => Mathf.Max(X, Y);

    /// <inheritdoc/>
    public readonly float AvgValue
    {
        get
        {
            const float InverseCount = 1.0f / 2.0f;
            return Vector64.Sum(this.AsVector64()) * InverseCount;
        }
    }

    /// <inheritdoc/>
    public readonly int ValuesSum => Vector64.Sum(this.AsVector64());

    /// <inheritdoc/>
    public int this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            Int2.ThrowIfOutOfRange(index);
            return VectorMath.GetRef<Int2, int>(ref Unsafe.AsRef(in this), index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            Int2.ThrowIfOutOfRange(index);
            VectorMath.GetRef<Int2, int>(ref this, index) = value;
        }
    }

    /// <inheritdoc/>
    public readonly float Length
    {
        get
        {
            float lengthSqr = LengthSquared;
            return lengthSqr * MathF.ReciprocalSqrtEstimate(lengthSqr);
        }
    }

    /// <inheritdoc/>
    public readonly float PreciseLength => MathF.Sqrt(LengthSquared);

    /// <inheritdoc/>
    public readonly int LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Vector64<int> vector = this.AsVector64();
            return Vector64.Sum(vector * vector);
        }
    }

    /// <inheritdoc/>
    public readonly bool IsOne => Vector64.All(this.AsVector64(), 1);

    /// <inheritdoc/>
    public readonly int[] ToArray() => [X, Y];

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 Add(in Int2 left, in Int2 right)
    {
        Vector64<int> result = left.AsVector64() + right.AsVector64();
        return result.AsVector2();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 Subtract(in Int2 left, in Int2 right)
    {
        Vector64<int> result = left.AsVector64() - right.AsVector64();
        return result.AsVector2();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 Multiply(in Int2 left, in Int2 right)
    {
        Vector64<int> result = left.AsVector64() * right.AsVector64();
        return result.AsVector2();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 Divide(in Int2 left, in Int2 right)
    {
        Vector64<int> result = left.AsVector64() / right.AsVector64();
        return result.AsVector2();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 Modulus(in Int2 left, in Int2 right)
    {
        Vector64<int> vLeft = left.AsVector64();
        Vector64<int> vRight = right.AsVector64();
        Vector64<int> div = vLeft / vRight;
        Vector64<int> result = vLeft - div * vRight;
        return result.AsVector2();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 Negate(in Int2 value) => (-value.AsVector64()).AsVector2();

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 Clamp(in Int2 value, in Int2 min, in Int2 max)
    {
        Vector64<int> result = Vector64.Clamp(value.AsVector64(), min.AsVector64(), max.AsVector64());
        return result.AsVector2();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Dot(in Int2 left, in Int2 right) => Vector64.Dot(left.AsVector64(), right.AsVector64());

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 Max(in Int2 left, in Int2 right)
    {
        Vector64<int> result = Vector64.Max(left.AsVector64(), right.AsVector64());
        return result.AsVector2();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 Min(in Int2 left, in Int2 right)
    {
        Vector64<int> result = Vector64.Min(left.AsVector64(), right.AsVector64());
        return result.AsVector2();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int2 Abs(in Int2 value)
    {
        Vector64<int> result = Vector64.Abs(value.AsVector64());
        return result.AsVector2();
    }

    /// <inheritdoc/>
    public static float Distance(in Int2 value1, in Int2 value2)
    {
        float sqrDistance = DistanceSquared(in value1, in value2);
        return sqrDistance * MathF.ReciprocalSqrtEstimate(sqrDistance);
    }

    /// <inheritdoc/>
    public static float PreciseDistance(in Int2 value1, in Int2 value2)
    {
        float sqrDistance = DistanceSquared(in value1, in value2);
        return MathF.Sqrt(sqrDistance);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DistanceSquared(in Int2 value1, in Int2 value2)
    {
        Vector64<int> difference = value1.AsVector64() - value2.AsVector64();
        return Vector64.Sum(difference * difference);
    }

    /// <inheritdoc/>
    public override readonly string ToString()
    {
        return string.Format(CultureInfo.CurrentCulture, _formatString, X, Y);
    }

    /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
    public readonly string ToString(string format)
    {
        if (format is null)
        {
            return ToString();
        }

        string x = X.ToString(format, CultureInfo.CurrentCulture);
        string y = Y.ToString(format, CultureInfo.CurrentCulture);
        return string.Format(CultureInfo.CurrentCulture, _formatString, x, y);
    }

    /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
    public readonly string ToString(IFormatProvider formatProvider)
    {
        return string.Format(formatProvider, _formatString, X, Y);
    }

    /// <inheritdoc/>
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        if (format is null)
        {
            return ToString(formatProvider);
        }

        return string.Format(formatProvider, _formatString, X.ToString(format, formatProvider), Y.ToString(format, formatProvider));
    }

    /// <inheritdoc/>
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in Int2 other) => X == other.X && Y == other.Y;

    /// <inheritdoc/>
    public override readonly bool Equals(object value) => value is Int2 other && Equals(in other);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int2" /> to <see cref="Int3" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Int3(Int2 value) => new(value, 0);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int2" /> to <see cref="Int4" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Int4(Int2 value) => new(value, 0, 0);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int2" /> to <see cref="Float2" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Float2(Int2 value) => new(value.X, value.Y);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int2" /> to <see cref="Float3" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Float3(Int2 value) => new(value.X, value.Y, 0.0f);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int2" /> to <see cref="Float4" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Float4(Int2 value) => new(value.X, value.Y, 0.0f, 0.0f);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int2" /> to <see cref="Vector2" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Vector2(Int2 value) => new(value.X, value.Y);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int2" /> to <see cref="Vector3" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Vector3(Int2 value) => new(value.X, value.Y, 0.0f);

    readonly bool Json.ICustomValueEquals.ValueEquals(object other) => Equals(other);
}
