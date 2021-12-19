using System;
using System.Collections.Generic;
using System.Linq;
using gase.Interfaces;

namespace gase
{
    public class Population<T> : IPopulation<T>
    {
        private Individual<T>[] _population;
        private double _populationFitness = -1;

        private readonly Random _rnd;
        /**
         * Initializes blank population of individuals
         * 
         * @param populationSize
         *            The size of the population
         */
        public Population(int populationSize)
        {
            _rnd = new Random();
            // Initial population
            _population = new Individual<T>[populationSize];
        }

        /**
         * Initializes population of individuals
         * 
         * @param populationSize The size of the population
         * @param timetable The timetable information
         */
        /*public Population(int populationSize, Timetable timetable)
        {
            // Initial population
            _population = new Individual[populationSize];

            // Loop over population size
            for (var individualCount = 0; individualCount < populationSize; individualCount++)
            {
                // Create individual
                var individual = new Individual(timetable);
                // Add individual to population
                _population[individualCount] = individual;
            }
        }*/


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
            _rnd = new Random();
            
            // Initial population
            _population = new Individual<T>[populationSize];

            // Loop over population size
            for (var individualCount = 0; individualCount < populationSize; individualCount++)
            {
                // Create individual
                var individual = new Individual<T>(chromosomeLength);
                // Add individual to population
                _population[individualCount] = individual;
            }
        }

        public Population(int populationSize, int chromosomeLength, List<T> pattern)
        {
            _rnd = new Random();

            // Initial population
            _population = new Individual<T>[populationSize];

            // Loop over population size
            for (var individualCount = 0; individualCount < populationSize; individualCount++)
            {
                // Create individual
                var individual = new Individual<T>(chromosomeLength, pattern);
                // Add individual to population
                _population[individualCount] = individual;
            }
        }

        /**
         * Get individuals from the population
         * 
         * @return individuals Individuals in population
         */
        public Individual<T>[] GetIndividuals()
        {
            return _population;
        }

        /**
         * Find fittest individual in the population
         * 
         * @param offset
         * @return individual Fittest individual at offset
         */
        public Individual<T> GetFittestByOffset(int offset)
        {
            //_population = _population.OrderByDescending(x => x.GetFitness()).ToArray();
            // Return the fittest individual
            return _population[offset];
        }

        public Individual<T> GetFittest()
        {
            return _population.OrderByDescending(x => x.GetFitness()).ToArray().First();
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
        public Individual<T> SetIndividual(int offset, Individual<T> individual)
        {
            return _population[offset] = individual;
        }

        /**
         * Get individual at offset
         * 
         * @param offset
         * @return individual
         */
        public Individual<T> GetIndividual(int offset)
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
            for (var i = _population.Length - 1; i > 0; i--)
            {
                var index = _rnd.Next(i + 1);
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
