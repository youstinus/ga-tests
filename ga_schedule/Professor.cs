namespace ga_schedule
{
    public class Professor
    {
        private readonly int _professorId;
        private readonly string _professorName;

        /**
         * Initalize new Professor
         * 
         * @param professorId The ID for this professor
         * @param professorName The name of this professor
         */
        public Professor(int professorId, string professorName)
        {
            _professorId = professorId;
            _professorName = professorName;
        }

        /**
         * Get professorId
         * 
         * @return professorId
         */
        public int GetProfessorId()
        {
            return _professorId;
        }

        /**
         * Get professor's name
         * 
         * @return professorName
         */
        public string GetProfessorName()
        {
            return _professorName;
        }
    }

}
