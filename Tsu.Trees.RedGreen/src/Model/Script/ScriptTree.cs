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

using Scriban.Runtime;

namespace Tsu.Trees.RedGreen.SourceGenerator.Model;

internal sealed class ScriptTree(Tree tree)
{
    public ScriptTypeSymbol GreenBase { get; } = new ScriptTypeSymbol(tree.GreenBase);
    public ScriptTypeSymbol RedBase { get; } = new ScriptTypeSymbol(tree.RedBase);
    public ScriptNode Root { get; } = new ScriptNode(tree.Root);
    public string Suffix => tree.Suffix;
    public ScriptTypeSymbol KindEnum { get; } = new ScriptTypeSymbol(tree.KindEnum);
    public bool CreateVisitors => tree.CreateVisitors;
    public bool CreateWalker => tree.CreateWalker;
    public bool CreateRewriter => tree.CreateRewriter;
    public bool DebugDump => tree.DebugDump;

    public IEnumerable<ScriptNode> Nodes
    {
        get
        {
            var stack = new Stack<ScriptNode>();
            stack.Push(Root);
            while (stack.Count > 0)
            {
                var node = stack.Pop();

                foreach (var desc in node.Descendants)
                    stack.Push(desc);

                yield return node;
            }
        }
    }

    public IEnumerable<ScriptNode> NonRootNodes
    {
        get
        {
            var stack = new Stack<ScriptNode>();
            foreach (var desc in Root.Descendants)
                stack.Push(desc);
            while (stack.Count > 0)
            {
                var node = stack.Pop();

                foreach (var desc in node.Descendants)
                    stack.Push(desc);

                yield return node;
            }
        }
    }
}
