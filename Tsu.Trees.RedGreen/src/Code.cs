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

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class Code
{
    public const string GreenNodeAttributeName = "Tsu.Trees.RedGreen.GreenNodeAttribute";

    public const string GreenNodeAttributeCode = """
    namespace Tsu.Trees.RedGreen;

    /// <summary>
    /// Marks this class as a green node.
    /// </summary>
    /// <param name="kinds">
    /// The kinds this node could have.
    /// This should be the value on the enum (e.g.: SyntaxKind.ClassDeclarationSyntax)
    /// </param>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class GreenNodeAttribute(params object[] kinds) : Attribute
    {
        /// <summary>
        /// This node's Kind.
        /// </summary>
        public object[] Kinds { get; } = kinds;
    }
    """;

    public const string GreenTreeRootAttributeName = "Tsu.Trees.RedGreen.GreenTreeRootAttribute";

    public const string GreenTreeRootAttributeCode = """
    namespace Tsu.Trees.RedGreen;

    /// <summary>
    /// An attribute that marks the given class as the base class for all nodes in a green node tree.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class GreenTreeRootAttribute(Type redBase, string suffix, Type kindEnum) : Attribute
    {
        /// <summary>
        /// The suffix for nodes in this tree.
        /// </summary>
        public string Suffix { get; } = suffix;

        /// <summary>
        /// The base node type for all nodes in the red tree.
        /// </summary>
        public Type RedBase { get; } = redBase;

        /// <summary>
        /// The enum type that contains the definitions for the node kinds.
        /// </summary>
        public Type KindEnum { get; } = kindEnum;

        /// <summary>
        /// Whether to create base visitor implementations for this tree.
        /// </summary>
        public bool CreateVisitors { get; set; }

        /// <summary>
        /// Whether to create a base walker implementation for this tree.
        /// </summary>
        public bool CreateWalker { get; set; }

        /// <summary>
        /// Whether to generate a base rewriter implementation for this tree.
        /// </summary>
        public bool CreateRewriter { get; set; }

        /// <summary>
        /// Whether to generate a file with only comments dumping the entire tree structure.
        /// </summary>
        public bool DebugDump { get; set; }
    }
    """;
}