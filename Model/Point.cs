using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Point
    {
        int left = 0;
        int top = 0;

        public Point (int x, int y)
        {
            this.left = x;
            this.top = y;
        }

        public int Left
        {
            get { return this.left; }
            set { this.left = value; }
        }

        public int Top
        {
            get { return this.top; }
            set { this.top = value; }
        }

        public double GetAngleBetweenPoints(Point point)
        {

            int yDiff = this.top - point.Top;
            int xDiff = this.left - point.Left;

            var deltaX = Math.Pow(xDiff, 2);
            var deltaY = Math.Pow(yDiff, 2);

            var radian = Math.Atan2(yDiff, xDiff);
            var angle = (radian * (180 / Math.PI) + 360) % 360 + 90;

            return angle;
        }
    }
}
