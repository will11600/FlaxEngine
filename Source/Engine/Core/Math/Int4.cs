// Copyright (c) Wojciech Figat. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace FlaxEngine;

/// <summary>
/// Represents a four dimensional mathematical vector (signed integers).
/// </summary>
[Serializable]
#if FLAX_EDITOR
[System.ComponentModel.TypeConverter(typeof(TypeConverters.Int4Converter))]
#endif
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "Implemented as extension members in VectorMath.")]
partial struct Int4 : IVector<Int4, int>, IMeasurableVector<Int4, float>, Json.ICustomValueEquals
{
    private static readonly string _formatString = "X:{0} Y:{1} Z:{2} W:{3}";

    /// <inheritdoc/>
    public static Int4 Zero { get; } = new();

    /// <summary>
    /// The X unit <see cref="Int4" /> (1, 0, 0, 0).
    /// </summary>
    public static Int4 UnitX { get; } = new(1, 0, 0, 0);

    /// <summary>
    /// The Y unit <see cref="Int4" /> (0, 1, 0, 0).
    /// </summary>
    public static Int4 UnitY { get; } = new(0, 1, 0, 0);

    /// <summary>
    /// The Z unit <see cref="Int4" /> (0, 0, 1, 0).
    /// </summary>
    public static Int4 UnitZ { get; } = new(0, 0, 1, 0);

    /// <summary>
    /// The W unit <see cref="Int4" /> (0, 0, 0, 1).
    /// </summary>
    public static Int4 UnitW { get; } = new(0, 0, 0, 1);

    /// <inheritdoc/>
    public static Int4 One { get; } = new(1, 1, 1, 1);

    /// <inheritdoc/>
    public static Int4 Minimum { get; } = new(int.MinValue);

    /// <inheritdoc/>
    public static Int4 Maximum { get; } = new(int.MaxValue);

    /// <summary>
    /// Initializes a new instance of the <see cref="Int4" /> struct.
    /// </summary>
    /// <returns/>
    /// <inheritdoc cref="Create(int)"/>
    public Int4(int value)
    {
        X = value;
        Y = value;
        Z = value;
        W = value;
    }

