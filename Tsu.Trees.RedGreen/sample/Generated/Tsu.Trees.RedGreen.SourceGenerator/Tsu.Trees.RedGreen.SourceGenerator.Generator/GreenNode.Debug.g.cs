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
//                 Tsu.Trees.RedGreen.Sample.SampleKind.FunctionCallExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 6)
//             Children:
//                 global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample (Name = identifier, IsOptional = False, PassToBase = False)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = firstArg, IsOptional = False, PassToBase = False)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample? (Name = secondArg, IsOptional = True, PassToBase = False)
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//         global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample
//             Kinds:
//                 Tsu.Trees.RedGreen.Sample.SampleKind.AdditionExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 2)
//                 Tsu.Trees.RedGreen.Sample.SampleKind.DivisionExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 5)
//                 Tsu.Trees.RedGreen.Sample.SampleKind.MultiplicationExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 4)
//                 Tsu.Trees.RedGreen.Sample.SampleKind.SubtractionExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 3)
//             Children:
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = _left, IsOptional = False, PassToBase = False)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = _right, IsOptional = False, PassToBase = False)
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//         global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample
//             Kinds:
//                 Tsu.Trees.RedGreen.Sample.SampleKind.NumericalLiteralExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 1)
//             Children:
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//                 double (Name = _value, IsOptional = False, PassToBase = False)
//         global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample
//             Kinds:
//                 Tsu.Trees.RedGreen.Sample.SampleKind.IdentifierExpression (IsNull = False, Type = global::Tsu.Trees.RedGreen.Sample.SampleKind, Value = 0)
//             Children:
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//                 string (Name = _name, IsOptional = False, PassToBase = False)
