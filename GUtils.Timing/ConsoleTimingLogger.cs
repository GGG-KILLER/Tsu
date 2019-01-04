using System;

namespace GUtils.Timing
{
    /// <summary>
    /// A <see cref="TimingLogger" /> that outputs to the Console
    /// </summary>
    public class ConsoleTimingLogger : TimingLogger
    {
        /// <summary>
        /// Initializes a new console timing logger
        /// </summary>
        public ConsoleTimingLogger ( )
        {
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="str"></param>
        protected override void WriteInternal ( String str ) => Console.Write ( str );

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        protected override void WriteInternal ( String str, ConsoleColor color )
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write ( str );
            Console.ForegroundColor = c;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="line"></param>
        protected override void WriteLineInternal ( String line ) => Console.WriteLine ( line );

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="line"></param>
        /// <param name="color"></param>
        protected override void WriteLineInternal ( String line, ConsoleColor color )
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine ( line );
            Console.ForegroundColor = c;
        }

        #region Extra Public I/O

        /// <summary>
        /// Reads an line of input from the console
        /// </summary>
        /// <returns></returns>
        public String ReadLine ( )
        {
            var line = Console.ReadLine ( );
            this.hasLineBeenPrefixed = false;
            return line;
        }

        #endregion
    }
}
