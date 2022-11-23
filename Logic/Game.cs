using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Logic
{
    public class Game
    {
        Canvas gameBoard = null;
        DispatcherTimer gameTimer = null;

        Goodie goodie = new Goodie("Goodie");
     
        List <Baddie> baddies = new List <Baddie>();
        List <Coin> coins = new List <Coin>();

        int boardWidth = 0;
        int boardHeight = 0;

        int gameTimerTickMs = 150;
        int baddiesAmount = 10;
        int coinesAmount = 5;
        int coinsScore = 0;
        int minimumDistanceBetweenCoins = 30;
        int minimumDistanceBetweenBaddies = 30;
        int minimumDistanceFromGoodie = 150;
        int goodieStep = 20;
        int aliveBaddies;
        int deadBaddies;

        bool isGameRunning = false;
        bool isGameOver = false;

        TextBlock txtScore = null;
        TextBlock txtClock = null;
        TextBlock txtDeadBaddies = null;
        Button btnGameControl=null;

        int elapsedTime;

        public Game(Canvas gameBoard)
        {
            this.gameBoard = gameBoard;
            this.boardWidth = (int) gameBoard.Width;
            this.boardHeight = (int) gameBoard.Height - 50; // fix for the buttons height in the bottom of the game

            this.SetGoodieController();
            this.InitTimer();
        }

        public bool IsGameRunning
        {
            set { this.isGameRunning = value; }
            get { return this.isGameRunning; }
        }

        public bool IsGameAlreadySarted()
        {
            return baddies.Count > 0;
        }

        public bool IsGameOver
        {
            get { return this.isGameOver; }
            set { this.isGameOver = value; }
        }

        public Button BtnGameControl
        {
            get { return this.btnGameControl; }
            set { this.btnGameControl = value; }
        }

        public TextBlock TxtScore
        {
            get { return this.txtScore; }
            set { this.txtScore = value; }
        }

        public TextBlock TxtClock
        {
            get { return this.txtClock; }
            set { this.txtClock = value; }
        }

        public TextBlock TxtDeadBaddies
        {
            get { return this.txtDeadBaddies; }
            set { this.txtDeadBaddies = value; }
        }

        private int GetRandomXPointOnScreen()
        {
            return this.GetRandomPointOnScreen(true);
        }

        private int GetRandomYPointOnScreen()
        {
            return this.GetRandomPointOnScreen(false);
        }

        private int GetRandomPointOnScreen(bool isXAxis)
        {
            Random random = new Random();
            // random num limit depending on axis
            int randomLimit = isXAxis ? this.boardWidth : this.boardHeight;
            return random.Next(10, randomLimit - 10);
        }

        private void UpdateLocationOnCanvas(Image image, Point point)
        {
            Canvas.SetTop(image, point.Top);
            Canvas.SetLeft(image, point.Left);
        }

        private void UpdatePlayerLocationOnCanvas(GameObject player)
        {
            this.UpdateLocationOnCanvas(player.PlayerImage, player.Location);
        }

        private void UpdateCoinLocationOnCanvas(Coin coin)
        {
            this.UpdateLocationOnCanvas(coin.playerImage, coin.Location);
        }

        private void SetBaddiesOnBoard()
        {
            for (int i = 0; i < baddiesAmount; i++)
            {
                Baddie baddie = new Baddie($"baddie_{i + 1}");

                int randomX = this.GetRandomXPointOnScreen();
                int randomY = this.GetRandomYPointOnScreen();

                bool isLocationValid = false;

                while (!isLocationValid)
                {
                    randomX = this.GetRandomXPointOnScreen();
                    randomY = this.GetRandomYPointOnScreen();

                    // checks for goodie location
                    bool isGoodieLocationValid = (Math.Abs(goodie.Location.Top - randomY) > minimumDistanceFromGoodie
                                               && Math.Abs(goodie.Location.Left - randomX) > minimumDistanceFromGoodie);

                    bool isBaddieLocatioValid = true;

                    // check for no overlapping baddies location
                    foreach (Baddie baddieForLocationCheck in baddies)
                    {
                        isBaddieLocatioValid = (Math.Abs(baddieForLocationCheck.Location.Top - randomY) > minimumDistanceBetweenBaddies
                                             && Math.Abs(baddieForLocationCheck.Location.Left - randomX) > minimumDistanceBetweenBaddies);
                        if (!isBaddieLocatioValid) break;
                    }
                    isLocationValid = isGoodieLocationValid && isBaddieLocatioValid;
                }
                baddie.SetLocation(randomX, randomY);

                int step = i + 1;
                if (i > 5) step = 5;
                baddie.StepSize = step;

                this.gameBoard.Children.Add(baddie.PlayerImage);
                this.UpdatePlayerLocationOnCanvas(baddie);
                this.baddies.Add(baddie);
            }
        }

        private void SetGoodieOnBoard()
        {
            this.goodie = new Goodie("Dino");
            this.goodie.SetLocation(10, 630);
            this.gameBoard.Children.Add(goodie.PlayerImage);
            this.UpdatePlayerLocationOnCanvas(this.goodie);
        }

        private void SetCoinsOnBoard()
        {
            for (int i = 0; i < this.coinesAmount; i++)
            {
                Coin newCoin = new Coin($"Dino-Coin_{i + 1}");

                int randomX = this.GetRandomXPointOnScreen();
                int randomY = this.GetRandomYPointOnScreen();

                bool isLocationValid = false;

                while (!isLocationValid)
                {
                    randomX = this.GetRandomXPointOnScreen();
                    randomY = this.GetRandomYPointOnScreen();

                    // checks for goodie location
                    bool isGoodieLocationValid = (Math.Abs(goodie.Location.Top - randomY) > minimumDistanceFromGoodie
                                               && Math.Abs(goodie.Location.Left - randomX) > minimumDistanceFromGoodie);

                    bool isCoinLocatioValid = true;

                    // check for no overlapping coins location
                    foreach (Coin coin in this.coins)
                    {
                        isCoinLocatioValid = (Math.Abs(coin.Location.Top - randomY) > minimumDistanceBetweenCoins
                                             && Math.Abs(coin.Location.Left - randomX) > minimumDistanceBetweenCoins);
                        if (!isCoinLocatioValid) break;
                    }

                    isLocationValid = isGoodieLocationValid && isCoinLocatioValid;
                }

                newCoin.SetLocation(randomX, randomY);

                this.gameBoard.Children.Add(newCoin.PlayerImage);
                this.UpdateCoinLocationOnCanvas(newCoin);
                this.coins.Add(newCoin);
            }
        }

        private void SetAllObjectsOnBoard()
        {
            this.SetGoodieOnBoard();
            this.SetBaddiesOnBoard();
            this.SetCoinsOnBoard();
        }

        void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {

            if (!this.isGameRunning || this.IsGameOver)
                return;

            // Goodie keyboard moves

            int new_X = this.goodie.Location.Left;
            int new_Y = this.goodie.Location.Top;
            bool shouldMove = false;

            int currentGoodieStep = this.goodieStep;

            switch (e.VirtualKey)
            {
                case VirtualKey.Up:
                    if (new_Y - this.goodieStep < 0)
                        currentGoodieStep = new_Y;

                    new_Y -= currentGoodieStep;
                    shouldMove = true;
                    break;

                case VirtualKey.Down:
                    if (new_Y + this.goodieStep > this.boardHeight - this.goodie.ImageSize)
                        currentGoodieStep = this.boardHeight - new_Y - this.goodie.ImageSize;

                    new_Y += currentGoodieStep;
                    shouldMove = true;
                    break;

                case VirtualKey.Left:
                    if (new_X - this.goodieStep < 0)
                        currentGoodieStep = new_X;
                    new_X -= currentGoodieStep;
                    shouldMove = true;
                    break;

                case VirtualKey.Right:
                    if (new_X + this.goodieStep > this.boardWidth - this.goodie.ImageSize)
                        currentGoodieStep = this.boardWidth - new_X - this.goodie.ImageSize;
                   
                    new_X += currentGoodieStep;
                    shouldMove = true;
                    break;

                case VirtualKey.Space:
                    new_X = GetRandomXPointOnScreen();
                    new_Y = GetRandomYPointOnScreen();
                    shouldMove = true;
                    break;
            }

            if (shouldMove && new_X >= 0 && new_Y >= 0)
            {
                this.goodie.SetLocation(new_X, new_Y);
                this.UpdatePlayerLocationOnCanvas(this.goodie);
                this.CheckGoodieForCollision();
                this.CheckCoinsForCollision();
            }
        }

        private void SetGoodieController()
        {
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void MoveBaddieToGoodie(Baddie baddie)
        {
            // move baddie to the center of the goodie for better animation

            int step_X;
            int step_Y;

            int goodieCenterTop = this.goodie.GetTopCenterLocation();
            int goodieCenterLeft = this.goodie.GetLeftCenterLocation();

            int baddieCenterTop = baddie.GetTopCenterLocation();
            int baddieCenterLeft = baddie.GetLeftCenterLocation();

            if (baddieCenterTop == goodieCenterTop)
                step_Y = 0;

            else if (baddieCenterTop > goodieCenterTop)
                step_Y = -baddie.StepSize;
            else
                step_Y = baddie.StepSize;


            if (baddieCenterLeft == goodieCenterLeft)
                step_X = 0;
            else if (baddieCenterLeft > goodieCenterLeft)
                step_X = -baddie.StepSize;
            else
                step_X = baddie.StepSize;

            int new_X = baddie.Location.Left + step_X;
            int new_Y = baddie.Location.Top + step_Y;

            baddie.SetLocation(new_X, new_Y);
        }

        private bool IsPlayersCollisionOccurred(GameObject objectA, GameObject objectB)
        {
            int objectALeft = objectA.Location.Left;
            int objectARight = objectALeft + objectA.ImageSize;
            int objectATop = objectA.Location.Top;
            int objectABottom = objectATop + objectA.ImageSize;

            int objectBLeft = objectB.Location.Left;
            int objectBRight = objectBLeft + objectB.ImageSize;
            int objectBTop = objectB.Location.Top;
            int objectBBottom = objectBTop + objectB.ImageSize;

            // x axis check!
            bool isXCollision = (
                 // B collison from left side
                 (objectBRight >= objectALeft && objectBRight <= objectARight)
                 // B collison from right side
                 || (objectBLeft >= objectALeft && objectBLeft <= objectARight)

                 || (objectARight >= objectBLeft && objectARight <= objectBRight)
                 // B collison from right side
                 || (objectALeft >= objectBLeft && objectALeft <= objectBRight));


           bool isYCollision = (
                 // B collison from down side
                 (objectBTop <= objectABottom && objectBTop >= objectATop)
                 // B collison from top side
                 || (objectBBottom >= objectATop && objectBBottom <= objectABottom)

                 || (objectATop <= objectBBottom && objectATop >= objectBTop)
                 // B collison from top side
                 || (objectABottom >= objectBTop && objectABottom <= objectBBottom)
             );
            return ( isXCollision && isYCollision);
        }

        private bool CheckGoodieForCollision()
        {
            foreach (Baddie baddieForLocationCheck in baddies)
            {
                if (!baddieForLocationCheck.IsDead)
                {
                    bool isBoundriesCollision = this.IsPlayersCollisionOccurred(this.goodie, baddieForLocationCheck);

                    if (isBoundriesCollision)
                    {
                        this.goodie.IsDead = true;
                        //baddieForLocationCheck.whoAmI();
                        return true;
                    }
                }
            }
            return false;
        }

        private void CheckBaddiesForCollision()
        {
            foreach (Baddie baddie in baddies)
            {
                foreach (Baddie baddieForLocationCheck in baddies)
                {
                    if (!baddie.IsDead && !baddieForLocationCheck.IsDead && baddie.Name != baddieForLocationCheck.Name)
                    {
                        bool isBoundriesCollision = this.IsPlayersCollisionOccurred(baddieForLocationCheck, baddie);

                        if (isBoundriesCollision)
                        {
                            baddie.IsDead = true;
                            this.RemoveObjectImageByName(baddie.PlayerImage.Name);
                        }

                        this.deadBaddies = this.GetDeadBaddiesCount();
                        this.txtDeadBaddies.Text = $"{this.deadBaddies} meteors crashed";
                    }
                }
            }
        }

        private bool CheckCoinsForCollision()
        {
            foreach (Coin coin in coins)
            {
                if (!coin.IsPicked)
                {
                    bool isBoundriesCollision = this.IsPlayersCollisionOccurred(this.goodie, coin);
                    if (isBoundriesCollision)
                    {
                        coin.IsPicked = true;
                        this.coinsScore++;
                        this.RemoveObjectImageByName(coin.PlayerImage.Name);
                        this.txtScore.Text = $"{this.coinsScore} coins collected";
                    }
                }
            }
            return false;
        }

        private void RemoveObjectImageByName(string name)
        {
            var images = this.gameBoard.Children.OfType<Image>().ToList();

            foreach (var image in images)
            {
                if (image.Name == name)
                {
                    this.gameBoard.Children.Remove(image);
                    break;
                }
            }
        }
        
        private int GetDeadBaddiesCount()
        {
            int deadBaddies = 0;
            foreach (Baddie baddie in baddies)
            {
                if (baddie.IsDead)
                    deadBaddies++;
            }
            return deadBaddies;
        }

        private void CheckForGameOver()
        {
            int aliveBaddies = 0;
            foreach (Baddie baddie in baddies)
            {
                if (!baddie.IsDead)
                    aliveBaddies++;
            }

            if (aliveBaddies == 1)
                this.isGameRunning = false;

            if (this.CheckGoodieForCollision())
                this.isGameRunning = false;

            if (!this.isGameRunning)
            {
                this.StopGameTimer();
                this.deadBaddies = this.GetDeadBaddiesCount();
                this.aliveBaddies = aliveBaddies;
                this.GameEndedMessage();
                this.isGameOver = true;
            }
        }

        public void ClearBoard()
        {
            this.baddies.Clear();
            this.coins.Clear();
            this.gameBoard.Children.Clear();
            this.isGameOver = false;
            this.coinsScore = 0;    
            this.txtScore.Text = "0 coins collected";
            this.btnGameControl.Content = "Pause";
            this.goodie.IsDead = false;
        }

        async void GameEndedMessage()
        {
            string message = "";
            if (goodie.IsDead)
            {
                message = "Game over!\n\nA meteorite crashed you into extinction :(";

            }
            else if (aliveBaddies == 1)
                message = "You win!\n\nThe meteorites missed you.\nYou get to live for another day :)";

            // Create a MessageDialog
            var dialog = new MessageDialog($"{message}\n\nMeteorites burned: {this.deadBaddies}\nCoins collected: {this.coinsScore}\n\nDo you want to play again?");

            // If you want to add custom buttons
            dialog.Commands.Add(new UICommand("Yes", delegate (IUICommand command)
            {
                this.StartGame();
                dialog.Commands.Clear();
            }));

            dialog.Commands.Add(new UICommand("No", delegate (IUICommand command)
            {
                Application.Current.Exit();
                dialog.Commands.Clear();
            }));

            // Show dialog and save result
            var result = await dialog.ShowAsync();
        }

        public void StartButtonClick()
        {
            if (this.IsGameRunning)
            {
                this.PauseGame();
                this.btnGameControl.Content = "Start";
            }
            else if (this.IsGameOver)
            {
                this.ClearBoard();
                //this.StartGame();
            }
            else
            {
                if (this.IsGameAlreadySarted())
                {
                    this.IsGameRunning = true;
                    this.StartGameTimer();
                }
                else 
                    this.StartGame();

                this.btnGameControl.Content = "Pause";
            }
        }

        public void ReStartButtonClick()
        {
            if(isGameRunning)
            {
                this.ClearBoard();
                this.StartGame();
            }
        }

        private void InitTimer()
        {
            this.gameTimer = new DispatcherTimer();
            this.gameTimer.Interval = new TimeSpan(0, 0, 0, 0, this.gameTimerTickMs);
            this.gameTimer.Tick += (sender, e) => OnTurnTimerEvent();
        }

        private void StartGameTimer()
        {
            this.elapsedTime = 0;
            this.gameTimer.Start();
        }

        private void StopGameTimer()
        {
            this.gameTimer.Stop();
        }

        public void OnTurnTimerEvent()
        {
            this.elapsedTime += this.gameTimerTickMs;

            foreach (Baddie baddie in baddies)
            {
                if (!baddie.IsDead)
                {
                    this.MoveBaddieToGoodie(baddie);
                    this.UpdatePlayerLocationOnCanvas(baddie);

                    double angle = baddie.getCenterPoint().GetAngleBetweenPoints(this.goodie.getCenterPoint());
                    baddie.PlayerImage.Rotation = (float)angle;
                    Vector3 vector = baddie.PlayerImage.ActualOffset;
                    baddie.SetLocation((int)vector.X, (int)vector.Y);
                }
            }
            CheckBaddiesForCollision();
            CheckForGameOver();

            TimeSpan time = TimeSpan.FromMilliseconds(elapsedTime);
            this.txtClock.Text = time.ToString(@"hh\:mm\:ss\:fff");
        }

        public void StartGame()
        {
            this.isGameRunning = true;
            this.ClearBoard();
            this.SetAllObjectsOnBoard();
            this.StartGameTimer();
        }

        public void PauseGame()
        {
            IsGameRunning = false;
            this.StopGameTimer();
        }
    }
}
