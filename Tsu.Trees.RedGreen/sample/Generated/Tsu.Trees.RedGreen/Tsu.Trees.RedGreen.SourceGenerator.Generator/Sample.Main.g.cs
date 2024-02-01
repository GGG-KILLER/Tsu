﻿// <auto-generated />

#nullable enable

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Tsu.Trees.RedGreen.Sample
{

    public partial class SampleVisitor
    {
        public virtual void Visit(global::Tsu.Trees.RedGreen.Sample.SampleNode? node)
        {
            if (node != null)
            {
                node.Accept(this);
            }
        }
        public virtual void VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample node) => this.DefaultVisit(node);
        public virtual void VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.AssignmentStatement node) => this.DefaultVisit(node);
        public virtual void VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample node) => this.DefaultVisit(node);
        public virtual void VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample node) => this.DefaultVisit(node);
        public virtual void VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample node) => this.DefaultVisit(node);
        public virtual void VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample node) => this.DefaultVisit(node);
        public virtual void VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample node) => this.DefaultVisit(node);
        protected virtual void DefaultVisit(global::Tsu.Trees.RedGreen.Sample.SampleNode node) { }
    }

    public partial class SampleVisitor<TResult>
    {
        public virtual TResult? Visit(global::Tsu.Trees.RedGreen.Sample.SampleNode? node) => node == null ? default : node.Accept(this
        );
        public virtual TResult? VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample node) => this.DefaultVisit(node);
        public virtual TResult? VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.AssignmentStatement node) => this.DefaultVisit(node);
        public virtual TResult? VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample node) => this.DefaultVisit(node);
        public virtual TResult? VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample node) => this.DefaultVisit(node);
        public virtual TResult? VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample node) => this.DefaultVisit(node);
        public virtual TResult? VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample node) => this.DefaultVisit(node);
        public virtual TResult? VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample node) => this.DefaultVisit(node);
        protected virtual TResult? DefaultVisit(global::Tsu.Trees.RedGreen.Sample.SampleNode node) => default;
    }

    public partial class SampleVisitor<T1, TResult>
    {
        public virtual TResult? Visit(global::Tsu.Trees.RedGreen.Sample.SampleNode? node, T1 arg1) => node == null ? default : node.Accept(this
        , arg1);
        public virtual TResult? VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.AssignmentStatement node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        public virtual TResult? VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample node, T1 arg1) => this.DefaultVisit(node, arg1);
        protected virtual TResult? DefaultVisit(global::Tsu.Trees.RedGreen.Sample.SampleNode node, T1 arg1) => default;
    }

    public partial class SampleVisitor<T1, T2, TResult>
    {
        public virtual TResult? Visit(global::Tsu.Trees.RedGreen.Sample.SampleNode? node, T1 arg1, T2 arg2) => node == null ? default : node.Accept(this
        , arg1, arg2);
        public virtual TResult? VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.AssignmentStatement node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        public virtual TResult? VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample node, T1 arg1, T2 arg2) => this.DefaultVisit(node, arg1, arg2);
        protected virtual TResult? DefaultVisit(global::Tsu.Trees.RedGreen.Sample.SampleNode node, T1 arg1, T2 arg2) => default;
    }

    public partial class SampleVisitor<T1, T2, T3, TResult>
    {
        public virtual TResult? Visit(global::Tsu.Trees.RedGreen.Sample.SampleNode? node, T1 arg1, T2 arg2, T3 arg3) => node == null ? default : node.Accept(this
        , arg1, arg2, arg3);
        public virtual TResult? VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.AssignmentStatement node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        public virtual TResult? VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample node, T1 arg1, T2 arg2, T3 arg3) => this.DefaultVisit(node, arg1, arg2, arg3);
        protected virtual TResult? DefaultVisit(global::Tsu.Trees.RedGreen.Sample.SampleNode node, T1 arg1, T2 arg2, T3 arg3) => default;
    }



    public partial class SampleRewriter : Tsu.Trees.RedGreen.Sample.SampleVisitor<global::Tsu.Trees.RedGreen.Sample.SampleNode>
    {
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitSemicolonToken(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample node) =>
            node;
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitAssignmentStatement(global::Tsu.Trees.RedGreen.Sample.AssignmentStatement node) =>
            node.Update((global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample?)Visit(node.Semicolon) ?? throw new global::System.InvalidOperationException("Semicolon cannot be null."), (global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample?)Visit(node.Identifier) ?? throw new global::System.InvalidOperationException("Identifier cannot be null."), (global::Tsu.Trees.RedGreen.Sample.ExpressionSample?)Visit(node.Value) ?? throw new global::System.InvalidOperationException("Value cannot be null."));
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitExpressionStatement(global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample node) =>
            node.Update((global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample?)Visit(node.Semicolon) ?? throw new global::System.InvalidOperationException("Semicolon cannot be null."), (global::Tsu.Trees.RedGreen.Sample.ExpressionSample?)Visit(node.Expression) ?? throw new global::System.InvalidOperationException("Expression cannot be null."));
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitIdentifierExpression(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample node) =>
            node.Update(node.Name);
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitNumericalLiteralExpression(global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample node) =>
            node.Update(node.Value);
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitBinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample node) =>
            node.Update(node.Kind, (global::Tsu.Trees.RedGreen.Sample.ExpressionSample?)Visit(node.Left) ?? throw new global::System.InvalidOperationException("Left cannot be null."), (global::Tsu.Trees.RedGreen.Sample.ExpressionSample?)Visit(node.Right) ?? throw new global::System.InvalidOperationException("Right cannot be null."));
        public override global::Tsu.Trees.RedGreen.Sample.SampleNode VisitFunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample node) =>
            node.Update((global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample?)Visit(node.Identifier) ?? throw new global::System.InvalidOperationException("Identifier cannot be null."), (global::Tsu.Trees.RedGreen.Sample.ExpressionSample?)Visit(node.FirstArg) ?? throw new global::System.InvalidOperationException("FirstArg cannot be null."), (global::Tsu.Trees.RedGreen.Sample.ExpressionSample?)Visit(node.SecondArg));
    }
    public static class SampleFactory
    {
        public static global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample SemicolonToken() =>
            (global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample) global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.SemicolonToken().CreateRed();

        public static global::Tsu.Trees.RedGreen.Sample.AssignmentStatement AssignmentStatement(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample semicolon, global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample identifier, global::Tsu.Trees.RedGreen.Sample.ExpressionSample value) =>
            (global::Tsu.Trees.RedGreen.Sample.AssignmentStatement) global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.AssignmentStatement((global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample)semicolon.Green, (global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample)identifier.Green, (global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample)value.Green).CreateRed();

        public static global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample ExpressionStatement(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample semicolon, global::Tsu.Trees.RedGreen.Sample.ExpressionSample expression) =>
            (global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample) global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.ExpressionStatement((global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample)semicolon.Green, (global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample)expression.Green).CreateRed();

        public static global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample IdentifierExpression(global::System.String name) =>
            (global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample) global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.IdentifierExpression(name).CreateRed();

        public static global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample NumericalLiteralExpression(global::System.Double value) =>
            (global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample) global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.NumericalLiteralExpression(value).CreateRed();

        public static global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample BinaryOperationExpression(global::Tsu.Trees.RedGreen.Sample.SampleKind kind, global::Tsu.Trees.RedGreen.Sample.ExpressionSample left, global::Tsu.Trees.RedGreen.Sample.ExpressionSample right) =>
            (global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample) global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.BinaryOperationExpression(kind, (global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample)left.Green, (global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample)right.Green).CreateRed();

        public static global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample FunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample identifier, global::Tsu.Trees.RedGreen.Sample.ExpressionSample firstArg) =>
            (global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample) global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.FunctionCallExpression((global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample)identifier.Green, (global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample)firstArg.Green).CreateRed();

        public static global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample FunctionCallExpression(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample identifier, global::Tsu.Trees.RedGreen.Sample.ExpressionSample firstArg, global::Tsu.Trees.RedGreen.Sample.ExpressionSample? secondArg) =>
            (global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample) global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.FunctionCallExpression((global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample)identifier.Green, (global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample)firstArg.Green, secondArg == null ? null : (global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample)secondArg.Green).CreateRed();
    }
}

