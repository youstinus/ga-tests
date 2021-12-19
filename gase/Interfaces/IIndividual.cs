namespace gase.Interfaces
{
    public interface IIndividual<T>
    {
        /**
         * Gets individual's chromosome
         * 
         * @return The individual's chromosome
         */
        T[] GetChromosome();

        /**
         * Gets individual's chromosome length
         * 
         * @return The individual's chromosome length
         */
        int GetChromosomeLength();

        /**
         * Set gene at offset
         * 
         * @param gene
         * @param offset
         */
        void SetGene(int offset, T gene);

        /**
         * Get gene at offset
         * 
         * @param offset
         * @return gene
         */
        T GetGene(int offset);

        /**
         * Store individual's fitness
         * 
         * @param fitness
         *            The individuals fitness
         */
        void SetFitness(double fitness);

        /**
         * Gets individual's fitness
         * 
         * @return The individual's fitness
         */
        double GetFitness();

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
        bool ContainsGene(T gene);

        bool ContainsGene2(T gene, T[] chrome);

        T FindBestGene(T[] parent1, T[] parent2);
    }
}
