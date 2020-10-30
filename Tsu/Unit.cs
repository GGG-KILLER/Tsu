using System;
using System.Diagnostics.CodeAnalysis;

namespace Tsu
{
    /// <summary>
    /// A unit value.
    /// </summary>
    public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
    {
        /// <summary>
        /// The unit type instance.
        /// </summary>
        public static readonly Unit Value;

        /// <inheritdoc/>
        public Int32 CompareTo ( Unit other ) => 0;

        /// <inheritdoc/>
        public Int32 CompareTo ( Object? obj ) => 0;

        /// <inheritdoc/>
        public override Boolean Equals ( Object? obj ) => obj is Unit;

        /// <inheritdoc/>
        public Boolean Equals ( Unit other ) => true;

        /// <inheritdoc/>
        public override Int32 GetHashCode ( ) => 0;

        /// <inheritdoc/>
        public override String ToString ( ) => "()";


        /// <summary>
        /// Checks if one <see cref="Unit"/> equals other <see cref="Unit"/>.
        /// Always true because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage ( "Style", "IDE0060:Remove unused parameter", Justification = "They are required." )]
        [SuppressMessage ( "Usage", "CA1801:Review unused parameters", Justification = "They are required." )]
        public static Boolean operator == ( Unit left, Unit right ) => true;

        /// <summary>
        /// Checks if one <see cref="Unit"/> is not equal to other <see cref="Unit"/>.
        /// Always false because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage ( "Style", "IDE0060:Remove unused parameter", Justification = "They are required." )]
        [SuppressMessage ( "Usage", "CA1801:Review unused parameters", Justification = "They are required." )]
        public static Boolean operator != ( Unit left, Unit right ) => false;

        /// <summary>
        /// Checks if one <see cref="Unit"/> is less than other <see cref="Unit"/>.
        /// Always false because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage ( "Style", "IDE0060:Remove unused parameter", Justification = "They are required." )]
        [SuppressMessage ( "Usage", "CA1801:Review unused parameters", Justification = "They are required." )]
        public static Boolean operator < ( Unit left, Unit right ) => false;

        /// <summary>
        /// Checks if one <see cref="Unit"/> is less than or equal other <see cref="Unit"/>.
        /// Always true because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage ( "Style", "IDE0060:Remove unused parameter", Justification = "They are required." )]
        [SuppressMessage ( "Usage", "CA1801:Review unused parameters", Justification = "They are required." )]
        public static Boolean operator <= ( Unit left, Unit right ) => true;

        /// <summary>
        /// Checks if one <see cref="Unit"/> is greater than other <see cref="Unit"/>.
        /// Always false because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage ( "Style", "IDE0060:Remove unused parameter", Justification = "They are required." )]
        [SuppressMessage ( "Usage", "CA1801:Review unused parameters", Justification = "They are required." )]
        public static Boolean operator > ( Unit left, Unit right ) => false;

        /// <summary>
        /// Checks if one <see cref="Unit"/> is greater than or equal other <see cref="Unit"/>.
        /// Always true because <see cref="Unit"/> can only assume 1 value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [SuppressMessage ( "Style", "IDE0060:Remove unused parameter", Justification = "They are required." )]
        [SuppressMessage ( "Usage", "CA1801:Review unused parameters", Justification = "They are required." )]
        public static Boolean operator >= ( Unit left, Unit right ) => true;
    }
}