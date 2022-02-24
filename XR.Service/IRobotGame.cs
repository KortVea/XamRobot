using System;

namespace XR.Service
{
    public interface IRobotGame
    {
        ExecResult Execute(string command);

        IObservable<Position> Location { get; }
    }
    
    public class Position
    {
        public int? X { get; }
        public int? Y { get; }
        public Bearing? Direction {get;}

        public Position(int? x, int? y, Bearing? direction)
        {
            this.X = x;
            this.Y = y;
            this.Direction = direction;
        }
    }

    public enum ExecResult
    {
        OK = 0, DENIED, ERROR
    }

    public enum Bearing
    {
        WEST = 0, NORTH, EAST, SOUTH
    }
}