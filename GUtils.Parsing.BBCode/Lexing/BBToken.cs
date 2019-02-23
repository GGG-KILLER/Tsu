using System;

namespace GUtils.Parsing.BBCode.Lexing
{
    internal readonly ref struct BBToken
    {
        public readonly BBTokenType Type;
        public readonly String Value;

        public BBToken ( BBTokenType type, String value )
        {
            this.Type  = type;
            this.Value = value;
        }
    }
}
