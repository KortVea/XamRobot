using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace XR.Service
{
    public class RobotGame : IRobotGame
    {
        private int? _x;
        private int? _y;
        private Bearing? _direction;
        private bool HasBearing => this.Direction != null;

        private bool IsValidState =>
            this.X != null
            && this.Y != null
            && this.HasBearing;

        private static bool IsValidCoordinate(int? i) =>
            i >= 0 && i <= 5;

        private int? X
        {
            get => this._x;
            set
            {
                if (IsValidCoordinate(value))
                {
                    this._x = value;
                }
            }
        }
        
        private int? Y
        {
            get => this._y;
            set
            {
                if (IsValidCoordinate(value))
                {
                    this._y = value;
                }
            }
        }

        private Bearing? Direction
        {
            get => this._direction;
            set
            {
                if(value == null)
                    return;
                this._direction = value;
            }
        }

        private ReplaySubject<Position> position = new ReplaySubject<Position>(1);

        public IObservable<Position> Location => this.position.AsObservable();

        public ExecResult Execute(string command)
        {
            var info = command?.Split(new char[]{' '}, 2, StringSplitOptions.RemoveEmptyEntries);
            
            if (info == null || info.Length == 0)
                return ExecResult.ERROR;
            
            switch (info.Length)
            {
                case 1 when info[0] != "PLACE":
                    return (ExecResult) (this
                        .GetType()
                        .GetMethod(info[0])?
                        .Invoke(this, null) ?? ExecResult.ERROR);
                case 2:
                {
                    var actionArgs = 
                        info[1]
                            .Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries)
                            .Select(i => i.Trim())
                            .ToArray();
                
                    if (actionArgs.Length != 2 && actionArgs.Length != 3)
                        return ExecResult.ERROR;

                    if (!int.TryParse(actionArgs[0], out var x) || !int.TryParse(actionArgs[1], out var y))
                        return ExecResult.ERROR;
                
                    Bearing? bearingValue = null;
                    
                    if (actionArgs.Length == 3 
                        && Enum.TryParse<Bearing>(actionArgs[2], out var bearing))
                    {
                        bearingValue = bearing;
                    }
                        
                    return (ExecResult) (
                        this
                            .GetType()
                            .GetMethod(info[0])?
                            .Invoke(this, new object[] {x, y, bearingValue})
                        ?? ExecResult.ERROR);
                }
                default:
                    return ExecResult.ERROR;
            }
        }

        public ExecResult PLACE(int x, int y, Bearing? bearing)
        {
            if (!HasBearing && bearing == null)
                return ExecResult.DENIED;
            if (!IsValidCoordinate(x) || !IsValidCoordinate(y))
                return ExecResult.DENIED;
            this.X = x;
            this.Y = y;
            this.Direction = bearing;
            
            this.position.OnNext(new Position(this.X, this.Y, this.Direction));
            return ExecResult.OK;
        }

        public ExecResult LEFT()
        {
            if (!this.HasBearing)
                return ExecResult.DENIED;
            
            switch (this.Direction)
            {
                case Bearing.WEST:
                    this.Direction = Bearing.SOUTH;
                    break;
                case Bearing.NORTH:
                    this.Direction = Bearing.WEST;
                    break;
                case Bearing.EAST:
                    this.Direction = Bearing.NORTH;
                    break;
                case Bearing.SOUTH:
                    this.Direction = Bearing.EAST;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.position.OnNext(new Position(this.X, this.Y, this.Direction));
            return ExecResult.OK;
        }

        public ExecResult RIGHT()
        {
            if (!this.HasBearing)
                return ExecResult.DENIED;
            
            switch (this.Direction)
            {
                case Bearing.WEST:
                    this.Direction = Bearing.NORTH;
                    break;
                case Bearing.NORTH:
                    this.Direction = Bearing.EAST;
                    break;
                case Bearing.EAST:
                    this.Direction = Bearing.SOUTH;
                    break;
                case Bearing.SOUTH:
                    this.Direction = Bearing.WEST;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.position.OnNext(new Position(this.X, this.Y, this.Direction));
            return ExecResult.OK;
        }

        public ExecResult MOVE()
        {
            if (!this.IsValidState)
                return ExecResult.DENIED;
            switch (this.Direction)
            {
                case Bearing.WEST:
                    this.X -= 1;
                    break;
                case Bearing.NORTH:
                    this.Y += 1;
                    break;
                case Bearing.EAST:
                    this.X += 1;
                    break;
                case Bearing.SOUTH:
                    this.Y -= 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.position.OnNext(new Position(this.X, this.Y, this.Direction));
            return ExecResult.OK;
        }

        public ExecResult REPORT()
        {
            var report = this.ToString();
            Console.WriteLine(report);
            return string.IsNullOrEmpty(report) ? ExecResult.DENIED : ExecResult.OK;
        }

        public override string ToString() =>
            this.IsValidState
                ? $"{this._x},{this._y},{this._direction.ToString()}"
                : null;
    }
}