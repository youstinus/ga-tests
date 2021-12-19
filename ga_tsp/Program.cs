using System;

namespace ga_tsp
{
    internal class Program
    {
        public static int MaxGenerations = 1000;

        private static void Main()
        {
            // Create cities
            const int numCities = 100;
            var cities = new City[numCities];
            var rnd = new Random();
            // Loop to create random cities
            for (var cityIndex = 0; cityIndex < numCities; cityIndex++)
            {
                // Generate x,y position
                var xPos = (int)(rnd.NextDouble() * 100);
                var yPos = (int)(rnd.NextDouble() * 100);

                // Add city
                cities[cityIndex] = new City(xPos, yPos);
            }

            // Initial GA
            var ga = new GeneticAlgorithm(100, 0.001, 0.9, 2, 5);

            // Initialize population
            var population = ga.InitPopulation(cities.Length);

            // Evaluate population
            ga.EvalPopulation(population, cities);

            // my sort
            population.Sort();
            var startRoute = new Route(population.GetFittest(0), cities);
            Console.WriteLine("Start Distance: " + startRoute.GetDistance());

            // Keep track of current generation
            var generation = 1;
            // Start evolution loop
            Route route;
            while (ga.IsTerminationConditionMet(generation, MaxGenerations) == false)
            {
                // my sort
                //population.Sort();
                // Print fittest individual from population
                route = new Route(population.GetFittest(0), cities);
                Console.WriteLine("G" + generation + " Best distance: " + route.GetDistance());

                // Apply crossover
                population = ga.CrossoverPopulation(population);

                // Apply mutation
                population = ga.MutatePopulation(population);

                // Evaluate population
                ga.EvalPopulation(population, cities);

                // Increment the current generation
                generation++;
            }

            // my sort
            population.Sort();
            Console.WriteLine("Stopped after " + MaxGenerations + " generations.");
            route = new Route(population.GetFittest(0), cities);
            Console.WriteLine("Best distance: " + route.GetDistance());
            Console.ReadKey();
        }
    }
}
