namespace Tsu.TreeSourceGen.Sample;

#pragma warning disable CS1591

[TreeNode(typeof(Root))]
public abstract class Root
{
}

[TreeNode(typeof(Root))]
public abstract class Expression
{
}

[TreeNode(typeof(Root))]
public sealed class Binary(char op, Expression left, Expression right)
{
    public char Op { get; } = op;
    public Expression Left { get; } = left;
    public Expression Right { get; } = right;
}

[TreeNode(typeof(Root))]
public sealed class Constant(double number)
{
    public double Number { get; } = number;
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