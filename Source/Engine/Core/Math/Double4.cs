// Copyright (c) Wojciech Figat. All rights reserved.

#if USE_LARGE_WORLDS
using Real = System.Double;
#else
using Real = System.Single;
#endif

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
* * Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* * The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
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
[System.ComponentModel.TypeConverter(typeof(TypeConverters.Double4Converter))]
#endif
partial struct Double4 : IEquatable<Double4>, IFormattable, Json.ICustomValueEquals
{
    private static readonly string _formatString = "X:{0:F2} Y:{1:F2} Z:{2:F2} W:{3:F2}";

    /// <summary>
    /// The size of the <see cref="Double4" /> type, in bytes.
    /// </summary>
    public static unsafe readonly int SizeInBytes = sizeof(Double4);

    /// <summary>
    /// A <see cref="Double4" /> with all of its components set to zero.
    /// </summary>
    public static readonly Double4 Zero;

    /// <summary>
    /// The X unit <see cref="Double4" /> (1, 0, 0, 0).
    /// </summary>
    public static readonly Double4 UnitX = new(1.0, 0.0, 0.0, 0.0);

    /// <summary>
    /// The Y unit <see cref="Double4" /> (0, 1, 0, 0).
    /// </summary>
    public static readonly Double4 UnitY = new(0.0, 1.0, 0.0, 0.0);

    /// <summary>
    /// The Z unit <see cref="Double4" /> (0, 0, 1, 0).
    /// </summary>
    public static readonly Double4 UnitZ = new(0.0, 0.0, 1.0, 0.0);

    /// <summary>
    /// The W unit <see cref="Double4" /> (0, 0, 0, 1).
    /// </summary>
    public static readonly Double4 UnitW = new(0.0, 0.0, 0.0, 1.0);

    /// <summary>
    /// A <see cref="Double4" /> with all of its components set to half.
    /// </summary>
    public static readonly Double4 Half = new(0.5, 0.5, 0.5, 0.5);

    /// <summary>
    /// A <see cref="Double4" /> with all of its components set to one.
    /// </summary>
    public static readonly Double4 One = new(1.0, 1.0, 1.0, 1.0);

    /// <summary>
    /// A <see cref="Double4" /> with all components equal to <see cref="double.MinValue"/>.
    /// </summary>
    public static readonly Double4 Minimum = new(double.MinValue);

    /// <summary>
    /// A <see cref="Double4" /> with all components equal to <see cref="double.MaxValue"/>.
    /// </summary>
    public static readonly Double4 Maximum = new(double.MaxValue);

