# CommandAttribute Class
```cs
[AttributeUsage ( AttributeTargets.Method, AllowMultiple = true, Inherited = true )]
public sealed class CommandAttribute : Attribute
```

Indicates a method is a command with the specified name

## Examples

In the following example, we create two commands: one called `exit` which stops the user input loop and another called `echo` that accepts a string and prints it back into the Console.

```cs
public class Program
{
	private static Boolean ShouldExit;

	public static void Main ( )
	{
		// Create a new command manager and
		// tell it to search for commands on
		// this class
		var manager = new CommandManager ( );
		manager.LoadCommands<Program> ( null );

		// The user input loop: read user input
		// lines and pass them to the command
		// manager to be parsed and passed to the
		// right method converted to the rigth
		// types
		while ( !ShouldExit )
		{
			var line = Console.ReadLine ( );
			manager.Execute ( line );
		}
	}

	// Here we create a command called 'exit'
	// which accepts no arguments
	[Command ( "exit" )]
	public static void Exit ( ) =>
		ShouldExit = true;

	// Here we create a command called 'echo'
	// that accepts a single string as its
	// argument
	[Command ( "echo" )]
	public static void Echo ( String arg ) =>
		Console.WriteLine ( $"ECHO! {arg}" );
}
```

And in this example, we show all possible types of commands one can create:

```cs
public class Program
{
	private static Boolean ShouldExit;

	public static void Main ( )
	{
		// Create a new command manager and
		// tell it to search for commands on
		// this class
		var manager = new CommandManager ( );
		manager.LoadCommands<Program> ( null );

		// The user input loop: read user input
		// lines and pass them to the command
		// manager to be parsed and passed to the
		// right method converted to the rigth
		// types
		while ( !ShouldExit )
		{
			var line = Console.ReadLine ( );
			manager.Execute ( line );
		}
	}

	// Usual exit command
	[Command ( "exit" )]
	public static void Exit ( ) =>
		ShouldExit = true;

	// Echo command but with a variable
	// argument count
	[Command ( "echo" )]
	public static void Echo ( params String[] args ) =>
		Console.WriteLine ( String.Join ( " ", args ) );

	// A command can also have an optional
	// argument (arguments with default values
	// are considered optional)
	[Command ( "add" )]
	public static void Add ( Int32 a, Int32 b = 5 ) =>
		Console.WriteLine ( a + b );

	// The RawInput attribute, tells the command
	// manager that the input for this command
	// should not be parsed
	[Command ( "print" ), RawInput]
	public static void Print ( String input ) =>
		Console.WriteLine ( $"p: {input}" );
}
```

And finally, an example of command overriding:

```cs
public class A
{
	[Command ( "say-hello" )]
	public void SayHello ( String name ) =>
		Console.WriteLine ( $"Hi {name}" );
}

public class B : A
{
	[Command ( "say-hello", Override = true )]
	public void SayHelloCheerfully ( String name ) =>
		Console.WriteLine ( $"Hey {name}!" );
}
```

## Remarks
- Argument names cannot have any whitespace in them.
- This attribute cannot be applied to any methods that:
	- Have type parameters (`SomeMethod<T>`);
	- Have `in`, `out` or `ref` parameters;
	- Have arguments which are not convertible by `Convert.ToType` neither by `Enum.Parse`;<sup>1</sup>
	- Have more than one argument, or an argument that is not a String and the `RawInput` attribute;
	- Have a `params` argument that is not a `String[]`;
	- Have non-`params` array arguments;


<sup>1</sup>: convertible types will be: `boolean`, `char`, `sbyte`/`byte`, `short`/`ushort`, `int`/`uint`, `long`/`ulong`, `float`, `double`, `decimal`, `DateTime`, `string`, `object` and any `enum`.

## Constructors

- `CommandAttribute ( String name )`: Initializes a new instance of the command attribute.

## Properties

- `Boolean Override`: Indicates that the command should override any other commands with the same name.
