using System;

namespace GUtils
{
    /// <summary>
    /// A unit value.
    /// </summary>
    public readonly struct Unit : IEquatable<Unit>
    {
        /// <summary>
        /// The instance of the Unit type.
        /// </summary>
        public static readonly Unit Instance = default;

        /// <inheritdoc />
        public override Boolean Equals ( Object? obj ) => obj is Unit;

        /// <inheritdoc />
        public Boolean Equals ( Unit other ) => true;

        /// <inheritdoc />
        public override Int32 GetHashCode ( ) => -1830369473;

        /// <inheritdoc />
        public override String ToString ( ) => "()";
    }
}