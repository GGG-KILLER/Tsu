using System;
using System.Windows.Markup;

// This was modified by me but made by Brian Lagunas
// (http://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/). In that post there was no license
// pointed out so if you want this removed please contact me at any time.
namespace GUtils.Windows.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension to bind to the values of a given enum type
    /// </summary>
    public class EnumValuesSourceExtension : MarkupExtension
    {
        private Type type;
        private Array values;

        /// <summary>
        /// The <see cref="System.Type" /> of the <see cref="Enum" />
        /// </summary>
        public Type Type
        {
            get => this.type;

            set
            {
                if ( this.type != value )
                {
                    if ( value != null )
                    {
                        Type actualType = Nullable.GetUnderlyingType ( value ) ?? value;
                        if ( !actualType.IsEnum )
                            throw new ArgumentException ( "Provided type is not an enum." );

                        Array vals = Enum.GetValues ( actualType );
                        if ( value != actualType ) // if we've got a nullable, null is also a possible value
                        {
                            var tmp = Array.CreateInstance ( value, vals.Length + 1 );
                            vals.CopyTo ( tmp, 1 );
                            vals = tmp;
                        }
                        this.values = vals;
                    }
                    else
                    {
                        this.values = null;
                    }

                    this.type = value;
                }
            }
        }

        /// <summary>
        /// Initializes this source extension
        /// </summary>
        public EnumValuesSourceExtension ( )
        {
        }

        /// <summary>
        /// Initializes this source extension with the provided <paramref name="type" />
        /// </summary>
        /// <param name="type"></param>
        public EnumValuesSourceExtension ( Type type )
        {
            this.Type = type;
        }

        /// <summary>
        /// Returns the possible values for the type stored in this source extension
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override Object ProvideValue ( IServiceProvider serviceProvider ) =>
            this.values ?? throw new InvalidOperationException ( "The type stored in this is null." );
    }
}
