﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Modified by the Tsu (https://github.com/GGG-KILLER/Tsu) project for embedding into other projects.
// <auto-generated />

#nullable enable

namespace Tsu.Trees.RedGreen.Sample
{
    abstract partial class SampleNode
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
        internal bool IsList => this.Green.IsList;

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

        /// <summary>
        /// This works the same as GetRed, but intended to be used in lists
        /// The only difference is that the public parent of the node is not the list,
        /// but the list's parent. (element's grand parent).
        /// </summary>
        protected global::Tsu.Trees.RedGreen.Sample.SampleNode? GetRedElement(ref global::Tsu.Trees.RedGreen.Sample.SampleNode? element, int slot)
        {
            global::System.Diagnostics.Debug.Assert(IsList);

            var result = element;

            if (result == null)
            {
                var green = Green.GetRequiredSlot(slot);
                // passing list's parent
                global::System.Threading.Interlocked.CompareExchange(ref element, green.CreateRed(Parent), null);
                result = element;
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
            global::System.Diagnostics.Debug.Assert(node != null);
            return node!;
        }

        public abstract void Accept(global::Tsu.Trees.RedGreen.Sample.SampleVisitor visitor);
        public abstract TResult? Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<TResult> visitor);
        public abstract TResult? Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, TResult> visitor, T1 arg1);
        public abstract TResult? Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2);
        public abstract TResult? Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3);

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
}