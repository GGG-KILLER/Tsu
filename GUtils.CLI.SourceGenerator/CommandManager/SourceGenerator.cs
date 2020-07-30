using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GUtils.CLI.SourceGenerator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Initialize ( InitializationContext context ) =>
            context.RegisterForSyntaxNotifications ( ( ) => new SyntaxReceiver ( ) );

        public void Execute ( SourceGeneratorContext context )
        {
            var commandManagetAttributeCode = SourceText.From ( CodeConstants.CommandManagerAttribute.Code );
            context.AddSource ( CodeConstants.CommandManagerAttribute.CodeFileName, commandManagetAttributeCode );


        }
    }
}