// GreenBase = global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
// RedBase = global::Tsu.Trees.RedGreen.Sample.SampleNode
// KindEnum = global::Tsu.Trees.RedGreen.Sample.SampleKind
// CreateVisitors = True
// CreateWalker = True
// CreateRewriter = True
// Root = global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
// global::Tsu.Trees.RedGreen.Sample.Internal.GreenNode
//     Children:
//     ExtraData:
//         global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = False)
//     global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample
//         Children:
//         ExtraData:
//             global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//         global::Tsu.Trees.RedGreen.Sample.Internal.FunctionCallExpressionSample
//             Children:
//                 global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample (Name = identifier, IsOptional = False, PassToBase = False)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = firstArg, IsOptional = False, PassToBase = False)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = secondArg, IsOptional = True, PassToBase = False)
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//         global::Tsu.Trees.RedGreen.Sample.Internal.BinaryOperationExpressionSample
//             Children:
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = _left, IsOptional = False, PassToBase = False)
//                 global::Tsu.Trees.RedGreen.Sample.Internal.ExpressionSample (Name = _right, IsOptional = False, PassToBase = False)
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//         global::Tsu.Trees.RedGreen.Sample.Internal.NumericalLiteralExpressionSample
//             Children:
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//                 double (Name = _value, IsOptional = False, PassToBase = False)
//         global::Tsu.Trees.RedGreen.Sample.Internal.IdentifierExpressionSample
//             Children:
//             ExtraData:
//                 global::Tsu.Trees.RedGreen.Sample.SampleKind (Name = _kind, IsOptional = False, PassToBase = True)
//                 string (Name = _name, IsOptional = False, PassToBase = False)
