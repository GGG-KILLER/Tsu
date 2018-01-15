namespace GUtils.Windows.Forms
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
		private ProgressBarMode mode;

		/// <summary>
		/// The <see cref="VerboseProgressBar" /><see cref="ProgressBarMode" />
		/// </summary>
		[DefaultValue ( ProgressBarMode.Percentage )]
		public ProgressBarMode Mode
		{
			get => this.mode;
			set
			{
				if ( value != ProgressBarMode.Percentage
					&& value != ProgressBarMode.CustomText )
				{
					throw new InvalidEnumArgumentException ( "Invalid value provided, must be either Percentage or CustomText" );
				}

				this.mode = value;
				this.Invalidate ( );
			}
		}

		/// <summary>
		/// The text to show if <see cref="Mode" /> is <see cref="ProgressBarMode.CustomText" />
		/// </summary>
		[Localizable ( true ), DefaultValue ( "" )]
		public new String Text
		{
			get => this.text;
			set
			{
				this.text = value;
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
			get => this.font;
			set
			{
				this.font = value;
				this.Invalidate ( );
			}
		}

		/// <summary>
		/// Property to hold the custom text color
		/// </summary>
		public Color TextColor
		{
			get => this.textColor;
			set
			{
				this.textColor = value;
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

		private const TextFormatFlags formatFlags = TextFormatFlags.VerticalCenter
				| TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine
				| TextFormatFlags.WordEllipsis;

		protected override void WndProc ( ref Message m )
		{
			base.WndProc ( ref m );
			if ( m.Msg == 0x000F ) // WM_PAINT
			{
				using ( var g = Graphics.FromHwnd ( this.Handle ) )
				using ( var textBrush = new SolidBrush ( this.ForeColor ) )
				{
					TextRenderer.DrawText (
						g,
						this.Mode == ProgressBarMode.CustomText
							? this.Text
							: $"{( Int32 ) Math.Round ( ( ( Double ) this.Value / this.Maximum ) * 100D )}%",
						this.Font,
						new Rectangle ( 0, 0, this.Width, this.Height ),
						this.TextColor,
						formatFlags );
				}
			}
		}
	}
}
