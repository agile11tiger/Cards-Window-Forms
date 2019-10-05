using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DurakGame
{
    /// <summary>
    /// Borrowed from http://www.codeproject.com/Articles/5649/Extended-Graphics-An-implementation-of-Rounded-Rec
    /// </summary>
    public static class GraphicsExtensions
    {
        public static void DrawRoundedRectangle(this Graphics g, Pen pen, RectangleF rect, float radius)
        {
            var path = GetRoundedRect(rect, radius);
            g.DrawPath(pen, path);
        }

        private static GraphicsPath GetRoundedRect(RectangleF baseRect, float radius)
        {
            if (radius <= 0.0F)
            {
                var mPath = new GraphicsPath();
                mPath.AddRectangle(baseRect);
                mPath.CloseFigure();
                return mPath;
            }
            
            if (radius >= Math.Min(baseRect.Width, baseRect.Height) / 2.0)
                return GetCapsule(baseRect);
            
            var diameter = radius * 2.0F;
            var sizeF = new SizeF(diameter, diameter);
            var arc = new RectangleF(baseRect.Location, sizeF);
            var path = new GraphicsPath();
            
            path.AddArc(arc, 180, 90);
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
        
        private static GraphicsPath GetCapsule(RectangleF baseRect)
        {
            float diameter;
            RectangleF arc;
            var path = new GraphicsPath();

            try
            {
                if (baseRect.Width > baseRect.Height)
                {
                    diameter = baseRect.Height;
                    var sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    diameter = baseRect.Width;
                    var sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                    path.AddEllipse(baseRect);
            }
            catch (Exception)
            {
                path.AddEllipse(baseRect);
            }
            finally
            {
                path.CloseFigure();
            }
            return path;
        }
    }
}
