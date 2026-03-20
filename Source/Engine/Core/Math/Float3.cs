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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace FlaxEngine;

[Serializable]
#if FLAX_EDITOR
[System.ComponentModel.TypeConverter(typeof(TypeConverters.Float3Converter))]
#endif
partial struct Float3 : IVector3<Float3, float>, ITrigonometricVector<Float3, float>, Json.ICustomValueEquals
{
    private static readonly string _formatString = "X:{0:F2} Y:{1:F2} Z:{2:F2}";

    /// <inheritdoc/>
    public static Float3 Zero { get; } = new();

    /// <summary>
    /// The X unit <see cref="Float3" /> (1, 0, 0).
    /// </summary>
    public static Float3 UnitX { get; } = new(1.0f, 0.0f, 0.0f);

    /// <summary>
    /// The Y unit <see cref="Float3" /> (0, 1, 0).
    /// </summary>
    public static Float3 UnitY { get; } = new(0.0f, 1.0f, 0.0f);

    /// <summary>
    /// The Z unit <see cref="Float3" /> (0, 0, 1).
    /// </summary>
    public static Float3 UnitZ { get; } = new(0.0f, 0.0f, 1.0f);

    /// <inheritdoc/>
    public static Float3 One { get; } = new(1.0f, 1.0f, 1.0f);

    /// <inheritdoc/>
    public static Float3 Half { get; } = new(0.5f, 0.5f, 0.5f);

    /// <summary>
    /// A unit <see cref="Float3" /> designating up (0, 1, 0).
    /// </summary>
    public static Float3 Up { get; } = new(0.0f, 1.0f, 0.0f);

    /// <summary>
    /// A unit <see cref="Float3" /> designating down (0, -1, 0).
    /// </summary>
    public static readonly Float3 Down = new(0.0f, -1.0f, 0.0f);

    /// <summary>
    /// A unit <see cref="Float3" /> designating left (-1, 0, 0).
    /// </summary>
    public static readonly Float3 Left = new(-1.0f, 0.0f, 0.0f);

    /// <summary>
    /// A unit <see cref="Float3" /> designating right (1, 0, 0).
    /// </summary>
    public static Float3 Right { get; } = new(1.0f, 0.0f, 0.0f);

    /// <summary>
    /// A unit <see cref="Float3" /> designating forward in a left-handed coordinate system (0, 0, 1).
    /// </summary>
    public static Float3 Forward { get; } = new(0.0f, 0.0f, 1.0f);

    /// <summary>
    /// A unit <see cref="Float3" /> designating backward in a left-handed coordinate system (0, 0, -1).
    /// </summary>
    public static Float3 Backward { get; } = new(0.0f, 0.0f, -1.0f);

    /// <inheritdoc/>
    public static Float3 Minimum { get; } = new(float.MinValue);

    /// <inheritdoc/>
    public static Float3 Maximum { get; } = new(float.MaxValue);

    /// <summary>
    /// Gets a value indicting whether this instance is normalized.
    /// </summary>
    public readonly bool IsNormalized => Mathf.Abs(ComputeLengthSquared(in this, out _) - 1.0f) < 1e-4f;

    /// <summary>
    /// Gets the normalized vector. Returned vector has length equal 1.
    /// </summary>
    public readonly Float3 Normalized
    {
        get
        {
            Float3 result = this;
            result.Normalize();
            return result;
        }
    }

    /// <inheritdoc/>
    public readonly bool IsZero => Mathf.IsZero(X) && Mathf.IsZero(Y) && Mathf.IsZero(Z);

    /// <inheritdoc/>
    public readonly bool IsOne => Mathf.IsOne(X) && Mathf.IsOne(Y) && Mathf.IsOne(Z);

    /// <inheritdoc/>
    public readonly float MinValue => Mathf.Min(X, Mathf.Min(Y, Z));

    /// <inheritdoc/>
    public readonly float MaxValue => Mathf.Max(X, Mathf.Max(Y, Z));

    /// <inheritdoc/>
    public readonly float AvgValue
    {
        get
        {
            const float InverseCount = 1.0f / 3.0f;
            return Vector128.Sum(this.AsVector128()) * InverseCount;
        }
    }

    /// <inheritdoc/>
    public readonly float ValuesSum => Vector128.Sum(this.AsVector128());

