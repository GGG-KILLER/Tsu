using System.ComponentModel;
using System.Diagnostics;
using Tsu.Trees.RedGreen.Utilities;

namespace Tsu.Trees.RedGreen.Internal;

/// <summary>
/// A global cache for all green nodes.
/// </summary>
/// <typeparam name="TGreenRoot"></typeparam>
/// <typeparam name="TRedRoot"></typeparam>
/// <typeparam name="TKind"></typeparam>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class GreenCache<TGreenRoot, TRedRoot, TKind>
    where TGreenRoot : class, IGreenNode<TGreenRoot, TRedRoot, TKind>
    where TRedRoot : class
    where TKind : Enum
{
        private const int CacheSizeBits = 16;
        private const int CacheSize = 1 << CacheSizeBits;
        private const int CacheMask = CacheSize - 1;

        private readonly struct Entry
        {
            public readonly int hash;
            public readonly TGreenRoot? node;

            internal Entry(int hash, TGreenRoot node)
            {
                this.hash = hash;
                this.node = node;
            }
        }

        private static readonly Entry[] s_cache = new Entry[CacheSize];

        /// <summary>
        /// Adds a node to the cache.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="hash"></param>
        public static void AddNode(TGreenRoot node, int hash)
        {
            if (AllChildrenInCache(node))
            {
                Debug.Assert(node.GetCacheHash() == hash);

                var idx = hash & CacheMask;
                s_cache[idx] = new Entry(hash, node);
            }
        }

        private static bool CanBeCached(TGreenRoot? child1) =>
            child1 == null || child1.IsCacheable;

        private static bool CanBeCached(TGreenRoot? child1, TGreenRoot? child2) =>
            CanBeCached(child1) && CanBeCached(child2);

        private static bool CanBeCached(TGreenRoot? child1, TGreenRoot? child2, TGreenRoot? child3) =>
            CanBeCached(child1) && CanBeCached(child2) && CanBeCached(child3);

        private static bool ChildInCache(TGreenRoot? child)
        {
            // for the purpose of this function consider that
            // null nodes, tokens and trivias are cached somewhere else.
            // TODO: should use slotCount
            if (child == null || child.SlotCount == 0) return true;

            int hash = child.GetCacheHash();
            int idx = hash & CacheMask;
            return s_cache[idx].node == child;
        }

        private static bool AllChildrenInCache(TGreenRoot node)
        {
            // TODO: should use slotCount
            var cnt = node.SlotCount;
            for (int i = 0; i < cnt; i++)
            {
                if (!ChildInCache(node.GetSlot(i)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Attempts to get the cached version of the given node from the cache.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="child1"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static TGreenRoot? TryGetNode(int kind, TGreenRoot? child1, out int hash)
        {
            if (CanBeCached(child1))
            {
                int h = hash = GetCacheHash(kind, child1);
                int idx = h & CacheMask;
                var e = s_cache[idx];
                if (e.hash == h && e.node != null && e.node.IsCacheEquivalent(kind, child1))
                {
                    return e.node;
                }
            }
            else
            {
                hash = -1;
            }

            return null;
        }

        /// <summary>
        /// Attempts to get the cached version of the given node from the cache.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="child1"></param>
        /// <param name="child2"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static TGreenRoot? TryGetNode(int kind, TGreenRoot? child1, TGreenRoot? child2, out int hash)
        {
            if (CanBeCached(child1, child2))
            {
                int h = hash = GetCacheHash(kind, child1, child2);
                int idx = h & CacheMask;
                var e = s_cache[idx];
                if (e.hash == h && e.node != null && e.node.IsCacheEquivalent(kind, child1, child2))
                {
                    return e.node;
                }
            }
            else
            {
                hash = -1;
            }

            return null;
        }

        /// <summary>
        /// Attempts to get the cached version of the given node from the cache.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="child1"></param>
        /// <param name="child2"></param>
        /// <param name="child3"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static TGreenRoot? TryGetNode(int kind, TGreenRoot? child1, TGreenRoot? child2, TGreenRoot? child3, out int hash)
        {
            if (CanBeCached(child1, child2, child3))
            {
                int h = hash = GetCacheHash(kind, child1, child2, child3);
                int idx = h & CacheMask;
                var e = s_cache[idx];
                if (e.hash == h && e.node != null && e.node.IsCacheEquivalent(kind, child1, child2, child3))
                {
                    return e.node;
                }
            }
            else
            {
                hash = -1;
            }

            return null;
        }

        private static int GetCacheHash(int kind, TGreenRoot? child1)
        {
            int code = kind;

            // the only child is never null
            // https://github.com/dotnet/roslyn/issues/41539
            code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child1!), code);

            // ensure nonnegative hash
            return code & int.MaxValue;
        }

        private static int GetCacheHash(int kind, TGreenRoot? child1, TGreenRoot? child2)
        {
            int code = kind;

            if (child1 != null)
            {
                code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child1), code);
            }
            if (child2 != null)
            {
                code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child2), code);
            }

            // ensure nonnegative hash
            return code & int.MaxValue;
        }

        private static int GetCacheHash(int kind, TGreenRoot? child1, TGreenRoot? child2, TGreenRoot? child3)
        {
            int code = kind;

            if (child1 != null)
            {
                code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child1), code);
            }
            if (child2 != null)
            {
                code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child2), code);
            }
            if (child3 != null)
            {
                code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child3), code);
            }

            // ensure nonnegative hash
            return code & int.MaxValue;
        }
}