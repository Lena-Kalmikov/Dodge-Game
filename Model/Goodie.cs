using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Model
{
    public class Goodie : GameObject
    {
        public Goodie(string name) : base(name)
        {
            this.ImageSize = 40;
            this.playerImage = new Image();
            this.playerImage.Width = this.ImageSize;
            this.playerImage.Height = this.ImageSize;
            this.playerImage.Name = name;
            this.playerImage.Source = new BitmapImage(new Uri("ms-appx:///images/Dino.png"));
        }
    }
}
