using Microsoft.CodeAnalysis;

namespace Tsu.Trees.RedGreen.SourceGenerator.Model;

public sealed record Component(
    ITypeSymbol Type,
    string Name,
    bool IsOptional,
    bool PassToBase
);