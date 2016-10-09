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
    using Math;
    using System;
    using System.Drawing;

    public class BitmapDrawing
    {
        private readonly OptmizedBitmap Canvas;
        private readonly Color Pencil;
        private readonly Int32 Height;
        private readonly Int32 Width;

        public BitmapDrawing ( Bitmap Canvas )
        {
            this.Canvas = new OptmizedBitmap ( Canvas );
            Width = Canvas.Width;
            Height = Canvas.Height;
        }

        public BitmapDrawing ( Bitmap Canvas, Color Pencil ) : this ( Canvas )
        {
            this.Pencil = Pencil;
        }

        public void DrawSquare ( Point Start, Point End )
        {
            Canvas.Lock ( );

            foreach ( Int32 Y in Counting.FromAToB ( Start.Y, End.Y ) )
                foreach ( Int32 X in Counting.FromAToB ( Start.X, End.X ) )
                    Canvas.SetPixel ( X, Y, Pencil );

            Canvas.UnLock ( true );
        }

        public void DrawLine ( Point Start, Point End )
        {
            Canvas.Lock ( );

            var Length = Vector2D.Distance ( Start, End );
            var Angle = Vector2D.GetAngle ( End, Start );

            var LX = -1;
            var LY = -1;
            for ( Double Len = 0 ; Len < Length ; Len += .1 )
            {
                var Point = Vector2D.GetPoint ( Start, Len, Angle );
                if ( Point.X == LX && Point.Y == LY || Point.Y < 0 || Point.X < 0 || Point.Y > Height || Point.X > Width )
                    continue;

                LX = Point.X;
                LY = Point.Y;

                Canvas.SetPixel ( Point.X, Point.Y, Pencil );
            }

            Canvas.UnLock ( true );
        }
    }
}