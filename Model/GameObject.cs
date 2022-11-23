using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Model
{
    public class GameObject
    {
        string name = "";
        int imageSize;
        bool isDead = false;
        
        Point location = null;
        
        List<Point> locationHistory = new List<Point>();

        public Image playerImage;

        public GameObject(string name)
        {
            this.name = name;
            this.location = new Point(0, 0);
        }

        public string Name
        {
            get { return this.name; }
        }

        public Image PlayerImage
        {
            get { return playerImage; }
            set { playerImage = value; }
        }

        public Point Location
        {
            get { return this.location; }
        }

        public bool IsDead
        {
            set { this.isDead = value; }
            get { return this.isDead; }
        }

        public int ImageSize
        {
            get { return this.imageSize; }
            set { this.imageSize = value; }
        }

        public void addToLocationHistory()
        {
            this.locationHistory.Add(new Point(this.location.Left, this.location.Top));
        }

        public void SetLocation(int x, int y)
        {
            this.location.Left = x;
            this.location.Top = y;
            this.addToLocationHistory();
        }

        public int GetTopCenterLocation()
        {
            return this.Location.Top + this.imageSize / 2;
        }

        public int GetLeftCenterLocation()
        {
            return this.Location.Left + this.imageSize / 2;
        }

        public Point getCenterPoint()
        {
            return new Point(this.GetLeftCenterLocation(), this.GetTopCenterLocation());    
        }
    }
}
