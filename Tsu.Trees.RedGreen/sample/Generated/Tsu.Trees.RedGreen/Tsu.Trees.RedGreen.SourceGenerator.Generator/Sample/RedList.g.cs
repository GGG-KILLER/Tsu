﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Modified by the Tsu (https://github.com/GGG-KILLER/Tsu) project for embedding into other projects.
// <auto-generated />

#nullable enable

namespace Tsu.Trees.RedGreen.Sample
{
    // This is supposed to be a hidden node so it is marked as internal.
    internal abstract partial class SampleList : global::Tsu.Trees.RedGreen.Sample.SampleNode
    {
        internal SampleList(global::Tsu.Trees.RedGreen.Sample.Internal.SampleList green, global::Tsu.Trees.RedGreen.Sample.SampleNode? parent)
            : base(green, parent)
        {
        }

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override void Accept(global::Tsu.Trees.RedGreen.Sample.SampleVisitor visitor) =>
            throw new global::System.InvalidOperationException("A list must not be visited directly.");

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<TResult> visitor) =>
            throw new global::System.InvalidOperationException("A list must not be visited directly.");

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, TResult> visitor, T1 arg1) =>
            throw new global::System.InvalidOperationException("A list must not be visited directly.");

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, TResult> visitor, T1 arg1, T2 arg2) =>
            throw new global::System.InvalidOperationException("A list must not be visited directly.");

        [return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
        public override TResult Accept<T1, T2, T3, TResult>(global::Tsu.Trees.RedGreen.Sample.SampleVisitor<T1, T2, T3, TResult> visitor, T1 arg1, T2 arg2, T3 arg3) =>
            throw new global::System.InvalidOperationException("A list must not be visited directly.");
    }
}
