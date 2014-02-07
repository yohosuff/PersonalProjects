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
    public partial class mainForm : Form
    {
        Snake _snake;
        Random _random = new Random();
        List<FoodPellet> _foodPellets = new List<FoodPellet>();
        public CDrawer _cDrawer;
        bool _gameOver = false;
        List<Portal> _portals = new List<Portal>();
        bool _paused = false;

        public mainForm()
        {
            InitializeComponent();

            _cDrawer = new CDrawer(620, 620);
            _cDrawer.Scale = 20;
            int size = _cDrawer.ScaledWidth;
            _portals.Add(new Portal(new Location(0, size / 2), new Location(size - 1, size / 2)));
            _portals.Add(new Portal(new Location(size / 2, 0), new Location(size / 2, size - 1)));
            _portals.Add(new Portal(new Location(0, 0), new Location(size - 1, size - 1)));
            _portals.Add(new Portal(new Location(size - 1, 0), new Location(0, size - 1)));
            
            Reset();
            Draw();
        }

        public void AddFoodPellet(int amount = 1)
        {
            for (int i = 0; i < amount; ++i)
                if (_foodPellets.Count < _cDrawer.ScaledWidth * _cDrawer.ScaledHeight - _snake._snakeBody.Count - _portals.Count * 2)
                    _foodPellets.Add(new FoodPellet(_random, _snake, _foodPellets, _portals));
        }

        public void CheckCollision()
        {
            SnakeSegment head = _snake._snakeBody.ElementAt(0);

            //did the snake enter a portal?
            for (int i = 0; i < _portals.Count; ++i)
            {
                if (head._lastLocation._x == _portals[i]._A._x && head._lastLocation._y == _portals[i]._A._y)
                {
                    if (!_portals[i].teleported)
                    {
                        _snake._snakeBody[0] = _snake._snakeBody[0].Move(_portals[i]._B);
                        Portal portal = new Portal(_portals[i]._A, _portals[i]._B);
                        portal.teleported = true;
                        _portals[i] = portal;
                        return;
                    }

                }
                else if (head._lastLocation._x == _portals[i]._B._x && head._lastLocation._y == _portals[i]._B._y)
                {
                    if (!_portals[i].teleported)
                    {
                        _snake._snakeBody[0] = _snake._snakeBody[0].Move(_portals[i]._A);
                        Portal portal = new Portal(_portals[i]._A, _portals[i]._B);
                        portal.teleported = true;
                        _portals[i] = portal;
                        return;
                    }
                }

                _portals[i] = new Portal(_portals[i]._A, _portals[i]._B);
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
                    _gameOver = true;
                    return;
                }
            }

            //did the snake collide with the walls?
            if (head._location._x >= _snake._cDrawer.ScaledWidth ||
               head._location._x < 0 ||
               head._location._y >= _snake._cDrawer.ScaledHeight ||
               head._location._y < 0)
            {
                _gameOver = true;
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

        private void mainForm_KeyDown(object sender, KeyEventArgs e)
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
                    if (_gameOver)
                        Reset();
                    break;
                case Keys.P:
                    if (_paused)
                    {
                        _paused = false;
                        timerSnake.Enabled = true;
                    }
                    else
                    {
                        _paused = true;
                        timerSnake.Enabled = false;
                        _cDrawer.AddText("Paused", 40, Color.Tomato);
                    }
                    break;
                case Keys.F:
                    if (e.Modifiers == Keys.Shift)
                        AddFoodPellet(1000);
                    else
                        AddFoodPellet();
                    break;

            }

        }

        public void Reset()
        {
            _gameOver = false;
            _foodPellets.Clear();
            _snake = new Snake(new Location(5, 5), _cDrawer);
            _snake.MoveHead(new Location(_snake._snakeBody.ElementAt(0)._location._x + 1, _snake._snakeBody.ElementAt(0)._location._y));
            _snake.MoveHead(new Location(_snake._snakeBody.ElementAt(0)._location._x + 1, _snake._snakeBody.ElementAt(0)._location._y));
            AddFoodPellet(2);
            timerSnake.Enabled = true;
        }

        public void Draw()
        {
            _snake._cDrawer.Clear();

            
            foreach (Portal portal in _portals)
            {
                _snake._cDrawer.AddCenteredRectangle(portal._A._x, portal._A._y, 1, 1, Color.Blue);
                _snake._cDrawer.AddCenteredRectangle(portal._B._x, portal._B._y, 1, 1, Color.Blue);
            }

            _snake.Draw();

            foreach (FoodPellet fp in _foodPellets)
                _snake._cDrawer.AddCenteredEllipse(fp._location._x, fp._location._y, 1, 1, Color.Wheat);

        }

        public void CheckGameOver()
        {
            if (_gameOver)
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
            if (!_gameOver)
                Draw();
            else
                GameOver();

        }
    }
}
