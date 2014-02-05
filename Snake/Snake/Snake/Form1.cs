using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GDIDrawer;

namespace Snake
{
    public enum Direction { Up, Down, Left, Right };

    public struct Snake
    {
        public CDrawer cDrawer;
        public List<SnakeSegment> _snakeBody;
        public Direction direction;

        public Snake(Location location)
        {
            direction = Direction.Right;

            cDrawer = new CDrawer(600, 600);
            cDrawer.Scale = 20;

            _snakeBody = new List<SnakeSegment>();
            SnakeSegment head = new SnakeSegment(location);
            head._color = Color.Yellow;
            _snakeBody.Add(head);
            Lengthen();
            Lengthen();


        }

        public void Draw()
        {
            foreach (SnakeSegment s in _snakeBody)
            {
                cDrawer.AddCenteredEllipse(s._location._x, s._location._y, 1, 1, s._color);
            }

            SnakeSegment head = _snakeBody.ElementAt(0);
            cDrawer.AddCenteredEllipse(head._location._x, head._location._y, 1, 1, head._color);
        }

        public void Lengthen()
        {
            SnakeSegment tail = _snakeBody.ElementAt(_snakeBody.Count - 1);
            SnakeSegment newSegment = new SnakeSegment(tail._location);
            _snakeBody.Add(newSegment);
        }

        public void MoveHead(Location newLocation)
        {
            _snakeBody[0] = _snakeBody[0].Move(newLocation);

            for (int i = 1; i < _snakeBody.Count; ++i)
            {
                _snakeBody[i] = _snakeBody[i].Move(_snakeBody.ElementAt(i - 1)._lastLocation);
            }
        }

    }

    public struct SnakeSegment
    {
        public bool _nulled;
        public Location _location;
        public Location _lastLocation;
        public Color _color;

        public SnakeSegment(Location location)
        {
            _nulled = false;
            _location = location;
            _lastLocation = new Location(location);
            _color = Color.Red;
        }

        public SnakeSegment(bool nulled)
        {
            _nulled = true;
            _location = new Location();
            _lastLocation = new Location();
            _color = Color.Red;
        }

        public SnakeSegment Move(Location newLocation)
        {
            _lastLocation = new Location(_location);
            _location = new Location(newLocation);
            return this;
        }
    }

    public struct Location
    {
        public int _x;
        public int _y;

        public Location(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Location(Location location)
        {
            _x = location._x;
            _y = location._y;
        }
    }

    public struct FoodPellet
    {
        public Location _location;

        public FoodPellet(Random random, Snake snake)
        {
            Location location;
            bool success = false;
            do
            {
                success = true;
                location = new Location(random.Next(0, snake.cDrawer.ScaledWidth), random.Next(0, snake.cDrawer.ScaledHeight));
                foreach (SnakeSegment s in snake._snakeBody)
                    if (s._location._x == location._x && s._location._y == location._y)
                    {
                        success = false;
                        break;
                    }
            } while (!success);

            _location = location;

        }
    }

    public partial class Form1 : Form
    {
        Snake snake;
        Random random = new Random();
        List<FoodPellet> foodPellets = new List<FoodPellet>();

        public Form1()
        {
            InitializeComponent();
            snake = new Snake(new Location(5, 5));
            snake.MoveHead(new Location(snake._snakeBody.ElementAt(0)._location._x + 1, snake._snakeBody.ElementAt(0)._location._y));
            snake.MoveHead(new Location(snake._snakeBody.ElementAt(0)._location._x + 1, snake._snakeBody.ElementAt(0)._location._y));
            AddFoodPellet();
            AddFoodPellet();
            timerSnake.Enabled = true;

            Draw();
        }

        public void AddFoodPellet()
        {
            foodPellets.Add(new FoodPellet(random, snake));
        }

        public void CheckCollision()
        {
            //did the snake eat a food pellet?
            for (int i = foodPellets.Count - 1; i >= 0; --i)
            {
                if (foodPellets[i]._location._x == snake._snakeBody.ElementAt(0)._location._x &&
                    foodPellets[i]._location._y == snake._snakeBody.ElementAt(0)._location._y)
                {
                    //yes, a pellet has been eaten
                    foodPellets.RemoveAt(i);
                    snake.Lengthen();
                    AddFoodPellet();
                    i = -1;
                }
            }

            //did the snake collide with itself?
            SnakeSegment head = snake._snakeBody.ElementAt(0);
            for (int i = 1; i < snake._snakeBody.Count; ++i)
            {
                if (head._location._x == snake._snakeBody.ElementAt(i)._location._x &&
                    head._location._y == snake._snakeBody.ElementAt(i)._location._y)
                {
                    //yes, the snake has collided with itself
                    GameOver();
                    i = snake._snakeBody.Count;
                }
            }

            //did the snake collide with the walls?
            if (head._location._x >= snake.cDrawer.ScaledWidth ||
               head._location._x < 0 ||
               head._location._y >= snake.cDrawer.ScaledHeight ||
               head._location._y < 0)
            {
                GameOver();
            }


        }

        public void GameOver()
        {
            timerSnake.Enabled = false;
            snake.cDrawer.AddText("GAME OVER", 40, Color.Tomato);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    snake.direction = Direction.Right;
                    break;
                case Keys.Left:
                    snake.direction = Direction.Left;
                    break;
                case Keys.Down:
                    snake.direction = Direction.Down;
                    break;
                case Keys.Up:
                    snake.direction = Direction.Up;
                    break;
                case Keys.Space:
                    snake.Lengthen();
                    break;
                case Keys.PageUp:
                    timerSnake.Interval += (timerSnake.Interval >= 1000) ? 0 : 25;
                    break;
                case Keys.PageDown:
                    timerSnake.Interval -= (timerSnake.Interval <= 25) ? 0 : 25;
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
            }

        }

        public void Draw()
        {
            snake.cDrawer.Clear();
            foreach (FoodPellet fp in foodPellets)
                snake.cDrawer.AddCenteredEllipse(fp._location._x, fp._location._y, 1, 1, Color.Wheat);
            snake.Draw();
        }

        private void timerSnake_Tick(object sender, EventArgs e)
        {
            switch (snake.direction)
            {
                case Direction.Right:
                    snake.MoveHead(new Location(snake._snakeBody.ElementAt(0)._location._x + 1, snake._snakeBody.ElementAt(0)._location._y));
                    break;
                case Direction.Left:
                    snake.MoveHead(new Location(snake._snakeBody.ElementAt(0)._location._x - 1, snake._snakeBody.ElementAt(0)._location._y));
                    break;
                case Direction.Down:
                    snake.MoveHead(new Location(snake._snakeBody.ElementAt(0)._location._x, snake._snakeBody.ElementAt(0)._location._y + 1));
                    break;
                case Direction.Up:
                    snake.MoveHead(new Location(snake._snakeBody.ElementAt(0)._location._x, snake._snakeBody.ElementAt(0)._location._y - 1));
                    break;
            }

            Draw();

            CheckCollision();
        }
    }
}
