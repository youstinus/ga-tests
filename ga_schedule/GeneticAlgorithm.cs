using System;

namespace ga_schedule
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
         * @param chromosomeLength
         *            The length of the individuals chromosome
         * @return population The initial population generated
         */
        public Population InitPopulation(Timetable timetable)
        {
            // Initialize population
            var population = new Population(_populationSize, timetable);
            return population;
        }

        /**
         * Check if population has met termination condition
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
         * Check if population has met termination condition
         *
         * @param population
         * @return boolean True if termination condition met, otherwise, false
         */
        public bool IsTerminationConditionMet(Population population)
        {
            return Math.Abs(population.GetFittest(0).GetFitness() - 1.0) < 0.00001;
        }

        /**
         * Calculate individual's fitness value
         * 
         * @param individual
         * @param timetable
         * @return fitness
         */
        public double CalcFitness(Individual individual, Timetable timetable)
        {

            // Create new timetable object to use -- cloned from an existing timetable
            var threadTimetable = new Timetable(timetable);
            threadTimetable.CreateClasses(individual);

            // Calculate fitness
            var clashes = threadTimetable.CalcClashes();
            var fitness = 1 / (double)(clashes + 1);

            individual.SetFitness(fitness);

            return fitness;
        }

        /**
         * Evaluate population
         * 
         * @param population
         * @param timetable
         */
        public void EvalPopulation(Population population, Timetable timetable)
        {
            double populationFitness = 0;

            // Loop over population evaluating individuals and summing population
            // fitness
            foreach (var individual in population.GetIndividuals())
            {
                populationFitness += CalcFitness(individual, timetable);
            }

            population.SetPopulationFitness(populationFitness);
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
         * @param population
         * @param timetable
         * @return The mutated population
         */
        public Population MutatePopulation(Population population, Timetable timetable)
        {
            var rnd = new Random();
            // Initialize new population
            var newPopulation = new Population(_populationSize);

            // Loop over current population by fitness
            for (var populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                var individual = population.GetFittest(populationIndex);

                // Create random individual to swap genes with
                var randomIndividual = new Individual(timetable);

                // Loop over individual's genes
                for (var geneIndex = 0; geneIndex < individual.GetChromosomeLength(); geneIndex++)
                {
                    // Skip mutation if this is an elite individual
                    if (populationIndex > _elitismCount)
                    {
                        // Does this gene need mutation?
                        if (_mutationRate > rnd.NextDouble())
                        {
                            // Swap for new gene
                            individual.SetGene(geneIndex, randomIndividual.GetGene(geneIndex));
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
         * Apply crossover to population
         * 
         * @param population The population to apply crossover to
         * @return The new population
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

                    // Loop over genome
                    for (var geneIndex = 0; geneIndex < parent1.GetChromosomeLength(); geneIndex++)
                    {
                        // Use half of parent1's genes and half of parent2's genes
                        if (0.5 > rnd.NextDouble())
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
