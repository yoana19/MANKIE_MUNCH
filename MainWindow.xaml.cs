using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading; // for the timer


namespace MANKIE_MUNCH
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int speed = 10; // declaring an integer called speed with value of 10

        bool goUp; // this is the go up boolean
        bool goDown; // this is the go down boolean
        bool goLeft; // this is the go left boolean
        bool goRight; // this is the go right boolean
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer dispatcherTimer = new DispatcherTimer(); // adding the timer to the form
            dispatcherTimer.Tick += Timer_Tick; // linking the timer event
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20); // running the timer every 20 milliseconds
            dispatcherTimer.Start(); // starting the timer
        }




        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Title.Visibility = Visibility.Hidden;
            monkeyImg.Visibility = Visibility.Hidden;
            fireImg.Visibility = Visibility.Hidden;
            startBtn.Visibility = Visibility.Hidden;
            
         

            bananaBackground.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "gameBackground.jpg"));

            this.Background = bananaBackground;

            MyCanvas.Visibility = Visibility.Visible;
           


        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                goDown = true; // down key is pressed go down will be true
            }
            else if (e.Key == Key.Up)
            {
                goUp = true; // up key is pressed go up will be true
            }
            else if (e.Key == Key.Left)
            {
                goLeft = true; // left key is pressed go left will be true
            }
            else if (e.Key == Key.Right)
            {
                goRight = true; // right key is pressed go right will be true
            }
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                goDown = false; // down is released go down will be false
            }
            else if (e.Key == Key.Up)
            {
                goUp = false; // up key is released go up will be false
            }
            else if (e.Key == Key.Left)
            {
                goLeft = false; // left key is released go left will be false
            }
            else if (e.Key == Key.Right)
            {
                goRight = false; // right key is released go right will be false
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (goUp && Canvas.GetTop(monkey) > 0)
            {
                Canvas.SetTop(monkey, Canvas.GetTop(monkey) - speed);
                // if go up is true and player is within the boundary from the top 
                // then we can use the set top to move the monkey towards top of the screen
            }
            if (goDown && Canvas.GetTop(monkey) + (monkey.Height * 2) < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(monkey, Canvas.GetTop(monkey) + speed);
                // if go down is true and player is within the boundary from the bottom of the screen
                // then we can set top of monkey to move down
            }
            if (goLeft && Canvas.GetLeft(monkey) > 0)
            {
                Canvas.SetLeft(monkey, Canvas.GetLeft(monkey) - speed);
                // if go left is true and player is inside the boundary from the left
                // then we can set left of the player to move towards left of the screen
            }
            if (goRight && Canvas.GetLeft(monkey) + (monkey.Width * 2) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(monkey, Canvas.GetLeft(monkey) + speed);
                // if go right is true and player is inside the boundary from the right
                // then we can set left of the player to move towards right of the screen
            }
        }



        }
}

