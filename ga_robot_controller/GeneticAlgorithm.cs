using System;
using System.Linq;

namespace ga_robot_controller
{
    /**
 * Please see chapter2/GeneticAlgorithm for additional comments.
 * 
 * This GeneticAlgorithm class is designed to solve the
 * "Robot Controller in a Maze" problem, and is necessarily a little different
 * from the chapter2/GeneticAlgorithm class.
 * 
 * This class introduces the concepts of tournament selection and single-point
 * crossover. Additionally, the calcFitness method is vastly different from the
 * AllOnesGA fitness method; in this case we actually have to evaluate how good
 * the robot is at navigating a maze!
 * 
 * @author bkanber
 *
 */
    public class GeneticAlgorithm
    {

        /**
         * See chapter2/GeneticAlgorithm for a description of these properties.
         */
        private readonly int _populationSize;
        private readonly double _mutationRate;
        private readonly double _crossoverRate;
        private readonly int _elitismCount;

        /**
         * A new property we've introduced is the size of the population used for
         * tournament selection in crossover.
         */
        protected int TournamentSize;

        public GeneticAlgorithm(int populationSize, double mutationRate, double crossoverRate, int elitismCount, int tournamentSize)
        {
            _populationSize = populationSize;
            _mutationRate = mutationRate;
            _crossoverRate = crossoverRate;
            _elitismCount = elitismCount;
            TournamentSize = tournamentSize;
        }

        /**
         * Initialize population
         * 
         * @param chromosomeLength
         *            The length of the individuals chromosome
         * @return population The initial population generated
         */
        public Population InitPopulation(int chromosomeLength)
        {
            // Initialize population
            var population = new Population(_populationSize, chromosomeLength);
            return population;
        }

        /**
         * Calculate fitness for an individual.
         * 
         * This fitness calculation is a little more involved than chapter2's. In
         * this case we initialize a new Robot class, and evaluate its performance
         * in the given maze.
         * 
         * @param individual
         *            the individual to evaluate
         * @return double The fitness value for individual
         */
        public double CalcFitness(Individual individual, Maze maze)
        {
            // Get individual's chromosome
            var chromosome = individual.GetChromosome();

            // Get fitness
            var robot = new Robot(chromosome, maze, 100);
            robot.Run();
            var fitness = maze.ScoreRoute(robot.GetRoute());

            // Store fitness
            individual.SetFitness(fitness);

            return fitness;
        }

        /**
         * Evaluate the whole population
         * 
         * Essentially, loop over the individuals in the population, calculate the
         * fitness for each, and then calculate the entire population's fitness. The
         * population's fitness may or may not be important, but what is important
         * here is making sure that each individual gets evaluated.
         * 
         * The difference between this method and the one in chapter2 is that this
         * method requires the maze itself as a parameter; unlike the All Ones
         * problem in chapter2, we can't determine a fitness just by looking at the
         * chromosome -- we need to evaluate each member against the maze.
         * 
         * @param population
         *            the population to evaluate
         * @param maze
         *            the maze to evaluate each individual against.
         */
        public void EvalPopulation(Population population, Maze maze)
        {
            var populationFitness = population.GetIndividuals().Sum(individual => CalcFitness(individual, maze));

            // Loop over population evaluating individuals and suming population
            // fitness

            population.SetPopulationFitness(populationFitness);
        }

        /**
         * Check if population has met termination condition
         * 
         * We don't actually know what a perfect solution looks like for the robot
         * controller problem, so the only constraint we can give to the genetic
         * algorithm is an upper bound on the number of generations.
         * 
         * @param generationsCount
         *            Number of generations passed
         * @param maxGenerations
         *            Number of generations to terminate after
         * @return boolean True if termination condition met, otherwise, false
         */
        public bool IsTerminationConditionMet(int generationsCount, int maxGenerations)
        {
            return (generationsCount > maxGenerations);
        }

