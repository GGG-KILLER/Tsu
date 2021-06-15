// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Tsu.CLI.SourceGenerator.CommandManager
{
    /// <summary>
    /// A dynamic <see cref="ExpressionSyntax" /> object.
    /// </summary>
    public class ExprSyntaxDynObj : DynamicObject
    {
        /// <summary>
        /// Initializes a new instance of this type.
        /// </summary>
        /// <param name="expressionSyntax"></param>
        /// <returns></returns>
        public static dynamic Create(object @object) =>
            new ExprSyntaxDynObj(GetExpressionSyntax(@object));

        private readonly ExpressionSyntax _expressionSyntax;

        /// <summary>
        /// Initializes a new instance of this type.
        /// </summary>
        /// <param name="expressionSyntax"></param>
        public ExprSyntaxDynObj(ExpressionSyntax expressionSyntax)
        {
            _expressionSyntax = expressionSyntax;
        }

        /// <inheritdoc />
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object? result)
        {
            switch (binder.Operation)
            {
                case ExpressionType.Add:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Divide:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                {
                    result = new ExprSyntaxDynObj(
                        BinaryExpression(
                            binder.Operation switch
                            {
                                ExpressionType.Add => SyntaxKind.AddExpression,
                                ExpressionType.And => SyntaxKind.BitwiseAndExpression,
                                ExpressionType.AndAlso => SyntaxKind.LogicalAndExpression,
                                ExpressionType.Divide => SyntaxKind.DivideExpression,
                                ExpressionType.ExclusiveOr => SyntaxKind.ExclusiveOrExpression,
                                ExpressionType.GreaterThan => SyntaxKind.GreaterThanExpression,
                                ExpressionType.GreaterThanOrEqual => SyntaxKind.GreaterThanOrEqualExpression,
                                ExpressionType.LeftShift => SyntaxKind.LeftShiftExpression,
                                ExpressionType.LessThan => SyntaxKind.LessThanExpression,
                                ExpressionType.LessThanOrEqual => SyntaxKind.LessThanOrEqualExpression,
                                ExpressionType.Modulo => SyntaxKind.ModuloExpression,
                                ExpressionType.Multiply => SyntaxKind.MultiplyExpression,
                                ExpressionType.NotEqual => SyntaxKind.NotEqualsExpression,
                                ExpressionType.Or => SyntaxKind.BitwiseOrExpression,
                                ExpressionType.OrElse => SyntaxKind.LogicalOrExpression,
                                ExpressionType.RightShift => SyntaxKind.RightShiftExpression,
                                ExpressionType.Subtract => SyntaxKind.SubtractExpression,
                                _ => throw new NotSupportedException(),
                            },
                            GroupIfRequired(_expressionSyntax),
                            GetExpressionSyntax(arg)
                        )
                    );
                }
                break;

                case ExpressionType.AddAssign:
                case ExpressionType.AndAssign:
                case ExpressionType.DivideAssign:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.OrAssign:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.SubtractAssign:
                {
                    result = new ExprSyntaxDynObj(
                        AssignmentExpression(
                            binder.Operation switch
                            {
                                ExpressionType.Add => SyntaxKind.AddAssignmentExpression,
                                ExpressionType.And => SyntaxKind.AndAssignmentExpression,
                                ExpressionType.Divide => SyntaxKind.DivideAssignmentExpression,
                                ExpressionType.ExclusiveOr => SyntaxKind.ExclusiveOrAssignmentExpression,
                                ExpressionType.LeftShift => SyntaxKind.LeftShiftAssignmentExpression,
                                ExpressionType.Modulo => SyntaxKind.ModuloAssignmentExpression,
                                ExpressionType.Multiply => SyntaxKind.MultiplyAssignmentExpression,
                                ExpressionType.Or => SyntaxKind.OrAssignmentExpression,
                                ExpressionType.RightShift => SyntaxKind.RightShiftAssignmentExpression,
                                ExpressionType.Subtract => SyntaxKind.SubtractAssignmentExpression,
                                _ => throw new NotSupportedException(),
                            },
                            _expressionSyntax,
                            GetExpressionSyntax(arg)
                        )
                    );
                }
                break;

                default:
                    result = null;
                    break;
            }

            return result != null;
        }

        /// <inheritdoc />
        public override bool TryConvert(ConvertBinder binder, out object? result)
        {
            if (binder.Type.IsAssignableFrom(_expressionSyntax.GetType()))
            {
                result = Cast(_expressionSyntax, binder.Type);
                return true;
            }

            result = null;
            return false;
        }

        /// <inheritdoc />
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = ElementAccessExpression(
                GroupIfRequired(_expressionSyntax),
                BracketedArgumentList(
                    SeparatedList(
                        Array.ConvertAll(indexes, index => Argument(GetExpressionSyntax(index)))
                    )
                )
            );
            return true;
        }

        /// <inheritdoc />
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                GroupIfRequired(_expressionSyntax),
                IdentifierName(binder.Name)
            );
            return true;
        }

        /// <inheritdoc />
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = InvocationExpression(
                GroupIfRequired(_expressionSyntax),
                ArgumentList(
                    SeparatedList(
                        Array.ConvertAll(args, arg => Argument(GetExpressionSyntax(arg)))
                    )
                )
            );
            return true;
        }

        /// <inheritdoc />
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = new ExprSyntaxDynObj(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        GroupIfRequired(_expressionSyntax),
                        IdentifierName(binder.Name)
                    ),
                    ArgumentList(
                        SeparatedList(
                            Array.ConvertAll(args, arg => Argument(GetExpressionSyntax(arg)))
                        )
                    )
                )
            );

            return true;
        }

        /// <inheritdoc />
        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object? result)
        {
            switch (binder.Operation)
            {
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.Negate:
                case ExpressionType.Not:
                case ExpressionType.OnesComplement:
                case ExpressionType.UnaryPlus:
                {
                    result = PrefixUnaryExpression(
                        binder.Operation switch
                        {
                            ExpressionType.Decrement => SyntaxKind.PreDecrementExpression,
                            ExpressionType.Increment => SyntaxKind.PreIncrementExpression,
                            ExpressionType.Negate => SyntaxKind.UnaryMinusExpression,
                            ExpressionType.Not => SyntaxKind.LogicalNotExpression,
                            ExpressionType.OnesComplement => SyntaxKind.BitwiseNotExpression,
                            ExpressionType.UnaryPlus => SyntaxKind.UnaryPlusExpression,
                            _ => throw new NotSupportedException()
                        },
                        GroupIfRequired(_expressionSyntax)
                    );
                }
                break;

                default:
                    result = null;
                    break;
            }

            return result != null;
        }

        /// <summary>
        /// Casts an <paramref name="object" /> to the provided type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        private static T Cast<T>(object @object) =>
            (T) @object;

        /// <summary>
        /// The MethodInfo reference to <see cref="Cast{T}(object)" />.
        /// </summary>
        private static readonly MethodInfo CastT_Info = typeof(ExprSyntaxDynObj).GetMethod(
            nameof(Cast),
            BindingFlags.Static | BindingFlags.NonPublic,
            Type.DefaultBinder,
            new[] { typeof(object) },
            Array.Empty<ParameterModifier>());

        /// <summary>
        /// Casts an <paramref name="object" /> to the provided <paramref name="type" />.
        /// </summary>
        /// <param name="object"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object Cast(object @object, Type type) =>
            CastT_Info.MakeGenericMethod(type).Invoke(null, new[] { @object });

        /// <summary>
        /// Converts an object a basic type to a <see cref="LiteralExpressionSyntax" /> and returns
        /// objects of the type <see cref="ExpressionSyntax" /> as they are.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <returns></returns>
        private static ExpressionSyntax GetExpressionSyntax(object obj) =>
            obj switch
            {
                char value => LiteralExpression(SyntaxKind.CharacterLiteralExpression, Literal(value)),
                string value => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value)),

                float value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),
                double value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),

                decimal value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),

                sbyte value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),
                byte value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),

                short value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),
                ushort value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),

                int value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),
                uint value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),

                long value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),
                ulong value => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)),

                ExpressionSyntax expression => expression,
                ExprSyntaxDynObj syntaxObject => (ExpressionSyntax) (dynamic) syntaxObject,

                _ => throw new InvalidOperationException("Cannot convert this object to an expression."),
            };

        /// <summary>
        /// Groups the provided expression if required.
        /// </summary>
        /// <param name="expressionSyntax"></param>
        /// <returns></returns>
        private static ExpressionSyntax GroupIfRequired(ExpressionSyntax expressionSyntax)
        {
            return expressionSyntax switch
            {
                AnonymousFunctionExpressionSyntax
                or AssignmentExpressionSyntax
                or AwaitExpressionSyntax
                or BinaryExpressionSyntax
                or CastExpressionSyntax
                or ConditionalExpressionSyntax
                or IsPatternExpressionSyntax
                or MakeRefExpressionSyntax
                or PrefixUnaryExpressionSyntax
                or QueryExpressionSyntax
                or RefExpressionSyntax => ParenthesizedExpression(expressionSyntax),
                _ => expressionSyntax,
            };
        }
    }

    /// <summary>
    /// <see cref="ExprSyntaxDynObj" /> related extension methods.
    /// </summary>
    public static class ExprSyntaxDynObjExtensions
    {
        /// <summary>
        /// Converts the <paramref name="expressionSyntax" /> to an <see cref="ExprSyntaxDynObj" />.
        /// </summary>
        /// <param name="expressionSyntax"></param>
        /// <returns></returns>
        public static dynamic AsDynamic(this ExpressionSyntax expressionSyntax) =>
            new ExprSyntaxDynObj(expressionSyntax);
    }
}