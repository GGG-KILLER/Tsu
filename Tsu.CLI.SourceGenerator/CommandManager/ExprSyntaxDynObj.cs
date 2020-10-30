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
        public static dynamic Create ( Object @object ) =>
            new ExprSyntaxDynObj ( GetExpressionSyntax ( @object ) );

        private readonly ExpressionSyntax _expressionSyntax;

        /// <summary>
        /// Initializes a new instance of this type.
        /// </summary>
        /// <param name="expressionSyntax"></param>
        public ExprSyntaxDynObj ( ExpressionSyntax expressionSyntax )
        {
            this._expressionSyntax = expressionSyntax;
        }

        /// <inheritdoc />
        public override Boolean TryBinaryOperation ( BinaryOperationBinder binder, Object arg, out Object? result )
        {
            switch ( binder.Operation )
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
                    result = new ExprSyntaxDynObj (
                        BinaryExpression (
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
                                _ => throw new NotSupportedException ( ),
                            },
                            GroupIfRequired ( this._expressionSyntax ),
                            GetExpressionSyntax ( arg )
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
                    result = new ExprSyntaxDynObj (
                        AssignmentExpression (
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
                                _ => throw new NotSupportedException ( ),
                            },
                            this._expressionSyntax,
                            GetExpressionSyntax ( arg )
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
        public override Boolean TryConvert ( ConvertBinder binder, out Object? result )
        {
            if ( binder.Type.IsAssignableFrom ( this._expressionSyntax.GetType ( ) ) )
            {
                result = Cast ( this._expressionSyntax, binder.Type );
                return true;
            }

            result = null;
            return false;
        }

        /// <inheritdoc />
        public override Boolean TryGetIndex ( GetIndexBinder binder, Object[] indexes, out Object result )
        {
            result = ElementAccessExpression (
                GroupIfRequired ( this._expressionSyntax ),
                BracketedArgumentList (
                    SeparatedList (
                        Array.ConvertAll ( indexes, index => Argument ( GetExpressionSyntax ( index ) ) )
                    )
                )
            );
            return true;
        }

        /// <inheritdoc />
        public override Boolean TryGetMember ( GetMemberBinder binder, out Object result )
        {
            result = MemberAccessExpression (
                SyntaxKind.SimpleMemberAccessExpression,
                GroupIfRequired ( this._expressionSyntax ),
                IdentifierName ( binder.Name )
            );
            return true;
        }

        /// <inheritdoc />
        public override Boolean TryInvoke ( InvokeBinder binder, Object[] args, out Object result )
        {
            result = InvocationExpression (
                GroupIfRequired ( this._expressionSyntax ),
                ArgumentList (
                    SeparatedList (
                        Array.ConvertAll ( args, arg => Argument ( GetExpressionSyntax ( arg ) ) )
                    )
                )
            );
            return true;
        }

        /// <inheritdoc />
        public override Boolean TryInvokeMember ( InvokeMemberBinder binder, Object[] args, out Object result )
        {
            result = new ExprSyntaxDynObj (
                InvocationExpression (
                    MemberAccessExpression (
                        SyntaxKind.SimpleMemberAccessExpression,
                        GroupIfRequired ( this._expressionSyntax ),
                        IdentifierName ( binder.Name )
                    ),
                    ArgumentList (
                        SeparatedList (
                            Array.ConvertAll ( args, arg => Argument ( GetExpressionSyntax ( arg ) ) )
                        )
                    )
                )
            );

            return true;
        }

        /// <inheritdoc />
        public override Boolean TryUnaryOperation ( UnaryOperationBinder binder, out Object? result )
        {
            switch ( binder.Operation )
            {
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.Negate:
                case ExpressionType.Not:
                case ExpressionType.OnesComplement:
                case ExpressionType.UnaryPlus:
                {
                    result = PrefixUnaryExpression (
                        binder.Operation switch
                        {
                            ExpressionType.Decrement => SyntaxKind.PreDecrementExpression,
                            ExpressionType.Increment => SyntaxKind.PreIncrementExpression,
                            ExpressionType.Negate => SyntaxKind.UnaryMinusExpression,
                            ExpressionType.Not => SyntaxKind.LogicalNotExpression,
                            ExpressionType.OnesComplement => SyntaxKind.BitwiseNotExpression,
                            ExpressionType.UnaryPlus => SyntaxKind.UnaryPlusExpression,
                            _ => throw new NotSupportedException ( )
                        },
                        GroupIfRequired ( this._expressionSyntax )
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
        private static T Cast<T> ( Object @object ) =>
            ( T ) @object;

        /// <summary>
        /// The MethodInfo reference to <see cref="Cast{T}(Object)" />.
        /// </summary>
        private static readonly MethodInfo CastT_Info = typeof ( ExprSyntaxDynObj ).GetMethod (
            nameof ( Cast ),
            BindingFlags.Static | BindingFlags.NonPublic,
            Type.DefaultBinder,
            new[] { typeof ( Object ) },
            Array.Empty<ParameterModifier> ( ) );

        /// <summary>
        /// Casts an <paramref name="object" /> to the provided <paramref name="type" />.
        /// </summary>
        /// <param name="object"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Object Cast ( Object @object, Type type ) =>
            CastT_Info.MakeGenericMethod ( type ).Invoke ( null, new[] { @object } );

        /// <summary>
        /// Converts an object a basic type to a <see cref="LiteralExpressionSyntax" /> and returns
        /// objects of the type <see cref="ExpressionSyntax" /> as they are.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <returns></returns>
        private static ExpressionSyntax GetExpressionSyntax ( Object obj ) =>
            obj switch
            {
                Char value => LiteralExpression ( SyntaxKind.CharacterLiteralExpression, Literal ( value ) ),
                String value => LiteralExpression ( SyntaxKind.StringLiteralExpression, Literal ( value ) ),

                Single value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),
                Double value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),

                Decimal value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),

                SByte value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),
                Byte value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),

                Int16 value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),
                UInt16 value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),

                Int32 value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),
                UInt32 value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),

                Int64 value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),
                UInt64 value => LiteralExpression ( SyntaxKind.NumericLiteralExpression, Literal ( value ) ),

                ExpressionSyntax expression => expression,
                ExprSyntaxDynObj syntaxObject => ( ExpressionSyntax ) ( dynamic ) syntaxObject,

                _ => throw new InvalidOperationException ( "Cannot convert this object to an expression." ),
            };

        /// <summary>
        /// Groups the provided expression if required.
        /// </summary>
        /// <param name="expressionSyntax"></param>
        /// <returns></returns>
        private static ExpressionSyntax GroupIfRequired ( ExpressionSyntax expressionSyntax )
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
                or RefExpressionSyntax => ParenthesizedExpression ( expressionSyntax ),
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
        public static dynamic AsDynamic ( this ExpressionSyntax expressionSyntax ) =>
            new ExprSyntaxDynObj ( expressionSyntax );
    }
}