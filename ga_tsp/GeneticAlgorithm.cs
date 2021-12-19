using System;
using System.Linq;

namespace ga_tsp
{
    public class GeneticAlgorithm
    {

        private readonly int _populationSize;
        private readonly double _mutationRate;
        private readonly double _crossoverRate;
        private readonly int _elitismCount;
        protected int TournamentSize;

        public GeneticAlgorithm(int populationSize, double mutationRate, double crossoverRate, int elitismCount,
                int tournamentSize)
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
         * @param chromosomeLength The length of the individuals chromosome
         * @return population The initial population generated
         */
        public Population InitPopulation(int chromosomeLength)
        {
            // Initialize population
            var population = new Population(_populationSize, chromosomeLength);
            return population;
        }

        /**
         * Check if population has met termination condition -- this termination
         * condition is a simple one; simply check if we've exceeded the allowed
         * number of generations.
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
         * Calculate individual's fitness value
         * 
         * Fitness, in this problem, is inversely proportional to the route's total
         * distance. The total distance is calculated by the Route class.
         * 
         * @param individual
         *            the individual to evaluate
         * @param cities
         *            the cities being referenced
         * @return double The fitness value for individual
         */
        public double CalcFitness(Individual individual, City[] cities)
        {
            // Get fitness
            var route = new Route(individual, cities);
            var fitness = 1 / route.GetDistance();

            // Store fitness
            individual.SetFitness(fitness);

            return fitness;
        }

        /**
         * Evaluate population -- basically run calcFitness on each individual.
         * 
         * @param population the population to evaluate
         * @param cities the cities being referenced
         */
        public void EvalPopulation(Population population, City[] cities)
        {
            var populationFitness = population.GetIndividuals().Sum(individual => CalcFitness(individual, cities));

            // Loop over population evaluating individuals and summing population fitness

            var avgFitness = populationFitness / population.Size();
            population.SetPopulationFitness(avgFitness);
        }

        /**
         * Selects parent for crossover using tournament selection
         * 
         * Tournament selection was introduced in Chapter 3
         * 
         * @param population
         *            
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
            tournament.Sort();
            return tournament.GetFittest(0);
        }


        /**
         * Ordered crossover mutation
         * 
         * Chromosomes in the TSP require that each city is visited exactly once.
         * Uniform crossover can break the chromosome by accidentally selecting a
         * city that has already been visited from a parent; this would lead to one
         * city being visited twice and another city being skipped altogether.
         * 
         * Additionally, uniform or random crossover doesn't really preserve the
         * most important aspect of the genetic information: the specific order of a
         * group of cities.
         * 
         * We need a more clever crossover algorithm here. What we can do is choose
         * two pivot points, add chromosomes from one parent for one of the ranges,
         * and then only add not-yet-represented cities to the second range. This
         * ensures that no cities are skipped or visited twice, while also
         * preserving ordered batches of cities.
         * 
         * @param population
         * @return The new population
         */
        public Population CrossoverPopulation(Population population)
        {
            var rnd = new Random();
            // Create new population
            var newPopulation = new Population(population.Size());

            // my sort
            population.Sort();
            // Loop over current population by fitness
            for (var populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                // Get parent1
                var parent1 = population.GetFittest(populationIndex);

                // Apply crossover to this individual?
                if (_crossoverRate > rnd.NextDouble() && populationIndex >= _elitismCount)
                {
                    // Find parent2 with tournament selection
                    var parent2 = SelectParent(population);

                    // Create blank offspring chromosome
                    var offspringChromosome = new int[parent1.GetChromosomeLength()];
                    offspringChromosome = Enumerable.Repeat(-1, offspringChromosome.Length).ToArray();
                    //Arrays.fill(offspringChromosome, -1);
                    var offspring = new Individual(offspringChromosome);

                    // Get subset of parent chromosomes
                    var substrPos1 = (int)(rnd.NextDouble() * parent1.GetChromosomeLength());
                    var substrPos2 = (int)(rnd.NextDouble() * parent1.GetChromosomeLength());

                    // make the smaller the start and the larger the end
                    var startSubstr = Math.Min(substrPos1, substrPos2);
                    var endSubstr = Math.Max(substrPos1, substrPos2);

                    // Loop and add the sub tour from parent1 to our child
                    for (var i = startSubstr; i < endSubstr; i++)
                    {
                        offspring.SetGene(i, parent1.GetGene(i));
                    }

                    // Loop through parent2's city tour
                    for (var i = 0; i < parent2.GetChromosomeLength(); i++)
                    {
                        var parent2Gene = i + endSubstr;
                        if (parent2Gene >= parent2.GetChromosomeLength())
                        {
                            parent2Gene -= parent2.GetChromosomeLength();
                        }

                        // If offspring doesn't have the city add it
                        if (offspring.ContainsGene(parent2.GetGene(parent2Gene)) == false)
                        {
                            // Loop to find a spare position in the child's tour
                            for (var ii = 0; ii < offspring.GetChromosomeLength(); ii++)
                            {
                                // Spare position found, add city
                                if (offspring.GetGene(ii) == -1)
                                {
                                    offspring.SetGene(ii, parent2.GetGene(parent2Gene));
                                    break;
                                }
                            }
                        }
                    }

                    // Add child
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

        /**
         * Apply mutation to population
         * 
         * Because the traveling salesman problem must visit each city only once,
         * this form of mutation will randomly swap two genes instead of
         * bit-flipping a gene like in earlier examples.
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


            // my sort
            population.Sort();
            // Loop over current population by fitness
            for (var populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                var individual = population.GetFittest(populationIndex);

                // Skip mutation if this is an elite individual
                if (populationIndex >= _elitismCount)
                {
                    // System.out.println("Mutating population member "+populationIndex);
                    // Loop over individual's genes
                    for (var geneIndex = 0; geneIndex < individual.GetChromosomeLength(); geneIndex++)
                    {
                        // System.out.println("\tGene index "+geneIndex);
                        // Does this gene need mutation?
                        if (_mutationRate > rnd.NextDouble())
                        {
                            // Get new gene position
                            var newGenePos = (int)(rnd.NextDouble() * individual.GetChromosomeLength());
                            // Get genes to swap
                            var gene1 = individual.GetGene(newGenePos);
                            var gene2 = individual.GetGene(geneIndex);
                            // Swap genes
                            individual.SetGene(geneIndex, gene1);
                            individual.SetGene(newGenePos, gene2);
                        }
                    }
                }

                // Add individual to population
                newPopulation.SetIndividual(populationIndex, individual);
            }

            // Return mutated population
            return newPopulation;
        }

    }
}