    /// <inheritdoc/>
    public static int Count => 3;

    /// <inheritdoc/>
    public readonly float Length
    {
        get
        {
            float lengthSqr = ComputeLengthSquared(in this, out _);
            return lengthSqr * MathF.ReciprocalSqrtEstimate(lengthSqr);
        }
    }

    /// <inheritdoc/>
    public readonly float LengthSquared => ComputeLengthSquared(in this, out _);

    /// <inheritdoc/>
    public readonly float PreciseLength
    {
        get
        {
            float lengthSqr = ComputeLengthSquared(in this, out _);
            return MathF.Sqrt(lengthSqr);
        }
    }

    float IVector2<Float3, float>.X
    {
        readonly get => X;
        set => X = value;
    }

    float IVector2<Float3, float>.Y
    {
        readonly get => Y;
        set => Y = value;
    }

    float IVector3<Float3, float>.Z
    {
        readonly get => Z;
        set => Z = value;
    }

    /// <inheritdoc/>
    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            Float3.ThrowIfOutOfRange(index);
            return VectorMath.GetRef<Float3, float>(ref Unsafe.AsRef(in this), index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            Float3.ThrowIfOutOfRange(index);
            VectorMath.GetRef<Float3, float>(ref this, index) = value;
        }
    }

    /// <param name="value">The value that will be assigned to all components.</param>
    /// <inheritdoc cref="Float3(float, float, float)"/>
    public Float3(float value)
    {
        X = value;
        Y = value;
        Z = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Float3" /> struct.
    /// </summary>
    /// <param name="x">Initial value for the X component of the vector.</param>
    /// <param name="y">Initial value for the Y component of the vector.</param>
    /// <param name="z">Initial value for the Z component of the vector.</param>
    public Float3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
    /// <inheritdoc cref="Float3(float, float, float)"/>
    /// <param name="z"/>
    public Float3(Float2 value, float z)
    {
        X = value.X;
        Y = value.Y;
        Z = z;
    }

    /// <param name="value">A vector containing the values with which to initialize the X, Y and Z components.</param>
    /// <inheritdoc cref="Float3(float, float, float)"/>
    public Float3(Vector3 value)
    {
        X = (float)value.X;
        Y = (float)value.Y;
        Z = (float)value.Z;
    }

    /// <param name="value">A vector containing the values with which to initialize the X, Y and Z components.</param>
    /// <inheritdoc cref="Float3(float, float, float)"/>
    public Float3(Double3 value)
    {
        X = (float)value.X;
        Y = (float)value.Y;
        Z = (float)value.Z;
    }

    /// <param name="value">A vector containing the values with which to initialize the X, Y and Z components.</param>
    /// <inheritdoc cref="Float3(float, float, float)"/>
    public Float3(Float4 value)
    {

        X = value.X;
        Y = value.Y;
        Z = value.Z;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Float3" /> struct.
    /// </summary>
    /// <returns/>
    /// <inheritdoc cref="Create(ReadOnlySpan{float})"/>
    public Float3(ReadOnlySpan<float> values)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(values.Length, Count, nameof(values));
        X = values[0];
        Y = values[1];
        Z = values[2];
    }

    /// <inheritdoc/>
    public static Float3 Create(float value) => new(value);

    /// <inheritdoc/>
    public static Float3 Create(ReadOnlySpan<float> values) => new(values);

    /// <inheritdoc/>
    public void Normalize()
    {
        float lengthSqr = ComputeLengthSquared(in this, out Vector128<float> vector);
        Vector128<float> result = vector * MathF.ReciprocalSqrtEstimate(lengthSqr);
        this = result.AsVector3();
    }

    /// <inheritdoc/>
    public void NormalizePrecise()
    {
        float lengthSqr = ComputeLengthSquared(in this, out Vector128<float> vector);
        Vector128<float> result = vector * (1.0f / MathF.Sqrt(lengthSqr));
        this = result.AsVector3();
    }

    /// <inheritdoc/>
    public readonly float[] ToArray() => [X, Y, Z];

    /// <inheritdoc/>
    public static Float3 Add(in Float3 left, in Float3 right)
    {
        Vector128<float> result = left.AsVector128Unsafe() + right.AsVector128Unsafe();
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Subtract(in Float3 left, in Float3 right)
    {
        Vector128<float> result = left.AsVector128Unsafe() - right.AsVector128Unsafe();
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Multiply(in Float3 left, in Float3 right)
    {
        Vector128<float> result = left.AsVector128Unsafe() * right.AsVector128Unsafe();
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Divide(in Float3 left, in Float3 right)
    {
        Vector128<float> result = left.AsVector128Unsafe() / right.AsVector128Unsafe();
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Modulus(in Float3 left, in Float3 right)
    {
        Vector128<float> result = VectorMath.Modulus(left.AsVector128Unsafe(), right.AsVector128Unsafe());
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Negate(in Float3 value)
    {
        Vector128<float> result = -value.AsVector128Unsafe();
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Barycentric(in Float3 value1, in Float3 value2, in Float3 value3, float amount1, float amount2)
    {
        Vector128<float> origin = value1.AsVector128Unsafe();
        Vector128<float> edge1 = value2.AsVector128Unsafe() - origin;
        Vector128<float> edge2 = value3.AsVector128Unsafe() - origin;
        Vector128<float> result = origin + Vector128.Create(amount1) * edge1 + Vector128.Create(amount2) * edge2;
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Clamp(in Float3 value, in Float3 min, in Float3 max)
    {
        Vector128<float> result = Vector128.Clamp(value.AsVector128Unsafe(), min.AsVector128Unsafe(), max.AsVector128Unsafe());
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Cross(in Float3 left, in Float3 right)
    {
        Vector128<float> v1 = left.AsVector128Unsafe();
        Vector128<float> v2 = right.AsVector128Unsafe();

        Vector128<float> temp1 = Vector128.Shuffle(v1, Vector128.Create(1, 2, 0, 0)) * Vector128.Shuffle(v2, Vector128.Create(2, 0, 1, 0));
        Vector128<float> temp2 = Vector128.Shuffle(v1, Vector128.Create(2, 0, 1, 0)) * Vector128.Shuffle(v2, Vector128.Create(1, 2, 0, 0));

        return (temp1 - temp2).AsVector3();
    }

    /// <inheritdoc/>
    public static bool NearEqual(in Float3 left, in Float3 right, float threshold)
    {
        Vector128<float> difference = Vector128.Abs(left.AsVector128() - right.AsVector128());
        return Vector128.LessThanOrEqualAll(difference, Vector128.Create(threshold));
    }

    /// <inheritdoc/>
    public static float Dot(in Float3 left, in Float3 right)
    {
        return Vector128.Dot(left.AsVector128Unsafe(), right.AsVector128Unsafe());
    }

    /// <summary>
    /// Converts the vector into a unit vector.
    /// </summary>
    /// <param name="value">The vector to normalize.</param>
    /// <param name="result">When the method completes, contains the normalized vector.</param>
    public static void Normalize(ref Float3 value, out Float3 result)
    {
        result = value;
        result.Normalize();
    }

    /// <summary>
    /// Converts the vector into a unit vector.
    /// </summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    public static Float3 Normalize(in Float3 value)
    {
        value.Normalize();
        return value;
    }

    /// <inheritdoc/>
    public static Float3 ClampLength(in Float3 vector, float min, float max) => ComputeLengthSquared(in vector, out Vector128<float> vVector) switch
    {
        float lengthSqr when lengthSqr < (min * min) => Scale(in vVector, min * MathF.ReciprocalSqrtEstimate(lengthSqr)),
        float lengthSqr when lengthSqr > (max * max) => Scale(in vVector, max * MathF.ReciprocalSqrtEstimate(lengthSqr)),
        _ => vector,
    };

    /// <inheritdoc/>
    public static Float3 Lerp(in Float3 start, in Float3 end, float amount)
    {
        Vector128<float> vStart = start.AsVector128Unsafe();
        Vector128<float> result = vStart + (end.AsVector128Unsafe() - vStart) * Vector128.Create(amount);
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 SmoothStep(in Float3 start, in Float3 end, float amount) => Lerp(in start, in end, Mathf.SmoothStep(amount));

    /// <summary>
    /// Moves a value current towards target.
    /// </summary>
    /// <param name="current">The position to move from.</param>
    /// <param name="target">The position to move towards.</param>
    /// <param name="maxDistanceDelta">The maximum distance that can be applied to the value.</param>
    /// <returns>The new position.</returns>
    public static Float3 MoveTowards(in Float3 current, in Float3 target, float maxDistanceDelta)
    {
        float distanceSqr = ComputeLengthSquared(target - current, out Vector128<float> to);

        if (distanceSqr != 0 && (maxDistanceDelta < 0 || distanceSqr > maxDistanceDelta * maxDistanceDelta))
        {
            float scale = maxDistanceDelta * MathF.ReciprocalSqrtEstimate(distanceSqr);
            Vector128<float> result = current.AsVector128Unsafe() + to * Vector128.Create(scale);
            return result.AsVector3();
        }

        return target;
    }

    /// <inheritdoc/>
    public static Float3 CatmullRom(in Float3 value1, in Float3 value2, in Float3 value3, in Float3 value4, float amount)
    {
        VectorMath.Cube(amount, out float squared, out float cubed);
        
        Vector128<float> v1 = value1.AsVector128Unsafe();
        Vector128<float> v2 = value2.AsVector128Unsafe();
        Vector128<float> v3 = value3.AsVector128Unsafe();
        Vector128<float> v4 = value4.AsVector128Unsafe();
        
        Vector128<float> vAmount = Vector128.Create(amount);
        Vector128<float> vSquared = Vector128.Create(squared);
        Vector128<float> vCubed = Vector128.Create(cubed);
        
        Vector128<float> term1 = Vector128.Create(2.0f) * v2;
        Vector128<float> term2 = (v3 - v1) * vAmount;
        Vector128<float> term3 = (Vector128.Create(2.0f) * v1 - Vector128.Create(5.0f) * v2 + Vector128.Create(4.0f) * v3 - v4) * vSquared;
        Vector128<float> term4 = (-v1 + Vector128.Create(3.0f) * v2 - Vector128.Create(3.0f) * v3 + v4) * vCubed;
        
        Vector128<float> result = Vector128.Create(0.5f) * (term1 + term2 + term3 + term4);
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Hermite(in Float3 value1, in Float3 tangent1, in Float3 value2, in Float3 tangent2, float amount)
    {
        VectorMath.Cube(amount, out float squared, out float cubed);
        float part1 = 2.0f * cubed - 3.0f * squared + 1.0f;
        float part2 = -2.0f * cubed + 3.0f * squared;
        float part3 = cubed - 2.0f * squared + amount;
        float part4 = cubed - squared;
        Unsafe.SkipInit(out Float3 result);
        result.X = value1.X * part1 + value2.X * part2 + tangent1.X * part3 + tangent2.X * part4;
        result.Y = value1.Y * part1 + value2.Y * part2 + tangent1.Y * part3 + tangent2.Y * part4;
        result.Z = value1.Z * part1 + value2.Z * part2 + tangent1.Z * part3 + tangent2.Z * part4;
        return result;
    }

    /// <inheritdoc/>
    public static Float3 Max(in Float3 left, in Float3 right)
    {
        Vector128<float> result = Vector128.Max(left.AsVector128Unsafe(), right.AsVector128Unsafe());
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Min(in Float3 left, in Float3 right)
    {
        Vector128<float> result = Vector128.Min(left.AsVector128Unsafe(), right.AsVector128Unsafe());
        return result.AsVector3();
    }

    /// <inheritdoc/>
    public static Float3 Abs(in Float3 value)
    {
        Vector128<float> result = Vector128.Abs(value.AsVector128Unsafe());
        return result.AsVector3();
    }

    /// <summary>
    /// Projects a vector onto another vector.
    /// </summary>
    /// <param name="vector">The vector to project.</param>
    /// <param name="onNormal">The projection normal vector.</param>
    /// <returns>The projected vector.</returns>
    public static Float3 Project(Float3 vector, Float3 onNormal)
    {
        float sqrMag = Dot(onNormal, onNormal);
        return sqrMag < Mathf.Epsilon ? Zero : onNormal * Dot(vector, onNormal) / sqrMag;
    }

    /// <summary>
    /// Projects a vector onto a plane defined by a normal orthogonal to the plane.
    /// </summary>
    /// <param name="vector">The vector to project.</param>
    /// <param name="planeNormal">The plane normal vector.</param>
    /// <returns>The projected vector.</returns>
    public static Float3 ProjectOnPlane(Float3 vector, Float3 planeNormal)
    {
        return vector - Project(vector, planeNormal);
    }

    /// <summary>
    /// Calculates the angle (in degrees) between <paramref name="from"/> and <paramref name="to"/>. This is always the smallest value.
    /// </summary>
    /// <param name="from">The first vector.</param>
    /// <param name="to">The second vector.</param>
    /// <returns>The angle (in degrees).</returns>
    public static float Angle(Float3 from, Float3 to)
    {
        float dot = Mathf.Clamp(Dot(from.Normalized, to.Normalized), -1.0f, 1.0f);
        return Mathf.Abs(dot) > (1.0f - Mathf.Epsilon) ? dot > 0.0f ? 0.0f : 180.0f : MathF.Acos(dot) * Mathf.RadiansToDegrees;
    }

    /// <summary>
    /// Projects a 3D vector from object space into screen space.
    /// </summary>
    /// <param name="vector">The vector to project.</param>
    /// <param name="x">The X position of the viewport.</param>
    /// <param name="y">The Y position of the viewport.</param>
    /// <param name="width">The width of the viewport.</param>
    /// <param name="height">The height of the viewport.</param>
    /// <param name="minZ">The minimum depth of the viewport.</param>
    /// <param name="maxZ">The maximum depth of the viewport.</param>
    /// <param name="worldViewProjection">The combined world-view-projection matrix.</param>
    /// <param name="result">When the method completes, contains the vector in screen space.</param>
    public static void Project(ref Float3 vector, float x, float y, float width, float height, float minZ, float maxZ, ref Matrix worldViewProjection, out Float3 result)
    {
        TransformCoordinate(ref vector, ref worldViewProjection, out var v);
        result = new Float3((1.0f + v.X) * 0.5f * width + x, (1.0f - v.Y) * 0.5f * height + y, v.Z * (maxZ - minZ) + minZ);
    }

    /// <summary>
    /// Projects a 3D vector from object space into screen space.
    /// </summary>
    /// <param name="vector">The vector to project.</param>
    /// <param name="x">The X position of the viewport.</param>
    /// <param name="y">The Y position of the viewport.</param>
    /// <param name="width">The width of the viewport.</param>
    /// <param name="height">The height of the viewport.</param>
    /// <param name="minZ">The minimum depth of the viewport.</param>
    /// <param name="maxZ">The maximum depth of the viewport.</param>
    /// <param name="worldViewProjection">The combined world-view-projection matrix.</param>
    /// <returns>The vector in screen space.</returns>
    public static Float3 Project(Float3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix worldViewProjection)
    {
        Project(ref vector, x, y, width, height, minZ, maxZ, ref worldViewProjection, out var result);
        return result;
    }

    /// <summary>
    /// Projects a 3D vector from screen space into object space.
    /// </summary>
    /// <param name="vector">The vector to project.</param>
    /// <param name="x">The X position of the viewport.</param>
    /// <param name="y">The Y position of the viewport.</param>
    /// <param name="width">The width of the viewport.</param>
    /// <param name="height">The height of the viewport.</param>
    /// <param name="minZ">The minimum depth of the viewport.</param>
    /// <param name="maxZ">The maximum depth of the viewport.</param>
    /// <param name="worldViewProjection">The combined world-view-projection matrix.</param>
    /// <param name="result">When the method completes, contains the vector in object space.</param>
    public static void Unproject(ref Float3 vector, float x, float y, float width, float height, float minZ, float maxZ, ref Matrix worldViewProjection, out Float3 result)
    {
        Matrix.Invert(ref worldViewProjection, out var matrix);
        var v = new Float3
        {
            X = (vector.X - x) / width * 2.0f - 1.0f,
            Y = -((vector.Y - y) / height * 2.0f - 1.0f),
            Z = (vector.Z - minZ) / (maxZ - minZ)
        };
        TransformCoordinate(ref v, ref matrix, out result);
    }

    /// <summary>
    /// Projects a 3D vector from screen space into object space.
    /// </summary>
    /// <param name="vector">The vector to project.</param>
    /// <param name="x">The X position of the viewport.</param>
    /// <param name="y">The Y position of the viewport.</param>
    /// <param name="width">The width of the viewport.</param>
    /// <param name="height">The height of the viewport.</param>
    /// <param name="minZ">The minimum depth of the viewport.</param>
    /// <param name="maxZ">The maximum depth of the viewport.</param>
    /// <param name="worldViewProjection">The combined world-view-projection matrix.</param>
    /// <returns>The vector in object space.</returns>
    public static Float3 Unproject(Float3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix worldViewProjection)
    {
        Unproject(ref vector, x, y, width, height, minZ, maxZ, ref worldViewProjection, out var result);
        return result;
    }

    /// <summary>
    /// Returns the reflection of a vector off a surface that has the specified normal.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="normal">Normal of the surface.</param>
    /// <param name="result">When the method completes, contains the reflected vector.</param>
    /// <remarks>Reflect only gives the direction of a reflection off a surface, it does not determine whether the original vector was close enough to the surface to hit it.</remarks>
    public static void Reflect(ref Float3 vector, ref Float3 normal, out Float3 result)
    {
        float dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
        result.X = vector.X - 2.0f * dot * normal.X;
        result.Y = vector.Y - 2.0f * dot * normal.Y;
        result.Z = vector.Z - 2.0f * dot * normal.Z;
    }

    /// <summary>
    /// Returns the reflection of a vector off a surface that has the specified normal.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="normal">Normal of the surface.</param>
    /// <returns>The reflected vector.</returns>
    /// <remarks>Reflect only gives the direction of a reflection off a surface, it does not determine whether the original vector was close enough to the surface to hit it.</remarks>
    public static Float3 Reflect(Float3 vector, Float3 normal)
    {
        Reflect(ref vector, ref normal, out var result);
        return result;
    }

    /// <summary>
    /// Transforms a 3D vector by the given <see cref="Quaternion" /> rotation.
    /// </summary>
    /// <param name="vector">The vector to rotate.</param>
    /// <param name="rotation">The <see cref="Quaternion" /> rotation to apply.</param>
    /// <param name="result">When the method completes, contains the transformed <see cref="Float3" />.</param>
    public static void Transform(ref Float3 vector, ref Quaternion rotation, out Float3 result)
    {
        float x = rotation.X + rotation.X;
        float y = rotation.Y + rotation.Y;
        float z = rotation.Z + rotation.Z;
        float wx = rotation.W * x;
        float wy = rotation.W * y;
        float wz = rotation.W * z;
        float xx = rotation.X * x;
        float xy = rotation.X * y;
        float xz = rotation.X * z;
        float yy = rotation.Y * y;
        float yz = rotation.Y * z;
        float zz = rotation.Z * z;
        result = new Float3(vector.X * (1.0f - yy - zz) + vector.Y * (xy - wz) + vector.Z * (xz + wy),
                            vector.X * (xy + wz) + vector.Y * (1.0f - xx - zz) + vector.Z * (yz - wx),
                            vector.X * (xz - wy) + vector.Y * (yz + wx) + vector.Z * (1.0f - xx - yy));
    }

    /// <summary>
    /// Transforms a 3D vector by the given <see cref="Quaternion" /> rotation.
    /// </summary>
    /// <param name="vector">The vector to rotate.</param>
    /// <param name="rotation">The <see cref="Quaternion" /> rotation to apply.</param>
    /// <returns>The transformed <see cref="Float3" />.</returns>
    public static Float3 Transform(Float3 vector, Quaternion rotation)
    {
        Transform(ref vector, ref rotation, out var result);
        return result;
    }

    /// <summary>
    /// Transforms a 3D vector by the given <see cref="Matrix3x3"/>.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="transform">The transformation <see cref="Matrix3x3"/>.</param>
    /// <param name="result">When the method completes, contains the transformed <see cref="Float3"/>.</param>
    public static void Transform(ref Float3 vector, ref Matrix3x3 transform, out Float3 result)
    {
        result = new Float3((vector.X * transform.M11) + (vector.Y * transform.M21) + (vector.Z * transform.M31),
                            (vector.X * transform.M12) + (vector.Y * transform.M22) + (vector.Z * transform.M32),
                            (vector.X * transform.M13) + (vector.Y * transform.M23) + (vector.Z * transform.M33));
    }

    /// <summary>
    /// Transforms a 3D vector by the given <see cref="Matrix3x3"/>.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="transform">The transformation <see cref="Matrix3x3"/>.</param>
    /// <returns>The transformed <see cref="Float3"/>.</returns>
    public static Float3 Transform(Float3 vector, Matrix3x3 transform)
    {
        Transform(ref vector, ref transform, out var result);
        return result;
    }

    /// <summary>
    /// Transforms a 3D vector by the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <param name="result">When the method completes, contains the transformed <see cref="Float3" />.</param>
    public static void Transform(ref Float3 vector, ref Matrix transform, out Float3 result)
    {
        result = new Float3(vector.X * transform.M11 + vector.Y * transform.M21 + vector.Z * transform.M31 + transform.M41,
                            vector.X * transform.M12 + vector.Y * transform.M22 + vector.Z * transform.M32 + transform.M42,
                            vector.X * transform.M13 + vector.Y * transform.M23 + vector.Z * transform.M33 + transform.M43);
    }

    /// <summary>
    /// Transforms a 3D vector by the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <param name="result">When the method completes, contains the transformed <see cref="Float4" />.</param>
    public static void Transform(ref Float3 vector, ref Matrix transform, out Float4 result)
    {
        result = new Float4(vector.X * transform.M11 + vector.Y * transform.M21 + vector.Z * transform.M31 + transform.M41,
                            vector.X * transform.M12 + vector.Y * transform.M22 + vector.Z * transform.M32 + transform.M42,
                            vector.X * transform.M13 + vector.Y * transform.M23 + vector.Z * transform.M33 + transform.M43,
                            vector.X * transform.M14 + vector.Y * transform.M24 + vector.Z * transform.M34 + transform.M44);
    }

    /// <summary>
    /// Transforms a 3D vector by the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <returns>The transformed <see cref="Float3" />.</returns>
    public static Float3 Transform(Float3 vector, Matrix transform)
    {
        Transform(ref vector, ref transform, out Float3 result);
        return result;
    }

    /// <summary>
    /// Performs a coordinate transformation using the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="coordinate">The coordinate vector to transform.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <param name="result">When the method completes, contains the transformed coordinates.</param>
    /// <remarks>
    /// A coordinate transform performs the transformation with the assumption that the w component
    /// is one. The four dimensional vector obtained from the transformation operation has each
    /// component in the vector divided by the w component. This forces the w component to be one and
    /// therefore makes the vector homogeneous. The homogeneous vector is often preferred when working
    /// with coordinates as the w component can safely be ignored.
    /// </remarks>
    public static void TransformCoordinate(ref Float3 coordinate, ref Matrix transform, out Float3 result)
    {
        var vector = new Float4
        {
            X = coordinate.X * transform.M11 + coordinate.Y * transform.M21 + coordinate.Z * transform.M31 + transform.M41,
            Y = coordinate.X * transform.M12 + coordinate.Y * transform.M22 + coordinate.Z * transform.M32 + transform.M42,
            Z = coordinate.X * transform.M13 + coordinate.Y * transform.M23 + coordinate.Z * transform.M33 + transform.M43,
            W = 1f / (coordinate.X * transform.M14 + coordinate.Y * transform.M24 + coordinate.Z * transform.M34 + transform.M44)
        };
        result = new Float3(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W);
    }

    /// <summary>
    /// Performs a coordinate transformation using the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="coordinate">The coordinate vector to transform.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <returns>The transformed coordinates.</returns>
    /// <remarks>
    /// A coordinate transform performs the transformation with the assumption that the w component
    /// is one. The four dimensional vector obtained from the transformation operation has each
    /// component in the vector divided by the w component. This forces the w component to be one and
    /// therefore makes the vector homogeneous. The homogeneous vector is often preferred when working
    /// with coordinates as the w component can safely be ignored.
    /// </remarks>
    public static Float3 TransformCoordinate(Float3 coordinate, Matrix transform)
    {
        TransformCoordinate(ref coordinate, ref transform, out var result);
        return result;
    }

    /// <summary>
    /// Performs a normal transformation using the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="normal">The normal vector to transform.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <param name="result">When the method completes, contains the transformed normal.</param>
    /// <remarks>
    /// A normal transform performs the transformation with the assumption that the w component
    /// is zero. This causes the fourth row and fourth column of the matrix to be unused. The
    /// end result is a vector that is not translated, but all other transformation properties
    /// apply. This is often preferred for normal vectors as normals purely represent direction
    /// rather than location because normal vectors should not be translated.
    /// </remarks>
    public static void TransformNormal(ref Float3 normal, ref Matrix transform, out Float3 result)
    {
        result = new Float3(normal.X * transform.M11 + normal.Y * transform.M21 + normal.Z * transform.M31,
                            normal.X * transform.M12 + normal.Y * transform.M22 + normal.Z * transform.M32,
                            normal.X * transform.M13 + normal.Y * transform.M23 + normal.Z * transform.M33);
    }

    /// <summary>
    /// Performs a normal transformation using the given <see cref="Matrix" />.
    /// </summary>
    /// <param name="normal">The normal vector to transform.</param>
    /// <param name="transform">The transformation <see cref="Matrix" />.</param>
    /// <returns>The transformed normal.</returns>
    /// <remarks>
    /// A normal transform performs the transformation with the assumption that the w component
    /// is zero. This causes the fourth row and fourth column of the matrix to be unused. The
    /// end result is a vector that is not translated, but all other transformation properties
    /// apply. This is often preferred for normal vectors as normals purely represent direction
    /// rather than location because normal vectors should not be translated.
    /// </remarks>
    public static Float3 TransformNormal(Float3 normal, Matrix transform)
    {
        TransformNormal(ref normal, ref transform, out var result);
        return result;
    }

    /// <summary>
    /// Snaps the input position into the grid.
    /// </summary>
    /// <param name="pos">The position to snap.</param>
    /// <param name="gridSize">The size of the grid.</param>
    /// <returns>The position snapped to the grid.</returns>
    public static Float3 SnapToGrid(Float3 pos, Float3 gridSize)
    {
        if (Mathf.Abs(gridSize.X) > Mathf.Epsilon)
            pos.X = Mathf.Ceil((pos.X - (gridSize.X * 0.5f)) / gridSize.X) * gridSize.X;
        if (Mathf.Abs(gridSize.Y) > Mathf.Epsilon)
            pos.Y = Mathf.Ceil((pos.Y - (gridSize.Y * 0.5f)) / gridSize.Y) * gridSize.Y;
        if (Mathf.Abs(gridSize.Z) > Mathf.Epsilon)
            pos.Z = Mathf.Ceil((pos.Z - (gridSize.Z * 0.5f)) / gridSize.Z) * gridSize.Z;
        return pos;
    }

    /// <inheritdoc/>
    public override readonly string ToString() => string.Format(CultureInfo.CurrentCulture, _formatString, X, Y, Z);

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
        return string.Format(CultureInfo.CurrentCulture, _formatString, x, y, z);
    }

    /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
    public readonly string ToString(IFormatProvider formatProvider)
    {
        return string.Format(formatProvider, _formatString, X, Y, Z);
    }

    /// <inheritdoc/>
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        if (format == null)
        {
            return ToString(formatProvider);
        }

        string x = X.ToString(format, formatProvider);
        string y = Y.ToString(format, formatProvider);
        string z = Z.ToString(format, formatProvider);
        return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}", x, y, z);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in Float3 other) => Vector128.EqualsAll(this.AsVector128(), other.AsVector128());

    /// <inheritdoc/>
    public override readonly bool Equals(object value) => value is Float3 other && Equals(in other);

    public static Float3 Transform(in Float3 vector, in Quaternion rotation)
    {
        throw new NotImplementedException();
    }

    public static Float3 Transform(in Float3 vector, in Matrix transform)
    {
        throw new NotImplementedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Float3 Scale(ref readonly Vector128<float> vector, float scale)
    {
        Vector128<float> scaleVector = Vector128.Create(scale);
        Vector128<float> result = vector * scaleVector;
        return result.AsVector3();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float ComputeLengthSquared(in Float3 value, out Vector128<float> vector)
    {
        vector = value.AsVector128();
        return Vector128.Sum(vector * vector);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="Float3" /> to <see cref="Vector3" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Vector3(Float3 value) => new(value.X, value.Y, value.Z);

    /// <summary>
    /// Performs an implicit conversion from <see cref="Float3" /> to <see cref="Double3" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Double3(Float3 value) => new(value.X, value.Y, value.Z);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Float3" /> to <see cref="Float2" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Float2(Float3 value) => new(value.X, value.Y);

    /// <summary>
    /// Performs an explicit conversion from <see cref="Float3" /> to <see cref="Float4" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Float4(Float3 value) => new(value, 0.0f);

    readonly bool ICustomValueEquals.ValueEquals(object other) => Equals(other);
}
