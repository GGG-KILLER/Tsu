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
            builtins.Import(new TemplateHelpers(tree));

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

    private sealed class TemplateHelpers : ScriptObject
    {
        private readonly Tree _tree;

        public TemplateHelpers(Tree tree) : base(9, autoImportStaticsFromThisType: false)
        {
            _tree = tree;

            var methods = typeof(TemplateHelpers).GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static);
            if (methods.Length == 0) throw new InvalidOperationException("No helper methods found.");
            foreach (var method in methods)
            {
                SetValue(
                    StandardMemberRenamer.Rename(method),
                    DynamicCustomFunction.Create(method.IsStatic ? null : this, method),
                    true
                );
            }
        }

        public string NoSuffix(string value) => value.WithoutSuffix(_tree.Suffix);

        public bool IsGreenNode(ScriptTypeSymbol symbol) => symbol.Symbol.DerivesFrom(_tree.GreenBase);

        public string AsRed(ScriptTypeSymbol symbol)
        {
            if (symbol.Symbol.DerivesFrom(_tree.GreenBase))
            {
                return $"{_tree.RedBase.ContainingNamespace.ToCSharpString(false)}.{symbol.Name}{(symbol.Symbol.NullableAnnotation == NullableAnnotation.Annotated ? "?" : "")}";
            }
            else
            {
                return symbol.CSharp;
            }
        }

        public string FieldType(ScriptComponent component, bool isGreen) => ParameterType(component, isGreen);

        public string ParameterType(ScriptComponent component, bool isGreen)
        {
            if (IsGreenNode(component.Type))
            {
                var ns = isGreen ? _tree.GreenBase.ContainingNamespace : _tree.RedBase.ContainingNamespace;
                if (component.IsList)
                {
                    return $"{ns.ToCSharpString(false)}.{_tree.Suffix}List";
                }
                else
                {
                    return $"{ns.ToCSharpString(false)}.{component.Type.Name}";
                }
            }
            else
            {
                return component.Type.CSharp;
            }
        }

        public string PropertyType(ScriptComponent component, bool isGreen)
        {
            if (IsGreenNode(component.Type))
            {
                var ns = isGreen ? _tree.GreenBase.ContainingNamespace : _tree.RedBase.ContainingNamespace;
                if (component.IsList)
                {
                    return $"{ns.ToCSharpString(false)}.{_tree.Suffix}List<{ns.ToCSharpString(false)}.{component.Type.Name}>";
                }
                else
                {
                    return $"{ns.ToCSharpString(false)}.{component.Type.Name}";
                }
            }
            else
            {
                return component.Type.CSharp;
            }
        }

        public static string NotNull(string value) => value.WithoutSuffix("?");
        public static string NotGlobal(string value) => value.StartsWith("global::") ? value.Substring("global::".Length) : value;
    }
}