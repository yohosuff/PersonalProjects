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

    struct Portal
    {
        public Location _A;
        public Location _B;
        public bool teleported;
        
        public Portal(Location A, Location B)
        {
            _A = A;
            _B = B;
            teleported = false;
        }
    }

    public struct Snake
    {
        public List<SnakeSegment> _snakeBody;
        public Direction _direction;
        public CDrawer _cDrawer;

        public Snake(Location location, CDrawer cDrawer)
        {
            _direction = Direction.Right;
            _cDrawer = cDrawer;

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
                _cDrawer.AddCenteredEllipse(s._location._x, s._location._y, 1, 1, s._color);
            }

            SnakeSegment head = _snakeBody.ElementAt(0);
            _cDrawer.AddCenteredEllipse(head._location._x, head._location._y, 1, 1, head._color);
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

        public FoodPellet(Random random, Snake snake, List<FoodPellet> foodPellets)
        {
            Location location;
            bool success = false;
            do
            {
                success = true;
                location = new Location(random.Next(0, snake._cDrawer.ScaledWidth), random.Next(0, snake._cDrawer.ScaledHeight));
                foreach (SnakeSegment s in snake._snakeBody)
                    if (s._location._x == location._x && s._location._y == location._y)
                    {
                        success = false;
                        break;
                    }

                foreach(FoodPellet fp in foodPellets)
                    if (fp._location._x == location._x && fp._location._y == location._y)
                    {
                        success = false;
                        break;
                    }
            } while (!success);

            _location = location;

        }
    }

    public partial class mainForm : Form
    {
        Snake _snake;
        Random _random = new Random();
        List<FoodPellet> _foodPellets = new List<FoodPellet>();
        public CDrawer _cDrawer;
        bool gameOver = false;
        List<Portal> portals = new List<Portal>();

        public mainForm()
        {
            InitializeComponent();

            portals.Add(new Portal(new Location(0, 0), new Location(15, 15)));
            portals.Add(new Portal(new Location(5, 5), new Location(25, 25)));

            _cDrawer = new CDrawer(600, 600);
            _cDrawer.Scale = 20;

            Reset();
            Draw();
        }

        public void AddFoodPellet()
        {
            _foodPellets.Add(new FoodPellet(_random, _snake, _foodPellets));
        }

        public void CheckCollision()
        {
            SnakeSegment head = _snake._snakeBody.ElementAt(0);
            
            //did the snake enter a portal?
            for (int i = 0; i < portals.Count; ++i)
            {
                if (head._lastLocation._x == portals[i]._A._x && head._lastLocation._y == portals[i]._A._y)
                {
                    if (!portals[i].teleported)
                    {
                        _snake._snakeBody[0] = _snake._snakeBody[0].Move(portals[i]._B);
                        Portal portal = new Portal(portals[i]._A, portals[i]._B);
                        portal.teleported = true;
                        portals[i] = portal;
                        return;
                    }

                }
                else if (head._lastLocation._x == portals[i]._B._x && head._lastLocation._y == portals[i]._B._y)
                {
                    if (!portals[i].teleported)
                    {
                        _snake._snakeBody[0] = _snake._snakeBody[0].Move(portals[i]._A);
                        Portal portal = new Portal(portals[i]._A, portals[i]._B);
                        portal.teleported = true;
                        portals[i] = portal;
                        return;
                    }
                }

                portals[i] = new Portal(portals[i]._A, portals[i]._B);
            }

            
            //did the snake eat a food pellet?
            for (int i = _foodPellets.Count - 1; i >= 0; --i)
            {
                if (_foodPellets[i]._location._x == _snake._snakeBody.ElementAt(0)._location._x &&
                    _foodPellets[i]._location._y == _snake._snakeBody.ElementAt(0)._location._y)
                {
                    //yes, a pellet has been eaten
                    _foodPellets.RemoveAt(i);
                    _snake.Lengthen();
                    AddFoodPellet();
                    i = -1;
                }
            }

            //did the snake collide with itself?
            for (int i = 1; i < _snake._snakeBody.Count; ++i)
            {
                if (head._location._x == _snake._snakeBody.ElementAt(i)._location._x &&
                    head._location._y == _snake._snakeBody.ElementAt(i)._location._y)
                {
                    //yes, the snake has collided with itself
                    gameOver = true;
                    return;
                }
            }

            //did the snake collide with the walls?
            if (head._location._x >= _snake._cDrawer.ScaledWidth ||
               head._location._x < 0 ||
               head._location._y >= _snake._cDrawer.ScaledHeight ||
               head._location._y < 0)
            {
                gameOver = true;
                return;
            }

        }

        public void GameOver()
        {
            timerSnake.Enabled = false;
            _cDrawer.AddText("GAME OVER\nScore: " + _snake._snakeBody.Count + "\n\nPress <Enter> to\nplay again.", 40, Color.Tomato);

        }

        public bool SnakeHeadWasJustHere(Location location)
        {
            SnakeSegment neck = _snake._snakeBody.ElementAt(1);
            if (neck._location._x == location._x && neck._location._y == location._y)
                return true;
            
            return false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           SnakeSegment head = _snake._snakeBody.ElementAt(0);

            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (!SnakeHeadWasJustHere(new Location(head._location._x + 1, head._location._y)))
                        _snake._direction = Direction.Right;
                    break;
                case Keys.Left:
                    if (!SnakeHeadWasJustHere(new Location(head._location._x - 1, head._location._y)))
                        _snake._direction = Direction.Left;
                    break;
                case Keys.Down:
                    if (!SnakeHeadWasJustHere(new Location(head._location._x, head._location._y + 1)))
                        _snake._direction = Direction.Down;
                    break;
                case Keys.Up:
                    if (!SnakeHeadWasJustHere(new Location(head._location._x, head._location._y - 1)))
                        _snake._direction = Direction.Up;
                    break;
                case Keys.Space:
                    _snake.Lengthen();
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
                case Keys.Enter:
                    if (gameOver)
                        Reset();
                    break;
            }

        }

        public void Reset()
        {
            gameOver = false;
            _foodPellets.Clear();
            _snake = new Snake(new Location(5, 5), _cDrawer);
            _snake.MoveHead(new Location(_snake._snakeBody.ElementAt(0)._location._x + 1, _snake._snakeBody.ElementAt(0)._location._y));
            _snake.MoveHead(new Location(_snake._snakeBody.ElementAt(0)._location._x + 1, _snake._snakeBody.ElementAt(0)._location._y));
            AddFoodPellet();
            AddFoodPellet();
            timerSnake.Enabled = true;
        }

        public void Draw()
        {
            _snake._cDrawer.Clear();

            foreach (FoodPellet fp in _foodPellets)
                _snake._cDrawer.AddCenteredEllipse(fp._location._x, fp._location._y, 1, 1, Color.Wheat);

            foreach (Portal portal in portals)
            {
                _snake._cDrawer.AddCenteredRectangle(portal._A._x, portal._A._y, 1, 1, Color.Blue);
                _snake._cDrawer.AddCenteredRectangle(portal._B._x, portal._B._y, 1, 1, Color.Blue);
            }
            _snake.Draw();
        }

        public void CheckGameOver()
        {
            if (gameOver)
                GameOver();
        }

        private void timerSnake_Tick(object sender, EventArgs e)
        {
            switch (_snake._direction)
            {
                case Direction.Right:
                    _snake.MoveHead(new Location(_snake._snakeBody.ElementAt(0)._location._x + 1, _snake._snakeBody.ElementAt(0)._location._y));
                    break;
                case Direction.Left:
                    _snake.MoveHead(new Location(_snake._snakeBody.ElementAt(0)._location._x - 1, _snake._snakeBody.ElementAt(0)._location._y));
                    break;
                case Direction.Down:
                    _snake.MoveHead(new Location(_snake._snakeBody.ElementAt(0)._location._x, _snake._snakeBody.ElementAt(0)._location._y + 1));
                    break;
                case Direction.Up:
                    _snake.MoveHead(new Location(_snake._snakeBody.ElementAt(0)._location._x, _snake._snakeBody.ElementAt(0)._location._y - 1));
                    break;
            }

            CheckCollision();
            if (!gameOver)
                Draw();
            else
                GameOver();
            
        }
    }
}
