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

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace FlaxEngine;

[Serializable]
#if FLAX_EDITOR
[System.ComponentModel.TypeConverter(typeof(TypeConverters.Float4Converter))]
#endif
partial struct Float4 : IEquatable<Float4>, IFormattable, Json.ICustomValueEquals
{
    private static readonly string _formatString = "X:{0:F2} Y:{1:F2} Z:{2:F2} W:{3:F2}";

    /// <summary>
    /// The size of the <see cref="Float4" /> type, in bytes.
    /// </summary>
    public static unsafe readonly int SizeInBytes = sizeof(Float4);

    /// <summary>
    /// A <see cref="Float4" /> with all of its components set to zero.
    /// </summary>
    public static readonly Float4 Zero;

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
    public static readonly Float4 Half = new(0.5f, 0.5f, 0.5f, 0.5f);

    /// <summary>
    /// A <see cref="Float4" /> with all of its components set to one.
    /// </summary>
    public static readonly Float4 One = new(1.0f, 1.0f, 1.0f, 1.0f);

    /// <summary>
    /// A <see cref="Float4" /> with all components equal to <see cref="float.MinValue"/>.
    /// </summary>
    public static readonly Float4 Minimum = new(float.MinValue);

    /// <summary>
    /// A <see cref="Float4" /> with all components equal to <see cref="float.MaxValue"/>.
    /// </summary>
    public static readonly Float4 Maximum = new(float.MaxValue);

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
    public readonly bool IsNormalized => IsNormalizedWithLength(out _);

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

    /// <remarks>
    /// Uses a fast approximation for the inverse square root, so the result may not be precise. 
    /// For a more accurate result, use <see cref="Float4.PreciseLength" />.
    /// </remarks>
    /// <inheritdoc cref="PreciseLength" />
    public readonly float Length => IsNormalizedWithLength(out float lengthSquared) ? 1.0f : MathF.ReciprocalSqrtEstimate(lengthSquared);

    /// <summary>
    /// Calculates the length of the vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    public readonly float PreciseLength => IsNormalizedWithLength(out float lengthSquared) ? 1.0f : MathF.Sqrt(lengthSquared);

    /// <summary>
    /// Calculates the squared length of the vector.
    /// </summary>
    /// <returns>The squared length of the vector.</returns>
    /// <remarks>
    /// This method may be preferred to <see cref="Float4.Length" /> when only a relative 
    /// length is needed and speed is of the essence.
    /// </remarks>
    public readonly float LengthSquared
    {
        get
        {
            Vector128<float> vValue = this.AsVector128();
            return Vector128.Sum(vValue * vValue);
        }
    }

    /// <remarks>
    /// Uses a fast approximation for the inverse square root, so the result may not be precise. 
    /// For a more accurate result, use <see cref="Float4.NormalizePrecise" />.
    /// </remarks>
    /// <inheritdoc cref="Float4.NormalizePrecise" />
    public void Normalize()
    {
        if (IsNormalizedWithLength(out float lengthSquared))
        {
            return;
        }

        float inv = MathF.ReciprocalSqrtEstimate(lengthSquared);
        Vector128<float> vInv = Vector128.Create(inv);
        this = (this.AsVector128() * vInv).AsVector4();
    }

