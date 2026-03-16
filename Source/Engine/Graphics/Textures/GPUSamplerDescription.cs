// Copyright (c) Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine;

partial struct GPUSamplerDescription : IEquatable<GPUSamplerDescription>
{
    /// <summary>
    /// Clears description.
    /// </summary>
    public void Clear()
    {
        this = new GPUSamplerDescription();
        MaxMipLevel = float.MaxValue;
    }

    /// <summary>
    /// Creates a new <see cref="GPUSamplerDescription" /> with default settings.
    /// </summary>
    /// <param name="filter">The filtering method.</param>
    /// <param name="addressMode">The addressing mode.</param>
    /// <returns>A new instance of <see cref="GPUSamplerDescription" /> class.</returns>
    public static GPUSamplerDescription New(GPUSamplerFilter filter = GPUSamplerFilter.Point, GPUSamplerAddressMode addressMode = GPUSamplerAddressMode.Wrap)
    {
        return new GPUSamplerDescription
        {
            Filter = filter,
            AddressU = addressMode,
            AddressV = addressMode,
            AddressW = addressMode,
            MaxMipLevel = float.MaxValue,
        };
    }

    /// <inheritdoc />
    public readonly bool Equals(GPUSamplerDescription other)
    {
        return Filter == other.Filter &&
               AddressU == other.AddressU &&
               AddressV == other.AddressV &&
               AddressW == other.AddressW &&
               Mathf.NearEqual(MipBias, other.MipBias) &&
               Mathf.NearEqual(MinMipLevel, other.MinMipLevel) &&
               Mathf.NearEqual(MaxMipLevel, other.MaxMipLevel) &&
               MaxAnisotropy == other.MaxAnisotropy &&
               BorderColor == other.BorderColor &&
               ComparisonFunction == other.ComparisonFunction;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is GPUSamplerDescription other && Equals(other);
    }

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Filter);
        hash.Add(AddressU);
        hash.Add(AddressV);
        hash.Add(AddressW);
        hash.Add(MipBias);
        hash.Add(MinMipLevel);
        hash.Add(MaxMipLevel);
        hash.Add(MaxAnisotropy);
        hash.Add(BorderColor);
        hash.Add(ComparisonFunction);
        return hash.ToHashCode();
    }
}
