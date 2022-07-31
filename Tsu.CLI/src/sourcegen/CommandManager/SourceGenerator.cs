// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
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

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tsu.CLI.SourceGenerator.CommandManager
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) =>
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());

        public void Execute(GeneratorExecutionContext context)
        {
            var commonSymbols = new CommonSymbols(context.Compilation);
            var commandManagetAttributeCode = SourceText.From(CodeConstants.CommandManagerAttribute.Code, Encoding.UTF8);
            context.AddSource(CodeConstants.CommandManagerAttribute.CodeFileName, commandManagetAttributeCode);

            if (context.SyntaxReceiver is SyntaxReceiver receiver)
            {
                foreach (var commandManagerDeclaration in receiver.CommandManagerClasses)
                {
                    var commandManagerClass = CommandManagerClass.Initialize(
                        context.Compilation,
                        context.Compilation.GetSemanticModel(commandManagerDeclaration.SyntaxTree),
                        commandManagerDeclaration,
                        commonSymbols,
                        context.CancellationToken);

                    if (commandManagerClass.IsErr)
                    {
                        context.ReportDiagnostic(commandManagerClass.Err.Value);
                    }
                    else
                    {
                        var code = commandManagerClass.Ok.Value.GenerateCommandManager();

                        if (code.IsErr)
                        {
                            context.ReportDiagnostic(code.Err.Value);
                        }
                        else
                        {
                            var managerName = commandManagerDeclaration.Identifier.ValueText;
                            context.AddSource($"{managerName}.GeneratedCommandManager.cs", SourceText.From(code.Ok.Value, Encoding.UTF8));
                        }
                    }
                }
            }
        }
    }
}