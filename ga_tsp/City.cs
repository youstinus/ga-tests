using System;

namespace ga_tsp
{
    public class City
    {
        private readonly int _x;
        private readonly int _y;

        /**
         * Initalize a city
         * 
         * @param x
         *            X position of city
         * @param y
         *            Y position of city
         */
        public City(int x, int y)
        {
            _x = x;
            _y = y;
        }

        /**
         * Calculate distance from another city
         * 
         * Pythagorean theorem: a^2 + b^2 = c^2
         * 
         * @param city
         *            The city to calculate the distance from
         * @return distance The distance from the given city
         */
        public double DistanceFrom(City city)
        {
            // Give difference in x,y
            var deltaXSq = Math.Pow((city.GetX() - GetX()), 2);
            var deltaYSq = Math.Pow((city.GetY() - GetY()), 2);

            // Calculate shortest path
            var distance = Math.Sqrt(Math.Abs(deltaXSq + deltaYSq));
            return distance;
        }

        /**
         * Get x position of city
         * 
         * @return x X position of city
         */
        public int GetX()
        {
            return _x;
        }

        /**
         * Get y position of city
         * 
         * @return y Y position of city
         */
        public int GetY()
        {
            return _y;
        }
    }

}
