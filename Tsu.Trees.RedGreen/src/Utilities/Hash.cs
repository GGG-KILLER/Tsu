// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// Sourced from: https://github.com/dotnet/roslyn/blob/cda55873bfd7f16ac6176f467f98ddf8f644e101/src/Compilers/Core/Portable/InternalUtilities/Hash.cs

using System.ComponentModel;

namespace Tsu.Trees.RedGreen.Utilities;

/// <summary>
/// Internal hashing utilities used by generated code.
/// </summary>
/// <remarks>
/// This should never be used directly unless if you know what you're doing.
/// It is left undocumented on purpose to discourage usage.
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class Hash
{
    #pragma warning disable CS1591 // We don't document "internal" interfaces.

    public static int Combine(int newKey, int currentKey)
    {
        return unchecked((currentKey * (int) 0xA5555529) + newKey);
    }

    public static int Combine(bool newKeyPart, int currentKey)
    {
        return Combine(currentKey, newKeyPart ? 1 : 0);
    }

    /// <summary>
    /// This is how VB Anonymous Types combine hash values for fields.
    /// PERF: Do not use with enum types because that involves multiple
    /// unnecessary boxing operations.  Unfortunately, we can't constrain
    /// T to "non-enum", so we'll use a more restrictive constraint.
    /// </summary>
    public static int Combine<T>(T newKeyPart, int currentKey) where T : class?
    {
        int hash = unchecked(currentKey * (int) 0xA5555529);

        if (newKeyPart != null)
        {
            return unchecked(hash + newKeyPart.GetHashCode());
        }

        return hash;
    }

    public static int CombineValues<T>(IEnumerable<T>? values, int maxItemsToHash = int.MaxValue)
    {
        if (values == null)
        {
            return 0;
        }

        var hashCode = 0;
        var count = 0;
        foreach (var value in values)
        {
            if (count++ >= maxItemsToHash)
            {
                break;
            }

            // Should end up with a constrained virtual call to object.GetHashCode (i.e. avoid boxing where possible).
            if (value != null)
            {
                hashCode = Combine(value.GetHashCode(), hashCode);
            }
        }

        return hashCode;
    }

    public static int CombineValues<T>(T[]? values, int maxItemsToHash = int.MaxValue)
    {
        if (values == null)
        {
            return 0;
        }

        var maxSize = Math.Min(maxItemsToHash, values.Length);
        var hashCode = 0;

        for (int i = 0; i < maxSize; i++)
        {
            T value = values[i];

            // Should end up with a constrained virtual call to object.GetHashCode (i.e. avoid boxing where possible).
            if (value != null)
            {
                hashCode = Combine(value.GetHashCode(), hashCode);
            }
        }

        return hashCode;
    }

    public static int CombineValues(IEnumerable<string?>? values, StringComparer stringComparer, int maxItemsToHash = int.MaxValue)
    {
        if (values == null)
        {
            return 0;
        }

        var hashCode = 0;
        var count = 0;
        foreach (var value in values)
        {
            if (count++ >= maxItemsToHash)
            {
                break;
            }

            if (value != null)
            {
                hashCode = Combine(stringComparer.GetHashCode(value), hashCode);
            }
        }

        return hashCode;
    }
}