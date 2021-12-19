﻿using System.Linq;

namespace ga_schedule
{
    public class Individual
    {

        /**
         * In this case, the chromosome is an array of integers rather than a string. 
         */
        private readonly int[] _chromosome;
        private double _fitness = -1;

        /**
         * Initializes random individual based on a timetable
         * 
         * The Timetable class is a bit overloaded. It knows both fixed information
         * (the courses that MUST be scheduled, the professors that MUST be given
         * jobs, the classrooms that DO exist) -- but it also understands how to
         * parse and unpack chromosomes which contain variable information (which
         * professor teaches which class and when?)
         * 
         * In this case, we use the Timetable for the fixed information only, and
         * generate a random chromosome, making guesses at the variable information.
         * 
         * Given the fixed information in a Timetable, we create a chromosome that
         * randomly assigns timeslots, rooms, and professors to the chromosome for
         * each student group and module.
         * 
         * @param timetable
         *            The timetable information
         */
        public Individual(Timetable timetable)
        {
            int numClasses = timetable.GetNumClasses();

            // 1 gene for room, 1 for time, 1 for professor
            int chromosomeLength = numClasses * 3;
            // Create random individual
            int[] newChromosome = new int[chromosomeLength];
            int chromosomeIndex = 0;
            // Loop through groups
            foreach (Group group in timetable.GetGroupsAsArray())
            {
                // Loop through modules
                foreach (int moduleId in group.GetModuleIds())
                {
                    // Add random time
                    int timeslotId = timetable.GetRandomTimeslot().GetTimeslotId();
                    newChromosome[chromosomeIndex] = timeslotId;
                    chromosomeIndex++;

                    // Add random room
                    int roomId = timetable.GetRandomRoom().GetRoomId();
                    newChromosome[chromosomeIndex] = roomId;
                    chromosomeIndex++;

                    // Add random professor
                    Module module = timetable.GetModule(moduleId);
                    newChromosome[chromosomeIndex] = module.GetRandomProfessorId();
                    chromosomeIndex++;
                }
            }

            _chromosome = newChromosome;
        }

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
            var individual = new int[chromosomeLength];

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
            for (int gene = 0; gene < chromosomeLength; gene++)
            {
                individual[gene] = gene;
            }

            _chromosome = individual;
        }

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
            for (int i = 0; i < _chromosome.Length; i++)
            {
                if (_chromosome[i] == gene)
                {
                    return true;
                }
            }
            return false;
        }



    }

}
