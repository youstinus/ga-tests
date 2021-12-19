using System;
using System.Collections.Generic;

namespace gase
{
    internal class Program
    {
        private static void Main()
        {
            // Initialize GA
            var ga = new GeneticAlgorithm<int>(100, 0.01, 0.9, 2, 5, new List<int>(){0,1});

            // Initialize population
            var population = ga.InitPopulation(100);

            // Evaluate population
            ga.EvalPopulation(population);

            // Keep track of current generation
            var generation = 1;

            // Start evolution loop
            while (ga.IsTerminationConditionMet(generation, 1000) == false
                   && ga.IsTerminationConditionMet(population) == false)
            {
                // Print fitness
                population.Sort();
                Console.WriteLine("G" + generation + " Best fitness: " + population.GetFittestByOffset(0).GetFitness());

                // Apply crossover
                population = ga.CrossoverPopulationSomeOther(population);

                // Apply mutation
                population = ga.MutatePopulationPickAvailable(population);

                // Evaluate population
                ga.EvalPopulation(population);

                // Increment the current generation
                generation++;
            }

            // Print fitness
            Console.WriteLine();
            Console.WriteLine("Solution found in " + generation + " generations");
            population.Sort();
            Console.WriteLine("Final solution fitness: " + population.GetFittestByOffset(0).GetFitness());
            Console.WriteLine("Solution: " + population.GetFittestByOffset(0));
            Console.ReadKey();
        }
    }
}
