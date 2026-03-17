// Copyright (c) Wojciech Figat. All rights reserved.

// -----------------------------------------------------------------------------
// Original code from SharpDX project. https://github.com/sharpdx/SharpDX/
// Greetings to Alexandre Mutel. Original code published with the following license:
// -----------------------------------------------------------------------------
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// -----------------------------------------------------------------------------
// Original code from SlimMath project. http://code.google.com/p/slimmath/
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using FlaxEngine.Json;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace FlaxEngine;

[Serializable]
#if FLAX_EDITOR
[System.ComponentModel.TypeConverter(typeof(TypeConverters.Float4Converter))]
#endif
partial struct Float4 : IVector4<Float4, float>, ICustomValueEquals
{
    private static readonly string _formatString = "X:{0:F2} Y:{1:F2} Z:{2:F2} W:{3:F2}";

    /// <summary>
    /// A <see cref="Float4" /> with all of its components set to zero.
    /// </summary>
    public static Float4 Zero { get; } = new Float4();

    /// <summary>
    /// The X unit <see cref="Float4" /> (1, 0, 0, 0).
    /// </summary>
    public static readonly Float4 UnitX = new(1.0f, 0.0f, 0.0f, 0.0f);

    /// <summary>
    /// The Y unit <see cref="Float4" /> (0, 1, 0, 0).
    /// </summary>
    public static readonly Float4 UnitY = new(0.0f, 1.0f, 0.0f, 0.0f);

    /// <summary>
    /// The Z unit <see cref="Float4" /> (0, 0, 1, 0).
    /// </summary>
    public static readonly Float4 UnitZ = new(0.0f, 0.0f, 1.0f, 0.0f);

    /// <summary>
    /// The W unit <see cref="Float4" /> (0, 0, 0, 1).
    /// </summary>
    public static readonly Float4 UnitW = new(0.0f, 0.0f, 0.0f, 1.0f);

    /// <summary>
    /// A <see cref="Float4" /> with all of its components set to half.
    /// </summary>
    public static Float4 Half { get; } = new(0.5f, 0.5f, 0.5f, 0.5f);

    /// <summary>
    /// A <see cref="Float4" /> with all of its components set to one.
    /// </summary>
    public static Float4 One { get; } = new(1.0f, 1.0f, 1.0f, 1.0f);

    /// <summary>
    /// A <see cref="Float4" /> with all components equal to <see cref="float.MinValue"/>.
    /// </summary>
    public static Float4 Minimum { get; } = new(float.MinValue);

    /// <summary>
    /// A <see cref="Float4" /> with all components equal to <see cref="float.MaxValue"/>.
    /// </summary>
    public static Float4 Maximum { get; } = new(float.MaxValue);

