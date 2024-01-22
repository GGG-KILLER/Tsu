// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Sourced from https://github.com/dotnet/Open-XML-SDK/blob/35b91d3c18fc06379b74f0980e3498f2b04c1fc8/gen/DocumentFormat.OpenXml.Generator/ParentClass.cs

namespace Tsu.TreeSourceGen;

internal sealed record class ParentClass(string Keyword, string Name, string TypeParams, string Constraints, ParentClass? Child) : IEnumerable<ParentClass>
{
    public IEnumerator<ParentClass> GetEnumerator()
    {
        ParentClass? child = this;

        while (child is not null)
        {
            yield return child;
            child = child.Child;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
