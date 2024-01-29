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

    }
}

namespace Tsu.Trees.RedGreen.Sample.Internal
{
    [GreenTreeRoot(typeof(SampleNode), "Sample", typeof(SampleKind), CreateRewriter = true, CreateVisitors = true, CreateWalker = true)]
    internal abstract partial class GreenNode
    {
    }

    [GreenNode(SampleKind.SemicolonToken)]
    internal sealed partial class SemicolonTokenSample : GreenNode
    {
    }

    internal abstract partial class ExpressionSample : GreenNode
    {
    }

    [GreenNode(SampleKind.IdentifierExpression)]
    internal sealed partial class IdentifierExpressionSample : ExpressionSample
    {
        private readonly string _name;
    }

    [GreenNode(SampleKind.NumericalLiteralExpression)]
    internal sealed partial class NumericalLiteralExpressionSample : ExpressionSample
    {
        private readonly double _value;
    }

    [GreenNode(SampleKind.AdditionExpression, SampleKind.DivisionExpression, SampleKind.MultiplicationExpression, SampleKind.SubtractionExpression)]
    internal sealed partial class BinaryOperationExpressionSample : ExpressionSample
    {
        private readonly ExpressionSample _left;
        private readonly ExpressionSample _right;
    }

    [GreenNode(SampleKind.FunctionCallExpression)]
    internal sealed partial class FunctionCallExpressionSample : ExpressionSample
    {
        private readonly IdentifierExpressionSample _identifier;
        private readonly ExpressionSample _firstArg;
        private readonly ExpressionSample? _secondArg;
    }

    internal abstract partial class StatementSample : GreenNode
    {
        protected readonly SemicolonTokenSample _semicolon;
    }

    [GreenNode(SampleKind.AssignmentStatement)]
    internal sealed partial class AssignmentStatement : StatementSample
    {
        private readonly IdentifierExpressionSample _identifier;
        private readonly ExpressionSample _value;
    }

    [GreenNode(SampleKind.ExpressionStatement)]
    internal sealed partial class ExpressionStatementSample : StatementSample
    {
        private readonly ExpressionSample _expression;
    }
}