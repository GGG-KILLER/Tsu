using System.Collections.Immutable;
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
        var templates = LoadTemplates();

        context.RegisterSourceOutput(trees, (ctx, tree) =>
        {
            var builtins = GetBuiltins();
            builtins.Import("nosuffix", (string str) => str.WithoutSuffix(tree.Suffix));
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

    private static BuiltinFunctions GetBuiltins()
    {
        var builtins = new BuiltinFunctions();
        builtins.SetValue("csharp", new CSharpFunctions(), true);
        return builtins;
    }

    private static string DumpObject(ScriptObject obj)
    {
        var builder = new StringBuilder();
        dumpKeys(builder, obj);
        return builder.ToString();

        static void dumpKeys(StringBuilder builder, ScriptObject obj, int depth = 0)
        {
            foreach (var kv in obj)
            {
                builder.Append(new string(' ', depth * 2));
                if (kv.Value is ScriptObject scriptObj && depth < 3)
                {
                    builder.AppendLine($"#region {kv.Key}");
                    dumpKeys(builder, scriptObj, depth + 1);
                    builder.AppendLine($"#endregion {kv.Key}");
                }
                else
                {
                    builder.Append("// ");
                    builder.AppendLine($"{kv.Key}: {kv.Value}");
                }
            }
        }
    }

    private sealed class CSharpFunctions : ScriptObject
    {
        public static string Namespace(INamespaceSymbol symbol, bool noGlobal = true) => symbol.ToCSharpString(noGlobal);
        public static string Type(ITypeSymbol symbol, bool addNullable = true) => symbol.ToCSharpString(addNullable);
        public static string Accessibility(Accessibility accessibility) => accessibility.ToCSharpString();
    }
}