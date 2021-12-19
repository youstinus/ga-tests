using System;
using System.Linq;

namespace ga_tsp
{
    public class Population
    {
        private Individual[] _population;
        private double _populationFitness = -1;

        /**
         * Initializes blank population of individuals
         * 
         * @param populationSize
         *            The size of the population
         */
        public Population(int populationSize)
        {
            // Initial population
            _population = new Individual[populationSize];
        }

        /**
         * Initializes population of individuals
         * 
         * @param populationSize
         *            The size of the population
         * @param chromosomeLength
         *            The length of the individuals chromosome
         */
        public Population(int populationSize, int chromosomeLength)
        {
            // Initial population
            _population = new Individual[populationSize];

            // Loop over population size
            for (var individualCount = 0; individualCount < populationSize; individualCount++)
            {
                // Create individual
                var individual = new Individual(chromosomeLength);
                // Add individual to population
                _population[individualCount] = individual;
            }
        }

        /**
         * Get individuals from the population
         * 
         * @return individuals Individuals in population
         */
        public Individual[] GetIndividuals()
        {
            return _population;
        }

        /**
         * Find fittest individual in the population
         * 
         * @param offset
         * @return individual Fittest individual at offset
         */
        public Individual GetFittest(int offset)
        {
            // Order population by fitness
            /*Arrays.sort(_population, new Comparator<Individual>() {

            public int compare(Individual o1, Individual o2)
            {
                if (o1.getFitness() > o2.getFitness())
                {
                    return -1;
                }
                else if (o1.getFitness() < o2.getFitness())
                {
                    return 1;
                }
                return 0;
            }
        });*/

            // Return the fittest individual
            return _population[offset];
        }

        /**
         * Set population's fitness
         * 
         * @param fitness
         *            The population's total fitness
         */
        public void SetPopulationFitness(double fitness)
        {
            _populationFitness = fitness;
        }

        /**
         * Get population's fitness
         * 
         * @return populationFitness The population's total fitness
         */
        public double GetPopulationFitness()
        {
            return _populationFitness;
        }

        /**
         * Get population's size
         * 
         * @return size The population's size
         */
        public int Size()
        {
            return _population.Length;
        }

        /**
         * Set individual at offset
         * 
         * @param individual
         * @param offset
         * @return individual
         */
        public Individual SetIndividual(int offset, Individual individual)
        {
            return _population[offset] = individual;
        }

        /**
         * Get individual at offset
         * 
         * @param offset
         * @return individual
         */
        public Individual GetIndividual(int offset)
        {
            return _population[offset];
        }

        /**
         * Shuffles the population in-place
         * 
         * @param void
         * @return void
         */
        public void Shuffle()
        {
            var rnd = new Random();
            for (var i = _population.Length - 1; i > 0; i--)
            {
                var index = rnd.Next(i + 1);
                var a = _population[index];
                _population[index] = _population[i];
                _population[i] = a;
            }
        }


        public void Sort()
        {
            _population = _population.OrderByDescending(x => x.GetFitness()).ToArray();
        }
    }
}
