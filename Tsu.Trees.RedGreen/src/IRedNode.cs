using Tsu.Trees.RedGreen.Internal;

namespace Tsu.Trees.RedGreen;

/// <summary>
/// Base interface for a red tree node.
/// </summary>
/// <typeparam name="TGreenRoot"></typeparam>
/// <typeparam name="TRedRoot"></typeparam>
/// <typeparam name="TKind"></typeparam>
public interface IRedNode<TGreenRoot, TRedRoot, TKind>
    where TGreenRoot : class, IGreenNode<TGreenRoot, TRedRoot, TKind>
    where TRedRoot : class, IRedNode<TGreenRoot, TRedRoot, TKind>
    where TKind : Enum
{

}