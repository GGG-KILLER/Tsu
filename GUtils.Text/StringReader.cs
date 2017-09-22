using System;

namespace GUtils.Text
{
    public class StringReader
    {
        private readonly String Value;

        public Int32 Position { get; set; }

        public StringReader ( String Value )
        {
            this.Value = Value;
            this.Position = 0;
        }

        #region Basic Checking

        /// <summary>
        /// Checks wether we can move <paramref name="Delta" /> characters
        /// </summary>
        /// <param name="Delta">Amount of characters to move</param>
        /// <returns></returns>
        public Boolean CanMove ( Int32 Delta )
        {
            var newPos = this.Position + Delta;
            return -1 < newPos && newPos < this.Value.Length;
        }

        /// <summary>
        /// Checks whether the next character after the
        /// <paramref name="dist" /> th character is <paramref name="ch" />
        /// </summary>
        /// <param name="ch">The character to find</param>
        /// <param name="dist">
        /// The distance at when to start looking for
        /// </param>
        /// <returns></returns>
        public Boolean IsNext ( Char ch, Int32 dist = 1 )
        {
            return Peek ( dist ) == ch;
        }

        /// <summary>
        /// Checks whether <paramref name="a" /> is before <paramref name="b" />
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public Boolean AIsBeforeB ( Char a, Char b )
        {
            var idxa = this.IndexOf ( a );
            var idxb = this.IndexOf ( b );
            return idxa != -1 && idxb != -1 && idxa < idxb;
        }

        #endregion Basic Checking

        #region Basic Movement

        /// <summary>
        /// Consumes the <paramref name="dist" /> th next character
        /// </summary>
        /// <param name="dist"></param>
        /// <returns></returns>
        public Char Read ( Int32 dist = 1 )
        {
            if ( !CanMove ( dist ) )
                return '\0';
            this.Position += dist;
            return this.Value[this.Position];
        }

        /// <summary>
        /// Returns the next <paramref name="dist" /> th character
        /// </summary>
        /// <param name="dist"></param>
        /// <returns></returns>
        public Char Peek ( Int32 dist = 1 )
        {
            if ( !CanMove ( dist ) )
                return '\0';
            return this.Value[this.Position + dist];
        }

        /// <summary>
        /// Returns the distance from <see cref="Position" /> +
        /// <paramref name="offset" /> to <paramref name="ch" />
        /// </summary>
        /// <param name="ch">Character to search for</param>
        /// <param name="offset">
        /// Distance from current position where to start
        /// searching for
        /// </param>
        /// <returns></returns>
        public Int32 IndexOf ( Char ch, Int32 offset = 0 )
        {
            var idx = this.Value.IndexOf ( ch, this.Position + offset );
            return idx == -1 ? -1 : idx - ( this.Position + offset );
        }

        /// <summary>
        /// Returns the to <paramref name="needle" />(NOT AN
        /// ABSOLUTE POSITION, IT IS RELATIVE TO
        /// <see cref="Position" /> + <paramref name="offset" />)
        /// </summary>
        /// <param name="needle">String to search for</param>
        /// <param name="offset">
        /// Distance from current position where to start
        /// searching for
        /// </param>
        /// <returns></returns>
        public Int32 IndexOf ( String needle, Int32 offset = 0 )
        {
            if ( needle == null )
                throw new ArgumentNullException ( nameof ( needle ) );
            var idx = this.Value.IndexOf ( needle, this.Position + offset );
            return idx == -1 ? -1 : idx - ( this.Position + offset );
        }

        /// <summary>
        /// Returns the index of a character that passes the
        /// <paramref name="Filter" /> (NOT AN ABSOLUTE POSITION,
        /// IT IS RELATIVE TO <see cref="Position" />
        /// + <paramref name="offset" />)
        /// </summary>
        /// <param name="Filter">The filter function</param>
        /// <param name="offset">
        /// Offset from current opsition where to start searching from
        /// </param>
        /// <returns></returns>
        public Int32 IndexOf ( Func<Char, Boolean> Filter, Int32 offset = 0 )
        {
            if ( Filter == null )
                throw new ArgumentNullException ( nameof ( Filter ) );

            for ( Int32 i = this.Position + offset ; i < this.Value.Length ; i++ )
                if ( Filter ( this.Value[i] ) )
                    return i - ( this.Position + offset );
            return -1;
        }

        #endregion Basic Movement

        #region Advanced Movement

        #region Char Based

        /// <summary>
        /// Reads until <paramref name="ch" /> is found
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public String ReadUntil ( Char ch )
        {
            var idx = this.IndexOf ( ch );
            return idx == -1 ? "" : this.ReadString ( idx );
        }

        /// <summary>
        /// Reads until <paramref name="Filter" /> is false
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public String ReadUntil ( Func<Char, Boolean> Filter )
        {
            var idx = this.IndexOf ( Filter );
            return idx == -1 ? "" : this.ReadString ( idx );
        }

        /// <summary>
        /// Reads until <paramref name="Filter" /> is false
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public String ReadUntil ( Func<Char, Char, Boolean> Filter )
        {
            var len = 0;
            while ( !Filter?.Invoke ( Peek ( len + 1 ), Peek ( len + 2 ) ) ?? default ( Boolean ) )
                len++;
            return ReadString ( len );
        }

        /// <summary>
        /// Reads until <paramref name="Filter" /> is false
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public String ReadUntilNot ( Func<Char, Boolean> Filter )
        {
            var len = 0;
            while ( Filter?.Invoke ( Peek ( 1 ) ) ?? default ( Boolean ) )
                len++;
            return this.ReadString ( len );
        }

        /// <summary>
        /// Reads until <paramref name="Filter" /> is false
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public String ReadUntilNot ( Func<Char, Char, Boolean> Filter )
        {
            var len = 0;
            while ( Filter?.Invoke ( Peek ( 1 ), Peek ( 2 ) ) ?? default ( Boolean ) )
                len++;
            return this.ReadString ( len );
        }

        #endregion Char Based

        #region String-based

        /// <summary>
        /// Reads up to <paramref name="str" />
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public String ReadUntil ( String str )
        {
            var i = IndexOf ( str, this.Position );
            return i == -1 ? "" : ReadString ( i - this.Position );
        }

        #endregion String-based

        /// <summary>
        /// Reads and returns a <see cref="String" /> of <paramref name="length" />
        /// </summary>
        /// <param name="length">Length of the string to read</param>
        /// <returns></returns>
        public String ReadString ( Int32 length )
        {
            if ( length == 0 )
                return String.Empty;
            try
            {
                return this.Value.Substring ( this.Position, length );
            }
            finally
            {
                this.Position += length;
            }
        }

        #endregion Advanced Movement
    }
}
