namespace ga_tsp
{
    public class Route
    {
        private readonly City[] _route;
        private double _distance;

        /**
         * Initialize Route
         * 
         * @param individual
         *            A GA individual
         * @param cities
         *            The cities referenced
         */
        public Route(Individual individual, City[] cities)
        {
            // Get individual's chromosome
            var chromosome = individual.GetChromosome();
            // Create route
            _route = new City[cities.Length];
            for (var geneIndex = 0; geneIndex < chromosome.Length; geneIndex++)
            {
                _route[geneIndex] = cities[chromosome[geneIndex]];
            }
        }

        /**
         * Get route distance
         * 
         * @return distance The route's distance
         */
        public double GetDistance()
        {
            if (_distance > 0)
            {
                return _distance;
            }

            // Loop over cities in route and calculate route distance
            double totalDistance = 0;
            for (var cityIndex = 0; cityIndex + 1 < _route.Length; cityIndex++)
            {
                totalDistance += _route[cityIndex].DistanceFrom(_route[cityIndex + 1]);
            }

            totalDistance += _route[_route.Length - 1].DistanceFrom(_route[0]);
            _distance = totalDistance;

            return totalDistance;
        }
    }
}
