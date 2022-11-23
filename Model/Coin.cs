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
    public class Coin : GameObject
    {
        bool isPicked = false;

        public Coin(string name) : base(name)
        {
            this.ImageSize = 30;
            this.playerImage = new Image();
            this.playerImage.Width = this.ImageSize;
            this.playerImage.Height = this.ImageSize;
            this.playerImage.Name = name;
            this.playerImage.Source = new BitmapImage(new Uri("ms-appx:///images/dinocoin.png"));
        }

        public bool IsPicked
        {
            get { return this.isPicked; }
            set { this.isPicked = value; }
        }
    }
}
