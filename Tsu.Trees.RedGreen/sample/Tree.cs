using Tsu.Trees.RedGreen;

namespace Tsu.Trees.RedGreen.Sample
{
    public enum SampleKind
    {
        IdentifierExpression,
        NumericalLiteralExpression,

        AdditionExpression,
        SubtractionExpression,
        MultiplicationExpression,
        DivisionExpression,

        FunctionCallExpression,
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

    internal abstract partial class ExpressionSample : GreenNode
    {
    }

    [GreenNode(SampleKind.IdentifierExpression)]
    internal sealed partial class IdentifierExpressionSample : ExpressionSample
    {
        private readonly string name;
    }

    [GreenNode(SampleKind.NumericalLiteralExpression)]
    internal sealed partial class NumericalLiteralExpressionSample : ExpressionSample
    {
        private readonly double value;
    }

    [GreenNode(SampleKind.AdditionExpression, SampleKind.DivisionExpression, SampleKind.MultiplicationExpression, SampleKind.SubtractionExpression)]
    internal sealed partial class BinaryOperationExpressionSample : ExpressionSample
    {
        private readonly ExpressionSample left;
        private readonly ExpressionSample right;
    }

    [GreenNode(SampleKind.FunctionCallExpression)]
    internal sealed partial class FunctionCallExpressionSample : ExpressionSample
    {
        private readonly IdentifierExpressionSample identifier;
        private readonly ExpressionSample firstArg;
        private readonly ExpressionSample? secondArg;
    }
}