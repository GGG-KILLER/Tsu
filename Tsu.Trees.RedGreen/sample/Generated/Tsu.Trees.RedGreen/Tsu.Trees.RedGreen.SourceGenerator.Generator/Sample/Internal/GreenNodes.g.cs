﻿// <auto-generated />

#nullable enable

namespace Tsu.Trees.RedGreen.Sample.Internal
{
    partial class ExpressionSample : global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
    {
        protected ExpressionSample(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind
        )
            : base(kind)
        {
        }







    }
    partial class FunctionCallExpressionSample : global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample
    {
        internal FunctionCallExpressionSample(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind,
            global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample identifier,
            global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? args
        )
            : base(kind)
        {
            this.SlotCount = 2;
            this._identifier = identifier;
            this._args = args;
        }

        public global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample Identifier => this._identifier;
        public global::Tsu.Trees.RedGreen.Sample.Internal.SampleList<global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample> Args => new global::Tsu.Trees.RedGreen.Sample.Internal.SampleList<global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample>(this._args);

        public override global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? GetSlot(int index) =>
            index switch
            {
                0 => this._identifier,
                1 => this._args,
                _ => null
            };

        public override global::Tsu.Trees.RedGreen.Sample.SampleNode CreateRed(global::Tsu.Trees.RedGreen.Sample.SampleNode? parent) =>
            new global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample(this, parent);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor visitor) =>
            visitor.VisitFunctionCallExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<TResult> visitor) =>
            visitor.VisitFunctionCallExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitFunctionCallExpression(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitFunctionCallExpression(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitFunctionCallExpression(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample Update(
            global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample identifier,
            global::Tsu.Trees.RedGreen.Sample.Internal.SampleList<global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample> args
        )
        {
            if (
                this.Identifier != identifier
                || this.Args != args
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.FunctionCallExpression(
                    identifier,
                    args
                );
            }

            return this;
        }
    }
    partial class BinaryOperationExpressionSample : global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample
    {
        internal BinaryOperationExpressionSample(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind,
            global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample left,
            global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample right
        )
            : base(kind)
        {
            this.SlotCount = 2;
            this._left = left;
            this._right = right;
        }

        public global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample Left => this._left;
        public global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample Right => this._right;

        public override global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? GetSlot(int index) =>
            index switch
            {
                0 => this._left,
                1 => this._right,
                _ => null
            };

        public override global::Tsu.Trees.RedGreen.Sample.SampleNode CreateRed(global::Tsu.Trees.RedGreen.Sample.SampleNode? parent) =>
            new global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample(this, parent);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor visitor) =>
            visitor.VisitBinaryOperationExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<TResult> visitor) =>
            visitor.VisitBinaryOperationExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitBinaryOperationExpression(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitBinaryOperationExpression(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitBinaryOperationExpression(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample Update(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind,
            global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample left,
            global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample right
        )
        {
            if (
                this.Kind != kind
                || this.Left != left
                || this.Right != right
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.BinaryOperationExpression(
                    kind,
                    left,
                    right
                );
            }

            return this;
        }
    }
    partial class NumericalLiteralExpressionSample : global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample
    {
        internal NumericalLiteralExpressionSample(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind,
            double value
        )
            : base(kind)
        {
            this.SlotCount = 0;
            this._value = value;
        }

        public double Value => this._value;

        public override global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? GetSlot(int index) =>
            null;

        public override global::Tsu.Trees.RedGreen.Sample.SampleNode CreateRed(global::Tsu.Trees.RedGreen.Sample.SampleNode? parent) =>
            new global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample(this, parent);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor visitor) =>
            visitor.VisitNumericalLiteralExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<TResult> visitor) =>
            visitor.VisitNumericalLiteralExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitNumericalLiteralExpression(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitNumericalLiteralExpression(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitNumericalLiteralExpression(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample Update(
            double value
        )
        {
            if (
                this.Value != value
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.NumericalLiteralExpression(
                    value
                );
            }

            return this;
        }
    }
    partial class IdentifierExpressionSample : global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample
    {
        internal IdentifierExpressionSample(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind,
            string name
        )
            : base(kind)
        {
            this.SlotCount = 0;
            this._name = name;
        }

        public string Name => this._name;

        public override global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? GetSlot(int index) =>
            null;

        public override global::Tsu.Trees.RedGreen.Sample.SampleNode CreateRed(global::Tsu.Trees.RedGreen.Sample.SampleNode? parent) =>
            new global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample(this, parent);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor visitor) =>
            visitor.VisitIdentifierExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<TResult> visitor) =>
            visitor.VisitIdentifierExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitIdentifierExpression(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitIdentifierExpression(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitIdentifierExpression(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample Update(
            string name
        )
        {
            if (
                this.Name != name
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.IdentifierExpression(
                    name
                );
            }

            return this;
        }
    }
    partial class StatementSample : global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
    {
        protected StatementSample(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind,
            global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample? semicolon
        )
            : base(kind)
        {
            this._semicolon = semicolon;
        }

        public global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample? Semicolon => this._semicolon;






    }
    partial class ExpressionStatementSample : global::Tsu.Trees.RedGreen.Sample.Internal.StatementSample
    {
        internal ExpressionStatementSample(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind,
            global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample expression,
            global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample? semicolon
        )
            : base(kind, semicolon)
        {
            this.SlotCount = 2;
            this._expression = expression;
        }

        public global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample Expression => this._expression;

        public override global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? GetSlot(int index) =>
            index switch
            {
                0 => this._expression,
                1 => this._semicolon,
                _ => null
            };

        public override global::Tsu.Trees.RedGreen.Sample.SampleNode CreateRed(global::Tsu.Trees.RedGreen.Sample.SampleNode? parent) =>
            new global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample(this, parent);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor visitor) =>
            visitor.VisitExpressionStatement(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<TResult> visitor) =>
            visitor.VisitExpressionStatement(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitExpressionStatement(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitExpressionStatement(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitExpressionStatement(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionStatementSample Update(
            global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample expression,
            global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample? semicolon
        )
        {
            if (
                this.Expression != expression
                || this.Semicolon != semicolon
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.ExpressionStatement(
                    expression,
                    semicolon
                );
            }

            return this;
        }
    }
    partial class AssignmentStatement : global::Tsu.Trees.RedGreen.Sample.Internal.StatementSample
    {
        internal AssignmentStatement(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind,
            global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample identifier,
            global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample value,
            global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample? semicolon
        )
            : base(kind, semicolon)
        {
            this.SlotCount = 3;
            this._identifier = identifier;
            this._value = value;
        }

        public global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample Identifier => this._identifier;
        public global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample Value => this._value;

        public override global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? GetSlot(int index) =>
            index switch
            {
                0 => this._identifier,
                1 => this._value,
                2 => this._semicolon,
                _ => null
            };

        public override global::Tsu.Trees.RedGreen.Sample.SampleNode CreateRed(global::Tsu.Trees.RedGreen.Sample.SampleNode? parent) =>
            new global::Tsu.Trees.RedGreen.Sample.AssignmentStatement(this, parent);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor visitor) =>
            visitor.VisitAssignmentStatement(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<TResult> visitor) =>
            visitor.VisitAssignmentStatement(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitAssignmentStatement(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitAssignmentStatement(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitAssignmentStatement(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.Internal.AssignmentStatement Update(
            global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample identifier,
            global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample value,
            global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample? semicolon
        )
        {
            if (
                this.Identifier != identifier
                || this.Value != value
                || this.Semicolon != semicolon
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.Internal.SampleFactory.AssignmentStatement(
                    identifier,
                    value,
                    semicolon
                );
            }

            return this;
        }
    }
    partial class SemicolonTokenSample : global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
    {
        internal SemicolonTokenSample(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind
        )
            : base(kind)
        {
            this.SlotCount = 0;
        }


        public override global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? GetSlot(int index) =>
            null;

        public override global::Tsu.Trees.RedGreen.Sample.SampleNode CreateRed(global::Tsu.Trees.RedGreen.Sample.SampleNode? parent) =>
            new global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample(this, parent);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor visitor) =>
            visitor.VisitSemicolonToken(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<TResult> visitor) =>
            visitor.VisitSemicolonToken(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitSemicolonToken(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitSemicolonToken(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitSemicolonToken(this, arg1, arg2, arg3);

    }
}