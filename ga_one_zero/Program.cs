using System;

namespace ga_one_zero
{
    /**
    * This is our main class used to run the genetic algorithm.
    * 
    * This case is one of the simplest problems we can solve: the objective is to
    * end up with an individual whose chromosome is all ones.
    * 
    * The simplicity of this problem makes the GeneticAlgorithm class'
    * "calcFitness" method very simple. We'll just count the number of ones in the
    * chromosome and use that as the fitness score. Similarly, the
    * "isTerminationConditionMet" method in the GeneticAlgorithm class for this
    * example is very simple: if the fitness score (ie, number of ones) is the same
    * as the length of the chromosome (ie, we're all ones), we're done!
    * 
    * @author bkanber
    *
    */
    internal class Program
    {
        private static void Main()
        {
            // Create GA object
            var ga = new GeneticAlgorithm(100, 0.001, 0.95, 2);

            // Initialize population
            var population = ga.InitPopulation(50);

            // Evaluate population
            ga.EvalPopulation(population);

            // Keep track of current generation
            var generation = 1;

            /**
             * Start the evolution loop
             * 
             * Every genetic algorithm problem has different criteria for finishing.
             * In this case, we know what a perfect solution looks like (we don't
             * always!), so our isTerminationConditionMet method is very
             * straightforward: if there's a member of the population whose
             * chromosome is all ones, we're done!
             */
            while (ga.IsTerminationConditionMet(population) == false)
            {
                // Print fittest individual from population
                Console.WriteLine("Best solution: " + population.GetFittest(0));

                // Apply crossover
                population = ga.CrossoverPopulation(population);

                // Apply mutation
                population = ga.MutatePopulation(population);

                // Evaluate population
                ga.EvalPopulation(population);

                // Increment the current generation
                generation++;
            }

            /**
             * We're out of the loop now, which means we have a perfect solution on
             * our hands. Let's print it out to confirm that it is actually all
             * ones, as promised.
             */
            Console.WriteLine("Found solution in " + generation + " generations");
            Console.WriteLine("Best solution: " + population.GetFittest(0));
            Console.ReadKey();
        }
    }
}
