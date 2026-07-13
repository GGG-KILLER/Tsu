# Tsu

[![NuGet](https://img.shields.io/nuget/v/Tsu.svg)](https://www.nuget.org/packages/Tsu/)
[![NuGet (with prereleases)](https://img.shields.io/nuget/vpre/Tsu.svg)](https://www.nuget.org/packages/Tsu/)
[![Test and Publish](https://github.com/GGG-KILLER/Tsu/actions/workflows/test-and-publish.yml/badge.svg)](https://github.com/GGG-KILLER/Tsu/actions/workflows/test-and-publish.yml)

A small collection of general-purpose utilities for .NET: an `Option<T>` and
`Result<TOk, TErr>` pair for Rust-style error/optionality handling, plus helpers for
formatting and parsing file sizes, SI-prefixed numbers and durations, a bit-vector
toolkit, and a no-op `Stream`.

Targets `netstandard2.0`, `netstandard2.1` and `net8.0`.

## Install

```sh
dotnet add package Tsu
```

## Features

### `Option<T>`

A struct-based optional value, avoiding `null` for "no value" cases.

```csharp
using Tsu;

Option<int> some = Option.Some(42);
Option<int> none = Option.None<int>();

if (some.TryGetValue(out int value))
    Console.WriteLine(value); // 42

int fallback = none.UnwrapOr(-1); // -1
```

### `Result<TOk, TErr>`

A struct-based result type for representing success or failure without exceptions.

```csharp
using Tsu;

Result<int, string> Divide(int a, int b) =>
    b == 0 ? Result.Err<int, string>("division by zero") : Result.Ok<int, string>(a / b);

Result<int, string> result = Divide(10, 2);
Console.WriteLine(result.UnwrapOr(-1)); // 5
```

### `Unit`

A type with exactly one possible value, useful as a placeholder generic argument
(e.g. `Result<Unit, string>` for operations that either succeed with no payload or
fail with an error).

### `Tsu.Numerics.FileSize`

Formatting and parsing of byte counts, with `KiB`/`MiB`/`GiB`/`TiB`/`PiB`/`EiB`
constants.

```csharp
using Tsu.Numerics;

FileSize.Format(1536);                              // "1.5 KiB"
FileSize.TryParseInteger("2KiB", out long bytes);   // bytes == 2048
```

### `Tsu.Numerics.SI`

Formatting and parsing of numbers with SI prefixes (from `Yocto` to `Yotta`).

```csharp
using Tsu.Numerics;

SI.Format(1500);      // "1.5 k"
SI.Parse("1.5k");     // 1500
```

### `Tsu.Numerics.Duration`

Formatting of tick counts and `TimeSpan`s into human-readable scaled durations
(hours down to nanoseconds).

```csharp
using Tsu.Numerics;

Duration.Format(TimeSpan.FromMinutes(90)); // "01.50h"
```

### `Tsu.Buffers.BitVectorHelpers` / `VariableLengthBitVector`

Low-level helpers for reading and writing individual bits within `byte`, `ushort`,
`uint` and `ulong` vectors (array/list and `Span<T>` overloads), plus
`VariableLengthBitVector`, a resizable bit vector built on top of them.

### `Tsu.IO.NullStream`

A `Stream` that discards everything written to it and throws on any read, akin to
piping to `/dev/null`.

## Building from source

```sh
dotnet restore --locked-mode
dotnet build --configuration Release
dotnet test --configuration Release
```

## License

MIT License. See [LICENSE](LICENSE) for details.
