using System;
using System.Collections.Generic;

namespace ga_robot_controller
{
    public class Robot
    {
        private enum Direction { North, East, South, West };

        private int _xPosition;
        private int _yPosition;
        private Direction _heading;
        private readonly int _maxMoves;
        private int _moves;
        private int _sensorVal;
        private readonly int[] _sensorActions;
        private readonly Maze _maze;
        private readonly List<int[]> _route;

        /**
         * Initalize a robot with controller
         * 
         * @param sensorActions The string to map the sensor value to actions
         * @param maze The maze the robot will use
         * @param maxMoves The maximum number of moves the robot can make
         */
        public Robot(int[] sensorActions, Maze maze, int maxMoves)
        {
            _sensorActions = CalcSensorActions(sensorActions);
            _maze = maze;
            var startPos = _maze.GetStartPosition();
            _xPosition = startPos[0];
            _yPosition = startPos[1];
            _sensorVal = -1;
            _heading = Direction.East;
            _maxMoves = maxMoves;
            _moves = 0;
            _route = new List<int[]> {startPos};
        }

        /**
         * Runs the robot's actions based on sensor inputs
         */
        public void Run()
        {
            while (true)
            {
                _moves++;

                // Break if the robot stops moving
                if (GetNextAction() == 0)
                {
                    return;
                }

                // Break if we reach the goal
                if (_maze.GetPositionValue(_xPosition, _yPosition) == 4)
                {
                    return;
                }

                // Break if we reach a maximum number of moves
                if (_moves > _maxMoves)
                {
                    return;
                }

                // Run action
                MakeNextAction();
            }
        }

        /**
         * Map robot's sensor data to actions from binary string
         * 
         * @param sensorActionsStr Binary GA chromosome
         * @return int[] An array to map sensor value to an action
         */
        private static int[] CalcSensorActions(IReadOnlyList<int> sensorActionsStr)
        {
            // How many actions are there?
            var numActions = sensorActionsStr.Count / 2;
            var sensorActions = new int[numActions];

            // Loop through actions
            for (var sensorValue = 0; sensorValue < numActions; sensorValue++)
            {
                // Get sensor action
                var sensorAction = 0;
                if (sensorActionsStr[sensorValue * 2] == 1)
                {
                    sensorAction += 2;
                }
                if (sensorActionsStr[(sensorValue * 2) + 1] == 1)
                {
                    sensorAction += 1;
                }

                // Add to sensor-action map
                sensorActions[sensorValue] = sensorAction;
            }

            return sensorActions;
        }

        /**
         * Runs the next action
         */
        public void MakeNextAction()
        {
            // If move forward
            if (GetNextAction() == 1)
            {
                var currentX = _xPosition;
                var currentY = _yPosition;

                // Move depending on current direction
                if (Direction.North == _heading)
                {
                    _yPosition += -1;
                    if (_yPosition < 0)
                    {
                        _yPosition = 0;
                    }
                }
                else if (Direction.East == _heading)
                {
                    _xPosition += 1;
                    if (_xPosition > _maze.GetMaxX())
                    {
                        _xPosition = _maze.GetMaxX();
                    }
                }
                else if (Direction.South == _heading)
                {
                    _yPosition += 1;
                    if (_yPosition > _maze.GetMaxY())
                    {
                        _yPosition = _maze.GetMaxY();
                    }
                }
                else if (Direction.West == _heading)
                {
                    _xPosition += -1;
                    if (_xPosition < 0)
                    {
                        _xPosition = 0;
                    }
                }

                // We can't move here
                if (_maze.IsWall(_xPosition, _yPosition))
                {
                    _xPosition = currentX;
                    _yPosition = currentY;
                }
                else
                {
                    if (currentX != _xPosition || currentY != _yPosition)
                    {
                        _route.Add(GetPosition());
                    }
                }
            }
            // Move clockwise
            else if (GetNextAction() == 2)
            {
                if (Direction.North == _heading)
                {
                    _heading = Direction.East;
                }
                else if (Direction.East == _heading)
                {
                    _heading = Direction.South;
                }
                else if (Direction.South == _heading)
                {
                    _heading = Direction.West;
                }
                else if (Direction.West == _heading)
                {
                    _heading = Direction.North;
                }
            }
            // Move anti-clockwise
            else if (GetNextAction() == 3)
            {
                if (Direction.North == _heading)
                {
                    _heading = Direction.West;
                }
                else if (Direction.East == _heading)
                {
                    _heading = Direction.North;
                }
                else if (Direction.South == _heading)
                {
                    _heading = Direction.East;
                }
                else if (Direction.West == _heading)
                {
                    _heading = Direction.South;
                }
            }

            // Reset sensor value
            _sensorVal = -1;
        }

