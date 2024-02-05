using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using System.Text;
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
        var initSw = Stopwatch.StartNew();
        var templates = LoadTemplates();
        initSw.Stop();

        context.RegisterSourceOutput(trees, (ctx, tree) =>
        {
            var treeInitSw = Stopwatch.StartNew();
            var scriptTree = new ScriptTree(tree);

            var builtins = new BuiltinFunctions();
            builtins.Import(new TemplateHelpers(scriptTree));

            var context = new TemplateContext(builtins, StringComparer.OrdinalIgnoreCase)
            {
                EnableRelaxedIndexerAccess = false,
                EnableRelaxedMemberAccess = false,
                MemberFilter = null
            };
            var globals = new ScriptObject();
            globals.Import(scriptTree);
            context.PushGlobal(globals);
            treeInitSw.Stop();

            var renderSw = new Stopwatch();
            var elapsed = new List<(string Path, TimeSpan Elapsed)>();
            foreach (var template in templates)
            {
                string rendered;
                try
                {
                    renderSw.Restart();
                    rendered = template.Template.Render(context);
                    renderSw.Stop();
                    elapsed.Add((template.Path, renderSw.Elapsed));
                }
                catch (Exception ex)
                {
                    rendered = ex.ToString();
                }

                ctx.AddSource($"{tree.Suffix}/{template.Path.WithoutSuffix(".sbn-cs")}.g.cs", rendered);
            }

#if DEBUG
            var builder = new StringBuilder();
            builder.AppendLine($"// Templates Load: {initSw.Elapsed.TotalMilliseconds}ms")
                   .AppendLine($"// Tree Init: {treeInitSw.Elapsed.TotalMilliseconds}ms");
            foreach (var s in elapsed.OrderByDescending<(string Path, TimeSpan Elapsed), TimeSpan>(x => x.Elapsed))
                builder.AppendLine($"// {s.Path}: {s.Elapsed.TotalMilliseconds}ms");
            builder.AppendLine($"// Total: {TimeSpan.FromTicks(initSw.Elapsed.Ticks + treeInitSw.Elapsed.Ticks + elapsed.Sum(x => x.Elapsed.Ticks)).TotalMilliseconds}ms");
            ctx.AddSource($"{tree.Suffix}/TemplateTimings.g.cs", builder.ToSourceText());
#endif
        });
    }

    private static ImmutableArray<(string Path, Template Template)> LoadTemplates()
    {
        var names = _assembly.GetManifestResourceNames().Where(x => x.EndsWith(".sbn-cs"));
        return names.Select(loadTemplate).ToImmutableArray();

        static (string Path, Template Template) loadTemplate(string path)
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
        }
    }

    private sealed class TemplateHelpers : ScriptObject
    {
        private static readonly MethodInfo[] _methods = typeof(TemplateHelpers).GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static);
        private readonly ScriptTree _tree;

        public TemplateHelpers(ScriptTree tree) : base(9, autoImportStaticsFromThisType: false)
        {
            _tree = tree;

            foreach (var method in _methods)
            {
                SetValue(
                    StandardMemberRenamer.Rename(method),
                    DynamicCustomFunction.Create(method.IsStatic ? null : this, method),
                    true
                );
            }
        }

        public string NoSuffix(string value) => value.WithoutSuffix(_tree.Suffix);

        public bool IsGreenNode(ScriptTypeSymbol symbol) => symbol.Symbol.DerivesFrom(_tree.GreenBase.Symbol);

        public string AsRed(ScriptTypeSymbol symbol)
        {
            return IsGreenNode(symbol)
                ? symbol.CSharpNoNullable.Replace(symbol.Namespace, _tree.RedBase.Namespace)
                : symbol.CSharp;
        }

        public string FieldType(ScriptComponent component, bool isGreen)
        {
            if (component.IsList)
            {
                return isGreen
                    ? _tree.GreenBase.CSharpNoNullable + '?'
                    : _tree.RedBase.CSharpNoNullable;
            }
            else if (IsGreenNode(component.Type))
            {
                var type = SymbolEqualityComparer.Default.Equals(component.Type.Symbol, _tree.GreenBase.Symbol)
                    ? _tree.RedBase
                    : component.Type;
                if (!component.IsOptional && isGreen)
                    return $"{(isGreen ? type.CSharpNoNullable : AsRed(type))}";
                else
                    return $"{(isGreen ? type.CSharpNoNullable : AsRed(type))}?";
            }
            else
            {
                return component.Type.CSharp;
            }
        }

        public string ParameterType(ScriptComponent component, bool isGreen) => PropertyType(component, isGreen);

        public string PropertyType(ScriptComponent component, bool isGreen)
        {
            if (IsGreenNode(component.Type))
            {
                var ns = isGreen ? _tree.GreenBase.Namespace : _tree.RedBase.Namespace;
                var type = isGreen ? component.Type.CSharpNoNullable : AsRed(component.Type);
                if (component.IsList)
                {
                    return $"{ns}.{_tree.Suffix}List<{type}>";
                }
                else
                {
                    return type;
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