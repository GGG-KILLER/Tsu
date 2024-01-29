// GreenBase = global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
// RedBase = global::Tsu.Trees.RedGreen.Sample.SampleNode
// KindEnum = global::Tsu.Trees.RedGreen.Sample.SampleKind
// CreateVisitors = True
// CreateWalker = True
// CreateRewriter = True
// Root = global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
// global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
//     Kinds:
//     Children:
//     ExtraData:
//         global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = False)
//     global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample
//         Kinds:
//         Children:
//         ExtraData:
//             global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//         global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample
//             Kinds:
//                 Tsu.Trees.RedGreen.Sample.SampleKind.FunctionCallExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 7)
//             Children:
//                 global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample (Name = _identifier, IsOptional = False, PassToBase = False)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = _firstArg, IsOptional = False, PassToBase = False)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample? (Name = _secondArg, IsOptional = True, PassToBase = False)
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//         global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample
//             Kinds:
//                 Tsu.Trees.RedGreen.Sample.SampleKind.AdditionExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 3)
//                 Tsu.Trees.RedGreen.Sample.SampleKind.DivisionExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 6)
//                 Tsu.Trees.RedGreen.Sample.SampleKind.MultiplicationExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 5)
//                 Tsu.Trees.RedGreen.Sample.SampleKind.SubtractionExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 4)
//             Children:
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = _left, IsOptional = False, PassToBase = False)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = _right, IsOptional = False, PassToBase = False)
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//         global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample
//             Kinds:
//                 Tsu.Trees.RedGreen.Sample.SampleKind.NumericalLiteralExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 2)
//             Children:
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//                 double (Name = _value, IsOptional = False, PassToBase = False)
//         global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample
//             Kinds:
//                 Tsu.Trees.RedGreen.Sample.SampleKind.IdentifierExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 1)
//             Children:
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//                 string (Name = _name, IsOptional = False, PassToBase = False)
//     global::Tsu.Trees.RedGreen.Sample.Internal.StatementSample
//         Kinds:
//         Children:
//             global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample (Name = _semicolon, IsOptional = False, PassToBase = False)
//         ExtraData:
//             global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//         global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionStatementSample
//             Kinds:
//                 Tsu.Trees.RedGreen.Sample.SampleKind.ExpressionStatement (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 9)
//             Children:
//                 global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample (Name = _semicolon, IsOptional = False, PassToBase = True)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = _expression, IsOptional = False, PassToBase = False)
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//         global::Tsu.Trees.RedGreen.Sample.Internal.AssignmentStatement
//             Kinds:
//                 Tsu.Trees.RedGreen.Sample.SampleKind.AssignmentStatement (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 8)
//             Children:
//                 global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample (Name = _semicolon, IsOptional = False, PassToBase = True)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample (Name = _identifier, IsOptional = False, PassToBase = False)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = _value, IsOptional = False, PassToBase = False)
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//     global::Tsu.Trees.RedGreen.Sample.Internal.SemicolonTokenSample
//         Kinds:
//             Tsu.Trees.RedGreen.Sample.SampleKind.SemicolonToken (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 0)
//         Children:
//         ExtraData:
//             global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
