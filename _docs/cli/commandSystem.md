## Command subsystem
### Overview
The command subsystem allows a user to create a shell-like environment where users can execute commands with the possibility to pass parameters to them.

The command subsystem is able to:
- Find [commands](attr/cmd) automatically inside a given type
- Parse comamnds with single/double quotes and hexadecimal/binary/decimal/octal/C escapes
- Generate methods (at runtime) that will validate and convert the arguments provided by the user for a better performance
- Generate a help command from the [help description](attr/desc) and [help example](attr/ex) attributes

### API Documentation
#### Attributes:
- [CommandAttribute](attr/cmd)
- [HelpDescriptionAttribute](attr/desc)
- [HelpExampleAttribute](attr/ex)
- [RawInputAttribute](attr/raw)
- ~~[JoinRestOfArgumentsAttribute](attr/join)~~ (obsolete)

#### Classes:
- [Command](cmd)
- [CommandManager](cmdManager)

#### Enums:
- [CommandManagerFlags](cmdManagerFlags)

[attr/cmd]: attributes/commandAttribute.md
[attr/desc]: attributes/helpDescriptionAttribute.md
[attr/ex]: attributes/helpExampleAttribute.md
[attr/raw]: attributes/rawInputAttribute.md
[attr/join]: attributes/joinRestOfargumentsAttribute.md
[cmd]: command.md
[cmdManager]: commandManager.md
[cmdManagerFlags]: commandManagerFlags.md