    /// <summary>
    /// Converts the vector into a unit vector with a length of 1.
    /// </summary>
    public void NormalizePrecise()
    {
        if (IsNormalizedWithLength(out float lengthSquared))
        {
            return;
        }

        float inv = 1.0f / MathF.Sqrt(lengthSquared);
        Vector128<float> vInv = Vector128.Create(inv);
        this = (this.AsVector128() * vInv).AsVector4();
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private readonly bool IsNormalizedWithLength(out float lengthSquared)
    {
        lengthSquared = LengthSquared;
        return MathF.Abs(lengthSquared - 1.0f) < 1e-4f;
    }

    /// <summary>
    /// Creates an array containing the elements of the vector.
    /// </summary>
    /// <returns>A four-element array containing the components of the vector.</returns>
    public readonly float[] ToArray()
    {
        return [X, Y, Z, W];
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <param name="result">When the method completes, contains the sum of the two vectors.</param>
    public static void Add(ref Float4 left, ref Float4 right, out Float4 result)
    {
        ref Vector128<float> vLeft = ref VectorExtensions.AsVector128(ref left);
        ref Vector128<float> vRight = ref VectorExtensions.AsVector128(ref right);
        Vector128<float> vResult = vLeft + vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>The sum of the two vectors.</returns>
    public static Float4 Add(Float4 left, Float4 right)
    {
        return (left.AsVector128() + right.AsVector128()).AsVector4();
    }

    /// <summary>
    /// Performs a component-wise addition.
    /// </summary>
    /// <param name="left">The input vector</param>
    /// <param name="right">The scalar value to be added to elements</param>
    /// <param name="result">The vector with added scalar for each element.</param>
    public static void Add(ref Float4 left, ref float right, out Float4 result)
    {
        ref Vector128<float> vLeft = ref VectorExtensions.AsVector128(ref left);
        Vector128<float> vRight = Vector128.Create(right);
        Vector128<float> vResult = vLeft + vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Performs a component-wise addition.
    /// </summary>
    /// <param name="left">The input vector</param>
    /// <param name="right">The scalar value to be added to elements</param>
    /// <returns>The vector with added scalar for each element.</returns>
    public static Float4 Add(Float4 left, float right)
    {
        return (left.AsVector128() + Vector128.Create(right)).AsVector4();
    }

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    /// <param name="left">The first vector to subtract.</param>
    /// <param name="right">The second vector to subtract.</param>
    /// <param name="result">When the method completes, contains the difference of the two vectors.</param>
    public static void Subtract(ref Float4 left, ref Float4 right, out Float4 result)
    {
        ref Vector128<float> vLeft = ref VectorExtensions.AsVector128(ref left);
        ref Vector128<float> vRight = ref VectorExtensions.AsVector128(ref right);
        Vector128<float> vResult = vLeft - vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    /// <param name="left">The first vector to subtract.</param>
    /// <param name="right">The second vector to subtract.</param>
    /// <returns>The difference of the two vectors.</returns>
    public static Float4 Subtract(Float4 left, Float4 right)
    {
        return (left.AsVector128() - right.AsVector128()).AsVector4();
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="left">The input vector</param>
    /// <param name="right">The scalar value to be subtracted from elements</param>
    /// <param name="result">The vector with subtracted scalar for each element.</param>
    public static void Subtract(ref Float4 left, ref float right, out Float4 result)
    {
        ref Vector128<float> vLeft = ref VectorExtensions.AsVector128(ref left);
        Vector128<float> vRight = Vector128.Create(right);
        Vector128<float> vResult = vLeft - vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="left">The input vector</param>
    /// <param name="right">The scalar value to be subtracted from elements</param>
    /// <returns>The vector with subtracted scalar for each element.</returns>
    public static Float4 Subtract(Float4 left, float right)
    {
        return (left.AsVector128() - Vector128.Create(right)).AsVector4();
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="left">The scalar value to be subtracted from elements</param>
    /// <param name="right">The input vector.</param>
    /// <param name="result">The vector with subtracted scalar for each element.</param>
    public static void Subtract(ref float left, ref Float4 right, out Float4 result)
    {
        Vector128<float> vLeft = Vector128.Create(left);
        ref Vector128<float> vRight = ref VectorExtensions.AsVector128(ref right);
        Vector128<float> vResult = vLeft - vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="left">The scalar value to be subtracted from elements</param>
    /// <param name="right">The input vector.</param>
    /// <returns>The vector with subtracted scalar for each element.</returns>
    public static Float4 Subtract(float left, Float4 right)
    {
        return (Vector128.Create(left) - right.AsVector128()).AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <param name="result">When the method completes, contains the scaled vector.</param>
    public static void Multiply(ref Float4 value, float scale, out Float4 result)
    {
        ref Vector128<float> vValue = ref VectorExtensions.AsVector128(ref value);
        Vector128<float> vScale = Vector128.Create(scale);
        Vector128<float> vResult = vValue * vScale;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 Multiply(Float4 value, float scale)
    {
        return (value.AsVector128() * Vector128.Create(scale)).AsVector4();
    }

    /// <summary>
    /// Multiplies a vector with another by performing component-wise multiplication.
    /// </summary>
    /// <param name="left">The first vector to multiply.</param>
    /// <param name="right">The second vector to multiply.</param>
    /// <param name="result">When the method completes, contains the multiplied vector.</param>
    public static void Multiply(ref Float4 left, ref Float4 right, out Float4 result)
    {
        ref Vector128<float> vLeft = ref VectorExtensions.AsVector128(ref left);
        ref Vector128<float> vRight = ref VectorExtensions.AsVector128(ref right);
        Vector128<float> vResult = vLeft * vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Multiplies a vector with another by performing component-wise multiplication.
    /// </summary>
    /// <param name="left">The first vector to multiply.</param>
    /// <param name="right">The second vector to multiply.</param>
    /// <returns>The multiplied vector.</returns>
    public static Float4 Multiply(Float4 left, Float4 right)
    {
        return (left.AsVector128() * right.AsVector128()).AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <param name="result">When the method completes, contains the scaled vector.</param>
    public static void Divide(ref Float4 value, float scale, out Float4 result)
    {
        ref Vector128<float> vValue = ref VectorExtensions.AsVector128(ref value);
        Vector128<float> vScale = Vector128.Create(scale);
        Vector128<float> vResult = vValue / vScale;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 Divide(Float4 value, float scale)
    {
        return (value.AsVector128() / Vector128.Create(scale)).AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <param name="value">The vector to scale.</param>
    /// <param name="result">When the method completes, contains the scaled vector.</param>
    public static void Divide(float scale, ref Float4 value, out Float4 result)
    {
        Vector128<float> vScale = Vector128.Create(scale);
        ref Vector128<float> vValue = ref VectorExtensions.AsVector128(ref value);
        Vector128<float> vResult = vScale / vValue;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 Divide(float scale, Float4 value)
    {
        return (Vector128.Create(scale) / value.AsVector128()).AsVector4();
    }

    /// <summary>
    /// Reverses the direction of a given vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <param name="result">When the method completes, contains a vector facing in the opposite direction.</param>
    public static void Negate(ref Float4 value, out Float4 result)
    {
        ref Vector128<float> vValue = ref VectorExtensions.AsVector128(ref value);
        Vector128<float> vResult = Vector128.Negate(vValue);
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Reverses the direction of a given vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>A vector facing in the opposite direction.</returns>
    public static Float4 Negate(Float4 value)
    {
        Negate(ref value, out Float4 result);
        return result;
    }

    /// <summary>
    /// Returns a <see cref="Float4" /> containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 4D triangle.
    /// </summary>
    /// <param name="value1">A <see cref="Float4" /> containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
    /// <param name="value2">A <see cref="Float4" /> containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
    /// <param name="value3">A <see cref="Float4" /> containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
    /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2" />).</param>
    /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3" />).</param>
    /// <param name="result">When the method completes, contains the 4D Cartesian coordinates of the specified point.</param>
    public static void Barycentric(ref Float4 value1, ref Float4 value2, ref Float4 value3, float amount1, float amount2, out Float4 result)
    {
        Vector128<float> v1 = VectorExtensions.AsVector128(ref value1);
        Vector128<float> v2 = VectorExtensions.AsVector128(ref value2);
        Vector128<float> v3 = VectorExtensions.AsVector128(ref value3);

        Vector128<float> a1 = Vector128.Create(amount1);
        Vector128<float> a2 = Vector128.Create(amount2);

        Vector128<float> vResult = v1 + (a1 * (v2 - v1)) + (a2 * (v3 - v1));

        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Returns a <see cref="Float4" /> containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 4D triangle.
    /// </summary>
    /// <param name="value1">A <see cref="Float4" /> containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
    /// <param name="value2">A <see cref="Float4" /> containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
    /// <param name="value3">A <see cref="Float4" /> containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
    /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2" />).</param>
    /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3" />).</param>
    /// <returns>A new <see cref="Float4" /> containing the 4D Cartesian coordinates of the specified point.</returns>
    public static Float4 Barycentric(Float4 value1, Float4 value2, Float4 value3, float amount1, float amount2)
    {
        Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out Float4 result);
        return result;
    }

    /// <summary>
    /// Restricts a value to be within a specified range.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">When the method completes, contains the clamped value.</param>
    public static void Clamp(ref Float4 value, ref Float4 min, ref Float4 max, out Float4 result)
    {
        ref Vector128<float> vValue = ref VectorExtensions.AsVector128(ref value);
        ref Vector128<float> vMin = ref VectorExtensions.AsVector128(ref min);
        ref Vector128<float> vMax = ref VectorExtensions.AsVector128(ref max);
        Vector128<float> vResult = Vector128.Clamp(vValue, vMin, vMax);
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Restricts a value to be within a specified range.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped value.</returns>
    public static Float4 Clamp(Float4 value, Float4 min, Float4 max)
    {
        Clamp(ref value, ref min, ref max, out Float4 result);
        return result;
    }

    /// <summary>
    /// Calculates the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">When the method completes, contains the distance between the two vectors.</param>
    /// <remarks><see cref="Float4.DistanceSquared(ref Float4, ref Float4, out float)" /> may be preferred when only the relative distance is needed and speed is of the essence.</remarks>
    public static void Distance(ref Float4 value1, ref Float4 value2, out float result)
    {
        DistanceSquared(ref value1, ref value2, out float sqrDistance);
        result = MathF.Sqrt(sqrDistance);
    }

    /// <summary>
    /// Calculates the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The distance between the two vectors.</returns>
    /// <remarks><see cref="Float4.DistanceSquared(Float4, Float4)" /> may be preferred when only the relative distance is needed and speed is of the essence.</remarks>
    public static float Distance(Float4 value1, Float4 value2)
    {
        Distance(ref value1, ref value2, out float result);
        return result;
    }

    /// <summary>
    /// Calculates the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">When the method completes, contains the squared distance between the two vectors.</param>
    public static void DistanceSquared(ref Float4 value1, ref Float4 value2, out float result)
    {
        ref Vector128<float> vValue1 = ref VectorExtensions.AsVector128(ref value1);
        ref Vector128<float> vValue2 = ref VectorExtensions.AsVector128(ref value2);
        Vector128<float> vDiff = Vector128.Subtract(vValue1, vValue2);
        Vector128<float> vSqr = Vector128.Multiply(vDiff, vDiff);
        result = Vector128.Sum(vSqr);
    }

    /// <summary>
    /// Calculates the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The squared distance between the two vectors.</returns>
    public static float DistanceSquared(Float4 value1, Float4 value2)
    {
        DistanceSquared(ref value1, ref value2, out float result);
        return result;
    }

    /// <summary>
    /// Tests whether one vector is near another vector.
    /// </summary>
    /// <param name="left">The left vector.</param>
    /// <param name="right">The right vector.</param>
    /// <param name="epsilon">The epsilon.</param>
    /// <returns><c>true</c> if left and right are near another, <c>false</c> otherwise</returns>
    public static bool NearEqual(Float4 left, Float4 right, float epsilon = Mathf.Epsilon)
    {
        return NearEqual(ref left, ref right, epsilon);
    }

    /// <summary>
    /// Tests whether one vector is near another vector.
    /// </summary>
    /// <param name="left">The left vector.</param>
    /// <param name="right">The right vector.</param>
    /// <param name="epsilon">The epsilon.</param>
    /// <returns><c>true</c> if left and right are near another, <c>false</c> otherwise</returns>
    public static bool NearEqual(ref Float4 left, ref Float4 right, float epsilon = Mathf.Epsilon)
    {
        return Mathf.WithinEpsilon(left.X, right.X, epsilon) && Mathf.WithinEpsilon(left.Y, right.Y, epsilon) && Mathf.WithinEpsilon(left.Z, right.Z, epsilon) && Mathf.WithinEpsilon(left.W, right.W, epsilon);
    }

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="left">First source vector</param>
    /// <param name="right">Second source vector.</param>
    /// <param name="result">When the method completes, contains the dot product of the two vectors.</param>
    public static void Dot(ref Float4 left, ref Float4 right, out float result)
    {
        ref Vector128<float> vLeft = ref VectorExtensions.AsVector128(ref left);
        ref Vector128<float> vRight = ref VectorExtensions.AsVector128(ref right);
        result = Vector128.Dot(vLeft, vRight);
    }

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="left">First source vector.</param>
    /// <param name="right">Second source vector.</param>
    /// <returns>The dot product of the two vectors.</returns>
    public static float Dot(Float4 left, Float4 right)
    {
        Dot(ref left, ref right, out float result);
        return result;
    }

    /// <summary>
    /// Converts the vector into a unit vector.
    /// </summary>
    /// <param name="value">The vector to normalize.</param>
    /// <param name="result">When the method completes, contains the normalized vector.</param>
    public static void Normalize(ref Float4 value, out Float4 result)
    {
        result = value;
        result.Normalize();
    }

    /// <summary>
    /// Converts the vector into a unit vector.
    /// </summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    public static Float4 Normalize(Float4 value)
    {
        value.Normalize();
        return value;
    }

    /// <summary>
    /// Makes sure that Length of the output vector is always below max and above 0.
    /// </summary>
    /// <param name="vector">Input vector.</param>
    /// <param name="max">Max Length</param>
    public static Float4 ClampLength(Float4 vector, float max)
    {
        return ClampLength(vector, 0, max);
    }

    /// <summary>
    /// Makes sure that Length of the output vector is always below max and above min.
    /// </summary>
    /// <param name="vector">Input vector.</param>
    /// <param name="min">Min Length</param>
    /// <param name="max">Max Length</param>
    public static Float4 ClampLength(Float4 vector, float min, float max)
    {
        ClampLength(vector, min, max, out Float4 result);
        return result;
    }

    /// <summary>
    /// Makes sure that Length of the output vector is always below max and above min.
    /// </summary>
    /// <param name="vector">Input vector.</param>
    /// <param name="min">Min Length</param>
    /// <param name="max">Max Length</param>
    /// <param name="result">The result vector.</param>
    public static void ClampLength(Float4 vector, float min, float max, out Float4 result)
    {
        result = vector;
        Vector128<float> vVector = vector.AsVector128();
        float lenSq = Vector128.Sum(vVector * vVector);
        if (lenSq > max * max)
        {
            Vector128<float> scaleFactor = Vector128.Create(max * MathF.ReciprocalSqrtEstimate(lenSq));
            result = VectorExtensions.AsVector4(scaleFactor * vVector);
        }
        if (lenSq < min * min)
        {
            Vector128<float> scaleFactor = Vector128.Create(min * MathF.ReciprocalSqrtEstimate(lenSq));
            result = VectorExtensions.AsVector4(scaleFactor * vVector);
        }
    }

    /// <summary>
    /// Performs a linear interpolation between two vectors.
    /// </summary>
    /// <param name="start">Start vector.</param>
    /// <param name="end">End vector.</param>
    /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end" />.</param>
    /// <param name="result">When the method completes, contains the linear interpolation of the two vectors.</param>
    /// <remarks>Passing <paramref name="amount" /> a value of 0 will cause <paramref name="start" /> to be returned; a value of 1 will cause <paramref name="end" /> to be returned.</remarks>
    public static void Lerp(ref Float4 start, ref Float4 end, float amount, out Float4 result)
    {
        ref Vector128<float> vStart = ref VectorExtensions.AsVector128(ref start);
        ref Vector128<float> vEnd = ref VectorExtensions.AsVector128(ref end);
        Vector128<float> vAmount = Vector128.Create(amount);
        Vector128<float> vResult = vStart + (vEnd - vStart) * vAmount;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Performs a linear interpolation between two vectors.
    /// </summary>
    /// <param name="start">Start vector.</param>
    /// <param name="end">End vector.</param>
    /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end" />.</param>
    /// <returns>The linear interpolation of the two vectors.</returns>
    /// <remarks>Passing <paramref name="amount" /> a value of 0 will cause <paramref name="start" /> to be returned; a value of 1 will cause <paramref name="end" /> to be returned.</remarks>
    public static Float4 Lerp(Float4 start, Float4 end, float amount)
    {
        Lerp(ref start, ref end, amount, out Float4 result);
        return result;
    }

    /// <summary>
    /// Performs a cubic interpolation between two vectors.
    /// </summary>
    /// <param name="start">Start vector.</param>
    /// <param name="end">End vector.</param>
    /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end" />.</param>
    /// <param name="result">When the method completes, contains the cubic interpolation of the two vectors.</param>
    public static void SmoothStep(ref Float4 start, ref Float4 end, float amount, out Float4 result)
    {
        amount = Mathf.SmoothStep(amount);
        Lerp(ref start, ref end, amount, out result);
    }

    /// <summary>
    /// Performs a cubic interpolation between two vectors.
    /// </summary>
    /// <param name="start">Start vector.</param>
    /// <param name="end">End vector.</param>
    /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end" />.</param>
    /// <returns>The cubic interpolation of the two vectors.</returns>
    public static Float4 SmoothStep(Float4 start, Float4 end, float amount)
    {
        SmoothStep(ref start, ref end, amount, out Float4 result);
        return result;
    }

    /// <summary>
    /// Performs a Hermite spline interpolation.
    /// </summary>
    /// <param name="value1">First source position vector.</param>
    /// <param name="tangent1">First source tangent vector.</param>
    /// <param name="value2">Second source position vector.</param>
    /// <param name="tangent2">Second source tangent vector.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <param name="result">When the method completes, contains the result of the Hermite spline interpolation.</param>
    public static void Hermite(ref Float4 value1, ref Float4 tangent1, ref Float4 value2, ref Float4 tangent2, float amount, out Float4 result)
    {
        float squared = amount * amount;
        float cubed = amount * squared;

        float part1 = 2.0f * cubed - 3.0f * squared + 1.0f;
        float part2 = -2.0f * cubed + 3.0f * squared;
        float part3 = cubed - 2.0f * squared + amount;
        float part4 = cubed - squared;

        Vector128<float> v1 = VectorExtensions.AsVector128(ref value1);
        Vector128<float> t1 = VectorExtensions.AsVector128(ref tangent1);
        Vector128<float> v2 = VectorExtensions.AsVector128(ref value2);
        Vector128<float> t2 = VectorExtensions.AsVector128(ref tangent2);

        Vector128<float> vp1 = Vector128.Create(part1);
        Vector128<float> vp2 = Vector128.Create(part2);
        Vector128<float> vp3 = Vector128.Create(part3);
        Vector128<float> vp4 = Vector128.Create(part4);

        Vector128<float> vResult = (v1 * vp1) + (v2 * vp2) + (t1 * vp3) + (t2 * vp4);

        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Performs a Hermite spline interpolation.
    /// </summary>
    /// <param name="value1">First source position vector.</param>
    /// <param name="tangent1">First source tangent vector.</param>
    /// <param name="value2">Second source position vector.</param>
    /// <param name="tangent2">Second source tangent vector.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <returns>The result of the Hermite spline interpolation.</returns>
    public static Float4 Hermite(Float4 value1, Float4 tangent1, Float4 value2, Float4 tangent2, float amount)
    {
        Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out Float4 result);
        return result;
    }

    /// <summary>
    /// Performs a Catmull-Rom interpolation using the specified positions.
    /// </summary>
    /// <param name="value1">The first position in the interpolation.</param>
    /// <param name="value2">The second position in the interpolation.</param>
    /// <param name="value3">The third position in the interpolation.</param>
    /// <param name="value4">The fourth position in the interpolation.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <param name="result">When the method completes, contains the result of the Catmull-Rom interpolation.</param>
    public static void CatmullRom(ref Float4 value1, ref Float4 value2, ref Float4 value3, ref Float4 value4, float amount, out Float4 result)
    {
        float squared = amount * amount;
        float cubed = amount * squared;

        Vector128<float> v1 = VectorExtensions.AsVector128(ref value1);
        Vector128<float> v2 = VectorExtensions.AsVector128(ref value2);
        Vector128<float> v3 = VectorExtensions.AsVector128(ref value3);
        Vector128<float> v4 = VectorExtensions.AsVector128(ref value4);

        Vector128<float> vT  = Vector128.Create(amount);
        Vector128<float> vT2 = Vector128.Create(squared);
        Vector128<float> vT3 = Vector128.Create(cubed);

        Vector128<float> term0 = Vector128.Create(2.0f) * v2;
        Vector128<float> term1 = (-v1 + v3) * vT;
        Vector128<float> term2 = (Vector128.Create(2.0f) * v1 - Vector128.Create(5.0f) * v2 + Vector128.Create(4.0f) * v3 - v4) * vT2;
        Vector128<float> term3 = (-v1 + Vector128.Create(3.0f) * v2 - Vector128.Create(3.0f) * v3 + v4) * vT3;

        result = (Vector128.Create(0.5f) * (term0 + term1 + term2 + term3)).AsVector4();
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
    public static Float4 CatmullRom(Float4 value1, Float4 value2, Float4 value3, Float4 value4, float amount)
    {
        CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out Float4 result);
        return result;
    }

    /// <summary>
    /// Returns a vector containing the largest components of the specified vectors.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <param name="result">When the method completes, contains an new vector composed of the largest components of the source vectors.</param>
    public static void Max(ref Float4 left, ref Float4 right, out Float4 result)
    {
        ref Vector128<float> vLeft = ref VectorExtensions.AsVector128(ref left);
        ref Vector128<float> vRight = ref VectorExtensions.AsVector128(ref right);
        Vector128<float> vResult = Vector128.Max(vLeft, vRight);
        result = vResult.AsVector4();
    }

    /// <summary>
    /// Returns a vector containing the largest components of the specified vectors.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>A vector containing the largest components of the source vectors.</returns>
    public static Float4 Max(Float4 left, Float4 right)
    {
        Max(ref left, ref right, out Float4 result);
        return result;
    }

    /// <summary>
    /// Returns a vector containing the smallest components of the specified vectors.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <param name="result">When the method completes, contains an new vector composed of the smallest components of the source vectors.</param>
    public static void Min(ref Float4 left, ref Float4 right, out Float4 result)
    {
        ref Vector128<float> vLeft = ref VectorExtensions.AsVector128(ref left);
        ref Vector128<float> vRight = ref VectorExtensions.AsVector128(ref right);
        Vector128<float> vResult = Vector128.Min(vLeft, vRight);
        result = vResult.AsVector4();
    }

    /// <summary>
    /// Returns a vector containing the smallest components of the specified vectors.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>A vector containing the smallest components of the source vectors.</returns>
    public static Float4 Min(Float4 left, Float4 right)
    {
        Min(ref left, ref right, out Float4 result);
        return result;
    }

    /// <summary>
    /// Returns the absolute value of a vector.
    /// </summary>
    /// <param name="v">The value.</param>
    /// <returns> A vector which components are less or equal to 0.</returns>
    public static Float4 Abs(Float4 v)
    {
        Vector128<float> result = Vector128.Abs(v.AsVector128());
        return result.AsVector4();
    }

    /// <summary>
    /// Transforms a 4D vector by the given <see cref="Quaternion" /> rotation.
    /// </summary>
    /// <param name="vector">The vector to rotate.</param>
    /// <param name="rotation">The <see cref="Quaternion" /> rotation to apply.</param>
    /// <param name="result">When the method completes, contains the transformed <see cref="Float4" />.</param>
    public static void Transform(ref Float4 vector, ref Quaternion rotation, out Float4 result)
    {
        float x = rotation.X + rotation.X;
        float y = rotation.Y + rotation.Y;
        float z = rotation.Z + rotation.Z;
        float wx = rotation.W * x, wy = rotation.W * y, wz = rotation.W * z;
        float xx = rotation.X * x, xy = rotation.X * y, xz = rotation.X * z;
        float yy = rotation.Y * y, yz = rotation.Y * z, zz = rotation.Z * z;

        Vector128<float> vX = Vector128.Create(vector.X);
        Vector128<float> vY = Vector128.Create(vector.Y);
        Vector128<float> vZ = Vector128.Create(vector.Z);

        Vector128<float> col1 = Vector128.Create(1.0f - yy - zz, xy + wz, xz - wy, 0.0f);
        Vector128<float> col2 = Vector128.Create(xy - wz, 1.0f - xx - zz, yz + wx, 0.0f);
        Vector128<float> col3 = Vector128.Create(xz + wy, yz - wx, 1.0f - xx - yy, 0.0f);

        Vector128<float> vResult = (vX * col1) + (vY * col2) + (vZ * col3);

        result = VectorExtensions.AsVector4(ref vResult);

        result.W = vector.W;
    }

    /// <summary>
    /// Transforms a 4D vector by the given <see cref="Quaternion" /> rotation.
    /// </summary>
    /// <param name="vector">The vector to rotate.</param>
    /// <param name="rotation">The <see cref="Quaternion" /> rotation to apply.</param>
    /// <returns>The transformed <see cref="Float4" />.</returns>
    public static Float4 Transform(Float4 vector, Quaternion rotation)
    {
        Transform(ref vector, ref rotation, out Float4 result);
        return result;
    }

    /// <summary>
    /// Transforms a 4D vector by the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <param name="result">When the method completes, contains the transformed <see cref="Float4" />.</param>
    public static void Transform(ref Float4 vector, ref Matrix transform, out Float4 result)
    {
        Vector128<float> vX = Vector128.Create(vector.X);
        Vector128<float> vY = Vector128.Create(vector.Y);
        Vector128<float> vZ = Vector128.Create(vector.Z);
        Vector128<float> vW = Vector128.Create(vector.W);

        ref Vector128<float> row1 = ref Unsafe.As<float, Vector128<float>>(ref transform.M11);
        ref Vector128<float> row2 = ref Unsafe.As<float, Vector128<float>>(ref transform.M21);
        ref Vector128<float> row3 = ref Unsafe.As<float, Vector128<float>>(ref transform.M31);
        ref Vector128<float> row4 = ref Unsafe.As<float, Vector128<float>>(ref transform.M41);

        Vector128<float> vResult = (vX * row1) + (vY * row2) + (vZ * row3) + (vW * row4);

        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Transforms a 4D vector by the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <returns>The transformed <see cref="Float4" />.</returns>
    public static Float4 Transform(Float4 vector, Matrix transform)
    {
        Transform(ref vector, ref transform, out Float4 result);
        return result;
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>The sum of the two vectors.</returns>
    public static Float4 operator +(Float4 left, Float4 right)
    {
        Add(ref left, ref right, out Float4 result);
        return result;
    }

    /// <summary>
    /// Multiplies a vector with another by performing component-wise multiplication equivalent to <see cref="Multiply(ref Float4,ref Float4,out Float4)" />.
    /// </summary>
    /// <param name="left">The first vector to multiply.</param>
    /// <param name="right">The second vector to multiply.</param>
    /// <returns>The multiplication of the two vectors.</returns>
    public static Float4 operator *(Float4 left, Float4 right)
    {
        Multiply(ref left, ref right, out Float4 result);
        return result;
    }

    /// <summary>
    /// Assert a vector (return it unchanged).
    /// </summary>
    /// <param name="value">The vector to assert (unchanged).</param>
    /// <returns>The asserted (unchanged) vector.</returns>
    public static Float4 operator +(Float4 value)
    {
        return value;
    }

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    /// <param name="left">The first vector to subtract.</param>
    /// <param name="right">The second vector to subtract.</param>
    /// <returns>The difference of the two vectors.</returns>
    public static Float4 operator -(Float4 left, Float4 right)
    {
        Subtract(ref left, ref right, out Float4 result);
        return result;
    }

    /// <summary>
    /// Reverses the direction of a given vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>A vector facing in the opposite direction.</returns>
    public static Float4 operator -(Float4 value)
    {
        Negate(ref value, out Float4 result);
        return result;
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 operator *(float scale, Float4 value)
    {
        Vector128<float> result = Vector128.Create(scale) * value.AsVector128();
        return result.AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 operator *(Float4 value, float scale)
    {
        Vector128<float> result = value.AsVector128() * Vector128.Create(scale);
        return result.AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 operator /(Float4 value, float scale)
    {
        Vector128<float> result = value.AsVector128() / Vector128.Create(scale);
        return result.AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <param name="value">The vector to scale.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 operator /(float scale, Float4 value)
    {
        Vector128<float> result = Vector128.Create(scale) / value.AsVector128();
        return result.AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 operator *(double scale, Float4 value)
    {
        return (float)scale * value;
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 operator *(Float4 value, double scale)
    {
        return value * (float)scale;
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 operator /(Float4 value, double scale)
    {
        return value / (float)scale;
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <param name="value">The vector to scale.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 operator /(double scale, Float4 value)
    {
        return (float)scale / value;
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Float4 operator /(Float4 value, Float4 scale)
    {
        Vector128<float> result = value.AsVector128() / scale.AsVector128();
        return result.AsVector4();
    }

    /// <summary>
    /// Remainder of value divided by scale.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The remained vector.</returns>
    public static Float4 operator %(Float4 value, float scale)
    {
        Vector128<float> vValue = value.AsVector128();
        Vector128<float> vScale = Vector128.Create(scale);

        Vector128<float> div = vValue / vScale;
        Vector128<float> trunc = Vector128.Truncate(div);
        Vector128<float> result = vValue - (vScale * trunc);

        return result.AsVector4();
    }

    /// <summary>
    /// Remainder of value divided by scale.
    /// </summary>
    /// <param name="value">The amount by which to scale the vector.</param>
    /// <param name="scale">The vector to scale.</param>
    /// <returns>The remained vector.</returns>
    public static Float4 operator %(float value, Float4 scale)
    {
        Vector128<float> vValue = Vector128.Create(value);
        Vector128<float> vScale = scale.AsVector128();

        Vector128<float> div = vValue / vScale;
        Vector128<float> trunc = Vector128.Truncate(div);
        Vector128<float> result = vValue - (vScale * trunc);

        return result.AsVector4();
    }

    /// <summary>
    /// Remainder of value divided by scale.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The remained vector.</returns>
    public static Float4 operator %(Float4 value, Float4 scale)
    {
        Vector128<float> vValue = value.AsVector128();
        Vector128<float> vScale = scale.AsVector128();

        Vector128<float> div = vValue / vScale;
        Vector128<float> trunc = Vector128.Truncate(div);
        Vector128<float> result = vValue - (vScale * trunc);

        return result.AsVector4();
    }

    /// <summary>
    /// Performs a component-wise addition.
    /// </summary>
    /// <param name="value">The input vector.</param>
    /// <param name="scalar">The scalar value to be added on elements</param>
    /// <returns>The vector with added scalar for each element.</returns>
    public static Float4 operator +(Float4 value, float scalar)
    {
        Vector128<float> result = value.AsVector128() + Vector128.Create(scalar);
        return result.AsVector4();
    }

    /// <summary>
    /// Performs a component-wise addition.
    /// </summary>
    /// <param name="value">The input vector.</param>
    /// <param name="scalar">The scalar value to be added on elements</param>
    /// <returns>The vector with added scalar for each element.</returns>
    public static Float4 operator +(float scalar, Float4 value)
    {
        Vector128<float> result = Vector128.Create(scalar) + value.AsVector128();
        return result.AsVector4();
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="value">The input vector.</param>
    /// <param name="scalar">The scalar value to be subtracted from elements</param>
    /// <returns>The vector with subtracted scalar from each element.</returns>
    public static Float4 operator -(Float4 value, float scalar)
    {
        Vector128<float> result = value.AsVector128() - Vector128.Create(scalar);
        return result.AsVector4();
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="value">The input vector.</param>
    /// <param name="scalar">The scalar value to be subtracted from elements</param>
    /// <returns>The vector with subtracted scalar from each element.</returns>
    public static Float4 operator -(float scalar, Float4 value)
    {
        Vector128<float> result = Vector128.Create(scalar) - value.AsVector128();
        return result.AsVector4();
    }

    /// <summary>
    /// Tests for equality between two objects.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns><c>true</c> if <paramref name="left" /> has the same value as <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Float4 left, Float4 right)
    {
        return left.Equals(ref right);
    }

    /// <summary>
    /// Tests for inequality between two objects.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns><c>true</c> if <paramref name="left" /> has a different value than <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Float4 left, Float4 right)
    {
        return !left.Equals(ref right);
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

    /// <inheritdoc />
    public readonly bool ValueEquals(object other)
    {
        var o = (Float4)other;
        return Equals(ref o);
    }

    /// <summary>
    /// Determines whether the specified <see cref="Float4" /> is equal to this instance.
    /// </summary>
    /// <param name="other">The <see cref="Float4" /> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="Float4" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public readonly bool Equals(ref Float4 other)
    {
        ref Vector128<float> vOther = ref VectorExtensions.AsVector128(ref other);
        return this.AsVector128().Equals(vOther);
    }

    /// <summary>
    /// Determines whether the specified <see cref="Float4" /> is equal to this instance.
    /// </summary>
    /// <param name="other">The <see cref="Float4" /> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="Float4" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Float4 other)
    {
        return this.AsVector128().Equals(other.AsVector128());
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="value">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override readonly bool Equals(object value)
    {
        return value is Float4 other && Equals(ref other);
    }
}
