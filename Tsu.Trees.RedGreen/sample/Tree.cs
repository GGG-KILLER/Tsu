using Tsu.Trees.RedGreen;

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