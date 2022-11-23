using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Model
{
    public class Baddie : GameObject
    {
        int stepSize;
        public Baddie(string name) : base (name)
        {
            this.ImageSize = 50;
            this.playerImage = new Image();
            this.playerImage.Width = this.ImageSize;
            this.playerImage.Height = this.ImageSize;
            this.playerImage.Name = name;
            this.playerImage.Source = new BitmapImage(new Uri("ms-appx:///images/Meteor.png"));
        }
        public int StepSize
        { 
            get { return this.stepSize; }
            set { this.stepSize = value; }
        }
    }
}
