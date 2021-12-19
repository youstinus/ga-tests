using System;

namespace ga_robot_controller
{
    internal class Program
    {
        public static int MaxGenerations = 1000;

        private static void Main()
        {
            /**
		 * Initialize a maze. We'll write this by hand, because, y'know, this
		 * book isn't called "maze generation algorithms".
		 * 
		 * The 3s represent the correct route through the maze. 1s are walls
		 * that can't be navigated through, and 0s are valid positions, but not
		 * the correct route. You can follow the 3s visually to find the correct
		 * path through the maze.
		 * 
		 * If you've read the docblock for
		 * GeneticAlgorithm::isTerminationConditionMet, I mention that we don't
		 * know what a perfect solution looks like, so the only constraint we
		 * can give the algorithm is the number of generations. That's both true
		 * and untrue. In this case, because we made the maze by hand, we
		 * actually DO know the winning fitness: it's 29, or the number of 3s
		 * below! However, we can't use that as a termination condition; if this
		 * maze were procedurally generated we would not necessarily know that
		 * the magic number is 29.
		 * 
		 * As a reminder: 
		 * 0 = Empty 
		 * 1 = Wall 
		 * 2 = Starting position 
		 * 3 = Route 
		 * 4 = Goal position
		 */

            var maze = new Maze(new[]
            {
                new[]{ 0, 0, 0, 0, 1, 0, 1, 3, 2 },
                new[]{ 1, 0, 1, 1, 1, 0, 1, 3, 1 },
                new[]{ 1, 0, 0, 1, 3, 3, 3, 3, 1 },
                new[]{ 3, 3, 3, 1, 3, 1, 1, 0, 1 },
                new[]{ 3, 1, 3, 3, 3, 1, 1, 0, 0 },
                new[]{ 3, 3, 1, 1, 1, 1, 0, 1, 1 },
                new[]{ 1, 3, 0, 1, 3, 3, 3, 3, 3 },
                new[]{ 0, 3, 1, 1, 3, 1, 0, 1, 3 },
                new[]{ 1, 3, 3, 3, 3, 1, 1, 1, 4 }
        });

            // Create genetic algorithm
            var ga = new GeneticAlgorithm(200, 0.05, 0.9, 2, 10);
            var population = ga.InitPopulation(128);
            ga.EvalPopulation(population, maze);
            // Keep track of current generation
            var generation = 1;
            // Start evolution loop
            Individual fittest;
            while (ga.IsTerminationConditionMet(generation, MaxGenerations) == false)
            {
                // Print fittest individual from population
                fittest = population.GetFittest(0);
                Console.WriteLine(
                        "G" + generation + " Best solution (" + fittest.GetFitness() + "): " + fittest);

                // Apply crossover
                population = ga.CrossoverPopulation(population);

                // Apply mutation
                population = ga.MutatePopulation(population);

                // Evaluate population
                ga.EvalPopulation(population, maze);

                // Increment the current generation
                generation++;
            }

            Console.WriteLine("Stopped after " + MaxGenerations + " generations.");
            fittest = population.GetFittest(0);
            Console.WriteLine("Best solution (" + fittest.GetFitness() + "): " + fittest);

            Console.ReadKey();
        }
    }
}
