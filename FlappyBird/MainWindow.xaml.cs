using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System;

using System.Windows.Threading;

namespace FlappyBird
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();

        double score;
		double velocity = 0;
		double gravity = 0.5;
		bool gameOver;
        Rect falappybirdHitbox;


		public MainWindow()
        {
            InitializeComponent();

			gameTimer.Tick += Game;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

		private void Game(object sender, EventArgs e)
        {
			Canvas.SetTop(madar, 190);
			return;

			falappybirdHitbox = new Rect(
			   Canvas.GetLeft(madar),
			   Canvas.GetTop(madar),
			   madar.Width,
			   madar.Height);

			velocity += gravity;
			Canvas.SetTop(madar, Canvas.GetTop(madar) + velocity);

			foreach (var x in MyCanvas.Children.OfType<Image>())
			{
				if ((string)x.Tag == "obs1")
				{
					Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

					Rect pipeHitBox = new Rect(
						Canvas.GetLeft(x),
						Canvas.GetTop(x),
						x.Width,
						x.Height);

					if (falappybirdHitbox.IntersectsWith(pipeHitBox))
					{
						EndGame();
					}

					if (Canvas.GetLeft(x) < -100)
					{
						Canvas.SetLeft(x, 800);
						score++;
						lbl_Score.Content = "Score: " + score;
					}
				}
			}

			if (Canvas.GetTop(madar) < 0 || Canvas.GetTop(madar) > 450)
			{
				EndGame();
			}
		}

		private void KeyIsDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space && !gameOver)
			{
				//gravity = -8;
			}

			if (e.Key == Key.R && gameOver)
			{
				StartGame();
			}
		}

		private void KeyIsUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space)
			{
				gravity = 8;
			}
		}

        private void StartGame()
        {
			Random rnd = new Random();
			int pipeX = 600;

			MyCanvas.Focus();
			score = 0;
			gameOver = false;
			lbl_Score.Content = "Score: 0";

			Canvas.SetTop(madar, 190);
			Canvas.SetLeft(madar, 50);

			foreach (var x in MyCanvas.Children.OfType<Image>())
			{
				if ((string)x.Tag == "obs1")
				{
					Canvas.SetLeft(x, pipeX);
					pipeX += 200;
				}
			}

			gameTimer.Start();
		}

        private void EndGame()
        {
			gameTimer.Stop();
			gameOver = true;
			lbl_Score.Content += "  GAME OVER (R)";
		}
        

	}
}