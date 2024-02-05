﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Modified by the Tsu (https://github.com/GGG-KILLER/Tsu) project for embedding into other projects.
// <auto-generated />

#nullable enable

namespace Tsu.Trees.RedGreen.Sample.Internal
{
    internal readonly struct SampleListBuilder<TNode> where TNode : global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
    {
        private readonly SampleListBuilder _builder;

        public SampleListBuilder(int size)
            : this(new SampleListBuilder(size))
        {
        }

        public static SampleListBuilder<TNode> Create() => new(8);

        internal SampleListBuilder(SampleListBuilder builder)
        {
            _builder = builder;
        }

        public bool IsNull => _builder == null;

        public int Count => _builder.Count;

        public TNode? this[int index]
        {
            get => (TNode?) _builder[index];

            set => _builder[index] = value;
        }

        public void Clear() => _builder.Clear();

        public SampleListBuilder<TNode> Add(TNode node)
        {
            _builder.Add(node);
            return this;
        }

        public void AddRange(TNode[] items, int offset, int length) =>
            _builder.AddRange(items, offset, length);

        public void AddRange(SampleList<TNode> nodes) => _builder.AddRange(nodes);

        public void AddRange(SampleList<TNode> nodes, int offset, int length) =>
            _builder.AddRange(nodes, offset, length);

        public bool Any(global::Tsu.Trees.RedGreen.Sample.SampleKind kind) => _builder.Any(kind);

        public SampleList<TNode> ToList() => _builder.ToList();

        public global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode? ToListNode() => _builder.ToListNode();

        public static implicit operator SampleListBuilder(SampleListBuilder<TNode> builder) => builder._builder;

        public static implicit operator SampleList<TNode>(SampleListBuilder<TNode> builder)
        {
            if (builder._builder != null)
            {
                return builder.ToList();
            }

            return default;
        }

        public SampleList<TDerived> ToList<TDerived>() where TDerived : global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode =>
            new(ToListNode());
    }
}
