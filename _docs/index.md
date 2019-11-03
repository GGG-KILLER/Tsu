# GUtils
## What is it?
A collection of common packages I reuse through my projects.

They used to be snippets inside a folder that I always copied around but I decided that adding them to a project that I could simply include with `git submodule` (and now with `nuget`) would make my life easier.

## Overview
#### GUtils.CLI
This project has utilities for programs that operate on the CLI.

Contents:
- The [command subsystem](cli/cmdSubSys)

#### GUtils<span></span>.IO
This project has utilities for programs that deal with files in general.

Contents:
- A [file copier](io/copier)
- A [file searcher](io/search)
- An artificial [/dev/null stream](io/null)
- And a [file size](io/size) utility

#### GUtils<span></span>.Net
This project contains utilities for programs that deal with networking.

Contents:
- An experimental faster [download client](net/downloader). It's able to use more of the user's bandwidth than `WebClient` in my probably arbitrary tests.

#### GUtils.Timing
This project contains various utilities to help with timing the execution of your program.

Contents:
- A [timing area](timing/area) that abuses `IDisposable` so that you don't need to manually indent and outdent the contents while logging.
- A [durations class](timing/durations) that contains methods to transform ticks into a human readable format.

#### GUtils.Windows
This project contains windows-specific general purpose utilities.

Contents:
- An [explorer.exe launching](windows/fexplorer) which I should really rename.


[cli/cmdSubSys]: cli/commandSystem.md
[io/copier]: io/fileCopier.md
[io/search]: io/fileSearch.md
[io/null]: io/nullStream.md
[io/size]: io/fileSize.md
[net/downloader]: net/downloadClient.md
[timing/area]: timing/timingArea.md
[timing/durations]: timing/durations.md
[windows/fexplorer]: windows/fexplorer.md
