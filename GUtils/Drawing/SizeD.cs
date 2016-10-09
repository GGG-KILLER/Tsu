namespace GUtils.Drawing
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;

	// Made from the original .NET reference source
    [Serializable]
    [System.Runtime.InteropServices.ComVisible ( true )]
    public struct SizeD
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='SizeD'/> class.
        /// </summary>
        public static readonly SizeD Empty = new SizeD();

        private Double width;
        private Double height;

        /// <summary>
        /// Initializes a new instance of the <see cref='SizeD'/> class
        /// from the specified existing <see cref='SizeD'/>.
        /// </summary>
        /// <param name="size"></param>
        public SizeD ( SizeD size )
        {
            width = size.width;
            height = size.height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='SizeD'/> class from
        /// the specified <see cref='PointD'/>.
        /// </summary>
        /// <param name="pt"></param>
        public SizeD ( PointD pt )
        {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='SizeD'/> class from
        /// the specified dimensions.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public SizeD ( Double width, Double height )
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Performs vector addition of two <see cref='SizeD'/> objects.
        /// </summary>
        /// <param name="sz1"></param>
        /// <param name="sz2"></param>
        /// <returns></returns>
        public static SizeD operator + ( SizeD sz1, SizeD sz2 )
        {
            return Add ( sz1, sz2 );
        }

        /// <summary>
        /// Contracts a <see cref='SizeD'/> by another <see cref='SizeD'/>.
        /// </summary>
        /// <param name="sz1"></param>
        /// <param name="sz2"></param>
        /// <returns></returns>
        public static SizeD operator - ( SizeD sz1, SizeD sz2 )
        {
            return Subtract ( sz1, sz2 );
        }

        /// <summary>
        /// Tests whether two <see cref='SizeD'/> objects
        /// are identical.
        /// </summary>
        /// <param name="sz1"></param>
        /// <param name="sz2"></param>
        /// <returns></returns>
        public static bool operator == ( SizeD sz1, SizeD sz2 )
        {
            return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
        }

        /// <summary>
        /// Tests whether two <see cref='SizeD'/> objects are different.
        /// </summary>
        /// <param name="sz1"></param>
        /// <param name="sz2"></param>
        /// <returns></returns>
        public static bool operator != ( SizeD sz1, SizeD sz2 )
        {
            return !( sz1 == sz2 );
        }

        /// <summary>
        /// Converts the specified <see cref='SizeD'/> to a <see cref='PointD'/>.
        /// </summary>
        /// <param name="size"></param>
        public static explicit operator PointD ( SizeD size )
        {
            return new PointD ( size.Width, size.Height );
        }

        /// <summary>
        /// Tests whether this <see cref='SizeD'/> has zero width and height.
        /// </summary>
        [Browsable ( false )]
        public bool IsEmpty
        {
            get
            {
                return width == 0 && height == 0;
            }
        }

        /// <summary>
        /// Represents the horizontal component of this <see cref='SizeD'/>.
        /// </summary>
        public Double Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        /// <summary>
        /// Represents the vertical component of this <see cref='SizeD'/>.
        /// </summary>
        public Double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        /// <summary>
        /// Performs vector addition of two <see cref='SizeD'/> objects.
        /// </summary>
        /// <param name="sz1"></param>
        /// <param name="sz2"></param>
        /// <returns></returns>
        public static SizeD Add ( SizeD sz1, SizeD sz2 )
        {
            return new SizeD ( sz1.Width + sz2.Width, sz1.Height + sz2.Height );
        }

        /// <summary>
        /// Contracts a <see cref='SizeD'/> by another <see cref='SizeD'/>.
        /// </summary>
        /// <param name="sz1"></param>
        /// <param name="sz2"></param>
        /// <returns></returns>
        public static SizeD Subtract ( SizeD sz1, SizeD sz2 )
        {
            return new SizeD ( sz1.Width - sz2.Width, sz1.Height - sz2.Height );
        }

        /// <summary>
        /// Tests to see whether the specified object is a <see cref='SizeD'/>
        /// with the same dimensions as this <see cref='SizeD'/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals ( object obj )
        {
            if ( !( obj is SizeD ) )
                return false;

            var comp = ( SizeD ) obj;

            return ( comp.Width == this.Width ) &&
            ( comp.Height == this.Height ) &&
            ( comp.GetType ( ).Equals ( GetType ( ) ) );
        }

        public override int GetHashCode ( )
        {
            return base.GetHashCode ( );
        }

        public PointD ToPointD ( )
        {
            return ( PointD ) this;
        }

        public Size ToSize ( )
        {
            return new Size ( ( Int32 ) Math.Truncate ( this.width ), ( Int32 ) Math.Truncate ( this.height ) );
        }

        /// <summary>
        /// Creates a human-readable string that represents this <see cref='SizeD'/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return "{Width=" + width.ToString ( CultureInfo.CurrentCulture ) + ", Height=" + height.ToString ( CultureInfo.CurrentCulture ) + "}";
        }
    }
}