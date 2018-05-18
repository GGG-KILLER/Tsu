/*
 * Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”),
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUtils.Windows.Forms
{
    /// <summary>
    /// Manages a tooltip to make it show upon hovering of a Control
    /// </summary>
    public class ToolTipManager : IDisposable
    {
        private readonly Form Window;
        private readonly ToolTip ToolTip;
        private readonly Dictionary<Control, ToolTipInfo> Controls;

        public ToolTipManager ( Form Window )
        {
            this.Window = Window;
            this.ToolTip = new ToolTip ( );
            this.Controls = new Dictionary<Control, ToolTipInfo> ( );
        }

        public void Add ( Control Control, ToolTipIcon Icon, String Title, String Message )
        {
            this.Controls[Control] = new ToolTipInfo ( Icon, Title, Message );
            Control.MouseEnter += this.Control_ShowToolTip;
            Control.GotFocus += this.Control_ShowToolTip;
            Control.MouseLeave += this.Control_HideToolTip;
            Control.LostFocus += this.Control_HideToolTip;
        }

        private void Control_ShowToolTip ( Object sender, EventArgs e )
        {
            var control = ( Control ) sender;
            ToolTipInfo info = this.Controls[control];
            Point point = this.Window.PointToClient ( control.Parent.PointToScreen ( control.Location ) );
            point.Offset ( new Point ( control.Size ) );

            this.ToolTip.ToolTipTitle = info.Title;
            this.ToolTip.ToolTipIcon = info.Icon;
            this.ToolTip.Show ( info.Message, this.Window, point );
        }

        private void Control_HideToolTip ( Object sender, EventArgs e )
        {
            this.ToolTip.Hide ( this.Window );
        }

        public void Remove ( Control Control )
        {
            if ( Control?.IsDisposed == false && this.Controls.ContainsKey ( Control ) )
            {
                Control.MouseEnter -= this.Control_ShowToolTip;
                Control.GotFocus -= this.Control_ShowToolTip;
                Control.MouseLeave -= this.Control_HideToolTip;
                Control.LostFocus -= this.Control_HideToolTip;
            }
        }

        public void Dispose ( )
        {
            this.ToolTip.Dispose ( );
            GC.SuppressFinalize ( this );
        }

        private class ToolTipInfo
        {
            internal ToolTipInfo ( ToolTipIcon Icon, String Title, String Message )
            {
                this.Icon = Icon;
                this.Title = Title;
                this.Message = Message;
            }

            internal ToolTipIcon Icon;
            internal String Title, Message;
        }
    }
}
