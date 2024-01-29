using Microsoft.CodeAnalysis;

namespace Tsu.Trees.RedGreen.SourceGenerator.Model;

public sealed record Component(
    ITypeSymbol Type,
    string FieldName,
    bool IsOptional,
    bool PassToBase
)
{
    private string? _parameterName, _propertyName;
    public string ParameterName => _parameterName ??= FieldName.TrimStart('_').ToCamelCase();
    public string PropertyName => _propertyName ??= FieldName.TrimStart('_').ToPascalCase();
}