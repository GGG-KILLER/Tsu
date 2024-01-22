using System.Diagnostics.CodeAnalysis;

namespace Tsu.TreeSourceGen.Sample;

public static class Program
{
    public static void Main()
    {
        Root tree = new FunctionCall("floor", [new Binary('+', new Constant(1), new Constant(1))]);
        var visitor = new MyVisitor();

        Console.WriteLine(visitor.Visit(tree)); // floor(1 + 1)
    }

    private class MyVisitor : Visitor<string>
    {
        [return: MaybeNull]
        public override string VisitBinary(Binary binary) => base.Visit(binary.Left) + $" {binary.Op} " + base.Visit(binary.Right);

        [return: MaybeNull]
        public override string VisitConstant(Constant constant) => constant.Number.ToString();

        [return: MaybeNull]
        public override string VisitFunctionCall(FunctionCall functionCall) => functionCall.Name + '(' + string.Join(", ", functionCall.Arguments.Select(Visit!)) + ')';
    }
}

[TreeNode(typeof(Root))]
public abstract partial class Root
{
}

[TreeNode(typeof(Root))]
public abstract partial class Expression : Root
{
}

[TreeNode(typeof(Root))]
public sealed partial class Binary(char op, Expression left, Expression right) : Expression
{
    public char Op { get; } = op;
    public Expression Left { get; } = left;
    public Expression Right { get; } = right;
}

[TreeNode(typeof(Root))]
public sealed partial class Constant(double number) : Expression
{
    public double Number { get; } = number;
}

[TreeNode(typeof(Root))]
public abstract partial class Statement : Root
{
}

[TreeNode(typeof(Root))]
public sealed partial class FunctionCall(string name, IEnumerable<Expression> arguments) : Statement
{
    public string Name { get; } = name;
    public IEnumerable<Expression> Arguments { get; } = arguments;
}

[TreeVisitor(typeof(Root))]
public abstract partial class Visitor
{
}

[TreeVisitor(typeof(Root))]
public abstract partial class Visitor<TReturn>
{
}

[TreeVisitor(typeof(Root))]
public abstract partial class Visitor<TReturn, TArg1>
{
}

[TreeVisitor(typeof(Root))]
public abstract partial class Visitor<TReturn, TArg1, TArg2>
{
}