    /// <summary>
    /// Initializes a new instance of the <see cref="Float4" /> struct.
    /// </summary>
    /// <param name="value">The value that will be assigned to all components.</param>
    public Float4(float value)
    {
        X = value;
        Y = value;
        Z = value;
        W = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Float4" /> struct.
    /// </summary>
    /// <param name="x">Initial value for the X component of the vector.</param>
    /// <param name="y">Initial value for the Y component of the vector.</param>
    /// <param name="z">Initial value for the Z component of the vector.</param>
    /// <param name="w">Initial value for the W component of the vector.</param>
    public Float4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Float4" /> struct.
    /// </summary>
    /// <param name="value">A vector containing the values with which to initialize the X, Y, and Z components.</param>
    /// <param name="w">Initial value for the W component of the vector.</param>
    public Float4(Float3 value, float w)
    {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Float4" /> struct.
    /// </summary>
    /// <param name="xy">A vector containing the values with which to initialize the X and Y components.</param>
    /// <param name="zw">A vector containing the values with which to initialize the Z and W components.</param>
    public Float4(Float4 xy, Float4 zw)
    {
        X = xy.X;
        Y = xy.Y;
        Z = zw.X;
        W = zw.Y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Float4" /> struct.
    /// </summary>
    /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
    /// <param name="z">Initial value for the Z component of the vector.</param>
    /// <param name="w">Initial value for the W component of the vector.</param>
    public Float4(Float2 value, float z, float w)
    {
        X = value.X;
        Y = value.Y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Float4" /> struct.
    /// </summary>
    /// <param name="values">The span of values to assign to the X, Y, Z, and W components. Must contain exactly four elements.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values" /> does not contain exactly four elements.</exception>
    public Float4(ReadOnlySpan<float> values)
    {
        if (values.Length != 4)
        {
            throw new ArgumentOutOfRangeException(nameof(values), "There must be four and only four input values for Float4.");
        }

        X = values[0];
        Y = values[1];
        Z = values[2];
        W = values[3];
    }

    /// <summary>
    /// Gets a value indicating whether this instance is normalized.
    /// </summary>
    /// <remarks>
    /// This property checks if the squared length of the vector is within a small epsilon of 1.0.
    /// </remarks>
    public readonly bool IsNormalized => MathF.Abs(LengthSquared - 1.0f) < 1e-4f;

    /// <summary>
    /// Gets a value indicating whether this vector is zero (0, 0, 0, 0).
    /// </summary>
    public readonly bool IsZero => this.AsVector128() == Vector128<float>.Zero;

    /// <summary>
    /// Gets a value indicating whether this vector is one (1, 1, 1, 1).
    /// </summary>
    public readonly bool IsOne => this.AsVector128() == Vector128.Create(1.0f);

    /// <summary>
    /// Gets a minimum component value
    /// </summary>
    public readonly float MinValue => Mathf.Min(X, Mathf.Min(Y, Mathf.Min(Z, W)));

    /// <summary>
    /// Gets a maximum component value
    /// </summary>
    public readonly float MaxValue => Mathf.Max(X, Mathf.Max(Y, Mathf.Max(Z, W)));


    /// <summary>
    /// Gets an arithmetic average value of all vector components.
    /// </summary>
    public readonly float AvgValue
    {
        get
        {
            const float OneOverFour = 1.0f / 4.0f;
            return ValuesSum * OneOverFour;
        }
    }

    /// <summary>
    /// Gets the sum of all vector components (X + Y + Z + W).
    /// </summary>
    public readonly float ValuesSum => Vector128.Sum(this.AsVector128());

    /// <summary>
    /// Gets a vector with values being absolute values of that vector.
    /// </summary>
    public readonly Float4 Absolute => Vector128.Abs(this.AsVector128()).AsVector4();

    /// <summary>
    /// Gets a vector with values being opposite to values of that vector.
    /// </summary>
    public readonly Float4 Negative => Negate(this);

    /// <summary>
    /// Gets or sets the component at the specified index.
    /// </summary>
    /// <value>The value of the X, Y, Z, or W component, depending on the index.</value>
    /// <param name="index">The index of the component to access. Use 0 for the X component, 1 for the Y component, 2 for the Z component, and 3 for the W component.</param>
    /// <returns>The value of the component at the specified index.</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index" /> is out of the range [0,3].</exception>
    public float this[int index]
    {
        readonly get => index switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            3 => W,
            _ => throw new ArgumentOutOfRangeException(nameof(index), "Indices for Float4 run from 0 to 3, inclusive."),
        };
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                case 2: Z = value; break;
                case 3: W = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(index), "Indices for Float4 run from 0 to 3, inclusive.");
            }
        }
    }