    /// <param name="x">Initial value for the X component of the vector.</param>
    /// <param name="y">Initial value for the Y component of the vector.</param>
    /// <param name="z">Initial value for the Z component of the vector.</param>
    /// <param name="w">Initial value for the W component of the vector.</param>
    /// <inheritdoc cref="Int4(int)"/>
    public Int4(int x, int y, int z, int w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <param name="value">A vector containing the values with which to initialize the X, Y, and Z components.</param>
    /// <inheritdoc cref="Int4(int, int, int, int)"/>
    /// <param name="w"/>
    public Int4(Int3 value, int w)
    {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
        W = w;
    }

    /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
    /// <inheritdoc cref="Int4(int, int, int, int)"/>
    /// <param name="z"/>
    /// <param name="w"/>
    public Int4(Int2 value, int z, int w)
    {
        X = value.X;
        Y = value.Y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Int4" /> struct.
    /// </summary>
    /// <returns/>
    /// <inheritdoc cref="Create(ReadOnlySpan{int})"/>
    public Int4(ReadOnlySpan<int> values)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(values.Length, Count, nameof(values));
        X = values[0];
        Y = values[1];
        Z = values[2];
        W = values[3];
    }

    /// <inheritdoc/>
    public static int Count => 4;

    /// <inheritdoc/>
    public static Int4 Create(int value) => new(value);

    /// <inheritdoc/>
    public static Int4 Create(ReadOnlySpan<int> values) => new(values);

    /// <inheritdoc/>
    public readonly bool IsZero => Vector128.All(this.AsVector128(), 0);

    /// <inheritdoc/>
    public readonly bool IsOne => Vector128.All(this.AsVector128(), 1);

    /// <summary>
    /// Gets a minimum component value
    /// </summary>
    public readonly int MinValue => Mathf.Min(X, Mathf.Min(Y, Mathf.Min(Z, W)));

    /// <summary>
    /// Gets a maximum component value
    /// </summary>
    public readonly int MaxValue => Mathf.Max(X, Mathf.Max(Y, Mathf.Max(Z, W)));

    /// <summary>
    /// Gets a sum of the component values.
    /// </summary>
    public readonly int ValuesSum => X + Y + Z + W;

    /// <inheritdoc/>
    public int this[int index]
    {
        readonly get => index switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            3 => W,
            _ => throw new ArgumentOutOfRangeException(nameof(index), "Indices for Int4 run from 0 to 3, inclusive."),
        };
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                case 2: Z = value; break;
                case 3: W = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(index), "Indices for Int4 run from 0 to 3, inclusive.");
            }
        }
    }

    /// <inheritdoc/>
    public readonly float Length
    {
        get
        {
            float lengthSquared = LengthSquared;
            return lengthSquared * MathF.ReciprocalSqrtEstimate(lengthSquared);
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
            Vector128<int> vector = this.AsVector128();
            return Vector128.Sum(vector * vector);
        }
    }

    /// <inheritdoc/>
    public readonly float AvgValue
    {
        get
        {
            const float InverseCount = 1.0f / 4.0f;
            return Vector128.Sum(this.AsVector128()) * InverseCount;
        }
    }

    /// <inheritdoc/>
    public readonly int[] ToArray() => [X, Y, Z, W];

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 Add(in Int4 left, in Int4 right)
    {
        Vector128<int> result = left.AsVector128() + right.AsVector128();
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 Subtract(in Int4 left, in Int4 right)
    {
        Vector128<int> result = left.AsVector128() - right.AsVector128();
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 Multiply(in Int4 left, in Int4 right)
    {
        Vector128<int> result = left.AsVector128() * right.AsVector128();
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 Divide(in Int4 left, in Int4 right)
    {
        Vector128<int> result = left.AsVector128() / right.AsVector128();
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 Modulus(in Int4 left, in Int4 right)
    {
        Vector128<int> vLeft = left.AsVector128();
        Vector128<int> vRight = right.AsVector128();
        Vector128<int> div = vLeft / vRight;
        Vector128<int> result = vLeft - div * vRight;
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 Negate(in Int4 value)
    {
        Vector128<int> result = -value.AsVector128();
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 Clamp(in Int4 value, in Int4 min, in Int4 max)
    {
        Vector128<int> result = Vector128.Clamp(value.AsVector128(), min.AsVector128(), max.AsVector128());
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(in Int4 value1, in Int4 value2)
    {
        float sqrDistance = DistanceSquared(in value1, in value2);
        return sqrDistance * MathF.ReciprocalSqrtEstimate(sqrDistance);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float PreciseDistance(in Int4 value1, in Int4 value2)
    {
        float sqrDistance = DistanceSquared(in value1, in value2);
        return MathF.Sqrt(sqrDistance);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DistanceSquared(in Int4 value1, in Int4 value2)
    {
        Vector128<int> difference = value1.AsVector128() - value2.AsVector128();
        return Vector128.Sum(difference * difference);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Dot(in Int4 left, in Int4 right) => Vector128.Dot(left.AsVector128(), right.AsVector128());

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 Max(in Int4 left, in Int4 right)
    {
        Vector128<int> result = Vector128.Max(left.AsVector128(), right.AsVector128());
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 Min(in Int4 left, in Int4 right)
    {
        Vector128<int> result = Vector128.Min(left.AsVector128(), right.AsVector128());
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int4 Abs(in Int4 value)
    {
        Vector128<int> result = Vector128.Abs(value.AsVector128());
        return result.AsVector4();
    }

    /// <inheritdoc/>
    public override readonly string ToString()
    {
        return string.Format(CultureInfo.CurrentCulture, _formatString, X, Y, Z, W);
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
        string z = Z.ToString(format, CultureInfo.CurrentCulture);
        string w = W.ToString(format, CultureInfo.CurrentCulture);
        return string.Format(CultureInfo.CurrentCulture, _formatString, x, y, z, w);
    }

    /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
    public readonly string ToString(IFormatProvider formatProvider)
    {
        return string.Format(formatProvider, _formatString, X, Y, Z, W);
    }

    /// <inheritdoc/>
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        if (format is null)
        {
            return ToString(formatProvider);
        }

        string x = X.ToString(format, formatProvider);
        string y = Y.ToString(format, formatProvider);
        string z = Z.ToString(format, formatProvider);
        string w = W.ToString(format, formatProvider);
        return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", x, y, z, w);
    }

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in Int4 other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

    /// <inheritdoc/>
    public override readonly bool Equals(object value) => value is Int4 other && Equals(in other);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int4" /> to <see cref="Int2" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Int2(Int4 value) => new(value.X, value.Y);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int4" /> to <see cref="Int3" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Int3(Int4 value) => new(value.X, value.Y, value.Z);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int4" /> to <see cref="Float2" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Float2(Int4 value) => new(value.X, value.Y);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int4" /> to <see cref="Float3" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Float3(Int4 value) => new(value.X, value.Y, value.Z);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int4" /> to <see cref="Float4" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Float4(Int4 value) => new(value.X, value.Y, value.Z, value.W);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int4" /> to <see cref="Vector2" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Vector2(Int4 value) => new(value.X, value.Y);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int4" /> to <see cref="Vector3" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Vector3(Int4 value) => new(value.X, value.Y, value.Z);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Int4" /> to <see cref="Vector4" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Vector4(Int4 value) => new(value.X, value.Y, value.Z, value.W);

    readonly bool Json.ICustomValueEquals.ValueEquals(object other) => Equals(other);
}
