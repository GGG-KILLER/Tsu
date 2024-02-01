using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Scriban;
using Scriban.Functions;
using Scriban.Runtime;
using Tsu.Trees.RedGreen.SourceGenerator.Model;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class TemplateGenerator
{
    private static readonly Assembly _assembly = typeof(TemplateGenerator).Assembly;

    public static void RegisterTemplateOutput(this IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<Tree> trees)
    {
        var templates = LoadTemplates();

        context.RegisterSourceOutput(trees, (ctx, tree) =>
        {
            var builtins = new BuiltinFunctions();

            builtins.Import("no_suffix", (string str) => str.WithoutSuffix(tree.Suffix));
            builtins.SetReadOnly("no_suffix", true);

            builtins.Import("derives_from", (ScriptTypeSymbol symbolA, ScriptTypeSymbol symbolB) =>
                symbolA.Symbol.DerivesFrom((INamedTypeSymbol) symbolB.Symbol));
            builtins.SetReadOnly("derives_from", true);

            builtins.Import("as_red", (ScriptTypeSymbol symbol) =>
            {
                if (symbol.Symbol.DerivesFrom(tree.GreenBase))
                {
                    return $"{tree.RedBase.ContainingNamespace.ToCSharpString(false)}.{symbol.Name}{(symbol.Symbol.NullableAnnotation == NullableAnnotation.Annotated ? "?" : "")}";
                }
                else
                {
                    return symbol.CSharp;
                }
            });
            builtins.SetReadOnly("as_red", true);

            builtins.Import("not_null", (string str) => str.WithoutSuffix("?"));
            builtins.SetReadOnly("not_null", true);

            builtins.Import("not_global", (string str) => str.StartsWith("global::") ? str.Substring("global::".Length) : str);
            builtins.SetReadOnly("not_global", true);

            builtins.Import("is_list", (ScriptTypeSymbol symbol) =>
            {
                return (SymbolEqualityComparer.Default.Equals(symbol.Symbol.ContainingNamespace, tree.GreenBase.ContainingNamespace)
                    || SymbolEqualityComparer.Default.Equals(symbol.Symbol.ContainingNamespace, tree.RedBase.ContainingNamespace))
                    && symbol.Name == $"{tree.Suffix}List";
            });

            var context = new TemplateContext(builtins, StringComparer.OrdinalIgnoreCase)
            {
                EnableRelaxedIndexerAccess = false,
                EnableRelaxedMemberAccess = false,
                MemberFilter = null
            };
            var globals = new ScriptObject();
            globals.Import(new ScriptTree(tree));
            context.PushGlobal(globals);

            foreach (var template in templates)
            {
                ctx.AddSource($"{tree.Suffix}/{template.Path.WithoutSuffix(".sbn-cs")}.g.cs", template.Template.Render(context));
            }
        });
    }

    private static ImmutableArray<(string Path, Template Template)> LoadTemplates()
    {
        return _assembly.GetManifestResourceNames().Where(x => x.EndsWith(".sbn-cs")).Select(path =>
        {
            string raw;
            using (var stream = _assembly.GetManifestResourceStream(path))
            using (var reader = new StreamReader(stream))
            {
                raw = reader.ReadToEnd();
            }
            var template = Template.Parse(raw, path);

            if (template.HasErrors)
            {
                throw new InvalidOperationException(string.Join("\n", template.Messages));
            }

            return (path, template);
        })
            .ToImmutableArray();
    }
}