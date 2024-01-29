﻿// <auto-generated />

#nullable enable

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Tsu.Trees.RedGreen.Sample
{
    abstract partial class SampleNode : global::Tsu.Trees.RedGreen.IRedNode<global::Tsu.Trees.RedGreen.Sample.SampleNode, global::Tsu.Trees.RedGreen.Sample.SampleKind>
    {
        private readonly global::Tsu.Trees.RedGreen.Sample.SampleNode? _parent;

        private protected SampleNode(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
        {
            this._parent = parent;
            this.Green = green;
        }

        public global::Tsu.Trees.RedGreen.Sample.SampleKind Kind => this.Green.Kind;
        internal global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode Green { get; }
        public global::Tsu.Trees.RedGreen.Sample.SampleNode? Parent => _parent;
        internal int SlotCount => this.Green.SlotCount;

        protected T? GetRed<T>(ref T? field, int index) where T : global::Tsu.Trees.RedGreen.Sample.SampleNode
        {
            var result = field;

            if (result == null)
            {
                var green = this.Green.GetSlot(index);
                if (green != null)
                {
                    global::System.Threading.Interlocked.CompareExchange(ref field, (T) green.CreateRed(this), null);
                    result = field;
                }
            }

            return result;
        }

        public bool IsEquivalentTo(global::Tsu.Trees.RedGreen.Sample.SampleNode? other)
        {
            if (this == other) return true;
            if (other == null) return false;

            return this.Green.IsEquivalentTo(other.Green);
        }

        public bool Contains(global::Tsu.Trees.RedGreen.Sample.SampleNode other)
        {
            for (var node = other; node != null; node = node.Parent)
            {
                if (node == this)
                    return true;
            }

            return false;
        }

        internal abstract global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index);

        internal global::Tsu.Trees.RedGreen.Sample.SampleNode GetRequiredNodeSlot(int index)
        {
            var node = this.GetNodeSlot(index);
            Debug.Assert(node != null);
            return node!;
        }

        public global::System.Collections.Generic.IEnumerable<global::Tsu.Trees.RedGreen.Sample.SampleNode> ChildNodes()
        {
            var count = this.SlotCount;
            for (var index = 0; index < count; index++)
                yield return this.GetRequiredNodeSlot(index);
        }

        public global::System.Collections.Generic.IEnumerable<global::Tsu.Trees.RedGreen.Sample.SampleNode> Ancestors() =>
            this.Parent?.AncestorsAndSelf() ?? global::System.Linq.Enumerable.Empty<global::Tsu.Trees.RedGreen.Sample.SampleNode>();

        public global::System.Collections.Generic.IEnumerable<global::Tsu.Trees.RedGreen.Sample.SampleNode> AncestorsAndSelf()
        {
            for (var node = this; node != null; node = node.Parent)
                yield return node;
        }

        public TNode? FirstAncestorOrSelf<TNode>(Func<TNode, bool>? predicate = null) where TNode : global::Tsu.Trees.RedGreen.Sample.SampleNode
        {
            for (var node = this; node != null; node = node.Parent)
            {
                if (node is TNode tnode && (predicate == null || predicate(tnode)))
                    return tnode;
            }

            return null;
        }

        public TNode? FirstAncestorOrSelf<TNode, TArg>(Func<TNode, TArg, bool> predicate, TArg argument) where TNode : global::Tsu.Trees.RedGreen.Sample.SampleNode
        {
            for (var node = this; node != null; node = node.Parent)
            {
                if (node is TNode tnode && (predicate == null || predicate(tnode, argument)))
                    return tnode;
            }

            return null;
        }

        public global::System.Collections.Generic.IEnumerable<global::Tsu.Trees.RedGreen.Sample.SampleNode> DescendantNodes(Func<global::Tsu.Trees.RedGreen.Sample.SampleNode, bool>? descendIntoChildren = null)
        {
            var stack = new Stack<global::Tsu.Trees.RedGreen.Sample.SampleNode>(24);
            foreach (var child in this.ChildNodes())
                stack.Push(child);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                yield return current;

                foreach (var child in current.ChildNodes().Reverse())
                {
                    stack.Push(child);
                }
            }
        }

        public global::System.Collections.Generic.IEnumerable<global::Tsu.Trees.RedGreen.Sample.SampleNode> DescendantNodesAndSelf(Func<global::Tsu.Trees.RedGreen.Sample.SampleNode, bool>? descendIntoChildren = null)
        {
            var stack = new Stack<global::Tsu.Trees.RedGreen.Sample.SampleNode>(24);
            stack.Push(this);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                yield return current;

                foreach (var child in current.ChildNodes().Reverse())
                {
                    stack.Push(child);
                }
            }
        }
    }

    public abstract partial class ExpressionSample : global::Tsu.Trees.RedGreen.Sample.SampleNode
    {

        internal ExpressionSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }


    }

    public sealed partial class FunctionCallExpressionSample : global::Tsu.Trees.RedGreen.Sample.ExpressionSample
    {
        private global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample? identifier;
        private global::Tsu.Trees.RedGreen.Sample.ExpressionSample? firstArg;
        private global::Tsu.Trees.RedGreen.Sample.ExpressionSample? secondArg;

        internal FunctionCallExpressionSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        public global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample Identifier => GetRed(ref this.identifier, 0)!;
        public global::Tsu.Trees.RedGreen.Sample.ExpressionSample FirstArg => GetRed(ref this.firstArg, 1)!;
        public global::Tsu.Trees.RedGreen.Sample.ExpressionSample? SecondArg => GetRed(ref this.secondArg, 2);

        internal override global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index) =>
            index switch
            {
                0 => GetRed(ref this.identifier, 0)!,
                1 => GetRed(ref this.firstArg, 1)!,
                2 => GetRed(ref this.secondArg, 2),
                _ => null
            };

        public global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample Update(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample identifier, global::Tsu.Trees.RedGreen.Sample.ExpressionSample firstArg, global::Tsu.Trees.RedGreen.Sample.ExpressionSample? secondArg)
        {
            if (identifier != this.Identifier && firstArg != this.FirstArg && secondArg != this.SecondArg)
            {
                return global::Tsu.Trees.RedGreen.Sample.SampleFactory.FunctionCallExpression(identifier, firstArg, secondArg);
            }

            return this;
        }

        public global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample WithIdentifier(global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample identifier) => this.Update(identifier, this.FirstArg, this.SecondArg);

        public global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample WithFirstArg(global::Tsu.Trees.RedGreen.Sample.ExpressionSample firstArg) => this.Update(this.Identifier, firstArg, this.SecondArg);

        public global::Tsu.Trees.RedGreen.Sample.FunctionCallExpressionSample WithSecondArg(global::Tsu.Trees.RedGreen.Sample.ExpressionSample? secondArg) => this.Update(this.Identifier, this.FirstArg, secondArg);
    }

    public sealed partial class BinaryOperationExpressionSample : global::Tsu.Trees.RedGreen.Sample.ExpressionSample
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

        public global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample Update(global::Tsu.Trees.RedGreen.Sample.SampleKind kind, global::Tsu.Trees.RedGreen.Sample.ExpressionSample left, global::Tsu.Trees.RedGreen.Sample.ExpressionSample right)
        {
            if (kind != this.Kind && left != this.Left && right != this.Right)
            {
                return global::Tsu.Trees.RedGreen.Sample.SampleFactory.BinaryOperationExpression(kind, left, right);
            }

            return this;
        }

        public global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample WithKind(global::Tsu.Trees.RedGreen.Sample.SampleKind kind) => this.Update(kind, this.Left, this.Right);

        public global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample WithLeft(global::Tsu.Trees.RedGreen.Sample.ExpressionSample left) => this.Update(this.Kind, left, this.Right);

        public global::Tsu.Trees.RedGreen.Sample.BinaryOperationExpressionSample WithRight(global::Tsu.Trees.RedGreen.Sample.ExpressionSample right) => this.Update(this.Kind, this.Left, right);
    }

    public sealed partial class NumericalLiteralExpressionSample : global::Tsu.Trees.RedGreen.Sample.ExpressionSample
    {

        internal NumericalLiteralExpressionSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        public double Value => ((global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample)this.Green).Value;

        internal override global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index) =>
            index switch
            {
                _ => null
            };

        public global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample Update(global::System.Double value)
        {
            if (value != this.Value)
            {
                return global::Tsu.Trees.RedGreen.Sample.SampleFactory.NumericalLiteralExpression(value);
            }

            return this;
        }

        public global::Tsu.Trees.RedGreen.Sample.NumericalLiteralExpressionSample WithValue(global::System.Double value) => this.Update(value);
    }

    public sealed partial class IdentifierExpressionSample : global::Tsu.Trees.RedGreen.Sample.ExpressionSample
    {

        internal IdentifierExpressionSample(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        public string Name => ((global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample)this.Green).Name;

        internal override global::Tsu.Trees.RedGreen.Sample.SampleNode? GetNodeSlot(int index) =>
            index switch
            {
                _ => null
            };

        public global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample Update(global::System.String name)
        {
            if (name != this.Name)
            {
                return global::Tsu.Trees.RedGreen.Sample.SampleFactory.IdentifierExpression(name);
            }

            return this;
        }

        public global::Tsu.Trees.RedGreen.Sample.IdentifierExpressionSample WithName(global::System.String name) => this.Update(name);
    }
}

