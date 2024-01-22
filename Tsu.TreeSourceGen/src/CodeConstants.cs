// Copyright © 2024 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Tsu.TreeSourceGen;

internal static class CodeConstants
{
    public const string Namespace = "Tsu.TreeSourceGen";

    internal static class TreeNodeAttribute
    {
        public const string FullName = "TreeNodeAttribute";

        public const string SourceCode = $$"""
        using System;

        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
        public sealed class {{FullName}}(Type treeRoot) : Attribute
        {
            public Type TreeRoot { get; } = treeRoot;

            public string? Name { get; set; }
        }
        """;
    }

    internal static class TreeVisitorAttribute
    {
        public const string FullName = "TreeVisitorAttribute";

        public const string SourceCode = $$"""
        using System;

        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
        public sealed class {{FullName}}(Type treeRoot) : Attribute
        {
            public Type TreeRoot { get; } = treeRoot;
        }
        """;
    }

    internal static class TreeWalkerAttribute
    {
        public const string FullName = "TreeWalkerAttribute";

        public const string SourceCode = $$"""
        using System;

        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
        public sealed class {{FullName}}(Type treeRoot) : Attribute
        {
            public Type TreeRoot { get; } = treeRoot;
        }
        """;
    }
}