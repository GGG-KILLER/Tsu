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
namespace GUtils.Drawing
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
	
	// Based on Bitmap Byte Array accessing on http://www.codeproject.com/Tips/240428/Work-with-bitmap-faster-with-Csharps
	// Made by me
    public class OptmizedBitmap
    {
        private IntPtr Pointer;
        private Byte[] Pixels;
        private BitmapData Data;
        private Boolean HasAlpha;
        private Bitmap Bitmap;

        internal Int32 Width;
        internal Int32 Height;

        internal OptmizedBitmap ( Bitmap Bitmap )
        {
            this.Bitmap = Bitmap;
            this.HasAlpha = Bitmap.PixelFormat.HasFlag ( PixelFormat.Alpha );
            this.Width = Bitmap.Width;
            this.Height = Bitmap.Height;
        }

        internal void Lock ( )
        {
            if ( Data != null )
                throw new Exception ( "Image already locked" );

            Data = Bitmap.LockBits ( new Rectangle ( 0, 0, Width, Height ), ImageLockMode.ReadWrite, Bitmap.PixelFormat );
            Pointer = Data.Scan0;
            Pixels = new Byte[Width * Height * ( HasAlpha ? 4 : 3 )];
            Marshal.Copy ( Pointer, Pixels, 0, Pixels.Length );
        }

        internal void UnLock ( Boolean HasChanges )
        {
            if ( Data == null )
                throw new Exception ( "Image not locked" );

            if ( HasChanges )
                Marshal.Copy ( Pixels, 0, Pointer, Pixels.Length );

            Bitmap.UnlockBits ( Data );
            Data = null;
            Pointer = IntPtr.Zero;
        }

        internal void SetPixel ( Int32 X, Int32 Y, Color Color )
        {
            if ( Data == null )
                throw new Exception ( "Image not locked" );

            var i = ( Y * Width + X ) * ( HasAlpha ? 4 : 3 );
            Pixels[i] = Color.B;
            Pixels[i + 1] = Color.G;
            Pixels[i + 2] = Color.R;
            if ( HasAlpha )
                Pixels[i + 3] = Color.A;
        }

        internal Color GetPixel ( Int32 X, Int32 Y )
        {
            if ( Data == null )
                throw new Exception ( "Image not locked" );

            var i = ( Y * Width + X ) * ( HasAlpha ? 4 : 3 );
            return Color.FromArgb ( HasAlpha ? Pixels[i + 3] : 255, Pixels[i + 2], Pixels[i + 1], Pixels[i] );
        }

        internal void Clear ( Color Color )
        {
            if ( Data == null )
                throw new Exception ( "Image not locked" );

            for ( var i = 0 ; i < Pixels.Length ; i += HasAlpha ? 4 : 3 )
            {
                Pixels[i] = Color.B;
                Pixels[i + 1] = Color.G;
                Pixels[i + 2] = Color.R;
                if ( HasAlpha )
                    Pixels[i + 3] = Color.A;
            }
        }
    }
}