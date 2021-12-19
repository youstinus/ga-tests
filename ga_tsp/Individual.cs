using System.Linq;

namespace ga_tsp
{
    public class Individual
    {

        /**
         * In this case, the chromosome is an array of integers rather than a string. 
         */
        private readonly int[] _chromosome;
        private double _fitness = -1;

        /**
         * Initializes individual with specific chromosome
         * 
         * @param chromosome
         *            The chromosome to give individual
         */
        public Individual(int[] chromosome)
        {
            // Create individualchromosome
            _chromosome = chromosome;
        }

        /**
         * Initializes random individual
         * 
         * @param chromosomeLength
         *            The length of the individuals chromosome
         */
        public Individual(int chromosomeLength)
        {
            // Create random individual
            var individual = new int[chromosomeLength];

            /**
             * In this case, we can no longer simply pick 0s and 1s -- we need to
             * use every city index available. We also don't need to randomize or
             * shuffle this chromosome, as crossover and mutation will ultimately
             * take care of that for us.
             */
            for (var gene = 0; gene < chromosomeLength; gene++)
            {
                individual[gene] = gene;
            }

            _chromosome = individual;
        }

        /**
         * Gets individual's chromosome
         * 
         * @return The individual's chromosome
         */
        public int[] GetChromosome()
        {
            return _chromosome;
        }

        /**
         * Gets individual's chromosome length
         * 
         * @return The individual's chromosome length
         */
        public int GetChromosomeLength()
        {
            return _chromosome.Length;
        }

        /**
         * Set gene at offset
         * 
         * @param gene
         * @param offset
         */
        public void SetGene(int offset, int gene)
        {
            _chromosome[offset] = gene;
        }

        /**
         * Get gene at offset
         * 
         * @param offset
         * @return gene
         */
        public int GetGene(int offset)
        {
            return _chromosome[offset];
        }

        /**
         * Store individual's fitness
         * 
         * @param fitness
         *            The individuals fitness
         */
        public void SetFitness(double fitness)
        {
            _fitness = fitness;
        }

        /**
         * Gets individual's fitness
         * 
         * @return The individual's fitness
         */
        public double GetFitness()
        {
            return _fitness;
        }

        public override string ToString()
        {
            return _chromosome.Aggregate("", (current, t) => current + (t + ","));
        }

        /**
         * Search for a specific integer gene in this individual.
         * 
         * For instance, in a Traveling Salesman Problem where cities are encoded as
         * integers with the range, say, 0-99, this method will check to see if the
         * city "42" exists.
         * 
         * @param gene
         * @return
         */
        public bool ContainsGene(int gene)
        {
            return _chromosome.Any(t => t == gene);
        }
    }
}
