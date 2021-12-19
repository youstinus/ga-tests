using System;
using System.Linq;
using gase;

namespace viking_test1_ga
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var games = new List<Game>();
            //Utility.ReadFile(games);
            //Utility.Serialize(games);
            // all games before
            var games = Utility.Deserialize();
            var gomes = games.Where(x => x.Nr > 1300).ToList();

            // start algorithm instance
            var ga = new GeneticAlgorithm<int>(50, 0.05, 0.3, 1, 5, Utility.MakeList(48));

            // Initialize population
            var population = ga.InitPopulation();

            // Evaluate population
            ga.EvalPopulationByModelList(population, gomes.Select(x => x.Numbers.ToArray()).ToList());

            // Keep track of current generation
            var generation = 1;

            // Start evolution loop
            while (ga.IsTerminationConditionMet(generation, 200) == false
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
                ga.EvalPopulationByModelList(population, gomes.Select(x => x.Numbers.ToArray()).ToList());

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
