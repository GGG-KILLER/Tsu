﻿// <auto-generated />

#nullable enable


namespace Tsu.Trees.RedGreen.Sample
{
    public abstract partial class ExpressionSample : global::Tsu.Trees.RedGreen.Sample.SampleNode
    {

        internal ExpressionSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }







    }
    public partial class FunctionCallExpressionSample : global::Tsu.Trees.RedGreen.Sample.ExpressionSample
    {
        private global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample? _identifier;
        private global::Tsu.Trees.RedGreen.Sample.SampleNode? _args;

        internal FunctionCallExpressionSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        public global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample Identifier => GetRed(ref this._identifier, 0)!;
            public global::Tsu.Trees.RedGreen.Sample.SampleList<global::Tsu.Trees.RedGreen.Sample.ExpressionSample> Args => new global::Tsu.Trees.RedGreen.Sample.SampleList<global::Tsu.Trees.RedGreen.Sample.ExpressionSample>(GetRed(ref this._args, 1));

        internal override global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index) =>
                index switch
                {
                    0 => GetRed(ref this._identifier, 0)!,
                    1 => GetRed(ref this._args, 1),
                    _ => null
                };

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.SampleVisitor visitor) =>
            visitor.VisitFunctionCallExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<TResult> visitor) =>
            visitor.VisitFunctionCallExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitFunctionCallExpression(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitFunctionCallExpression(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitFunctionCallExpression(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample Update(
            global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample identifier,
            global::Tsu.Trees.RedGreen.Sample.SampleList<global::Tsu.Trees.RedGreen.Sample.ExpressionSample> args
        )
        {
            if (
                this.Identifier != identifier
                || this.Args != args
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.SampleFactory.FunctionCallExpression(
                    identifier,
                    args
                );
            }

            return this;
        }

        public global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample WithIdentifier(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample identifier) =>
            this.Update(
                identifier,
                this.Args
            );
        public global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample WithArgs(global::Tsu.Trees.RedGreen.Sample.SampleList<global::Tsu.Trees.RedGreen.Sample.ExpressionSample> args) =>
            this.Update(
                this.Identifier,
                args
            );
    }
    public partial class BinaryOperationExpressionSample : global::Tsu.Trees.RedGreen.Sample.ExpressionSample
    {
        private global::Tsu.Trees.RedGreen.Sample.ExpressionSample? _left;
        private global::Tsu.Trees.RedGreen.Sample.ExpressionSample? _right;

        internal BinaryOperationExpressionSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        public global::Tsu.Trees.RedGreen.Sample.ExpressionSample Left => GetRed(ref this._left, 0)!;
        public global::Tsu.Trees.RedGreen.Sample.ExpressionSample Right => GetRed(ref this._right, 1)!;

        internal override global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index) =>
                index switch
                {
                    0 => GetRed(ref this._left, 0)!,
                    1 => GetRed(ref this._right, 1)!,
                    _ => null
                };

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.SampleVisitor visitor) =>
            visitor.VisitBinaryOperationExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<TResult> visitor) =>
            visitor.VisitBinaryOperationExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitBinaryOperationExpression(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitBinaryOperationExpression(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitBinaryOperationExpression(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample Update(
            global::Tsu.Trees.RedGreen.Sample.SampleKind kind,
            global::Tsu.Trees.RedGreen.Sample.ExpressionSample left,
            global::Tsu.Trees.RedGreen.Sample.ExpressionSample right
        )
        {
            if (
                this.Kind != kind
                || this.Left != left
                || this.Right != right
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.SampleFactory.BinaryOperationExpression(
                    kind,
                    left,
                    right
                );
            }

            return this;
        }

        public global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample WithKind(global::Tsu.Trees.RedGreen.Sample.SampleKind kind) =>
            this.Update(
                kind,
                this.Left,
                this.Right
            );
        public global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample WithLeft(global::Tsu.Trees.RedGreen.Sample.ExpressionSample left) =>
            this.Update(
                this.Kind,
                left,
                this.Right
            );
        public global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample WithRight(global::Tsu.Trees.RedGreen.Sample.ExpressionSample right) =>
            this.Update(
                this.Kind,
                this.Left,
                right
            );
    }
    public partial class NumericalLiteralExpressionSample : global::Tsu.Trees.RedGreen.Sample.ExpressionSample
    {

        internal NumericalLiteralExpressionSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        public  double Value => ((global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample)this.Green).Value;

        internal override global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index) =>
                null;

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.SampleVisitor visitor) =>
            visitor.VisitNumericalLiteralExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<TResult> visitor) =>
            visitor.VisitNumericalLiteralExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitNumericalLiteralExpression(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitNumericalLiteralExpression(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitNumericalLiteralExpression(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample Update(
            double value
        )
        {
            if (
                this.Value != value
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.SampleFactory.NumericalLiteralExpression(
                    value
                );
            }

            return this;
        }

        public global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample WithValue(double value) =>
            this.Update(
                value
            );
    }
    public partial class IdentifierExpressionSample : global::Tsu.Trees.RedGreen.Sample.ExpressionSample
    {

        internal IdentifierExpressionSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        public  string Name => ((global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample)this.Green).Name;

        internal override global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index) =>
                null;

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.SampleVisitor visitor) =>
            visitor.VisitIdentifierExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<TResult> visitor) =>
            visitor.VisitIdentifierExpression(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitIdentifierExpression(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitIdentifierExpression(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitIdentifierExpression(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample Update(
            string name
        )
        {
            if (
                this.Name != name
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.SampleFactory.IdentifierExpression(
                    name
                );
            }

            return this;
        }

        public global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample WithName(string name) =>
            this.Update(
                name
            );
    }
    public abstract partial class StatementSample : global::Tsu.Trees.RedGreen.Sample.SampleNode
    {

        internal StatementSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        public abstract global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample Semicolon { get; }






    }
    public partial class ExpressionStatementSample : global::Tsu.Trees.RedGreen.Sample.StatementSample
    {
        private global::Tsu.Trees.RedGreen.Sample.ExpressionSample? _expression;
        private global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample? _semicolon;

        internal ExpressionStatementSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        public global::Tsu.Trees.RedGreen.Sample.ExpressionSample Expression => GetRed(ref this._expression, 0)!;
        public override global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample Semicolon => GetRed(ref this._semicolon, 1)!;

        internal override global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index) =>
                index switch
                {
                    0 => GetRed(ref this._expression, 0)!,
                    1 => GetRed(ref this._semicolon, 1)!,
                    _ => null
                };

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.SampleVisitor visitor) =>
            visitor.VisitExpressionStatement(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<TResult> visitor) =>
            visitor.VisitExpressionStatement(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitExpressionStatement(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitExpressionStatement(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitExpressionStatement(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample Update(
            global::Tsu.Trees.RedGreen.Sample.ExpressionSample expression,
            global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample semicolon
        )
        {
            if (
                this.Expression != expression
                || this.Semicolon != semicolon
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.SampleFactory.ExpressionStatement(
                    expression,
                    semicolon
                );
            }

            return this;
        }

        public global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample WithExpression(global::Tsu.Trees.RedGreen.Sample.ExpressionSample expression) =>
            this.Update(
                expression,
                this.Semicolon
            );
        public global::Tsu.Trees.RedGreen.Sample.ExpressionStatementSample WithSemicolon(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample semicolon) =>
            this.Update(
                this.Expression,
                semicolon
            );
    }
    public partial class AssignmentStatement : global::Tsu.Trees.RedGreen.Sample.StatementSample
    {
        private global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample? _identifier;
        private global::Tsu.Trees.RedGreen.Sample.ExpressionSample? _value;
        private global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample? _semicolon;

        internal AssignmentStatement(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        public global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample Identifier => GetRed(ref this._identifier, 0)!;
        public global::Tsu.Trees.RedGreen.Sample.ExpressionSample Value => GetRed(ref this._value, 1)!;
        public override global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample Semicolon => GetRed(ref this._semicolon, 2)!;

        internal override global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index) =>
                index switch
                {
                    0 => GetRed(ref this._identifier, 0)!,
                    1 => GetRed(ref this._value, 1)!,
                    2 => GetRed(ref this._semicolon, 2)!,
                    _ => null
                };

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.SampleVisitor visitor) =>
            visitor.VisitAssignmentStatement(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<TResult> visitor) =>
            visitor.VisitAssignmentStatement(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitAssignmentStatement(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitAssignmentStatement(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitAssignmentStatement(this, arg1, arg2, arg3);

        public global::Tsu.Trees.RedGreen.Sample.AssignmentStatement Update(
            global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample identifier,
            global::Tsu.Trees.RedGreen.Sample.ExpressionSample value,
            global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample semicolon
        )
        {
            if (
                this.Identifier != identifier
                || this.Value != value
                || this.Semicolon != semicolon
            )
            {
                return global::Tsu.Trees.RedGreen.Sample.SampleFactory.AssignmentStatement(
                    identifier,
                    value,
                    semicolon
                );
            }

            return this;
        }

        public global::Tsu.Trees.RedGreen.Sample.AssignmentStatement WithIdentifier(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample identifier) =>
            this.Update(
                identifier,
                this.Value,
                this.Semicolon
            );
        public global::Tsu.Trees.RedGreen.Sample.AssignmentStatement WithValue(global::Tsu.Trees.RedGreen.Sample.ExpressionSample value) =>
            this.Update(
                this.Identifier,
                value,
                this.Semicolon
            );
        public global::Tsu.Trees.RedGreen.Sample.AssignmentStatement WithSemicolon(global::Tsu.Trees.RedGreen.Sample.SemicolonTokenSample semicolon) =>
            this.Update(
                this.Identifier,
                this.Value,
                semicolon
            );
    }
    public partial class SemicolonTokenSample : global::Tsu.Trees.RedGreen.Sample.SampleNode
    {

        internal SemicolonTokenSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }


        internal override global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index) =>
                null;

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.SampleVisitor visitor) =>
            visitor.VisitSemicolonToken(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<TResult> visitor) =>
            visitor.VisitSemicolonToken(this);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            visitor.VisitSemicolonToken(this, arg1);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            visitor.VisitSemicolonToken(this, arg1, arg2);

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            visitor.VisitSemicolonToken(this, arg1, arg2, arg3);


    }
}