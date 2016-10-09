namespace GUtils.Drawing
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;

	// Made from the original .NET reference source
    /// <summary>
    /// Represents a point in 2D coordinate space
    /// (float precision floating-point coordinates)
    /// </summary>
    [Serializable]
    [System.Runtime.InteropServices.ComVisible ( true )]
    public struct PointD
    {
        /// <summary>
        /// Creates a new instance of the <see cref='PointD'/> class
        /// with member data left uninitialized.
        /// </summary>
        public static readonly PointD Empty = new PointD();

        private Double x;
        private Double y;

        /// <summary>
        /// Initializes a new instance of the <see cref='PointD'/> class
        /// with the specified coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public PointD ( Double x, Double y )
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref='PointD'/> is empty.
        /// </summary>
        [Browsable ( false )]
        public bool IsEmpty
        {
            get
            {
                return x == 0d && y == 0d;
            }
        }

        /// <summary>
        /// Gets the x-coordinate of this <see cref='PointD'/>.
        /// </summary>
        public Double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='PointD'/>.
        /// </summary>
        public Double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='SizeD'/> .
        /// </summary>
        public static PointD operator + ( PointD pt, SizeD sz )
        {
            return Add ( pt, sz );
        }

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='SizeD'/>.
        /// </summary>
        public static PointD operator - ( PointD pt, SizeD sz )
        {
            return Subtract ( pt, sz );
        }

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD operator + ( PointD pt, SizeF sz )
        {
            return Add ( pt, sz );
        }

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD operator - ( PointD pt, SizeF sz )
        {
            return Subtract ( pt, sz );
        }

        /// <summary>
        /// Compares two <see cref='PointD'/> objects. The result specifies
        /// whether the values of the <see cref='PointD.X'/> and <see cref='PointD.Y'/> properties of the two <see cref='PointD'/>
        /// objects are equal.
        /// </summary>
        public static bool operator == ( PointD left, PointD right )
        {
            return left.X == right.X && left.Y == right.Y;
        }

        /// <summary>
        /// Compares two <see cref='PointD'/> objects. The result specifies whether the values
        /// of the <see cref='PointD.X'/> or <see cref='PointD.Y'/> properties of the two
        /// <see cref='PointD'/> objects are unequal.
        /// </summary>
        public static bool operator != ( PointD left, PointD right )
        {
            return !( left == right );
        }

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='SizeD'/> .
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="sz"></param>
        public static PointD Add ( PointD pt, SizeD sz )
        {
            return new PointD ( pt.X + sz.Width, pt.Y + sz.Height );
        }

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='SizeD'/> .
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static PointD Subtract ( PointD pt, SizeD sz )
        {
            return new PointD ( pt.X - sz.Width, pt.Y - sz.Height );
        }

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static PointD Add ( PointD pt, SizeF sz )
        {
            return new PointD ( pt.X + sz.Width, pt.Y + sz.Height );
        }

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static PointD Subtract ( PointD pt, SizeF sz )
        {
            return new PointD ( pt.X - sz.Width, pt.Y - sz.Height );
        }

        /// <summary>
        /// Checks if the <see cref="PointD"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals ( object obj )
        {
            if ( !( obj is PointD ) )
                return false;
            var comp = ( PointD ) obj;
            return
            comp.X == this.X &&
            comp.Y == this.Y &&
            comp.GetType ( ).Equals ( this.GetType ( ) );
        }

        public override int GetHashCode ( )
        {
            return base.GetHashCode ( );
        }

        public override string ToString ( )
        {
            return string.Format ( CultureInfo.CurrentCulture, "{{X={0}, Y={1}}}", x, y );
        }
    }
}