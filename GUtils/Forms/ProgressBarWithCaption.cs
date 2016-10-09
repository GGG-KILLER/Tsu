namespace GUtils.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public enum ProgressBarDisplayText
    {
        Percentage,
        CustomText
    }

    /// <summary>
    /// A progressbar with text on it
    /// Taken from: http://stackoverflow.com/a/29175656
    /// </summary>
    public class ProgressBarWithCaption : ProgressBar
    {
        private String m_CustomText;
        private Font m_CustomTextFont = SystemFonts.DefaultFont;
        private Color m_CustomTextColor = Color.Black;

        /// <summary>
        /// Property to set to decide whether to print a % or Text
        /// </summary>
        [DefaultValue ( ProgressBarDisplayText.Percentage )]
        public ProgressBarDisplayText DisplayStyle { get; set; }

        /// <summary>
        /// Property to hold the custom text
        /// </summary>
        [Localizable ( true ), DefaultValue ( "" )]
        public String CustomText
        {
            get { return m_CustomText; }
            set
            {
                m_CustomText = value;
                this.Invalidate ( );
            }
        }

        /// <summary>
        /// Property to hold the custom text font
        /// </summary>
        [Localizable ( true ), AmbientValue ( null )]
        public Font CustomTextFont
        {
            get { return m_CustomTextFont; }
            set
            {
                m_CustomTextFont = value;
                this.Invalidate ( );
            }
        }

        /// <summary>
        /// Property to hold the custom text color
        /// </summary>
        public Color CustomTextColor
        {
            get { return m_CustomTextColor; }
            set
            {
                m_CustomTextColor = value;
                this.Invalidate ( );
            }
        }

        public ProgressBarWithCaption ( ) { SetStyle ( ControlStyles.OptimizedDoubleBuffer, true ); }

        public ProgressBarWithCaption ( IContainer container ) { container.Add ( this ); SetStyle ( ControlStyles.OptimizedDoubleBuffer, true ); }

        private const Int32 WM_PAINT = 0x000F;
        protected override void WndProc ( ref Message m )
        {
            base.WndProc ( ref m );

            switch ( m.Msg )
            {
                case WM_PAINT:
                    var m_Percent = Convert.ToInt32 ( ( Convert.ToDouble ( Value ) / Convert.ToDouble ( Maximum ) ) * 100 );
                    dynamic flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis;

                    using ( Graphics g = Graphics.FromHwnd ( Handle ) )
                    {
                        using ( Brush textBrush = new SolidBrush ( ForeColor ) )
                        {

                            switch ( DisplayStyle )
                            {
                                case ProgressBarDisplayText.CustomText:
                                    TextRenderer.DrawText ( g, CustomText, m_CustomTextFont, new Rectangle ( 0, 0, this.Width, this.Height ), m_CustomTextColor, flags );
                                    break;
                                case ProgressBarDisplayText.Percentage:
                                    TextRenderer.DrawText ( g, $"{m_Percent}%", m_CustomTextFont, new Rectangle ( 0, 0, this.Width, this.Height ), m_CustomTextColor, flags );
                                    break;
                            }

                        }
                    }

                    break;
            }

        }
    }
}