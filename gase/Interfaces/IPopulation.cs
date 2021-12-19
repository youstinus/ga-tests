namespace gase.Interfaces
{
    public interface IPopulation<T>
    {
        Individual<T>[] GetIndividuals();

        /**
         * Find fittest individual in the population
         * 
         * @param offset
         * @return individual Fittest individual at offset
         */
        Individual<T> GetFittestByOffset(int offset);
        
        Individual<T> GetFittest();
        
        /**
         * Set population's fitness
         * 
         * @param fitness
         *            The population's total fitness
         */
        void SetPopulationFitness(double fitness);
        
        /**
         * Get population's fitness
         * 
         * @return populationFitness The population's total fitness
         */
        double GetPopulationFitness();

        /**
         * Get population's size
         * 
         * @return size The population's size
         */
        int Size();

        /**
         * Set individual at offset
         * 
         * @param individual
         * @param offset
         * @return individual
         */
        Individual<T> SetIndividual(int offset, Individual<T> individual);

        /**
         * Get individual at offset
         * 
         * @param offset
         * @return individual
         */
        Individual<T> GetIndividual(int offset);

        /**
         * Shuffles the population in-place
         * 
         * @param void
         * @return void
         */
        void Shuffle();

        void Sort();
    }
}
