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
using System.Windows.Shapes;
using System.Windows.Threading; // for the timer

namespace MANKIE_MUNCH
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {

        //make a game timer
        DispatcherTimer gameTimer = new DispatcherTimer();
        // move left and move right boolean decleration
        bool moveLeft, moveRight;
        // make a new items remove list
        List<Rectangle> itemstoremove = new List<Rectangle>();
        // make a new random class to generate random numbers from
        Random rand = new Random();


        int fallingObjectSpriteCounter; // int to help change fallingObject images
        int fallingObjectCounter = 100; // fallingObject spawn time
        int monkeySpeed = 10; // monkey movement speed
        int limit = 50; // limit of fallingObject spawns
        int score = 0; // default score
        int damage = 0; // default damage

        Rect monkeyHitBox; // monkey hit box to check for collision against fallingObject

        public Game()
        {
            InitializeComponent();

            DispatcherTimer dispatcherTimer = new DispatcherTimer(); // adding the timer to the form
            dispatcherTimer.Tick += Timer_Tick; // linking the timer event
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20); // running the timer every 20 milliseconds
            dispatcherTimer.Start(); // starting the timer

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            // link the game engine event to the timer
            gameTimer.Tick += gameEngine;
            // start the timer
            gameTimer.Start();
            // make my canvas focus of this game
            MyCanvas.Focus();

        }

        int speed = 10; // declaring an integer called speed with value of 10

        bool goUp; // this is the go up boolean
        bool goDown; // this is the go down boolean
        bool goLeft; // this is the go left boolean
        bool goRight; // this is the go right boolean
   
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
                // if go up is true and monkey is within the boundary from the top 
                // then we can use the set top to move the monkey towards top of the screen
            }
            if (goDown && Canvas.GetTop(monkey) + (monkey.Height * 2) < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(monkey, Canvas.GetTop(monkey) + speed);
                // if go down is true and monkey is within the boundary from the bottom of the screen
                // then we can set top of monkey to move down
            }
            if (goLeft && Canvas.GetLeft(monkey) > 0)
            {
                Canvas.SetLeft(monkey, Canvas.GetLeft(monkey) - speed);
                // if go left is true and monkey is inside the boundary from the left
                // then we can set left of the monkey to move towards left of the screen
            }
            if (goRight && Canvas.GetLeft(monkey) + (monkey.Width * 2) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(monkey, Canvas.GetLeft(monkey) + speed);
                // if go right is true and monkey is inside the boundary from the right
                // then we can set left of the monkey to move towards right of the screen
            }
        }
        private void makeEnemies()
        {
            // this function will make the enemies for us including assignning them images

            ImageBrush fallingObjectSprite = new ImageBrush(); // make a new image brush called fallingObject sprite

            fallingObjectSprite.ImageSource = new BitmapImage(new Uri("fallingObject.png", UriKind.Relative));

            fallingObjectSpriteCounter = rand.Next(1, 3); // generate a random number inside the fallingObject sprite counter integer

            // below switch statement will check what number is generated inside the fallingObject sprite counter
            // and then assign a new image to the fallingObject sprite image brush depending on the number

            Rectangle fallingObject = new Rectangle
            {
                Tag = "fallingObject",
                Height = 100,
                Width = 60,
                Fill = System.Windows.Media.Brushes.AliceBlue
            };

            switch (fallingObjectSpriteCounter)
            {
                case 1:
                    fallingObject.Fill = System.Windows.Media.Brushes.Gold;
                    break;
                case 2:
                    fallingObject.Fill = System.Windows.Media.Brushes.Black;
                    break;
                default:
                    fallingObject.Fill = System.Windows.Media.Brushes.Gold;
                    break;
            }

           

            // make a new rectangle called new fallingObject
            // this rectangle has a fallingObject tag, height 50 and width 56 pixels
            // background fill is assigned to the randomly generated fallingObject sprite from the switch statement above


            Canvas.SetTop(fallingObject, -100); // set the top position of the fallingObject to -100
            // randomly generate the left position of the fallingObject
            Canvas.SetLeft(fallingObject, rand.Next(30, 430));
            // add the fallingObject object to the screen
            MyCanvas.Children.Add(fallingObject);

            // garbage collection
            GC.Collect(); // collect any unused resources for this game
        }

        private void gameEngine(object sender, EventArgs e)
        {
            // link the monkey hit box to the monkey rectangle
            monkeyHitBox = new Rect(Canvas.GetLeft(monkey), Canvas.GetTop(monkey), monkey.Width, monkey.Height);

            // reduce one from the fallingObject counter integer
            fallingObjectCounter--;

            scoreText.Text = "Score: " + score; // link the score text to score integer
            damageText.Text = "Damaged " + damage; // link the damage text to damage integer

            // if fallingObject counter is less than 0
            if (fallingObjectCounter < 0)
            {
                makeEnemies(); // run the make enemies function
                fallingObjectCounter = limit; //reset the fallingObject counter to the limit integer
            }

            // monkey movement begins

            if (moveLeft && Canvas.GetLeft(monkey) > 0)
            {
                // if move left is true AND monkey is inside the boundary then move monkey to the left
                Canvas.SetLeft(monkey, Canvas.GetLeft(monkey) - monkeySpeed);
            }
            if (moveRight && Canvas.GetLeft(monkey) + 90 < Application.Current.MainWindow.Width)
            {
                // if move right is true AND monkey left + 90 is less than the width of the form
                // then move the monkey to the right
                Canvas.SetLeft(monkey, Canvas.GetLeft(monkey) + monkeySpeed);
            }

            // monkey movement ends

            // search for bullets, enemies and collision begins

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                // if any rectangle has the tag bullet in it
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    // move the bullet rectangle towards top of the screen
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    // make a rect class with the bullet rectangles properties
                    Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // check if bullet has reached top part of the screen
                    if (Canvas.GetTop(x) < 10)
                    {
                        // if it has then add it to the item to remove list
                        itemstoremove.Add(x);
                    }

                    // run another for each loop inside of the main loop this one has a local variable called y
                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        // if y is a rectangle and it has a tag called fallingObject
                        if (y is Rectangle && (string)y.Tag == "fallingObject")
                        {
                            // make a local rect called fallingObject and put the enemies properties into it
                            Rect fallingObject = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);


                            // now check if bullet and fallingObject is colliding or not
                            // if the bullet is colliding with the fallingObject rectangle
                            if (bullet.IntersectsWith(fallingObject))
                            {

                                itemstoremove.Add(x); // remove bullet
                                itemstoremove.Add(y); // remove fallingObject
                                score++; // add one to the score
                            }
                        }

                    }
                }

                // outside the second loop lets check for the fallingObject again
                if (x is Rectangle && (string)x.Tag == "fallingObject")
                {
                    // if we find a rectangle with the fallingObject tag

                    Canvas.SetTop(x, Canvas.GetTop(x) + 10); // move the fallingObject downwards

                    // make a new fallingObject rect for fallingObject hit box
                    Rect fallingObject = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // first check if the fallingObject object has gone passed the monkey meaning
                    // its gone passed 700 pixels from the top
                    if (Canvas.GetTop(x) + 150 > 700)
                    {
                        // if so first remove the fallingObject object
                        itemstoremove.Add(x);
                        damage += 10; // add 10 to the damage
                    }

                    // if the monkey hit box and the fallingObject is colliding 
                    if (monkeyHitBox.IntersectsWith(fallingObject))
                    {
                        damage += 5; // add 5 to the damage
                        itemstoremove.Add(x); // remove the fallingObject object
                    }
                }


            }

            // search for bullets, enemies and collision ENDs

            // if the score is greater than 5
            if (score > 5)
            {
                limit = 20; // reduce the limit to 20
                // now the enemies will spawn faster
            }

            // if the damage integer is greater than 99
            if (damage > 99)
            {
                gameTimer.Stop(); // stop the main timer
               // damageText.Content = "Damaged: 100"; // show this on the damaged text
                //damageText.Foreground = Brushes.Red; // change the text colour to 100
                MessageBox.Show("Well Done Star Captain!" + Environment.NewLine + "You have destroyed " + score + " Alien ships");
                // show the message box with the message inside of it
            }

            // removing the rectangles

            // check how many rectangles are inside of the item to remove list
            foreach (Rectangle y in itemstoremove)
            {
                // remove them permanently from the canvas
                MyCanvas.Children.Remove(y);
            }


        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            // if the left key is pressed
            // set move left to true


            // if the right key is pressed
            // set move right to true

            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

     
    }
}
