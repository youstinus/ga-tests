using System;
using System.Collections.Generic;

namespace ga_robot_controller
{
    /**
 * This class abstracts a maze through which a robot will have to navigate. The
 * maze is represented as a 2d array of integers, with different environment
 * types represented by integers as follows:
 * 
 * 0 = Empty 
 * 1 = Wall 
 * 2 = Starting position 
 * 3 = Route 
 * 4 = Goal position
 * 
 * The most significant method in this class is `scoreRoute`, which will return
 * a fitness score for a path; it is this score that the genetic algorithm will
 * optimize.
 * 
 * @author bkanber
 *
 */
    public class Maze
    {
        private readonly int[][] _maze;
	    private int[] _startPosition = { -1, -1 };

        public Maze(int[][] maze)
        {
            _maze = maze;
        }

        /**
         * Get start position of maze
         * 
         * @return int[] x,y start position of maze
         */
        public int[] GetStartPosition()
        {
            // Check we already found start position
            if (_startPosition[0] != -1 && _startPosition[1] != -1)
            {
                return _startPosition;
            }

            // Default return value
            int[] startPosition = { 0, 0 };

            // Loop over rows
            for (var rowIndex = 0; rowIndex < _maze.Length; rowIndex++)
            {
                // Loop over columns
                for (var colIndex = 0; colIndex < _maze[rowIndex].Length; colIndex++)
                {
                    // 2 is the type for start position
                    if (_maze[rowIndex][colIndex] == 2)
                    {
                        _startPosition = new[] { colIndex, rowIndex };
                        return new[] { colIndex, rowIndex };
                    }
                }
            }

            return startPosition;
        }

        /**
         * Gets value for position of maze
         * 
         * @param x
         *            position
         * @param y
         *            position
         * @return int Position value
         */
        public int GetPositionValue(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _maze.Length || y >= _maze[0].Length)
            {
                return 1;
            }
            return _maze[y][x];
        }

        /**
         * Check if position is wall
         * 
         * @param x
         *            position
         * @param y
         *            position
         * @return bool
         */
        public bool IsWall(int x, int y)
        {
            return (GetPositionValue(x, y) == 1);
        }

        /**
         * Gets maximum index of x position
         * 
         * @return int Max index
         */
        public int GetMaxX()
        {
            return _maze[0].Length - 1;
        }

        /**
         * Gets maximum index of y position
         * 
         * @return int Max index
         */
        public int GetMaxY()
        {
            return _maze.Length - 1;
        }

        /**
         * Scores a maze route
         * 
         * This method inspects a route given as an array, and adds a point for each
         * correct step made. We also have to be careful not to reward re-visiting
         * correct paths, otherwise you could get an infinite score just by wiggling
         * back and forth on the route.
         * 
         * @return int Max index
         */
        public int ScoreRoute(List<int[]> route)
        {
            var score = 0;
            var visited = new bool[GetMaxY() + 1, GetMaxX() + 1];

		// Loop over route and score each move
		foreach (Object routeStep in route) {
			var step = (int[])routeStep;
			if (_maze[step[1]][step[0]] == 3 && visited[step[1],step[0]] == false) {
            // Increase score for correct move
            score++;
            // Remove reward
            visited[step[1],step[0]] = true;
        }
        }

		return score;
	}
}

}
