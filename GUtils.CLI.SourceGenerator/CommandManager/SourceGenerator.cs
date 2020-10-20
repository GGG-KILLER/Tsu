using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GUtils.CLI.SourceGenerator.CommandManager
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Initialize ( GeneratorInitializationContext context ) =>
            context.RegisterForSyntaxNotifications ( ( ) => new SyntaxReceiver ( ) );

        public void Execute ( GeneratorExecutionContext context )
        {
            var commonSymbols = new CommonSymbols ( context.Compilation );
            var commandManagetAttributeCode = SourceText.From ( CodeConstants.CommandManagerAttribute.Code, Encoding.UTF8 );
            context.AddSource ( CodeConstants.CommandManagerAttribute.CodeFileName, commandManagetAttributeCode );

            if ( context.SyntaxReceiver is SyntaxReceiver receiver )
            {
                foreach ( ClassDeclarationSyntax commandManagerDeclaration in receiver.CommandManagerClasses )
                {
                    Result<CommandManagerClass, Diagnostic> commandManagerClass = CommandManagerClass.Initialize (
                        context.Compilation,
                        context.Compilation.GetSemanticModel ( commandManagerDeclaration.SyntaxTree ),
                        commandManagerDeclaration,
                        commonSymbols,
                        context.CancellationToken );

                    if ( commandManagerClass.IsErr )
                    {
                        context.ReportDiagnostic ( commandManagerClass.Err.Value );
                    }
                    else
                    {
                        Result<String, Diagnostic> code = commandManagerClass.Ok.Value.GenerateCommandManager ( );

                        if ( code.IsErr )
                        {
                            context.ReportDiagnostic ( code.Err.Value );
                        }
                        else
                        {
                            var managerName = commandManagerDeclaration.Identifier.ValueText;
                            context.AddSource ( $"{managerName}.GeneratedCommandManager.cs", SourceText.From ( code.Ok.Value, Encoding.UTF8 ) );
                        }
                    }
                }
            }
        }
    }
}