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
    struct Snake
    {
        public CDrawer cDrawer;
        public List<SnakeSegment> _snakeBody;

        public Snake(Location location)
        {
            cDrawer = new CDrawer(800, 800);
            cDrawer.Scale = 20;

            _snakeBody = new List<SnakeSegment>();
            _snakeBody.Add(new SnakeSegment(location));
            Lengthen();
            Lengthen();
        }

        public void Draw()
        {
            cDrawer.Clear();
            foreach(SnakeSegment s in _snakeBody)
            {
                cDrawer.AddCenteredEllipse(s._location._x, s._location._y, 1, 1, Color.Red);
            }
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

    struct SnakeSegment
    {
        public bool _nulled;
        public Location _location;
        public Location _lastLocation;
        
        public SnakeSegment(Location location)
        {
            _nulled = false;
            _location = location;
            _lastLocation = new Location(location);
        }

        public SnakeSegment(bool nulled)
        {
            _nulled = true;
            _location = new Location();
            _lastLocation = new Location();
        }

        public SnakeSegment Move(Location newLocation)
        {
            _lastLocation = new Location(_location);
            _location = new Location(newLocation);
            return this;
        }
    }

    struct Location
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
    
    public partial class Form1 : Form
    {
        Snake snake;
        
        public Form1()
        {
            InitializeComponent();
            snake = new Snake(new Location(5,5));
            snake.Draw();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    snake.MoveHead(new Location(snake._snakeBody.ElementAt(0)._location._x + 1, snake._snakeBody.ElementAt(0)._location._y));
                    break;
                case Keys.Left:
                    snake.MoveHead(new Location(snake._snakeBody.ElementAt(0)._location._x - 1, snake._snakeBody.ElementAt(0)._location._y));
                    break;
                case Keys.Down:
                    snake.MoveHead(new Location(snake._snakeBody.ElementAt(0)._location._x, snake._snakeBody.ElementAt(0)._location._y + 1));
                    break;
                case Keys.Up:
                    snake.MoveHead(new Location(snake._snakeBody.ElementAt(0)._location._x, snake._snakeBody.ElementAt(0)._location._y - 1));
                    break;
                case Keys.Space:
                    snake.Lengthen();
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
            }

            snake.Draw();


        }
    }
}
