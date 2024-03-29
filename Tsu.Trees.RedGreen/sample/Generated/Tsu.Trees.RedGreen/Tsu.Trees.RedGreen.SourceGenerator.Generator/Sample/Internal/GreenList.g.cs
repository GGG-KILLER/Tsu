﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Modified by the Tsu (https://github.com/GGG-KILLER/Tsu) project for embedding into other projects.
// <auto-generated />

#nullable enable

namespace Tsu.Trees.RedGreen.Sample.Internal
{
    internal abstract partial class SampleList : global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
    {
        internal SampleList()
            : base(ListKind)
        {
        }

        internal static global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode List(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode child) => child;

        internal static WithTwoChildren List(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode child0, global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode child1)
        {
            global::System.Diagnostics.Debug.Assert(child0 != null);
            global::System.Diagnostics.Debug.Assert(child1 != null);

            return new WithTwoChildren(child0, child1);
        }

        internal static WithThreeChildren List(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode child0, global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode child1, global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode child2)
        {
            global::System.Diagnostics.Debug.Assert(child0 != null);
            global::System.Diagnostics.Debug.Assert(child1 != null);
            global::System.Diagnostics.Debug.Assert(child2 != null);

            return new WithThreeChildren(child0, child1, child2);
        }

        internal static SampleList List(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode[] children)
        {
            return new WithManyChildren(children);
        }

        internal abstract void CopyTo(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode[] array, int offset);

        internal static global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? Concat(global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? left, global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? right)
        {
            if (left == null)
            {
                return right;
            }

            if (right == null)
            {
                return left;
            }

            var rightList = right as SampleList;
            if (left is SampleList leftList)
            {
                if (rightList != null)
                {
                    var tmp = new global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode[left.SlotCount + right.SlotCount];
                    leftList.CopyTo(tmp, 0);
                    rightList.CopyTo(tmp, left.SlotCount);
                    return List(tmp);
                }
                else
                {
                    var tmp = new global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode[left.SlotCount + 1];
                    leftList.CopyTo(tmp, 0);
                    tmp[left.SlotCount] = right;
                    return List(tmp);
                }
            }
            else if (rightList != null)
            {
                var tmp = new global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode[rightList.SlotCount + 1];
                tmp[0] = left;
                rightList.CopyTo(tmp, 1);
                return List(tmp);
            }
            else
            {
                return List(left, right);
            }
        }

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor visitor) =>
            throw new global::System.InvalidOperationException("A list must not be visited directly.");

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<TResult> visitor) =>
            throw new global::System.InvalidOperationException("A list must not be visited directly.");

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            throw new global::System.InvalidOperationException("A list must not be visited directly.");

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            throw new global::System.InvalidOperationException("A list must not be visited directly.");

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.Internal.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            throw new global::System.InvalidOperationException("A list must not be visited directly.");
    }
}
