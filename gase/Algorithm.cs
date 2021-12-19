using System;
using System.Collections.Generic;
using System.Linq;
using gase.Interfaces;

namespace gase
{
    public class GeneticAlgorithm<T> : IAlgorithm<T>
    {
        private readonly List<T> _posibleValues;
        private readonly int _populationSize;
        private readonly double _mutationRate;
        private readonly double _crossoverRate;
        private readonly int _elitismCount;
        protected int TournamentSize;
        private readonly Random _rnd;

        public GeneticAlgorithm(int populationSize, double mutationRate, double crossoverRate, int elitismCount, int tournamentSize, List<T> posibleValues)
        {
            _posibleValues = posibleValues;
            _rnd = new Random();
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
        public Population<T> InitPopulation(int chromosomeLength)
        {
            // Initialize population
            var population = new Population<T>(_populationSize, chromosomeLength);
            return population;
        }

        public Population<T> InitPopulation()
        {
            // Initialize population
            var population = new Population<T>(_populationSize, 6, _posibleValues);
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
        public bool IsTerminationConditionMet(Population<T> population)
        {
            population.Sort();
            return Math.Abs(population.GetFittestByOffset(0).GetFitness() - 1.0) < 0.00001;
        }

        /**
  * Check if population has met termination condition
  * 
  * For this simple problem, we know what a perfect solution looks like, so
  * we can simply stop evolving once we've reached a fitness of one.
  * 
  * @param population
  * @return boolean True if termination condition met, otherwise, false
  */
        public bool IsTerminationConditionMet2(Population<T> population)
        {
            return population.GetIndividuals().Any(individual => Math.Abs(individual.GetFitness() - 1) < 0.000001);
        }

        /**
         * Calculate individual's fitness value
         * 
         * @param individual
         * @param timetable
         * @return fitness
         */
        /*public double CalcFitness(Individual individual, Timetable timetable)
        {

            // Create new timetable object to use -- cloned from an existing timetable
            var threadTimetable = new Timetable(timetable);
            threadTimetable.CreateClasses(individual);

            // Calculate fitness
            var clashes = threadTimetable.CalcClashes();
            var fitness = 1 / (double)(clashes + 1);

            individual.SetFitness(fitness);

            return fitness;
        }*/


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
        public double CalcFitness(Individual<T> individual)
        {

            // Track number of correct genes
            var correctGenes = 0;

            // Loop over individual's genes
            for (var geneIndex = 0; geneIndex < individual.GetChromosomeLength(); geneIndex++)
            {
                // Add one fitness point for each "1" found
                if (individual.GetGene(geneIndex).Equals(1)) // fix ?
                {
                    correctGenes += 1;
                }
            }

            // Calculate fitness
            var fitness = (double)correctGenes / individual.GetChromosomeLength();

            // Store fitness
            individual.SetFitness(fitness);

            return fitness;
        }

        public double CalcFitnessByModel(Individual<T> individual, T[] pattern)
        {

            // Track number of correct genes
            var correctGenes = 0;

            // Loop over individual's genes
            for (var geneIndex = 0; geneIndex < individual.GetChromosomeLength(); geneIndex++)
            {
                // Add one fitness point for each "1" found
                if (pattern.Contains(individual.GetGene(geneIndex)))
                {
                    // add more then one in higher fitness ?
                    correctGenes += 1;
                }
            }

            // Calculate fitness
            var fitness = (double)correctGenes / individual.GetChromosomeLength();

            // Store fitness
            individual.SetFitness(fitness);

            return fitness;
        }

        public double CalcFitnessByModelWithoutSetting(Individual<T> individual, T[] pattern)
        {

            // Track number of correct genes
            var correctGenes = 0;

            // Loop over individual's genes
            for (var geneIndex = 0; geneIndex < individual.GetChromosomeLength(); geneIndex++)
            {
                // Add one fitness point for each "1" found
                if (pattern.Contains(individual.GetGene(geneIndex)))
                {
                    // add more then one in higher fitness ?
                    correctGenes += 1;
                }
            }

            return correctGenes;
            // Calculate fitness
            //var fitness = (double)correctGenes / individual.GetChromosomeLength();

            // Store fitness
            //individual.SetFitness(fitness);

            //return fitness;
        }



        /**
         * Evaluate population
         * 
         * @param population
         * @param timetable
         */
        /* public void EvalPopulation(Population population, Timetable timetable)
         {
             double populationFitness = 0;

             // Loop over population evaluating individuals and summing population
             // fitness
             foreach (var individual in population.GetIndividuals())
             {
                 populationFitness += CalcFitness(individual, timetable);
             }

             population.SetPopulationFitness(populationFitness);
         }*/


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
        public void EvalPopulation(Population<T> population)
        {
            var populationFitness = population.GetIndividuals().Sum(individual => CalcFitness(individual));

            // Loop over population evaluating individuals and suming population
            // fitness

            population.SetPopulationFitness(populationFitness);
        }

        public void EvalPopulationByModel(Population<T> population, T[] pattern)
        {
            var populationFitness = population.GetIndividuals().Sum(individual => CalcFitnessByModel(individual, pattern));

            // Loop over population evaluating individuals and suming population
            // fitness

            population.SetPopulationFitness(populationFitness);
        }


        // other methods
        public double GetIndividualsFitnessWithoutSetting(Individual<T> individual, T[] pattern)
        {
            return CalcFitnessByModelWithoutSetting(individual, pattern);
        }

        public double SumFitnessAndSetting(Individual<T> individual, List<T[]> patterns)
        {
            var populationFitness = patterns.Sum(x => GetIndividualsFitnessWithoutSetting(individual, x));
            var fitniss = populationFitness / (patterns.Count * individual.GetChromosomeLength());
            individual.SetFitness(fitniss);
            return fitniss;
        }

        public void EvalPopulationByModelList(Population<T> population, List<T[]> patterns)
        {
            var sum = population.GetIndividuals().Sum(individual => SumFitnessAndSetting(individual, patterns));
            population.SetPopulationFitness(sum);
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
        public Individual<T> SelectParent(Population<T> population)
        {
            // Create tournament
            var tournament = new Population<T>(TournamentSize);

            // Add random individuals to the tournament
            population.Shuffle();
            for (var i = 0; i < TournamentSize; i++)
            {
                var tournamentIndividual = population.GetIndividual(i);
                tournament.SetIndividual(i, tournamentIndividual);
            }

            tournament.Sort();
            // Return the best
            return tournament.GetFittestByOffset(0);
        }

        /**
        * Select parent for crossover
        * 
        * @param population
        *            The population to select parent from
        * @return The individual selected as a parent
        */
        public Individual<T> SelectParent2(Population<T> population)
        {
            // Get individuals
            var individuals = population.GetIndividuals();

            // Spin roulette wheel
            var populationFitness = population.GetPopulationFitness();
            var rouletteWheelPosition = _rnd.NextDouble() * populationFitness;

            // Find parent
            double spinWheel = 0;
            foreach (var individual in individuals)
            {
                spinWheel += individual.GetFitness();
                if (spinWheel >= rouletteWheelPosition)
                {
                    return individual;
                }
            }
            return individuals[population.Size() - 1];
        }


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
        public Population<T> MutatePopulationPickAvailable(Population<T> population)
        {
            // Initialize new population
            var newPopulation = new Population<T>(_populationSize);

            // Loop over current population by fitness
            for (var populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                var individual = population.GetFittestByOffset(populationIndex);

                // Loop over individual's genes
                for (var geneIndex = 0; geneIndex < individual.GetChromosomeLength(); geneIndex++)
                {
                    // Skip mutation if this is an elite individual
                    if (populationIndex > _elitismCount)
                    {
                        // Does this gene need mutation?
                        if (_mutationRate > _rnd.NextDouble())
                        {
                            // Get new gene
                            var newGene = _posibleValues[_rnd.Next(0, _posibleValues.Count)];
                            while (individual.ContainsGene(newGene))
                            {
                                newGene = _posibleValues[_rnd.Next(0, _posibleValues.Count)];
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
        public Population<T> CrossoverPopulationSomeOther(Population<T> population)
        {
            // Create new population
            var newPopulation = new Population<T>(population.Size());

            population.Sort();
            // Loop over current population by fitness
            for (var populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                var parent1 = population.GetFittestByOffset(populationIndex);

                // Apply crossover to this individual?
                if (_crossoverRate > _rnd.NextDouble() && populationIndex >= _elitismCount)
                {
                    // Initialize offspring
                    var offspring = new Individual<T>(parent1.GetChromosomeLength());

                    // Find second parent
                    var parent2 = SelectParent(population);

                    var len = parent2.GetChromosomeLength();
                    var point1 = _rnd.Next(0, len);

                    if (0.5 > _rnd.NextDouble())
                    {
                        for (var i = 0; i < point1; i++)
                        {
                            offspring.SetGene(i, parent1.GetGene(i));
                        }

                        for (var i = point1; i < len; i++)
                        {
                            var gynas = parent2.GetGene(i);
                            if (!offspring.ContainsGene(gynas))
                            {
                                offspring.SetGene(i, gynas);
                            }
                            else
                            {
                                offspring.SetGene(i, offspring.FindBestGene(parent1.GetChromosome(), parent2.GetChromosome()));
                            }
                        }
                    }
                    else
                    {
                        for (var i = 0; i < point1; i++)
                        {
                            offspring.SetGene(i, parent2.GetGene(i));
                        }

                        for (var i = point1; i < len; i++)
                        {
                            var gynas = parent1.GetGene(i);
                            if (!offspring.ContainsGene(gynas))
                            {
                                offspring.SetGene(i, gynas);
                            }
                            else
                            {
                                offspring.SetGene(i, offspring.FindBestGene(parent1.GetChromosome(), parent2.GetChromosome()));
                            }
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