        /**
         * Get next action depending on sensor mapping
         * 
         * @return int Next action
         */
        public int GetNextAction()
        {
            return _sensorActions[GetSensorValue()];
        }

        /**
         * Get sensor value
         * 
         * @return int Next sensor value
         */
        public int GetSensorValue()
        {
            // If sensor value has already been calculated
            if (_sensorVal > -1)
            {
                return _sensorVal;
            }

            bool frontSensor, frontLeftSensor, frontRightSensor, leftSensor, rightSensor, backSensor;

            // Find which sensors have been activated
            if (GetHeading() == Direction.North)
            {
                frontSensor = _maze.IsWall(_xPosition, _yPosition - 1);
                frontLeftSensor = _maze.IsWall(_xPosition - 1, _yPosition - 1);
                frontRightSensor = _maze.IsWall(_xPosition + 1, _yPosition - 1);
                leftSensor = _maze.IsWall(_xPosition - 1, _yPosition);
                rightSensor = _maze.IsWall(_xPosition + 1, _yPosition);
                backSensor = _maze.IsWall(_xPosition, _yPosition + 1);
            }
            else if (GetHeading() == Direction.East)
            {
                frontSensor = _maze.IsWall(_xPosition + 1, _yPosition);
                frontLeftSensor = _maze.IsWall(_xPosition + 1, _yPosition - 1);
                frontRightSensor = _maze.IsWall(_xPosition + 1, _yPosition + 1);
                leftSensor = _maze.IsWall(_xPosition, _yPosition - 1);
                rightSensor = _maze.IsWall(_xPosition, _yPosition + 1);
                backSensor = _maze.IsWall(_xPosition - 1, _yPosition);
            }
            else if (GetHeading() == Direction.South)
            {
                frontSensor = _maze.IsWall(_xPosition, _yPosition + 1);
                frontLeftSensor = _maze.IsWall(_xPosition + 1, _yPosition + 1);
                frontRightSensor = _maze.IsWall(_xPosition - 1, _yPosition + 1);
                leftSensor = _maze.IsWall(_xPosition + 1, _yPosition);
                rightSensor = _maze.IsWall(_xPosition - 1, _yPosition);
                backSensor = _maze.IsWall(_xPosition, _yPosition - 1);
            }
            else
            {
                frontSensor = _maze.IsWall(_xPosition - 1, _yPosition);
                frontLeftSensor = _maze.IsWall(_xPosition - 1, _yPosition + 1);
                frontRightSensor = _maze.IsWall(_xPosition - 1, _yPosition - 1);
                leftSensor = _maze.IsWall(_xPosition, _yPosition + 1);
                rightSensor = _maze.IsWall(_xPosition, _yPosition - 1);
                backSensor = _maze.IsWall(_xPosition + 1, _yPosition);
            }

            // Calculate sensor value
            var sensorVal = 0;

            if (frontSensor)
            {
                sensorVal += 1;
            }
            if (frontLeftSensor)
            {
                sensorVal += 2;
            }
            if (frontRightSensor)
            {
                sensorVal += 4;
            }
            if (leftSensor)
            {
                sensorVal += 8;
            }
            if (rightSensor)
            {
                sensorVal += 16;
            }
            if (backSensor)
            {
                sensorVal += 32;
            }

            _sensorVal = sensorVal;

            return sensorVal;
        }

        /**
         * Get robot's position
         * 
         * @return int[] Array with robot's position
         */
        public int[] GetPosition()
        {
            return new[] { _xPosition, _yPosition };
        }

        /**
         * Get robot's heading
         * 
         * @return Direction Robot's heading
         */
        private Direction GetHeading()
        {
            return _heading;
        }

        /**
         * Returns robot's complete route around the maze
         * 
         * @return List<int> Robot's route
         */
        public List<int[]> GetRoute()
        {
            return _route;
        }

        /**
         * Returns route in printable format
         * 
         * @return String Robot's route
         */
        public string PrintRoute()
        {
            var route = "";

            foreach (object routeStep in _route)
            {
                var step = (int[])routeStep;
                route += "{" + step[0] + "," + step[1] + "}";
            }
            return route;
        }
    }

}
