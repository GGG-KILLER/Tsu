#pragma warning disable CS1591
namespace Tsu.TreeSourceGen.Sample;

public static class Program
{
    public static void Main()
    {
        // IVisitor visitor = null;
        Console.WriteLine("Hello");
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