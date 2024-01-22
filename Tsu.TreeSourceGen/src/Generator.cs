// Copyright © 2024 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Tsu.TreeSourceGen;

/// <summary>
/// The main source generator of this library.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class Generator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource($"{CodeConstants.TreeNodeAttribute.FullName}.g.cs", CodeConstants.TreeNodeAttribute.SourceCode);
            ctx.AddSource($"{CodeConstants.TreeVisitorAttribute.FullName}.g.cs", CodeConstants.TreeVisitorAttribute.SourceCode);
            ctx.AddSource($"{CodeConstants.TreeWalkerAttribute.FullName}.g.cs", CodeConstants.TreeWalkerAttribute.SourceCode);
        });

        // This pipeline finds all marked nodes and then proceeds to assemble the tree "structure" from them.
        // (structure in quote because we don't look at inheritance as it's not necessary)
        var trees = context.GetTrees();

        // Map all visitors that were defined
        var visitors = context.GetVisitors($"{CodeConstants.Namespace}.{CodeConstants.TreeVisitorAttribute.FullName}").Collect();

        // Collect all declared walkers
        var walkers = context.GetVisitors($"{CodeConstants.Namespace}.{CodeConstants.TreeWalkerAttribute.FullName}");

        // Make the final pairs
        var visitorPairs = trees.MakeVisitorPairs(visitors);
        var walkerSets = trees.MakeWalkerSets(visitors, walkers);

        // Validate that the nodes properly inherit from the root.
        context.RegisterSourceOutput(trees, (ctx, tree) =>
        {
            foreach (var node in tree.Nodes)
            {
                var inherits = false;
                for (var parent = node.TypeSymbol; parent is not null; parent = parent.BaseType)
                {
                    if (SymbolEqualityComparer.Default.Equals(tree.Root, parent))
                        inherits = true;
                }

                if (!inherits)
                {
                    ctx.ReportDiagnostic(TsgDiagnostics.Create(
                        descriptor: TsgDiagnostics.NodeDoesNotInheritFromRoot,
                        node.TypeSymbol,
                        messageArgs: node.TypeSymbol.Name));
                }
            }
        });

        // Write out all the code
        var namespaces = new[]
        {
            "System",
            "System.Collections.Generic",
            "System.Diagnostics.CodeAnalysis",
        };

        context.RegisterSourceOutput(
            visitorPairs,
            (ctx, pair) =>
            {
                var tree = pair.Tree;
                var visitors = pair.Visitors;

                var rootNs = tree.Root.GetContainingNamespace();

                var builder = new StringBuilder();
                var writer = new VisitorWriter(builder);

                foreach (var visitor in visitors!.Visitors)
                {
                    builder.Clear();

                    writer.WriteFileHeader();

                    foreach (var ns in namespaces)
                    {
                        writer.Write("using ");
                        writer.Write(ns);
                        writer.WriteLine(';');
                    }

                    foreach (var node in tree.Nodes)
                    {
                        writer.WriteLine();
                        writer.Write("namespace ");
                        writer.WriteLine(node.TypeSymbol.GetContainingNamespace());
                        writer.WriteLine('{');
                        writer.Indent++;

                        writer.Indent--;
                        writer.WriteLine('}');
                    }

                    writer.WriteLine();
                    writer.Write("namespace ");
                    writer.WriteLine(visitor.Namespace);
                    writer.WriteLine('{');
                    writer.Indent++;
                    writer.WithParents(visitor.RootClass, vclass =>
                    {
                        // Write out the interface
                        {
                            writer.Write("interface I");
                            writer.Write(vclass.Name);
                            writer.WriteTypeParameterList(visitor.Arity);
                            writer.WriteLine();
                            writer.WriteLine('{');
                            writer.Indent++;

                            foreach (var node in tree.Nodes)
                            {
                                writer.WriteSignature(node, visitor);
                                writer.WriteLine(';');
                            }

                            writer.Indent--;
                            writer.WriteLine('}');
                        }

                        // Write out the base class
                        {
                            writer.Write("partial ");
                            writer.Write(vclass.Keyword);
                            writer.Write(' ');
                            writer.Write(vclass.Name);
                            writer.WriteTypeParameterList(visitor.Arity);
                            writer.Write(" : I");
                            writer.Write(vclass.Name);
                            writer.WriteTypeParameterList(visitor.Arity);
                            writer.Write(' ');
                            writer.WriteLine(vclass.Constraints);
                            writer.WriteLine('{');
                            writer.Indent++;
                            {
                                // Entry visit method
                                {
                                    writer.Write("public virtual ");
                                    var arg = writer.WriteSignature(rootNs, tree.Root.Name, tree.Root.Name, visitor.Arity);
                                    writer.Write('{');
                                    writer.Indent++;

                                    writer.Write("if (");
                                    writer.Write(arg);
                                    writer.WriteLine(" is not null)");
                                    writer.Indent++;
                                    writer.Write("return ");
                                    writer.Write(arg);
                                    writer.Write(".Accept(");
                                    writer.Write("this");
                                    for (var idx = 1; idx < visitor.Arity; idx++)
                                    {
                                        writer.Write(", ");
                                        writer.Write("arg");
                                        writer.Write(idx);
                                    }
                                    writer.WriteLine(");");

                                    writer.WriteLine();
                                    writer.WriteLine("return default;");
                                    writer.Indent--;
                                    writer.Write('}');
                                }

                                writer.WriteLine();

                                // Default Visit Method
                                {
                                    writer.Write("public virtual ");
                                    writer.Write(visitor.Arity > 0 ? "TReturn " : "void ");
                                    writer.Write("DefaultVisit");
                                    writer.Write('(');

                                    writer.Write(rootNs);
                                    writer.Write('.');
                                    writer.Write(tree.Root.Name);
                                    writer.Write(' ');
                                    writer.Write("node");
                                    for (var idx = 1; idx < visitor.Arity; idx++)
                                    {
                                        writer.Write(", ");
                                        writer.Write("TArg");
                                        writer.Write(idx);
                                        writer.Write(" arg");
                                        writer.Write(idx);
                                    }
                                    writer.WriteLine(") => default;");
                                }

                                foreach (var node in tree.Nodes)
                                {
                                    writer.WriteLine();
                                    var arg = writer.WriteSignature(node, visitor);
                                    writer.WriteLine(" => DefaultVisit(");
                                    writer.Write(arg);
                                    for (var idx = 1; idx < visitor.Arity; idx++)
                                    {
                                        writer.Write(", arg");
                                        writer.Write(idx);
                                    }
                                    writer.WriteLine(");");
                                }
                            }
                            writer.Indent--;
                            writer.WriteLine('}');
                        }
                    });
                    writer.Indent--;
                    writer.WriteLine('}');

                    ctx.AddSource($"{tree.Root.Name}.{visitor.RootClass.Last().Name}`{visitor.Arity}", builder.ToString());
                }
            });
    }
}