        /**
         * Selects parent for crossover using tournament selection
         * 
         * Tournament selection works by choosing N random individuals, and then
         * choosing the best of those.
         * 
         * @param population
         * @return The individual selected as a parent
         */
        public Individual SelectParent(Population population)
        {
            // Create tournament
            var tournament = new Population(TournamentSize);

            // Add random individuals to the tournament
            population.Shuffle();
            for (var i = 0; i < TournamentSize; i++)
            {
                var tournamentIndividual = population.GetIndividual(i);
                tournament.SetIndividual(i, tournamentIndividual);
            }

            // Return the best
            return tournament.GetFittest(0);
        }

        /**
         * Apply mutation to population
         * 
         * This method is the same as chapter2's version.
         * 
         * @param population
         *            The population to apply mutation to
         * @return The mutated population
         */
        public Population MutatePopulation(Population population)
        {
            var rnd = new Random();
            // Initialize new population
            var newPopulation = new Population(_populationSize);

            // Loop over current population by fitness
            for (var populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                var individual = population.GetFittest(populationIndex);

                // Loop over individual's genes
                for (var geneIndex = 0; geneIndex < individual.GetChromosomeLength(); geneIndex++)
                {
                    // Skip mutation if this is an elite individual
                    if (populationIndex >= _elitismCount)
                    {
                        // Does this gene need mutation?
                        if (_mutationRate > rnd.NextDouble())
                        {
                            // Get new gene
                            var newGene = 1;
                            if (individual.GetGene(geneIndex) == 1)
                            {
                                newGene = 0;
                            }
                            // Mutate gene
                            individual.SetGene(geneIndex, newGene);
                        }
                    }
                }


                // Add individual to population
                newPopulation.SetIndividual(populationIndex, individual);
            }

            // Return mutated population
            return newPopulation;
        }

        /**
         * Crossover population using single point crossover
         * 
         * Single-point crossover differs from the crossover used in chapter2.
         * Chapter2's version simply selects genes at random from each parent, but
         * in this case we want to select a contiguous region of the chromosome from
         * each parent.
         * 
         * For instance, chapter2's version would look like this:
         * 
         * Parent1: AAAAAAAAAA 
         * Parent2: BBBBBBBBBB 
         * Child  : AABBAABABA
         * 
         * This version, however, might look like this:
         * 
         * Parent1: AAAAAAAAAA 
         * Parent2: BBBBBBBBBB 
         * Child  : AAAABBBBBB
         * 
         * @param population
         *            Population to crossover
         * @return Population The new population
         */
        public Population CrossoverPopulation(Population population)
        {
            var rnd = new Random();
            // Create new population
            var newPopulation = new Population(population.Size());

            // Loop over current population by fitness
            for (var populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                var parent1 = population.GetFittest(populationIndex);

                // Apply crossover to this individual?
                if (_crossoverRate > rnd.NextDouble() && populationIndex >= _elitismCount)
                {
                    // Initialize offspring
                    var offspring = new Individual(parent1.GetChromosomeLength());

                    // Find second parent
                    var parent2 = SelectParent(population);

                    // Get random swap point
                    var swapPoint = (int)(rnd.NextDouble() * (parent1.GetChromosomeLength() + 1));

                    // Loop over genome
                    for (var geneIndex = 0; geneIndex < parent1.GetChromosomeLength(); geneIndex++)
                    {
                        // Use half of parent1's genes and half of parent2's genes
                        if (geneIndex < swapPoint)
                        {
                            offspring.SetGene(geneIndex, parent1.GetGene(geneIndex));
                        }
                        else
                        {
                            offspring.SetGene(geneIndex, parent2.GetGene(geneIndex));
                        }
                    }

                    // Add offspring to new population
                    newPopulation.SetIndividual(populationIndex, offspring);
                }
                else
                {
                    // Add individual to new population without applying crossover
                    newPopulation.SetIndividual(populationIndex, parent1);
                }
            }

            return newPopulation;
        }

    }

}
