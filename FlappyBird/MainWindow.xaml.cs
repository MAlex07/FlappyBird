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

        double jumpForce;
        double gravity;
        double pipeSpeed;
        Random rnd = new Random();
        int pipeGap = 150;
        int pipeSpawnX = 800;

        bool inMenu = true;


        DispatcherTimer gameTimer = new DispatcherTimer();

        double score;
		double velocity = 0;
        bool gameOver;
        Rect falappybirdHitbox;


		public MainWindow()
        {
            InitializeComponent();

			gameTimer.Tick += Game;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            jumpForce = -6;
            gravity = 0.5;
            pipeSpeed = 5;

            StartFromMenu();
        }

        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            jumpForce = -5;
            gravity = 0.65;
            pipeSpeed = 7;

            StartFromMenu();
        }

        private void StartFromMenu()
        {
            MenuPanel.Visibility = Visibility.Collapsed;
            inMenu = false;
            StartGame();
        }

        private void Game(object sender, EventArgs e)
        {


			if (gameOver) return;

			falappybirdHitbox = new Rect(
				Canvas.GetLeft(madar),
				Canvas.GetTop(madar),
				madar.Width,
				madar.Height);

			velocity += gravity;
			Canvas.SetTop(madar, Canvas.GetTop(madar) + velocity);

            var pipes = MyCanvas.Children
                .OfType<Image>()
                .Where(x => x.Tag != null &&
                       ((string)x.Tag == "pipeTop" || (string)x.Tag == "pipeBottom"))
                .ToList();

            for (int i = 0; i + 1 < pipes.Count; i += 2)
            {
                Image topPipe = pipes[i];
                Image bottomPipe = pipes[i + 1];

                Canvas.SetLeft(topPipe, Canvas.GetLeft(topPipe) - pipeSpeed);
                Canvas.SetLeft(bottomPipe, Canvas.GetLeft(bottomPipe) - pipeSpeed);

                
                if (falappybirdHitbox.IntersectsWith(
                        new Rect(Canvas.GetLeft(topPipe), Canvas.GetTop(topPipe),
                                 topPipe.Width, topPipe.Height)) ||
                    falappybirdHitbox.IntersectsWith(
                        new Rect(Canvas.GetLeft(bottomPipe), Canvas.GetTop(bottomPipe),
                                 bottomPipe.Width, bottomPipe.Height)))
                {
                    EndGame();
                    return;
                }

                
                if (Canvas.GetLeft(topPipe) < -100)
                {
                    int gapY = rnd.Next(200, 350);

                    Canvas.SetLeft(topPipe, pipeSpawnX);
                    Canvas.SetTop(topPipe, gapY - pipeGap / 2 - topPipe.Height);

                    Canvas.SetLeft(bottomPipe, pipeSpawnX);
                    Canvas.SetTop(bottomPipe, gapY + pipeGap / 2);

                    topPipe.Name = topPipe.Name.Replace("_scored", "");
                    bottomPipe.Name = bottomPipe.Name.Replace("_scored", "");
                }

                
                if (!bottomPipe.Name.Contains("_scored") &&
                    Canvas.GetLeft(bottomPipe) + bottomPipe.Width <
                    Canvas.GetLeft(madar))
                {
                    score++;
                    lbl_Score.Content = "Score: " + score;
                    bottomPipe.Name += "_scored";
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
                velocity = jumpForce;
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
				gravity = 0.8;
			}
		}

        private void StartGame()
        {
            if (inMenu) return;

            foreach (var x in MyCanvas.Children.OfType<Image>())
			{
				x.Visibility = Visibility.Visible;
			}

			Random rnd = new Random();
			int pipeX = 600;
			int gap = 150;

			var pipes = MyCanvas.Children
			.OfType<Image>()
			.Where(x => (string)x.Tag == "pipeTop" || (string)x.Tag == "pipeBottom")
			.ToList();

			MyCanvas.Focus();

			
			score = 0;
			gameOver = false;
			velocity = 0;


			lbl_Score.Content = "Score: 0";

			
			Canvas.SetTop(madar, 190);
			Canvas.SetLeft(madar, 50);


			for (int i = 0; i < pipes.Count; i += 2)
			{
				Image topPipe = pipes[i];
				Image bottomPipe = pipes[i + 1];

				int gapY = rnd.Next(200, 350);   

				Canvas.SetLeft(topPipe, pipeX);
				Canvas.SetTop(topPipe, gapY - gap / 2 - topPipe.Height);

				Canvas.SetLeft(bottomPipe, pipeX);
				Canvas.SetTop(bottomPipe, gapY + gap / 2);

				pipeX += 250;
			}




			gameTimer.Stop();
			gameTimer.Start();
		}

        private void EndGame()
        {
			gameTimer.Stop();
			gameOver = true;
			lbl_Score.Content = "Score: " + score + "  GAME OVER (R)";

            MenuPanel.Visibility = Visibility.Visible;
            inMenu = true;
        }
        

	}
}