internal sealed class VisitorWriter(StringBuilder sb) : IndentedTextWriter(new StringWriter(sb, CultureInfo.InvariantCulture))
{
    public void WriteFileHeader()
    {
        WriteLine("// <auto-generated/>");
        WriteLine();
        WriteLine("#nullable enable");
        WriteLine();
    }

    public void WithParents(ParentClass root, Action<ParentClass> action)
    {
        var count = Indent;
        ParentClass? c;
        for (c = root; c?.Child is not null; c = c.Child)
        {
            Write("partial ");
            Write(c.Keyword);
            Write(' ');
            Write(c.Name);
            Write(' ');
            WriteLine(c.Constraints);
            WriteLine("{");

            Indent++;
        }

        action(c!);

        while (Indent > count)
        {
            Indent--;
            WriteLine("}");
        }
    }

    public void WriteTypeParameterList(int count)
    {
        if (count > 0)
        {
            Write('<');
            Write("TReturn");

            for (var idx = 1; idx < count; idx++)
            {
                Write(", TArg");
                Write(idx);
            }
            Write('>');
        }
    }

    public string WriteSignature(Node node, Visitor visitor) =>
        WriteSignature(node.Namespace, node.TypeSymbol.Name, node.Name ?? node.TypeSymbol.Name, visitor.Arity);

    public string WriteSignature(string typeNamespace, string typeName, string nodeName, int arity)
    {
        Write(arity > 0 ? "TReturn " : "void ");
        Write("Visit");
        Write(nodeName);
        Write('(');

        Write(typeNamespace);
        Write('.');
        Write(typeName);
        Write(' ');
        var nodeArgName = char.ToLower(nodeName[0]) + nodeName.Substring(1);
        Write(nodeArgName);
        for (var idx = 1; idx < arity; idx++)
        {
            Write(", ");
            Write("TArg");
            Write(idx);
            Write(" arg");
            Write(idx);
        }
        WriteLine(')');
        return nodeArgName;
    }
}