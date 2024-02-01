using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tsu.Trees.RedGreen.SourceGenerator;

internal static class ResourceCopier
{
    private static readonly Assembly _assembly = typeof(TemplateGenerator).Assembly;

    public static void RegisterResourcesToCopy(this IncrementalGeneratorInitializationContext context, IEnumerable<string> paths)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            foreach (var path in paths)
            {
                SourceText sourceText;

                using (var stream = _assembly.GetManifestResourceStream(path))
                using (var reader = new StreamReader(stream))
                {
                    sourceText = SourceText.From(reader, (int) stream.Length, Encoding.UTF8);
                }

                ctx.AddSource(path, sourceText);
            }
        });
    }
}