    /// <summary>
    /// Initializes a new instance of the <see cref="Double4" /> struct.
    /// </summary>
    /// <param name="value">The value that will be assigned to all components.</param>
    public Double4(double value)
    {
        X = value;
        Y = value;
        Z = value;
        W = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Double4" /> struct.
    /// </summary>
    /// <param name="x">Initial value for the X component of the vector.</param>
    /// <param name="y">Initial value for the Y component of the vector.</param>
    /// <param name="z">Initial value for the Z component of the vector.</param>
    /// <param name="w">Initial value for the W component of the vector.</param>
    public Double4(double x, double y, double z, double w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Double4" /> struct.
    /// </summary>
    /// <param name="value">A vector containing the values with which to initialize the X, Y, Z, and W components.</param>
    public Double4(Vector4 value)
    {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
        W = value.W;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Double4" /> struct.
    /// </summary>
    /// <param name="value">A vector containing the values with which to initialize the X, Y, and Z components.</param>
    /// <param name="w">Initial value for the W component of the vector.</param>
    public Double4(Double3 value, double w)
    {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Double4" /> struct.
    /// </summary>
    /// <param name="xy">A vector containing the values with which to initialize the X and Y components.</param>
    /// <param name="zw">A vector containing the values with which to initialize the Z and W components.</param>
    public Double4(Double2 xy, Double2 zw)
    {
        X = xy.X;
        Y = xy.Y;
        Z = zw.X;
        W = zw.Y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Double4" /> struct.
    /// </summary>
    /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
    /// <param name="z">Initial value for the Z component of the vector.</param>
    /// <param name="w">Initial value for the W component of the vector.</param>
    public Double4(Double2 value, double z, double w)
    {
        X = value.X;
        Y = value.Y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Double4" /> struct.
    /// </summary>
    /// <param name="values">The span of values to assign to the X, Y, Z, and W components. Must contain exactly four elements.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values" /> does not contain exactly four elements.</exception>
    public Double4(ReadOnlySpan<double> values)
    {
        if (values.Length != 4)
        {
            throw new ArgumentOutOfRangeException(nameof(values), "There must be four and only four input values for Double4.");
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
    public readonly bool IsZero => this.AsVector256() == Vector256<double>.Zero;

    /// <summary>
    /// Gets a value indicating whether this vector is one (1, 1, 1, 1).
    /// </summary>
    public readonly bool IsOne => this.AsVector256() == Vector256.Create(1.0);

    /// <summary>
    /// Gets a minimum component value
    /// </summary>
    public readonly double MinValue => Mathd.Min(X, Mathd.Min(Y, Mathd.Min(Z, W)));

    /// <summary>
    /// Gets a maximum component value
    /// </summary>
    public readonly double MaxValue => Mathd.Max(X, Mathd.Max(Y, Mathd.Max(Z, W)));

    /// <summary>
    /// Gets an arithmetic average value of all vector components.
    /// </summary>
    public readonly double AvgValue
    {
        get
        {
            const double OneOverFour = 1.0 / 4.0;
            return ValuesSum * OneOverFour;
        }
    }

    /// <summary>
    /// Gets the sum of all vector components (X + Y + Z + W).
    /// </summary>
    public readonly double ValuesSum => Vector256.Sum(this.AsVector256());

    /// <summary>
    /// Gets a vector with values being absolute values of that vector.
    /// </summary>
    public readonly Double4 Absolute => Vector256.Abs(this.AsVector256()).AsVector4();

    /// <summary>
    /// Gets a vector with values being opposite to values of that vector.
    /// </summary>
    public readonly Double4 Negative => Negate(this);

    /// <summary>
    /// Gets or sets the component at the specified index.
    /// </summary>
    /// <value>The value of the X, Y, Z, or W component, depending on the index.</value>
    /// <param name="index">The index of the component to access. Use 0 for the X component, 1 for the Y component, 2 for the Z component, and 3 for the W component.</param>
    /// <returns>The value of the component at the specified index.</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index" /> is out of the range [0,3].</exception>
    public double this[int index]
    {
        readonly get => index switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            3 => W,
            _ => throw new ArgumentOutOfRangeException(nameof(index), "Indices for Double4 run from 0 to 3, inclusive."),
        };
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                case 2: Z = value; break;
                case 3: W = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(index), "Indices for Double4 run from 0 to 3, inclusive.");
            }
        }
    }

    /// <remarks>
    /// Uses a fast approximation for the inverse square root, so the result may not be precise. 
    /// For a more accurate result, use <see cref="Double4.PreciseLength" />.
    /// </remarks>
    /// <inheritdoc cref="PreciseLength" />
    public readonly double Length => IsNormalizedWithLength(out double lengthSquared) ? 1.0 : Math.ReciprocalSqrtEstimate(lengthSquared);

    /// <summary>
    /// Calculates the length of the vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    public readonly double PreciseLength => IsNormalizedWithLength(out double lengthSquared) ? 1.0 : Math.Sqrt(lengthSquared);

    /// <summary>
    /// Calculates the squared length of the vector.
    /// </summary>
    /// <returns>The squared length of the vector.</returns>
    /// <remarks>This method may be preferred to <see cref="Double4.Length" /> when only a relative length is needed and speed is of the essence.</remarks>
    public readonly double LengthSquared
    {
        get
        {
            Vector256<double> vValue = this.AsVector256();
            return Vector256.Sum(vValue * vValue);
        }
    }

    /// <remarks>
    /// Uses a fast approximation for the inverse square root, so the result may not be precise. 
    /// For a more accurate result, use <see cref="Double4.NormalizePrecise" />.
    /// </remarks>
    /// <inheritdoc cref="Double4.NormalizePrecise" />
    public void Normalize()
    {
        if (IsNormalizedWithLength(out double lengthSquared))
        {
            return;
        }

        double inv = Math.ReciprocalSqrtEstimate(lengthSquared);
        Vector256<double> vInv = Vector256.Create(inv);
        this = (this.AsVector256() * vInv).AsVector4();
    }

    /// <summary>
    /// Converts the vector into a unit vector with a length of 1.
    /// </summary>
    public void NormalizePrecise()
    {
        if(IsNormalizedWithLength(out double lengthSquared))
        {
            return;
        }

        double inv = 1.0 / Math.Sqrt(lengthSquared);
        Vector256<double> vInv = Vector256.Create(inv);
        this = (this.AsVector256() * vInv).AsVector4();
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private readonly bool IsNormalizedWithLength(out double lengthSquared)
    {
        lengthSquared = LengthSquared;
        return Math.Abs(lengthSquared - 1.0) < 1e-4;
    }

    /// <summary>
    /// Creates an array containing the elements of the vector.
    /// </summary>
    /// <returns>A four-element array containing the components of the vector.</returns>
    public readonly double[] ToArray()
    {
        return [X, Y, Z, W];
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <param name="result">When the method completes, contains the sum of the two vectors.</param>
    public static void Add(ref Double4 left, ref Double4 right, out Double4 result)
    {
        ref Vector256<double> vLeft = ref VectorExtensions.AsVector256(ref left);
        ref Vector256<double> vRight = ref VectorExtensions.AsVector256(ref right);
        Vector256<double> vResult = vLeft + vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>The sum of the two vectors.</returns>
    public static Double4 Add(Double4 left, Double4 right)
    {
        return (left.AsVector256() + right.AsVector256()).AsVector4();
    }

    /// <summary>
    /// Performs a component-wise addition.
    /// </summary>
    /// <param name="left">The input vector</param>
    /// <param name="right">The scalar value to be added to elements</param>
    /// <param name="result">The vector with added scalar for each element.</param>
    public static void Add(ref Double4 left, ref double right, out Double4 result)
    {
        ref Vector256<double> vLeft = ref VectorExtensions.AsVector256(ref left);
        Vector256<double> vRight = Vector256.Create(right);
        Vector256<double> vResult = vLeft + vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Performs a component-wise addition.
    /// </summary>
    /// <param name="left">The input vector</param>
    /// <param name="right">The scalar value to be added to elements</param>
    /// <returns>The vector with added scalar for each element.</returns>
    public static Double4 Add(Double4 left, double right)
    {
        return (left.AsVector256() + Vector256.Create(right)).AsVector4();
    }

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    /// <param name="left">The first vector to subtract.</param>
    /// <param name="right">The second vector to subtract.</param>
    /// <param name="result">When the method completes, contains the difference of the two vectors.</param>
    public static void Subtract(ref Double4 left, ref Double4 right, out Double4 result)
    {
        ref Vector256<double> vLeft = ref VectorExtensions.AsVector256(ref left);
        ref Vector256<double> vRight = ref VectorExtensions.AsVector256(ref right);
        Vector256<double> vResult = vLeft - vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    /// <param name="left">The first vector to subtract.</param>
    /// <param name="right">The second vector to subtract.</param>
    /// <returns>The difference of the two vectors.</returns>
    public static Double4 Subtract(Double4 left, Double4 right)
    {
        return (left.AsVector256() - right.AsVector256()).AsVector4();
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="left">The input vector</param>
    /// <param name="right">The scalar value to be subtracted from elements</param>
    /// <param name="result">The vector with subtracted scalar for each element.</param>
    public static void Subtract(ref Double4 left, ref double right, out Double4 result)
    {
        ref Vector256<double> vLeft = ref VectorExtensions.AsVector256(ref left);
        Vector256<double> vRight = Vector256.Create(right);
        Vector256<double> vResult = vLeft - vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="left">The input vector</param>
    /// <param name="right">The scalar value to be subtracted from elements</param>
    /// <returns>The vector with subtracted scalar for each element.</returns>
    public static Double4 Subtract(Double4 left, double right)
    {
        return (left.AsVector256() - Vector256.Create(right)).AsVector4();
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="left">The scalar value to be subtracted from elements</param>
    /// <param name="right">The input vector.</param>
    /// <param name="result">The vector with subtracted scalar for each element.</param>
    public static void Subtract(ref double left, ref Double4 right, out Double4 result)
    {
        Vector256<double> vLeft = Vector256.Create(left);
        ref Vector256<double> vRight = ref VectorExtensions.AsVector256(ref right);
        Vector256<double> vResult = vLeft - vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="left">The scalar value to be subtracted from elements</param>
    /// <param name="right">The input vector.</param>
    /// <returns>The vector with subtracted scalar for each element.</returns>
    public static Double4 Subtract(double left, Double4 right)
    {
        return (Vector256.Create(left) - right.AsVector256()).AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <param name="result">When the method completes, contains the scaled vector.</param>
    public static void Multiply(ref Double4 value, double scale, out Double4 result)
    {
        ref Vector256<double> vValue = ref VectorExtensions.AsVector256(ref value);
        Vector256<double> vScale = Vector256.Create(scale);
        Vector256<double> vResult = vValue * vScale;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Double4 Multiply(Double4 value, double scale)
    {
        return (value.AsVector256() * Vector256.Create(scale)).AsVector4();
    }

    /// <summary>
    /// Multiplies a vector with another by performing component-wise multiplication.
    /// </summary>
    /// <param name="left">The first vector to multiply.</param>
    /// <param name="right">The second vector to multiply.</param>
    /// <param name="result">When the method completes, contains the multiplied vector.</param>
    public static void Multiply(ref Double4 left, ref Double4 right, out Double4 result)
    {
        ref Vector256<double> vLeft = ref VectorExtensions.AsVector256(ref left);
        ref Vector256<double> vRight = ref VectorExtensions.AsVector256(ref right);
        Vector256<double> vResult = vLeft * vRight;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Multiplies a vector with another by performing component-wise multiplication.
    /// </summary>
    /// <param name="left">The first vector to multiply.</param>
    /// <param name="right">The second vector to multiply.</param>
    /// <returns>The multiplied vector.</returns>
    public static Double4 Multiply(Double4 left, Double4 right)
    {
        return (left.AsVector256() * right.AsVector256()).AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <param name="result">When the method completes, contains the scaled vector.</param>
    public static void Divide(ref Double4 value, double scale, out Double4 result)
    {
        ref Vector256<double> vValue = ref VectorExtensions.AsVector256(ref value);
        Vector256<double> vScale = Vector256.Create(scale);
        Vector256<double> vResult = vValue / vScale;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Double4 Divide(Double4 value, double scale)
    {
        return (value.AsVector256() / Vector256.Create(scale)).AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <param name="value">The vector to scale.</param>
    /// <param name="result">When the method completes, contains the scaled vector.</param>
    public static void Divide(double scale, ref Double4 value, out Double4 result)
    {
        Vector256<double> vScale = Vector256.Create(scale);
        ref Vector256<double> vValue = ref VectorExtensions.AsVector256(ref value);
        Vector256<double> vResult = vScale / vValue;
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Double4 Divide(double scale, Double4 value)
    {
        return (Vector256.Create(scale) / value.AsVector256()).AsVector4();
    }

    /// <summary>
    /// Reverses the direction of a given vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <param name="result">When the method completes, contains a vector facing in the opposite direction.</param>
    public static void Negate(ref Double4 value, out Double4 result)
    {
        ref Vector256<double> vValue = ref VectorExtensions.AsVector256(ref value);
        Vector256<double> vResult = Vector256.Negate(vValue);
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Reverses the direction of a given vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>A vector facing in the opposite direction.</returns>
    public static Double4 Negate(Double4 value)
    {
        Negate(ref value, out Double4 result);
        return result;
    }

    /// <summary>
    /// Returns a <see cref="Double4" /> containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 4D triangle.
    /// </summary>
    /// <param name="value1">A <see cref="Double4" /> containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
    /// <param name="value2">A <see cref="Double4" /> containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
    /// <param name="value3">A <see cref="Double4" /> containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
    /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2" />).</param>
    /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3" />).</param>
    /// <param name="result">When the method completes, contains the 4D Cartesian coordinates of the specified point.</param>
    public static void Barycentric(ref Double4 value1, ref Double4 value2, ref Double4 value3, double amount1, double amount2, out Double4 result)
    {
        Vector256<double> v1 = VectorExtensions.AsVector256(ref value1);
        Vector256<double> v2 = VectorExtensions.AsVector256(ref value2);
        Vector256<double> v3 = VectorExtensions.AsVector256(ref value3);

        Vector256<double> a1 = Vector256.Create(amount1);
        Vector256<double> a2 = Vector256.Create(amount2);

        Vector256<double> vResult = v1 + (a1 * (v2 - v1)) + (a2 * (v3 - v1));

        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Returns a <see cref="Double4" /> containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 4D triangle.
    /// </summary>
    /// <param name="value1">A <see cref="Double4" /> containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
    /// <param name="value2">A <see cref="Double4" /> containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
    /// <param name="value3">A <see cref="Double4" /> containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
    /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2" />).</param>
    /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3" />).</param>
    /// <returns>A new <see cref="Double4" /> containing the 4D Cartesian coordinates of the specified point.</returns>
    public static Double4 Barycentric(Double4 value1, Double4 value2, Double4 value3, double amount1, double amount2)
    {
        Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out Double4 result);
        return result;
    }

    /// <summary>
    /// Restricts a value to be within a specified range.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">When the method completes, contains the clamped value.</param>
    public static void Clamp(ref Double4 value, ref Double4 min, ref Double4 max, out Double4 result)
    {
        ref Vector256<double> vValue = ref VectorExtensions.AsVector256(ref value);
        ref Vector256<double> vMin = ref VectorExtensions.AsVector256(ref min);
        ref Vector256<double> vMax = ref VectorExtensions.AsVector256(ref max);
        Vector256<double> vResult = Vector256.Clamp(vValue, vMin, vMax);
        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Restricts a value to be within a specified range.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped value.</returns>
    public static Double4 Clamp(Double4 value, Double4 min, Double4 max)
    {
        Clamp(ref value, ref min, ref max, out Double4 result);
        return result;
    }

    /// <summary>
    /// Calculates the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">When the method completes, contains the distance between the two vectors.</param>
    /// <remarks><see cref="Double4.DistanceSquared(ref Double4, ref Double4, out double)" /> may be preferred when only the relative distance is needed and speed is of the essence.</remarks>
    public static void Distance(ref Double4 value1, ref Double4 value2, out double result)
    {
        DistanceSquared(ref value1, ref value2, out double sqrDistance);
        result = Math.Sqrt(sqrDistance);
    }

    /// <summary>
    /// Calculates the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The distance between the two vectors.</returns>
    /// <remarks><see cref="Double4.DistanceSquared(Double4, Double4)" /> may be preferred when only the relative distance is needed and speed is of the essence.</remarks>
    public static double Distance(Double4 value1, Double4 value2)
    {
        Distance(ref value1, ref value2, out double result);
        return result;
    }

    /// <summary>
    /// Calculates the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">When the method completes, contains the squared distance between the two vectors.</param>
    public static void DistanceSquared(ref Double4 value1, ref Double4 value2, out double result)
    {
        ref Vector256<double> vValue1 = ref VectorExtensions.AsVector256(ref value1);
        ref Vector256<double> vValue2 = ref VectorExtensions.AsVector256(ref value2);
        Vector256<double> vDiff = Vector256.Subtract(vValue1, vValue2);
        Vector256<double> vSqr = Vector256.Multiply(vDiff, vDiff);
        result = Vector256.Sum(vSqr);
    }

    /// <summary>
    /// Calculates the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The squared distance between the two vectors.</returns>
    public static double DistanceSquared(Double4 value1, Double4 value2)
    {
        DistanceSquared(ref value1, ref value2, out double result);
        return result;
    }

    /// <summary>
    /// Tests whether one vector is near another vector.
    /// </summary>
    /// <param name="left">The left vector.</param>
    /// <param name="right">The right vector.</param>
    /// <param name="epsilon">The epsilon.</param>
    /// <returns><c>true</c> if left and right are near another, <c>false</c> otherwise</returns>
    public static bool NearEqual(Double4 left, Double4 right, double epsilon = Mathd.Epsilon)
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
    public static bool NearEqual(ref Double4 left, ref Double4 right, double epsilon = Mathd.Epsilon)
    {
        return Mathd.WithinEpsilon(left.X, right.X, epsilon) && Mathd.WithinEpsilon(left.Y, right.Y, epsilon) && Mathd.WithinEpsilon(left.Z, right.Z, epsilon) && Mathd.WithinEpsilon(left.W, right.W, epsilon);
    }

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="left">First source vector</param>
    /// <param name="right">Second source vector.</param>
    /// <param name="result">When the method completes, contains the dot product of the two vectors.</param>
    public static void Dot(ref Double4 left, ref Double4 right, out double result)
    {
        ref Vector256<double> vLeft = ref VectorExtensions.AsVector256(ref left);
        ref Vector256<double> vRight = ref VectorExtensions.AsVector256(ref right);
        result = Vector256.Dot(vLeft, vRight);
    }

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="left">First source vector.</param>
    /// <param name="right">Second source vector.</param>
    /// <returns>The dot product of the two vectors.</returns>
    public static double Dot(Double4 left, Double4 right)
    {
        Dot(ref left, ref right, out double result);
        return result;
    }

    /// <summary>
    /// Converts the vector into a unit vector.
    /// </summary>
    /// <param name="value">The vector to normalize.</param>
    /// <param name="result">When the method completes, contains the normalized vector.</param>
    public static void Normalize(ref Double4 value, out Double4 result)
    {
        result = value;
        result.Normalize();
    }

    /// <summary>
    /// Converts the vector into a unit vector.
    /// </summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    public static Double4 Normalize(Double4 value)
    {
        value.Normalize();
        return value;
    }

    /// <summary>
    /// Makes sure that Length of the output vector is always below max and above 0.
    /// </summary>
    /// <param name="vector">Input Vector.</param>
    /// <param name="max">Max Length</param>
    public static Double4 ClampLength(Double4 vector, double max)
    {
        return ClampLength(vector, 0, max);
    }

    /// <summary>
    /// Makes sure that Length of the output vector is always below max and above min.
    /// </summary>
    /// <param name="vector">Input Vector.</param>
    /// <param name="min">Min Length</param>
    /// <param name="max">Max Length</param>
    public static Double4 ClampLength(Double4 vector, double min, double max)
    {
        ClampLength(vector, min, max, out Double4 result);
        return result;
    }

    /// <summary>
    /// Makes sure that Length of the output vector is always below max and above min.
    /// </summary>
    /// <param name="vector">Input Vector.</param>
    /// <param name="min">Min Length</param>
    /// <param name="max">Max Length</param>
    /// <param name="result">The result vector.</param>
    public static void ClampLength(Double4 vector, double min, double max, out Double4 result)
    {
        result = vector;
        Vector256<double> vVector = vector.AsVector256();
        double lenSq = Vector256.Sum(vVector * vVector);
        if (lenSq > max * max)
        {
            Vector256<double> scaleFactor = Vector256.Create(max / Math.Sqrt(lenSq));
            result = VectorExtensions.AsVector4(scaleFactor * vVector);
        }
        if (lenSq < min * min)
        {
            Vector256<double> scaleFactor = Vector256.Create(min / Math.Sqrt(lenSq));
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
    public static void Lerp(ref Double4 start, ref Double4 end, double amount, out Double4 result)
    {
        ref Vector256<double> vStart = ref VectorExtensions.AsVector256(ref start);
        ref Vector256<double> vEnd = ref VectorExtensions.AsVector256(ref end);
        Vector256<double> vAmount = Vector256.Create(amount);
        Vector256<double> vResult = vStart + (vEnd - vStart) * vAmount;
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
    public static Double4 Lerp(Double4 start, Double4 end, double amount)
    {
        Lerp(ref start, ref end, amount, out Double4 result);
        return result;
    }

    /// <summary>
    /// Performs a cubic interpolation between two vectors.
    /// </summary>
    /// <param name="start">Start vector.</param>
    /// <param name="end">End vector.</param>
    /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end" />.</param>
    /// <param name="result">When the method completes, contains the cubic interpolation of the two vectors.</param>
    public static void SmoothStep(ref Double4 start, ref Double4 end, double amount, out Double4 result)
    {
        amount = Mathd.SmoothStep(amount);
        Lerp(ref start, ref end, amount, out result);
    }

    /// <summary>
    /// Performs a cubic interpolation between two vectors.
    /// </summary>
    /// <param name="start">Start vector.</param>
    /// <param name="end">End vector.</param>
    /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end" />.</param>
    /// <returns>The cubic interpolation of the two vectors.</returns>
    public static Double4 SmoothStep(Double4 start, Double4 end, double amount)
    {
        SmoothStep(ref start, ref end, amount, out Double4 result);
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
    public static void Hermite(ref Double4 value1, ref Double4 tangent1, ref Double4 value2, ref Double4 tangent2, double amount, out Double4 result)
    {
        double squared = amount * amount;
        double cubed = amount * squared;

        double part1 = 2.0 * cubed - 3.0 * squared + 1.0;
        double part2 = -2.0 * cubed + 3.0 * squared;
        double part3 = cubed - 2.0 * squared + amount;
        double part4 = cubed - squared;

        Vector256<double> v1 = VectorExtensions.AsVector256(ref value1);
        Vector256<double> t1 = VectorExtensions.AsVector256(ref tangent1);
        Vector256<double> v2 = VectorExtensions.AsVector256(ref value2);
        Vector256<double> t2 = VectorExtensions.AsVector256(ref tangent2);

        Vector256<double> vp1 = Vector256.Create(part1);
        Vector256<double> vp2 = Vector256.Create(part2);
        Vector256<double> vp3 = Vector256.Create(part3);
        Vector256<double> vp4 = Vector256.Create(part4);

        Vector256<double> vResult = (v1 * vp1) + (v2 * vp2) + (t1 * vp3) + (t2 * vp4);

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
    public static Double4 Hermite(Double4 value1, Double4 tangent1, Double4 value2, Double4 tangent2, double amount)
    {
        Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out Double4 result);
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
    public static void CatmullRom(ref Double4 value1, ref Double4 value2, ref Double4 value3, ref Double4 value4, double amount, out Double4 result)
    {
        double squared = amount * amount;
        double cubed = amount * squared;

        Vector256<double> v1 = VectorExtensions.AsVector256(ref value1);
        Vector256<double> v2 = VectorExtensions.AsVector256(ref value2);
        Vector256<double> v3 = VectorExtensions.AsVector256(ref value3);
        Vector256<double> v4 = VectorExtensions.AsVector256(ref value4);

        Vector256<double> vT = Vector256.Create(amount);
        Vector256<double> vT2 = Vector256.Create(squared);
        Vector256<double> vT3 = Vector256.Create(cubed);

        Vector256<double> term0 = Vector256.Create(2.0) * v2;
        Vector256<double> term1 = (-v1 + v3) * vT;
        Vector256<double> term2 = (Vector256.Create(2.0) * v1 - Vector256.Create(5.0) * v2 + Vector256.Create(4.0) * v3 - v4) * vT2;
        Vector256<double> term3 = (-v1 + Vector256.Create(3.0) * v2 - Vector256.Create(3.0) * v3 + v4) * vT3;

        result = (Vector256.Create(0.5) * (term0 + term1 + term2 + term3)).AsVector4();
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
    public static Double4 CatmullRom(Double4 value1, Double4 value2, Double4 value3, Double4 value4, double amount)
    {
        CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out Double4 result);
        return result;
    }

    /// <summary>
    /// Returns a vector containing the largest components of the specified vectors.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <param name="result">When the method completes, contains an new vector composed of the largest components of the source vectors.</param>
    public static void Max(ref Double4 left, ref Double4 right, out Double4 result)
    {
        ref Vector256<double> vLeft = ref VectorExtensions.AsVector256(ref left);
        ref Vector256<double> vRight = ref VectorExtensions.AsVector256(ref right);
        Vector256<double> vResult = Vector256.Max(vLeft, vRight);
        result = vResult.AsVector4();
    }

    /// <summary>
    /// Returns a vector containing the largest components of the specified vectors.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>A vector containing the largest components of the source vectors.</returns>
    public static Double4 Max(Double4 left, Double4 right)
    {
        Max(ref left, ref right, out Double4 result);
        return result;
    }

    /// <summary>
    /// Returns a vector containing the smallest components of the specified vectors.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <param name="result">When the method completes, contains an new vector composed of the smallest components of the source vectors.</param>
    public static void Min(ref Double4 left, ref Double4 right, out Double4 result)
    {
        ref Vector256<double> vLeft = ref VectorExtensions.AsVector256(ref left);
        ref Vector256<double> vRight = ref VectorExtensions.AsVector256(ref right);
        Vector256<double> vResult = Vector256.Min(vLeft, vRight);
        result = vResult.AsVector4();
    }

    /// <summary>
    /// Returns a vector containing the smallest components of the specified vectors.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>A vector containing the smallest components of the source vectors.</returns>
    public static Double4 Min(Double4 left, Double4 right)
    {
        Min(ref left, ref right, out Double4 result);
        return result;
    }

    /// <summary>
    /// Returns the absolute value of a vector.
    /// </summary>
    /// <param name="v">The value.</param>
    /// <returns> A vector which components are less or equal to 0.</returns>
    public static Double4 Abs(Double4 v)
    {
        Vector256<double> result = Vector256.Abs(v.AsVector256());
        return result.AsVector4();
    }

    /// <summary>
    /// Transforms a 4D vector by the given <see cref="Quaternion" /> rotation.
    /// </summary>
    /// <param name="vector">The vector to rotate.</param>
    /// <param name="rotation">The <see cref="Quaternion" /> rotation to apply.</param>
    /// <param name="result">When the method completes, contains the transformed <see cref="Double4" />.</param>
    public static void Transform(ref Double4 vector, ref Quaternion rotation, out Double4 result)
    {
        double x = rotation.X + rotation.X;
        double y = rotation.Y + rotation.Y;
        double z = rotation.Z + rotation.Z;
        double wx = rotation.W * x, wy = rotation.W * y, wz = rotation.W * z;
        double xx = rotation.X * x, xy = rotation.X * y, xz = rotation.X * z;
        double yy = rotation.Y * y, yz = rotation.Y * z, zz = rotation.Z * z;

        Vector256<double> vX = Vector256.Create(vector.X);
        Vector256<double> vY = Vector256.Create(vector.Y);
        Vector256<double> vZ = Vector256.Create(vector.Z);

        Vector256<double> col1 = Vector256.Create(1.0 - yy - zz, xy + wz, xz - wy, 0.0);
        Vector256<double> col2 = Vector256.Create(xy - wz, 1.0 - xx - zz, yz + wx, 0.0);
        Vector256<double> col3 = Vector256.Create(xz + wy, yz - wx, 1.0 - xx - yy, 0.0);

        Vector256<double> vResult = (vX * col1) + (vY * col2) + (vZ * col3);

        result = VectorExtensions.AsVector4(ref vResult);

        result.W = vector.W;
    }

    /// <summary>
    /// Transforms a 4D vector by the given <see cref="Quaternion" /> rotation.
    /// </summary>
    /// <param name="vector">The vector to rotate.</param>
    /// <param name="rotation">The <see cref="Quaternion" /> rotation to apply.</param>
    /// <returns>The transformed <see cref="Double4" />.</returns>
    public static Double4 Transform(Double4 vector, Quaternion rotation)
    {
        Transform(ref vector, ref rotation, out Double4 result);
        return result;
    }

    /// <summary>
    /// Transforms a 4D vector by the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <param name="result">When the method completes, contains the transformed <see cref="Double4" />.</param>
    public static void Transform(ref Double4 vector, ref Matrix transform, out Double4 result)
    {
        Vector256<double> vX = Vector256.Create(vector.X);
        Vector256<double> vY = Vector256.Create(vector.Y);
        Vector256<double> vZ = Vector256.Create(vector.Z);
        Vector256<double> vW = Vector256.Create(vector.W);

        // Explicitly create rows to avoid Unsafe.As float vs double precision traps depending on USE_LARGE_WORLDS
        Vector256<double> row1 = Vector256.Create((double)transform.M11, (double)transform.M12, (double)transform.M13, (double)transform.M14);
        Vector256<double> row2 = Vector256.Create((double)transform.M21, (double)transform.M22, (double)transform.M23, (double)transform.M24);
        Vector256<double> row3 = Vector256.Create((double)transform.M31, (double)transform.M32, (double)transform.M33, (double)transform.M34);
        Vector256<double> row4 = Vector256.Create((double)transform.M41, (double)transform.M42, (double)transform.M43, (double)transform.M44);

        Vector256<double> vResult = (vX * row1) + (vY * row2) + (vZ * row3) + (vW * row4);

        result = VectorExtensions.AsVector4(ref vResult);
    }

    /// <summary>
    /// Transforms a 4D vector by the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <returns>The transformed <see cref="Double4" />.</returns>
    public static Double4 Transform(Double4 vector, Matrix transform)
    {
        Transform(ref vector, ref transform, out Double4 result);
        return result;
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>The sum of the two vectors.</returns>
    public static Double4 operator +(Double4 left, Double4 right)
    {
        Add(ref left, ref right, out Double4 result);
        return result;
    }

    /// <summary>
    /// Multiplies a vector with another by performing component-wise multiplication equivalent to <see cref="Multiply(ref Double4,ref Double4,out Double4)" />.
    /// </summary>
    /// <param name="left">The first vector to multiply.</param>
    /// <param name="right">The second vector to multiply.</param>
    /// <returns>The multiplication of the two vectors.</returns>
    public static Double4 operator *(Double4 left, Double4 right)
    {
        Multiply(ref left, ref right, out Double4 result);
        return result;
    }

    /// <summary>
    /// Assert a vector (return it unchanged).
    /// </summary>
    /// <param name="value">The vector to assert (unchanged).</param>
    /// <returns>The asserted (unchanged) vector.</returns>
    public static Double4 operator +(Double4 value)
    {
        return value;
    }

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    /// <param name="left">The first vector to subtract.</param>
    /// <param name="right">The second vector to subtract.</param>
    /// <returns>The difference of the two vectors.</returns>
    public static Double4 operator -(Double4 left, Double4 right)
    {
        Subtract(ref left, ref right, out Double4 result);
        return result;
    }

    /// <summary>
    /// Reverses the direction of a given vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>A vector facing in the opposite direction.</returns>
    public static Double4 operator -(Double4 value)
    {
        Negate(ref value, out Double4 result);
        return result;
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Double4 operator *(double scale, Double4 value)
    {
        Vector256<double> result = Vector256.Create(scale) * value.AsVector256();
        return result.AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Double4 operator *(Double4 value, double scale)
    {
        Vector256<double> result = value.AsVector256() * Vector256.Create(scale);
        return result.AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Double4 operator /(Double4 value, double scale)
    {
        Vector256<double> result = value.AsVector256() / Vector256.Create(scale);
        return result.AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <param name="value">The vector to scale.</param>
    /// <returns>The scaled vector.</returns>
    public static Double4 operator /(double scale, Double4 value)
    {
        Vector256<double> result = Vector256.Create(scale) / value.AsVector256();
        return result.AsVector4();
    }

    /// <summary>
    /// Scales a vector by the given value.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Double4 operator /(Double4 value, Double4 scale)
    {
        Vector256<double> result = value.AsVector256() / scale.AsVector256();
        return result.AsVector4();
    }

    /// <summary>
    /// Remainder of value divided by scale.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The remained vector.</returns>
    public static Double4 operator %(Double4 value, double scale)
    {
        Vector256<double> vValue = value.AsVector256();
        Vector256<double> vScale = Vector256.Create(scale);

        Vector256<double> div = vValue / vScale;
        Vector256<double> trunc = Vector256.Truncate(div);
        Vector256<double> result = vValue - (vScale * trunc);

        return result.AsVector4();
    }

    /// <summary>
    /// Remainder of value divided by scale.
    /// </summary>
    /// <param name="value">The amount by which to scale the vector.</param>
    /// <param name="scale">The vector to scale.</param>
    /// <returns>The remained vector.</returns>
    public static Double4 operator %(double value, Double4 scale)
    {
        Vector256<double> vValue = Vector256.Create(value);
        Vector256<double> vScale = scale.AsVector256();

        Vector256<double> div = vValue / vScale;
        Vector256<double> trunc = Vector256.Truncate(div);
        Vector256<double> result = vValue - (vScale * trunc);

        return result.AsVector4();
    }

    /// <summary>
    /// Remainder of value divided by scale.
    /// </summary>
    /// <param name="value">The vector to scale.</param>
    /// <param name="scale">The amount by which to scale the vector.</param>
    /// <returns>The remained vector.</returns>
    public static Double4 operator %(Double4 value, Double4 scale)
    {
        Vector256<double> vValue = value.AsVector256();
        Vector256<double> vScale = scale.AsVector256();

        Vector256<double> div = vValue / vScale;
        Vector256<double> trunc = Vector256.Truncate(div);
        Vector256<double> result = vValue - (vScale * trunc);

        return result.AsVector4();
    }

    /// <summary>
    /// Performs a component-wise addition.
    /// </summary>
    /// <param name="value">The input vector.</param>
    /// <param name="scalar">The scalar value to be added on elements</param>
    /// <returns>The vector with added scalar for each element.</returns>
    public static Double4 operator +(Double4 value, double scalar)
    {
        Vector256<double> result = value.AsVector256() + Vector256.Create(scalar);
        return result.AsVector4();
    }

    /// <summary>
    /// Performs a component-wise addition.
    /// </summary>
    /// <param name="value">The input vector.</param>
    /// <param name="scalar">The scalar value to be added on elements</param>
    /// <returns>The vector with added scalar for each element.</returns>
    public static Double4 operator +(double scalar, Double4 value)
    {
        Vector256<double> result = Vector256.Create(scalar) + value.AsVector256();
        return result.AsVector4();
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="value">The input vector.</param>
    /// <param name="scalar">The scalar value to be subtracted from elements</param>
    /// <returns>The vector with subtracted scalar from each element.</returns>
    public static Double4 operator -(Double4 value, double scalar)
    {
        Vector256<double> result = value.AsVector256() - Vector256.Create(scalar);
        return result.AsVector4();
    }

    /// <summary>
    /// Performs a component-wise subtraction.
    /// </summary>
    /// <param name="value">The input vector.</param>
    /// <param name="scalar">The scalar value to be subtracted from elements</param>
    /// <returns>The vector with subtracted scalar from each element.</returns>
    public static Double4 operator -(double scalar, Double4 value)
    {
        Vector256<double> result = Vector256.Create(scalar) - value.AsVector256();
        return result.AsVector4();
    }

    /// <summary>
    /// Tests for equality between two objects.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns><c>true</c> if <paramref name="left" /> has the same value as <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Double4 left, Double4 right)
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
    public static bool operator !=(Double4 left, Double4 right)
    {
        return !left.Equals(ref right);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="Double3" /> to <see cref="Float4" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Float4(Double4 value)
    {
        return new Float4((float)value.X, (float)value.Y, (float)value.Z, (float)value.W);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="Double3" /> to <see cref="Vector4" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Vector4(Double4 value)
    {
        return new Vector4((Real)value.X, (Real)value.Y, (Real)value.Z, (Real)value.W);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="Double4" /> to <see cref="Double2" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Double2(Double4 value)
    {
        return new Double2(value.X, value.Y);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="Double4" /> to <see cref="Double3" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Double3(Double4 value)
    {
        return new Double3(value.X, value.Y, value.Z);
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
        if (format == null)
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
        if (format == null)
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
        var o = (Double4)other;
        return Equals(ref o);
    }

    /// <summary>
    /// Determines whether the specified <see cref="Double4" /> is equal to this instance.
    /// </summary>
    /// <param name="other">The <see cref="Double4" /> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="Double4" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public readonly bool Equals(ref Double4 other)
    {
        ref Vector256<double> vOther = ref VectorExtensions.AsVector256(ref other);
        return this.AsVector256().Equals(vOther);
    }

    /// <summary>
    /// Determines whether the specified <see cref="Double4" /> is equal to this instance.
    /// </summary>
    /// <param name="other">The <see cref="Double4" /> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="Double4" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Double4 other)
    {
        return this.AsVector256().Equals(other.AsVector256());
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="value">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override readonly bool Equals(object value)
    {
        return value is Double4 other && Equals(ref other);
    }
}
