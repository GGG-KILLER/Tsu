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

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Tsu.CLI.Commands;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Tsu.CLI.SourceGenerator.CommandManager
{
    public class CommandClass : ICommandClassOrMethod
    {
        /// <summary>
        /// Attempts to create a new <see cref="CommandClass" />.
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="classSymbol"></param>
        /// <param name="attributeSyntax"></param>
        /// <param name="verb"></param>
        /// <returns></returns>
        public static Result<CommandClass, Diagnostic> Create(
            CommandManagerClass commandManager,
            INamedTypeSymbol classSymbol,
            AttributeSyntax attributeSyntax,
            string? verb = null)
        {
            if (verb is not null && string.IsNullOrWhiteSpace(verb))
            {
                return Result.Err<CommandClass, Diagnostic>(Diagnostic.Create(
                    DiagnosticDescriptors.InvalidVerbPassedToCommandManagerAttribute,
                    attributeSyntax.GetLocation()));
            }

            return Result.Ok<CommandClass, Diagnostic>(new CommandClass(
                commandManager,
                classSymbol,
                attributeSyntax,
                verb));
        }

        /// <summary>
        /// The command manager class.
        /// </summary>
        public CommandManagerClass CommandManager { get; }

        /// <summary>
        /// The command class symbol.
        /// </summary>
        public INamedTypeSymbol ClassSymbol { get; }

        /// <summary>
        /// The typeof syntax for this command class' class.
        /// </summary>
        public AttributeSyntax AttributeSyntax { get; }

        /// <summary>
        /// The command class verb.
        /// </summary>
        public string? Verb { get; }

        /// <inheritdoc cref="CommandManagerClass.CommonSymbols" />
        public CommonSymbols CommonSymbols => CommandManager.CommonSymbols;

        /// <summary>
        /// Initializes a new command class
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="classSymbol"></param>
        /// <param name="attributeSyntax"></param>
        /// <param name="verb"></param>
        private CommandClass(
            CommandManagerClass commandManager,
            INamedTypeSymbol classSymbol,
            AttributeSyntax attributeSyntax,
            string? verb = null)
        {
            CommandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            ClassSymbol = classSymbol ?? throw new ArgumentNullException(nameof(classSymbol));
            AttributeSyntax = attributeSyntax ?? throw new ArgumentNullException(nameof(attributeSyntax));
            Verb = verb;
        }

        /// <summary>
        /// Gets all command methods from this command class.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Result<ImmutableDictionary<string, CommandMethod>, Diagnostic> GetCommandMethods(CancellationToken cancellationToken = default)
        {
            var dictionaryBuilder =
                ImmutableDictionary.CreateBuilder<string, CommandMethod>();

            foreach (var methodSymbol in ClassSymbol.GetMembers()
                                                    .OfType<IMethodSymbol>()
                                                    .Where(method => method.GetAttributes()
                                                                           .Any(attr => CommandManager.IsCommandAttributeSymbol(attr.AttributeClass!))))
            {
                cancellationToken.ThrowIfCancellationRequested();
                foreach (var attrData in methodSymbol.GetAttributes()
                                                     .Where(attr => CommandManager.IsCommandAttributeSymbol(attr.AttributeClass!)))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var attr = Utilities.AttributeFromAttributeData<CommandAttribute>(attrData);

                    if (Utilities.IsValidCommandName(attr.Name))
                    {
                        return Result.Err<ImmutableDictionary<string, CommandMethod>, Diagnostic>(Diagnostic.Create(
                            DiagnosticDescriptors.InvalidNamePassedToCommandAttribute,
                            AttributeSyntax.GetLocation(),
                            methodSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
                    }

                    if (!dictionaryBuilder.ContainsKey(attr.Name)
                        || attr.Overwrite)
                    {
                        var commandMethod = CommandMethod.Create(attr, this, methodSymbol);
                        if (commandMethod.IsErr)
                            return Result.Err<ImmutableDictionary<string, CommandMethod>, Diagnostic>(commandMethod.Err.Value);
                        else
                            dictionaryBuilder[attr.Name] = commandMethod.Ok.Value;
                    }
                }
            }

            return Result.Ok<ImmutableDictionary<string, CommandMethod>, Diagnostic>(dictionaryBuilder.ToImmutable());
        }

        /// <inheritdoc />
        public Result<BlockSyntax, Diagnostic> ToSyntaxNode(IdentifierNameSyntax inputIdentifier, IdentifierNameSyntax spaceIndexIdentifier)
        {
            return GetCommandMethods().AndThen(commands =>
          {
              IdentifierNameSyntax secondSpaceIndexIdentifier = IdentifierName(spaceIndexIdentifier.Identifier.ValueText + "2"),
                  subCommandNameIdentifier = IdentifierName("subCommandName");

              var commandsSwitchSections = new List<SwitchSectionSyntax>(commands.Count);
              foreach (var command in commands)
              {
                  var block = command.Value.ToSyntaxNode(inputIdentifier, secondSpaceIndexIdentifier);
                  if (block.IsErr)
                      return block;

                  commandsSwitchSections.Add(SwitchSection(
                      List(new SwitchLabelSyntax[]
                      {
                            CaseSwitchLabel (
                                LiteralExpression (
                                    SyntaxKind.StringLiteralExpression,
                                    Literal ( command.Key )
                                )
                            )
                      }),
                      List(new StatementSyntax[]
                      {
                            block.Ok.Value,
                            BreakStatement ( )
                      })
                  ));
              }

              return (Result<BlockSyntax, Diagnostic>) Block(new StatementSyntax[]
              {
                    /* Int32 secondSpaceIndex = input.IndexOf ( ' ', spaceIndex ) */
                    LocalDeclarationStatement(
                        /* Int32 secondSpaceIndex = input.IndexOf ( ' ', spaceIndex + 1 ) */
                        VariableDeclaration(
                            /* Int32 */
                            Utilities.GetTypeSyntax(CommandManager.CommonSymbols.System_Int32),
                            /* secondSpaceIndex = input.IndexOf ( ' ', spaceIndex + 1 ) */
                            SeparatedList(new[]
                            {
                                /* secondSpaceIndex = input.IndexOf ( ' ', spaceIndex + 1 ) */
                                VariableDeclarator(
                                    secondSpaceIndexIdentifier.Identifier,
                                    null,
                                    /* = input.IndexOf ( ' ', spaceIndex + 1 ) */
                                    EqualsValueClause(
                                        (ExpressionSyntax)
                                        inputIdentifier.AsDynamic()
                                                       .IndexOf(' ', spaceIndexIdentifier.AsDynamic() + 1)
                                    )
                                    /* /= input.IndexOf ( ' ', spaceIndex ) */
                                )
                                /* /secondSpaceIndex = input.IndexOf ( ' ', spaceIndex ) */
                            } )
                        )
                        /* /Int32 secondSpaceIndex = input.IndexOf ( ' ', spaceIndex ) */
                    ),
                    /* /Int32 secondSpaceIndex = input.IndexOf ( ' ', spaceIndex ) */
                    /* String subCommandName = secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input; */
                    LocalDeclarationStatement(
                        VariableDeclaration(
                            /* String */
                            Utilities.GetTypeSyntax(CommandManager.CommonSymbols.System_String),
                            /* subCommandName = secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input; */
                            SeparatedList(new[]
                            {
                                /* subCommandName = secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input */
                                VariableDeclarator(
                                    subCommandNameIdentifier.Identifier,
                                    null,
                                    EqualsValueClause(
                                        /* secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input */
                                        ConditionalExpression(
                                            (ExpressionSyntax)
                                            (secondSpaceIndexIdentifier.AsDynamic() != -1),
                                            (ExpressionSyntax)
                                            inputIdentifier.AsDynamic()
                                                           .Substring(spaceIndexIdentifier.AsDynamic() + 1,
                                                                      secondSpaceIndexIdentifier),
                                            inputIdentifier
                                        )
                                        /* /secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input */
                                    )
                                )
                                /* /subCommandName = secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input */
                            })
                        )
                    ),
                    /* /String subCommandName = secondSpaceIndex != -1 ? input.Substring ( spaceIndex + 1, secondSpaceIndex ) : input; */
                    SwitchStatement(
                        subCommandNameIdentifier,
                        List(commandsSwitchSections)
                    ),
              });
          });
        }
    }
}