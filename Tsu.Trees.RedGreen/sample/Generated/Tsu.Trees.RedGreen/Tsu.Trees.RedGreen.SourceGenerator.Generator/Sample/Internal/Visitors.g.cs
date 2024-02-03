﻿// <auto-generated />

#nullable enable

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Tsu.Trees.RedGreen.Sample.Internal
{

    internal static class SampleFactory
    {
        public static global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample SemicolonToken()
        {
#if DEBUG
#endif // DEBUG

            return new global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample(global::Tsu.Trees.RedGreen.Sample.SampleKind.SemicolonToken);
        }

        public static global::Tsu.Trees.RedGreen.Sample.Internal.AssignmentStatement AssignmentStatement(global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample identifier, global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample value, global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample semicolon)
        {
#if DEBUG
            if (identifier == null) throw new global::System.ArgumentNullException(nameof(identifier));
            if (value == null) throw new global::System.ArgumentNullException(nameof(value));
            if (semicolon == null) throw new global::System.ArgumentNullException(nameof(semicolon));
#endif // DEBUG

            return new global::Tsu.Trees.RedGreen.Sample.Internal.AssignmentStatement(global::Tsu.Trees.RedGreen.Sample.SampleKind.AssignmentStatement, identifier, value, semicolon);
        }

        public static global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionStatementSample ExpressionStatement(global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample expression, global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample semicolon)
        {
#if DEBUG
            if (expression == null) throw new global::System.ArgumentNullException(nameof(expression));
            if (semicolon == null) throw new global::System.ArgumentNullException(nameof(semicolon));
#endif // DEBUG

            return new global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionStatementSample(global::Tsu.Trees.RedGreen.Sample.SampleKind.ExpressionStatement, expression, semicolon);
        }

        public static global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample IdentifierExpression(string name)
        {
#if DEBUG
            if (name == null) throw new global::System.ArgumentNullException(nameof(name));
#endif // DEBUG

            return new global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample(global::Tsu.Trees.RedGreen.Sample.SampleKind.IdentifierExpression, name);
        }

        public static global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample NumericalLiteralExpression(double value)
        {
#if DEBUG
#endif // DEBUG

            return new global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample(global::Tsu.Trees.RedGreen.Sample.SampleKind.NumericalLiteralExpression, value);
        }

        public static global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample BinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.SampleKind kind, global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample left, global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample right)
        {
#if DEBUG
            if (left == null) throw new global::System.ArgumentNullException(nameof(left));
            if (right == null) throw new global::System.ArgumentNullException(nameof(right));
            switch (kind)
            {
                case Tsu.Trees.RedGreen.Sample.SampleKind.AdditionExpression:
                case Tsu.Trees.RedGreen.Sample.SampleKind.DivisionExpression:
                case Tsu.Trees.RedGreen.Sample.SampleKind.MultiplicationExpression:
                case Tsu.Trees.RedGreen.Sample.SampleKind.SubtractionExpression:
                    break;
                default:
                    throw new global::System.ArgumentException("Kind not accepted for this node.", nameof(kind));
            }
#endif // DEBUG

            return new global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample(kind, left, right);
        }

        public static global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample FunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample identifier)
        {
#if DEBUG
            if (identifier == null) throw new global::System.ArgumentNullException(nameof(identifier));
#endif // DEBUG

            return new global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample(global::Tsu.Trees.RedGreen.Sample.SampleKind.FunctionCallExpression, identifier, default);
        }

        public static global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample FunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample identifier, global::Tsu.Trees.RedGreen.Sample.Internal.SampleList<global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample> args)
        {
#if DEBUG
            if (identifier == null) throw new global::System.ArgumentNullException(nameof(identifier));
#endif // DEBUG

            return new global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample(global::Tsu.Trees.RedGreen.Sample.SampleKind.FunctionCallExpression, identifier, args.Node);
        }
    }


    internal partial class SampleVisitor
    {
        public virtual void Visit(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? node)
        {
            if (node != null)
            {
                node.Accept(this);
            }
        }
        public virtual void VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample node) => this.DefaultVisit(node);
        public virtual void VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.Internal.AssignmentStatement node) => this.DefaultVisit(node);
        public virtual void VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionStatementSample node) => this.DefaultVisit(node);
        public virtual void VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample node) => this.DefaultVisit(node);
        public virtual void VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample node) => this.DefaultVisit(node);
        public virtual void VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample node) => this.DefaultVisit(node);
        public virtual void VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample node) => this.DefaultVisit(node);
        protected virtual void DefaultVisit(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode node) { }
    }

    internal partial class SampleVisitor<TResult>
    {
        public virtual TResult? Visit(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? node) => node == null ? default : node.Accept(this
        );
        public virtual TResult? VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample node) => this.DefaultVisit(node);
        public virtual TResult? VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.Internal.AssignmentStatement node) => this.DefaultVisit(node);
        public virtual TResult? VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionStatementSample node) => this.DefaultVisit(node);
        public virtual TResult? VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample node) => this.DefaultVisit(node);
        public virtual TResult? VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample node) => this.DefaultVisit(node);
        public virtual TResult? VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample node) => this.DefaultVisit(node);
        public virtual TResult? VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample node) => this.DefaultVisit(node);
        protected virtual TResult? DefaultVisit(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode node) => default;
    }

    internal partial class SampleVisitor<T1, TResult>
    {
        public virtual TResult? Visit(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? node, T1 arg1) => node == null ? default : node.Accept(this
        , arg1);
        public virtual TResult? VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.Internal.AssignmentStatement node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionStatementSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        protected virtual TResult? DefaultVisit(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode node, T1 arg1) => default;
    }

    internal partial class SampleVisitor<T1, T2, TResult>
    {
        public virtual TResult? Visit(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? node, T1 arg1, T2 arg2) => node == null ? default : node.Accept(this
        , arg1, arg2);
        public virtual TResult? VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.Internal.AssignmentStatement node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionStatementSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        protected virtual TResult? DefaultVisit(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode node, T1 arg1, T2 arg2) => default;
    }

    internal partial class SampleVisitor<T1, T2, T3, TResult>
    {
        public virtual TResult? Visit(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? node, T1 arg1, T2 arg2, T3 arg3) => node == null ? default : node.Accept(this
        , arg1, arg2, arg3);
        public virtual TResult? VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.Internal.AssignmentStatement node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionStatementSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        protected virtual TResult? DefaultVisit(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode node, T1 arg1, T2 arg2, T3 arg3) => default;
    }


    public partial class SampleRewriter : Tsu.Trees.RedGreen.Sample.SampleVisitor<global::Tsu.Trees.RedGreen.Sample.SampleNode>
    {
        public global::Tsu.Trees.RedGreen.Sample.SampleList<TNode> VisitList<TNode>(global::Tsu.Trees.RedGreen.Sample.SampleList<TNode> list) where TNode : global::Tsu.Trees.RedGreen.Sample.SampleNode
        {
            global::Tsu.Trees.RedGreen.Sample.SampleListBuilder? alternate = null;
            for (int i = 0, n = list.Count; i < n; i++)
            {
                var item = list[i];
                var visited = Visit(item);
                if (item != visited && alternate == null)
                {
                    alternate = new global::Tsu.Trees.RedGreen.Sample.SampleListBuilder(n);
                    alternate.AddRange(list, 0, i);
                }

                if (alternate != null && visited != null && visited.Kind != global::Tsu.Trees.RedGreen.Sample.SampleKind.None)
                {
                    alternate.Add(visited);
                }
            }

            if (alternate != null)
            {
                return alternate.ToList();
            }

            return list;
        }

        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample node) =>
            node;
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.AssignmentStatement node) =>
            node.Update((global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample?)Visit(node.Identifier) ?? throw new global::System.InvalidOperationException("Identifier cannot be null."), (global::Tsu.Trees.RedGreen.Sample.ExpressionSample?)Visit(node.Value) ?? throw new global::System.InvalidOperationException("Value cannot be null."), (global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample?)Visit(node.Semicolon) ?? throw new global::System.InvalidOperationException("Semicolon cannot be null."));
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample node) =>
            node.Update((global::Tsu.Trees.RedGreen.Sample.ExpressionSample?)Visit(node.Expression) ?? throw new global::System.InvalidOperationException("Expression cannot be null."), (global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample?)Visit(node.Semicolon) ?? throw new global::System.InvalidOperationException("Semicolon cannot be null."));
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample node) =>
            node.Update(node.Name);
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample node) =>
            node.Update(node.Value);
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample node) =>
            node.Update(node.Kind, (global::Tsu.Trees.RedGreen.Sample.ExpressionSample?)Visit(node.Left) ?? throw new global::System.InvalidOperationException("Left cannot be null."), (global::Tsu.Trees.RedGreen.Sample.ExpressionSample?)Visit(node.Right) ?? throw new global::System.InvalidOperationException("Right cannot be null."));
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample node) =>
            node.Update((global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample?)Visit(node.Identifier) ?? throw new global::System.InvalidOperationException("Identifier cannot be null."), VisitList(node.Args));
    }
}

