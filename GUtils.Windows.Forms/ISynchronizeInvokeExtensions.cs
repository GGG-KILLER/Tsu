namespace GUtils.Windows.Forms
{
    using System;
    using System.ComponentModel;

    // Credit goes to http://stackoverflow.com/a/711419/2671392
    public static class ISynchronizeInvokeExtensions
    {
        public static void InvokeEx<T> ( this T @this, Action<T> action ) where T : ISynchronizeInvoke
        {
            try
            {
                if ( @this.InvokeRequired )
                {
                    @this.Invoke ( action, new Object[] { @this } );
                }
                else
                {
                    action?.Invoke ( @this );
                }
            }
            // When form is disposed.
            catch ( Exception )
            {
                return;
            }
        }
    }
}