    /// <inheritdoc/>
    public readonly float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            float lengthSquared = LengthSquared;
            return lengthSquared * MathF.ReciprocalSqrtEstimate(lengthSquared);
        }
    }

    /// <inheritdoc/>
    public readonly float PreciseLength
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            float lengthSquared = LengthSquared;
            return lengthSquared / MathF.Sqrt(lengthSquared);
        }
    }

    /// <inheritdoc/>
    public readonly float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Vector128<float> vValue = this.AsVector128();
            return Vector128.Sum(vValue * vValue);
        }
    }

    /// <inheritdoc/>
    public static int Count => 4;

    /// <inheritdoc/>
    public static Float4 Create(float value) => new(value);

    /// <inheritdoc/>
    public static Float4 Create(ReadOnlySpan<float> values) => new(values);

    /// <inheritdoc/>
    public void Normalize() => this = Multiply(in this, new Float4(MathF.ReciprocalSqrtEstimate(LengthSquared)));

    /// <inheritdoc/>
    public void NormalizePrecise() => this = Multiply(in this, new Float4(1.0f / MathF.Sqrt(LengthSquared)));

    /// <inheritdoc/>
    public readonly float[] ToArray() => [X, Y, Z, W];

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float4 Add(in Float4 left, in Float4 right)
    {
        return (left.AsVector128() + right.AsVector128()).AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float4 Subtract(in Float4 left, in Float4 right)
    {
        return (left.AsVector128() - right.AsVector128()).AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float4 Multiply(in Float4 left, in Float4 right)
    {
        return (left.AsVector128() * right.AsVector128()).AsVector4();
    }

    /// <inheritdoc/>
    public static Float4 Divide(in Float4 left, in Float4 right)
    {
        return (left.AsVector128() / right.AsVector128()).AsVector4();
    }

    /// <inheritdoc/>
    public static Float4 Modulus(in Float4 left, in Float4 right)
    {
        Vector128<float> vLeft = left.AsVector128();
        Vector128<float> vRight = right.AsVector128();

        Vector128<float> div = vLeft / vRight;
        Vector128<float> trunc = Vector128.Truncate(div);
        Vector128<float> result = vLeft - (vRight * trunc);

        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float4 Negate(in Float4 value)
    {
        Vector128<float> vResult = Vector128.Negate(value.AsVector128());
        return vResult.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float4 Barycentric(in Float4 value1, in Float4 value2, in Float4 value3, float amount1, float amount2)
    {
        Vector128<float> result = VectorMath.Barycentric(value1.AsVector128(), value2.AsVector128(), value3.AsVector128(), amount1, amount2);
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float4 Clamp(in Float4 value, in Float4 min, in Float4 max)
    {
        var result = Vector128.Clamp(value.AsVector128(), min.AsVector128(), max.AsVector128());
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(in Float4 value1, in Float4 value2)
    {
        float sqrDistance = DistanceSquared(value1, value2);
        return sqrDistance * MathF.ReciprocalSqrtEstimate(sqrDistance);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float PreciseDistance(in Float4 value1, in Float4 value2)
    {
        float sqrDistance = DistanceSquared(value1, value2);
        return MathF.Sqrt(sqrDistance);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(in Float4 value1, in Float4 value2)
    {
        Vector128<float> difference = value1.AsVector128() - value2.AsVector128();
        return Vector128.Sum(difference * difference);
    }

    /// <inheritdoc/>
    public static Float4 Hermite(in Float4 value1, in Float4 tangent1, in Float4 value2, in Float4 tangent2, float amount)
    {
        Vector128<float> result = VectorMath.Hermite(value1.AsVector128(), tangent1.AsVector128(), value2.AsVector128(), tangent2.AsVector128(), amount);
        return result.AsVector4();
    }

    /// <summary>
    /// Performs a Catmull-Rom interpolation using the specified positions.
    /// </summary>
    /// <param name="value1">The first position in the interpolation.</param>
    /// <param name="value2">The second position in the interpolation.</param>
    /// <param name="value3">The third position in the interpolation.</param>
    /// <param name="value4">The fourth position in the interpolation.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <returns>A vector that is the result of the Catmull-Rom interpolation.</returns>
    public static Float4 CatmullRom(in Float4 value1, in Float4 value2, in Float4 value3, in Float4 value4, float amount)
    {
        Vector128<float> result = VectorMath.CatmullRom(value1.AsVector128(), value2.AsVector128(), value3.AsVector128(), value3.AsVector128(), amount);
        return result.AsVector4();
    }

    /// <inheritdoc/>
    public static Float4 Abs(in Float4 value)
    {
        Vector128<float> result = Vector128.Abs(value.AsVector128());
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float4 Transform(in Float4 vector, in Quaternion rotation)
    {
        Vector128<float> result = VectorMath.Transform(vector.AsVector128(), in rotation);
        return result.AsVector4();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Float4 Transform(in Float4 vector, in Matrix transform)
    {
        Vector128<float> result = VectorMath.Transform(vector.AsVector128(), in transform);
        return result.AsVector4();
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="Float4" /> to <see cref="Vector4" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Vector4(Float4 value)
    {
        return new Vector4(value.X, value.Y, value.Z, value.W);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="Float4" /> to <see cref="Double4" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Double4(Float4 value)
    {
        return new Double4(value.X, value.Y, value.Z, value.W);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="Float4" /> to <see cref="Float2" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Float2(Float4 value)
    {
        return new Float2(value.X, value.Y);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="Float4" /> to <see cref="Float3" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Float3(Float4 value)
    {
        return new Float3(value.X, value.Y, value.Z);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="Float4" /> to <see cref="Int4" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Int4(Float4 value)
    {
        return new Int4((int)value.X, (int)value.Y, (int)value.Z, (int)value.W);
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override readonly string ToString()
    {
        return string.Format(CultureInfo.CurrentCulture, _formatString, X, Y, Z, W);
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public readonly string ToString(string format)
    {
        if (format is null)
        {
            return ToString();
        }

        return string.Format(CultureInfo.CurrentCulture, _formatString, X.ToString(format, CultureInfo.CurrentCulture), Y.ToString(format, CultureInfo.CurrentCulture), Z.ToString(format, CultureInfo.CurrentCulture), W.ToString(format, CultureInfo.CurrentCulture));
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public readonly string ToString(IFormatProvider formatProvider)
    {
        return string.Format(formatProvider, _formatString, X, Y, Z, W);
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        if (format is null)
        {
            return ToString(formatProvider);
        }

        return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", X.ToString(format, formatProvider), Y.ToString(format, formatProvider), Z.ToString(format, formatProvider), W.ToString(format, formatProvider));
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in Float4 other)
    {
        return this.AsVector128().Equals(other.AsVector128());
    }

    /// <inheritdoc/>
    public override readonly bool Equals(object value)
    {
        return value is Float4 other && Equals(in other);
    }

    /// <inheritdoc/>
    public static bool NearEqual(in Float4 left, in Float4 right, float tolerance)
    {
        Vector128<float> difference = Vector128.Abs(left.AsVector128() - right.AsVector128());
        return Vector128.LessThanOrEqualAll(difference, Vector128.Create(tolerance));
    }

    /// <inheritdoc/>
    public static Float4 ClampLength(in Float4 vector, float min, float max)
    {
        Vector128<float> vVector = vector.AsVector128();
        float lenSq = Vector128.Sum(vVector * vVector);
        if (lenSq > max * max)
        {
            Vector128<float> scaleFactor = Vector128.Create(max * MathF.ReciprocalSqrtEstimate(lenSq));
            return VectorMath.AsVector4(scaleFactor * vVector);
        }
        if (lenSq < min * min)
        {
            Vector128<float> scaleFactor = Vector128.Create(min * MathF.ReciprocalSqrtEstimate(lenSq));
            return VectorMath.AsVector4(scaleFactor * vVector);
        }
        return vector;
    }

    /// <inheritdoc/>
    public static Float4 Lerp(in Float4 start, in Float4 end, float amount)
    {
        Vector128<float> vStart = start.AsVector128();
        Vector128<float> vAmount = Vector128.Create(amount);
        Vector128<float> result = vStart + (end.AsVector128() - vStart) * vAmount;
        return result.AsVector4();
    }

    /// <inheritdoc/>
    public static Float4 SmoothStep(in Float4 start, in Float4 end, float amount) => Lerp(in start, in end, Mathf.SmoothStep(amount));

    /// <inheritdoc/>
    public static float Dot(in Float4 left, in Float4 right) => Vector128.Dot(left.AsVector128(), right.AsVector128());

    /// <inheritdoc/>
    public static Float4 Max(in Float4 left, in Float4 right)
    {
        return Vector128.Max(left.AsVector128(), right.AsVector128()).AsVector4();
    }

    /// <inheritdoc/>
    public static Float4 Min(in Float4 left, in Float4 right)
    {
        return Vector128.Min(left.AsVector128(), right.AsVector128()).AsVector4();
    }

    readonly bool ICustomValueEquals.ValueEquals(object other) => Equals(other);
}
