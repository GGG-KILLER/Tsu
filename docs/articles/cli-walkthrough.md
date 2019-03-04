---
uid: cli-walkthrough.md
title: Command Subsystem Walkthrough
---

## Command subsystem
### Overview
The command subsystem allows a user to create a shell-like environment where users can execute commands with the possibility to pass parameters to them.

The command subsystem is able to:
- Find commands automatically inside a given type
- Parse comamnds with single/double quotes and hexadecimal/binary/decimal/octal/C escapes
- Generate methods (at runtime) that will validate and convert the arguments provided by the user for a better performance
- Generate a help command from parameter types and description/example attributes

### Writing a simple CLI interface:
- Add the nuget feed to your project:

Add a `nuget.config` file to the root of your project (alongside the `.sln` file) with the following:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
	<packageSources>
		<add key="GUtils & GParse" value="https://www.myget.org/F/ggg-killer/api/v3/index.json" />
	</packageSources>
</configuration>
```

- Install `GUtils.CLI` in your project:

# [Package Manager](#tab/pm-install)
```
PM> Install-Package GUtils.CLI -Version 6.1.0
```

# [NuGet.exe](#tab/nuget-install)
```
nuget.exe install GUtils.CLI -Version 6.1.0
```

# [.NET CLI](#tab/dotnet-install)
```
dotnet add package GUtils.CLI --version 6.1.0
```

# [`.csproj`](#tab/csproj-install)
```xml
<PackageReference Include="GUtils.CLI" Version="6.1.0" />
```

***


- Then use the following code:

```csharp
using System;
using GUtils.CLI.Commands;

namespace CLIExample
{
	public class Program
	{
		private enum CustomEnum
		{
			There,
			Are,
			Many,
			Values
		}

		public static void Main()
		{
			// Initialize a new command manager
			var commandManager = new ConsoleCommandManager
			{
				// This is the prompt prefix (will be printed
				// before reading every command line)
				Prompt = "> "
			};
			// Load commands from this class, passing a null
			// instance because all command methods are static
			commandManager.LoadCommands<Program> ( null );
			// Add a command to stop the read and execute loop
			commandManager.AddStopCommand ( "q", "quit", "exit", "leave" );
			// Add the default help command from the
			// ConsoleCommandManager
			commandManager.AddHelpCommand ( );

			// Start the read and execute command loop
			commandManager.Start ( );
		}

		[Command ( "print-string" )]
		[HelpDescription ( "Prints a string" )]
		[HelpExample ( "print-string Hello" )]
		[HelpExample ( "print-string \"Hello\\x20there\"" )]
		public static void PrintString (
			[HelpDescription ( "The string to be printed" )]
			String val )
		{
			Console.WriteLine ( $"Provided string: {val}" );
		}

		[Command ( "print-number" )]
		[HelpDescription ( "Prints a number" )]
		[HelpExample ( "print-number 2" )]
		[HelpExample ( "print-number 250" )]
		public static void PrintNumber (
			[HelpDescription ( "The number to be printed" )]
			Int32 number )
		{
			Console.WriteLine ( $"Provided number: {number}" );
		}

		[Command ( "print-enum" )]
		[HelpDescription ( "Prints an enum value" )]
		[HelpExample ( "print-enum There" )]
		[HelpExample ( "print-enum MaNy" )]
		public static void PrintEnum (
			[HelpDescription ( "The enum value to be printed" )]
			CustomEnum @enum )
		{
			Console.WriteLine ( $"Provided enum value: {@enum}" );
		}

		[Command ( "print-nullable-int" )]
		[HelpDescription ( "Prints a nullable integer" )]
		[HelpExample ( "print-nullable-int" )]
		[HelpExample ( "print-nullable-int 2" )]
		[HelpExample ( "print-nullable-int 250" )]
		public static void PrintNullableInteger (
			[HelpDescription ( "The nullable integer to be printed" )]
			Int32? num = null )
		{
			Console.WriteLine ( $"Provided nullable integer: {num}" );
		}
	}
}
```

#### Execution results:

```invalid
> print-string Hello
Provided string: Hello
> print-string "Hello\x20there"
Provided string: Hello there
> print-number 20
Provided number: 20
> print-number 256
Provided number: 256
> print-enum There
Provided enum value: There
> print-enum mAnY
Provided enum value: Many
> print-enum aaaa
Error while executing command 'print-enum': Invalid argument #0.
> print-nullable-int
Provided nullable integer:
> print-nullable-int 250
Provided nullable integer: 250
> help
Showing help for all commands:
	print-string - Prints a string
		Usage:
			print-string val
		Arguments:
			val:String - The string to be printed
		Example:
			print-string Hello
			print-string "Hello\x20there"
	print-number - Prints a number
		Usage:
			print-number number
		Arguments:
			number:Int32 - The number to be printed
		Example:
			print-number 2
			print-number 250
	print-enum - Prints an enum value
		Usage:
			print-enum enum
		Arguments:
			enum:CustomEnum - The enum value to be printed
				Possible values: There, Are, Many, Values
		Example:
			print-enum There
			print-enum MaNy
	print-nullable-int - Prints a nullalbe integer
		Usage:
			print-nullable-int [num]
		Arguments:
			num:Nullable`1 - The nullable integer to be printed
		Example:
			print-nullable-int
			print-nullable-int 2
			print-nullable-int 250
	q - Exits this command loop.
		Usage:
			q
		Aliases: q, quit, exit, leave
	help - Shows help text
		Usage:
			help [commandName]
		Arguments:
			commandName:String - name of the command to get the help text
		Examples:
			help      (will list all commands)
			help help (will show the help text for this command)
```
