using System;
using System.Collections.Generic;
using System.Text;

namespace GUtils.CLI.SourceGenerator.CommandManager
{
    public enum ErrorKind
    {
        CommandManagerClassIsNotPartial,
        ClassDoesNotContainCommandManagerAttribute,
        ClassContainsTooManyCommandManagerAttributes,
        InvalidFirstArgumentPassedToAttributeConstructor,
        InvalidSecondArgumentPassedToattributeConstructor
    }
}
