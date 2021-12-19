using System;
using System.Linq;

namespace ga_robot_controller
{
    /**
 * An "Individual" represents a single candidate solution. The core piece of
 * information about an individual is its "chromosome", which is an encoding of
 * a possible solution to the problem at hand. A chromosome can be a string, an
 * array, a list, etc -- in this class, the chromosome is an integer array. 
 * 
 * An individual position in the chromosome is called a gene, and these are the
 * atomic pieces of the solution that can be manipulated or mutated. When the
 * chromosome is a string, as in this case, each character or set of characters
 * can be a gene.
 * 
 * An individual also has a "fitness" score; this is a number that represents
 * how good a solution to the problem this individual is. The meaning of the
 * fitness score will vary based on the problem at hand.
 * 
 * @author bkanber
 *
 */
    public class Individual
    {
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
            // Create individual chromosome
            _chromosome = chromosome;
        }

        /**
         * Initializes random individual.
         * 
         * This constructor assumes that the chromosome is made entirely of 0s and
         * 1s, which may not always be the case, so make sure to modify as
         * necessary. This constructor also assumes that a "random" chromosome means
         * simply picking random zeroes and ones, which also may not be the case
         * (for instance, in a traveling salesman problem, this would be an invalid
         * solution).
         * 
         * @param chromosomeLength
         *            The length of the individuals chromosome
         */
        public Individual(int chromosomeLength)
        {
            var rnd = new Random();
            _chromosome = new int[chromosomeLength];
            for (var gene = 0; gene < chromosomeLength; gene++)
            {
                SetGene(gene, 0.5 < rnd.NextDouble() ? 1 : 0);
            }

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
         * @return gene
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


        /**
         * Display the chromosome as a string.
         * 
         * @return string representation of the chromosome
         */
        public override string ToString()
        {
            return _chromosome.Aggregate("", (current, t) => current + t);
        }
    }

}
