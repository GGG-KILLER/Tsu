using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GUtils.Forms
{
    /// <summary>
    /// Manages a tooltip to make it show upon hovering of a Control
    /// </summary>
    public class ToolTipManager : IDisposable
    {
        private Form Window;
        private ToolTip ToolTip;
        private Dictionary<Control, ToolTipInfo> Controls;

        public ToolTipManager ( Form Window )
        {
            this.Window = Window;
            this.ToolTip = new ToolTip ( );
            this.Controls = new Dictionary<Control, ToolTipInfo> ( );
        }

        public void Add ( Control Control, ToolTipIcon Icon, String Title, String Message )
        {
            Controls[Control] = new ToolTipInfo ( Icon, Title, Message );
            Control.MouseEnter += Control_MouseEnter;
            Control.MouseLeave += Control_MouseLeave;
        }

        private void Control_MouseEnter ( Object sender, EventArgs e )
        {
            var C = ( Control ) sender;
            var I = Controls[C];
            var P = Window.PointToClient ( C.Parent.PointToScreen ( C.Location ) );
            P.Offset ( new System.Drawing.Point ( C.Size ) );

            this.ToolTip.ToolTipTitle = I.Title;
            this.ToolTip.ToolTipIcon = I.Icon;
            this.ToolTip.Show ( I.Message, Window, P );
        }

        private void Control_MouseLeave ( Object sender, EventArgs e )
        {
            this.ToolTip.Hide ( Window );
        }

        public void Remove ( Control Control )
        {
#pragma warning disable CC0004
            try
            {
                Control.MouseEnter -= Control_MouseEnter;
            }
            catch ( Exception ) {  }
            try
            {
                Control.MouseLeave -= Control_MouseLeave;
            }
            catch ( Exception ) {  }
            try
            {
                Controls.Remove ( Control );
            }
            catch ( Exception ) {  }
#pragma warning restore CC0004
        }

        public void Dispose ( )
        {
            ToolTip.Dispose ( );
            GC.SuppressFinalize ( this );
        }

        private class ToolTipInfo
        {
            internal ToolTipInfo ( ToolTipIcon I, String T, String M )
            {
                Icon = I;
                Title = T;
                Message = M;
            }

            internal ToolTipIcon Icon;
            internal String Title, Message;
        }
    }
}