using System;
using System.Collections.Generic;
using System.Linq;
using gase.Interfaces;

namespace gase
{
    public class Individual<T> : IIndividual<T>
    {

        /**
         * In this case, the chromosome is an array of integers rather than a string. 
         */
        private readonly T[] _chromosome;
        private double _fitness = -1;

        /**
         * Initializes random individual
         * 
         * The book instructs you to copy this constructor over from Chapter 4. This
         * case is a little tricky -- used in Chapter 4, this constructor will
         * create a valid chromosome for a list of cities for the TSP, by using each
         * city once and only once.
         * 
         * If used in Chapter 5, however, this will create an utterly INVALID
         * chromosome for the class scheduler. So you should not use this
         * constructor if you hope to create a valid random individual. For that
         * purpose, use the Individual(Timetable) constructor, which will create a
         * valid Individual from the fixed information in the Timetable object.
         * 
         * However, Chapter 5 still needs an Individual(int) constructor that
         * creates an Individual with a chromosome of a given size. It's used in the
         * crossoverPopulation method in order to initialize the offspring. The fact
         * that this creates an invalid Individual doesn't matter in this case,
         * because the crossover algorithm immediately rewrites the whole
         * chromosome.
         * 
         * 
         * @param chromosomeLength
         *            The length of the individuals chromosome
         */
        public Individual(int chromosomeLength)
        {
            // Create random individual
            var individual = new T[chromosomeLength];
            _chromosome = individual;
        }

        public Individual(int chromosomeLength, IReadOnlyList<T> posibleItems)
        {
            var rnd = new Random();
            // Create random individual
            var individual = new T[chromosomeLength];

            /**
             * This comment and the for loop doesn't make sense for this chapter.
             * But I'm leaving it in here because you were instructed to copy this
             * class from Chapter 4 -- and NOT having this comment here might be
             * more confusing than keeping it in.
             * 
             * Comment from Chapter 4:
             * 
             * "In this case, we can no longer simply pick 0s and 1s -- we need to
             * use every city index available. We also don't need to randomize or
             * shuffle this chromosome, as crossover and mutation will ultimately
             * take care of that for us."
             */
            for (var i = 0; i < chromosomeLength; i++)
            {
                var genns = posibleItems[rnd.Next(0, posibleItems.Count)];
                while (ContainsGene2(genns, individual))
                {
                    genns = posibleItems[rnd.Next(0, posibleItems.Count)];
                }

                individual[i] = genns;
            }
                

            _chromosome = individual;
        }

        /**
         * Initializes individual with specific chromosome
         * 
         * @param chromosome
         *            The chromosome to give individual
         */
        public Individual(T[] chromosome)
        {
            // Create individual chromosome
            _chromosome = chromosome;
        }

        /**
         * Gets individual's chromosome
         * 
         * @return The individual's chromosome
         */
        public T[] GetChromosome()
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
        public void SetGene(int offset, T gene)
        {
            _chromosome[offset] = gene;
        }

        /**
         * Get gene at offset
         * 
         * @param offset
         * @return gene
         */
        public T GetGene(int offset)
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
        public bool ContainsGene(T gene)
        {
            return _chromosome.Any(gener => gener != null && gener.Equals(gene));

            //return _chromosome.Any(t => t.Equals(gene));
        }

        public bool ContainsGene2(T gene, T[] chrome)
        {
            return chrome.Any(gener => gener != null && gener.Equals(gene));

            //return _chromosome.Any(t => t.Equals(gene));
        }

        public T FindBestGene(T[] parent1, T[] parent2)
        {
            for (var i = _chromosome.Length - 1; i >= 0; i--)
            {
                if (!ContainsGene(parent1[i]))
                {
                    return parent1[i];
                }
                if (!ContainsGene(parent2[i]))
                {
                    return parent2[i];
                }
            }

            return parent1[0];
        }
    }
}
