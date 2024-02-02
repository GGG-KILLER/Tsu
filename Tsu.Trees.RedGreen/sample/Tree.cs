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

namespace Tsu.Trees.RedGreen.Sample
{
    public enum SampleKind
    {
        // Meta kinds (required)
        None = 0,
        List,

        SemicolonToken,
        IdentifierExpression,
        NumericalLiteralExpression,

        AdditionExpression,
        SubtractionExpression,
        MultiplicationExpression,
        DivisionExpression,

        FunctionCallExpression,

        AssignmentStatement,
        ExpressionStatement,
    }

    public abstract partial class SampleNode
    {
        // Stub to be filled in by codegen
    }
}

namespace Tsu.Trees.RedGreen.Sample.Internal
{
    [GreenTreeRoot(
        typeof(SampleNode),
        "Sample",
        typeof(SampleKind),
        CreateRewriter = true,
        CreateVisitors = true,
        CreateWalker = true,
        CreateLists = true,
        DebugDump = true)]
    internal abstract partial class GreenNode
    {
        // Stub to be filled in by codegen
    }

    partial class SampleList : GreenNode
    {
        // Stub to be filled in by codegen
    }

    [GreenNode(SampleKind.SemicolonToken)]
    internal sealed partial class SemicolonTokenSample : GreenNode
    {
        // Stub to be filled in by codegen
    }

    internal abstract partial class ExpressionSample : GreenNode
    {
        // Stub to be filled in by codegen
    }

    [GreenNode(SampleKind.IdentifierExpression)]
    internal sealed partial class IdentifierExpressionSample : ExpressionSample
    {
        private readonly string _name;

        // Stub to be filled in by codegen
    }

    [GreenNode(SampleKind.NumericalLiteralExpression)]
    internal sealed partial class NumericalLiteralExpressionSample : ExpressionSample
    {
        private readonly double _value;

        // Stub to be filled in by codegen
    }

    [GreenNode(SampleKind.AdditionExpression, SampleKind.DivisionExpression, SampleKind.MultiplicationExpression, SampleKind.SubtractionExpression)]
    internal sealed partial class BinaryOperationExpressionSample : ExpressionSample
    {
        private readonly ExpressionSample _left;
        private readonly ExpressionSample _right;

        // Stub to be filled in by codegen
    }

    [GreenNode(SampleKind.FunctionCallExpression)]
    internal sealed partial class FunctionCallExpressionSample : ExpressionSample
    {
        private readonly IdentifierExpressionSample _identifier;

        [GreenList(typeof(ExpressionSample))]
        private readonly SampleList _args;

        // Stub to be filled in by codegen
    }

    internal abstract partial class StatementSample : GreenNode
    {
        [NodeComponent(Order = -1)]
        protected readonly SemicolonTokenSample _semicolon;

        // Stub to be filled in by codegen
    }

    [GreenNode(SampleKind.AssignmentStatement)]
    internal sealed partial class AssignmentStatement : StatementSample
    {
        private readonly IdentifierExpressionSample _identifier;
        private readonly ExpressionSample _value;

        // Stub to be filled in by codegen
    }

    [GreenNode(SampleKind.ExpressionStatement)]
    internal sealed partial class ExpressionStatementSample : StatementSample
    {
        private readonly ExpressionSample _expression;

        // Stub to be filled in by codegen
    }
}