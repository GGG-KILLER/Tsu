namespace GUtils.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public enum ProgressBarMode
    {
        Percentage,
        CustomText
    }

    /// <summary>
    /// A progressbar with text on it Taken from: http://stackoverflow.com/a/29175656
    /// </summary>
    public class VerboseProgressBar : ProgressBar
    {
        private String          text        = "";
        private Font            font        = SystemFonts.DefaultFont;
        private Color           textColor   = Color.Black;
        private ProgressBarMode mode        = ProgressBarMode.Percentage;

        /// <summary>
        /// The <see cref="VerboseProgressBar" /><see cref="ProgressBarMode" />
        /// </summary>
        [DefaultValue ( ProgressBarMode.Percentage )]
        public ProgressBarMode Mode
        {
            get => mode;
            set
            {
                if ( value != ProgressBarMode.Percentage && value != ProgressBarMode.CustomText )
                    throw new Exception ( "Invalud value provided, these are not flags!" );

                mode = value;
                this.Invalidate ( );
            }
        }

        /// <summary>
        /// The text to show if <see cref="Mode" /> is <see cref="ProgressBarMode.CustomText" />
        /// </summary>
        [Localizable ( true ), DefaultValue ( "" )]
        public new String Text
        {
            get => text;
            set
            {
                text = value;
                this.Invalidate ( );
            }
        }

        /// <summary>
        /// The <see cref="System.Drawing.Font" /> to use when
        /// drawing the text
        /// </summary>
        [Localizable ( true ), AmbientValue ( null )]
        public new Font Font
        {
            get => font;
            set
            {
                font = value;
                this.Invalidate ( );
            }
        }

        /// <summary>
        /// Property to hold the custom text color
        /// </summary>
        public Color TextColor
        {
            get => textColor;
            set
            {
                textColor = value;
                this.Invalidate ( );
            }
        }

        /// <summary>
        /// Initializes a new <see cref="VerboseProgressBar" />
        /// </summary>
        public VerboseProgressBar ( )
        {
            SetStyle ( ControlStyles.OptimizedDoubleBuffer, true );
        }

        /// <summary>
        /// Initializes a new <see cref="VerboseProgressBar" />
        /// </summary>
        /// <param name="container"></param>
        public VerboseProgressBar ( IContainer container ) : base ( )
        {
            container.Add ( this );
        }

        private const TextFormatFlags formatFlags = TextFormatFlags.VerticalCenter |
                TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine |
                TextFormatFlags.WordEllipsis;

        protected override void WndProc ( ref Message m )
        {
            base.WndProc ( ref m );
            if ( m.Msg == 0x000F ) // WM_PAINT
                using ( var g = Graphics.FromHwnd ( Handle ) )
                using ( var textBrush = new SolidBrush ( ForeColor ) )
                    TextRenderer.DrawText (
                        g,
                        Mode == ProgressBarMode.CustomText
                            ? Text
                            : $"{( Int32 ) Math.Round ( ( ( Double ) Value / Maximum ) * 100D )}%",
                        Font,
                        new Rectangle ( 0, 0, this.Width, this.Height ),
                        TextColor,
                        formatFlags );
        }
    }
}