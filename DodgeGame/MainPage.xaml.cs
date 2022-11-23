using Logic;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Timers;
using System.ServiceModel.Channels;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DodgeGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public  partial class MainPage : Page
    {
        public Baddie baddies;
        public Game game;

        public MainPage()
        {
            this.InitializeComponent();

            this.game = new Game(gameBoard);

            this.game.TxtScore = txtScore;
            this.game.BtnGameControl = btnStart;
            this.game.TxtClock = txtClock;
            this.game.TxtDeadBaddies = txtDeadBaddies;

            ApplicationView.PreferredLaunchViewSize = new Size(gameBoard.Width, gameBoard.Height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.game.StartButtonClick();
            //dummy button for pulling the focus away from the start button
            //this will allow the goodie to use the space key to move to a random loc.
            //without triggering the start button.
            dummyButton.Focus(FocusState.Programmatic);
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            this.game.ReStartButtonClick();
            dummyButton.Focus(FocusState.Programmatic);
        }
    }
}
