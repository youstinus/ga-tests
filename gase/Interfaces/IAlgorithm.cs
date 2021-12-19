using System.Collections.Generic;

namespace gase.Interfaces
{
    public interface IAlgorithm<T>
    {
        /**
         * Initialize population
         * 
         * @param chromosomeLength The length of the individuals chromosome
         * @return population The initial population generated
         */
        Population<T> InitPopulation(int chromosomeLength);

        Population<T> InitPopulation();

        /**
         * Check if population has met termination condition
         * 
         * @param generationsCount
         *            Number of generations passed
         * @param maxGenerations
         *            Number of generations to terminate after
         * @return boolean True if termination condition met, otherwise, false
         */
        bool IsTerminationConditionMet(int generationsCount, int maxGenerations);

        /**
         * Check if population has met termination condition
         *
         * @param population
         * @return boolean True if termination condition met, otherwise, false
         */
        bool IsTerminationConditionMet(Population<T> population);

        /**
         * Check if population has met termination condition
         * 
         * For this simple problem, we know what a perfect solution looks like, so
         * we can simply stop evolving once we've reached a fitness of one.
         * 
         * @param population
         * @return boolean True if termination condition met, otherwise, false
         */
        bool IsTerminationConditionMet2(Population<T> population);
        
        /**
         * Calculate fitness for an individual.
         * 
         * In this case, the fitness score is very simple: it's the number of ones
         * in the chromosome. Don't forget that this method, and this whole
         * GeneticAlgorithm class, is meant to solve the problem in the "AllOnesGA"
         * class and example. For different problems, you'll need to create a
         * different version of this method to appropriately calculate the fitness
         * of an individual.
         * 
         * @param individual
         *            the individual to evaluate
         * @return double The fitness value for individual
         */
        double CalcFitness(Individual<T> individual);

        double CalcFitnessByModel(Individual<T> individual, T[] pattern);

        double CalcFitnessByModelWithoutSetting(Individual<T> individual, T[] pattern);
        
        /**
        * Evaluate the whole population
        * 
        * Essentially, loop over the individuals in the population, calculate the
        * fitness for each, and then calculate the entire population's fitness. The
        * population's fitness may or may not be important, but what is important
        * here is making sure that each individual gets evaluated.
        * 
        * @param population
        *       the population to evaluate
        */
        void EvalPopulation(Population<T> population);

        void EvalPopulationByModel(Population<T> population, T[] pattern);
        
        // other methods
        double GetIndividualsFitnessWithoutSetting(Individual<T> individual, T[] pattern);

        double SumFitnessAndSetting(Individual<T> individual, List<T[]> patterns);

        void EvalPopulationByModelList(Population<T> population, List<T[]> patterns);

        /**
         * Selects parent for crossover using tournament selection
         * 
         * Tournament selection works by choosing N random individuals, and then
         * choosing the best of those.
         * 
         * @param population
         * @return The individual selected as a parent
         */
        Individual<T> SelectParent(Population<T> population);

        /**
        * Select parent for crossover
        * 
        * @param population
        *            The population to select parent from
        * @return The individual selected as a parent
        */
        Individual<T> SelectParent2(Population<T> population);
        
        /**
         * Apply mutation to population
         * 
         * Mutation affects individuals rather than the population. We look at each
         * individual in the population, and if they're lucky enough (or unlucky, as
         * it were), apply some randomness to their chromosome. Like crossover, the
         * type of mutation applied depends on the specific problem we're solving.
         * In this case, we simply randomly flip 0s to 1s and vice versa.
         * 
         * This method will consider the GeneticAlgorithm instance's mutationRate
         * and elitismCount
         * 
         * @param population
         *            The population to apply mutation to
         * @return The mutated population
         */
        Population<T> MutatePopulationPickAvailable(Population<T> population);
        
        /**
         * Apply crossover to population
         * 
         * Crossover, more colloquially considered "mating", takes the population
         * and blends individuals to create new offspring. It is hoped that when two
         * individuals crossover that their offspring will have the strongest
         * qualities of each of the parents. Of course, it's possible that an
         * offspring will end up with the weakest qualities of each parent.
         * 
         * This method considers both the GeneticAlgorithm instance's crossoverRate
         * and the elitismCount.
         * 
         * The type of crossover we perform depends on the problem domain. We don't
         * want to create invalid solutions with crossover, so this method will need
         * to be changed for different types of problems.
         * 
         * This particular crossover method selects random genes from each parent.
         * 
         * @param population
         *            The population to apply crossover to
         * @return The new population
         */
        Population<T> CrossoverPopulationSomeOther(Population<T> population);
    }
}
