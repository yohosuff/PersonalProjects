using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GDIDrawer;

namespace Snake
{
    public enum Direction { Up, Down, Left, Right };

    public struct Portal
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

        public FoodPellet(Random random, Snake snake, List<FoodPellet> foodPellets, List<Portal> portals)
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

                foreach (FoodPellet fp in foodPellets)
                    if (fp._location._x == location._x && fp._location._y == location._y)
                    {
                        success = false;
                        break;
                    }

                foreach (Portal portal in portals)
                    if (portal._A._x == location._x && portal._A._y == location._y
                        || portal._B._x == location._x && portal._B._y == location._y)
                    {
                        success = false;
                        break;
                    }

            } while (!success);

            _location = location;

        }
    }
}
