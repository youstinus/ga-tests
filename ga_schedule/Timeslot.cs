namespace ga_schedule
{
    public class Timeslot
    {
        private readonly int _timeslotId;
        private readonly string _timeslot;

        /**
         * Initalize new Timeslot
         * 
         * @param timeslotId The ID for this timeslot
         * @param timeslot The timeslot being initalized
         */
        public Timeslot(int timeslotId, string timeslot)
        {
            _timeslotId = timeslotId;
            _timeslot = timeslot;
        }

        /**
         * Returns the timeslotId
         * 
         * @return timeslotId
         */
        public int GetTimeslotId()
        {
            return _timeslotId;
        }

        /**
         * Returns the timeslot
         * 
         * @return timeslot
         */
        public string GetTimeslot()
        {
            return _timeslot;
        }
    }

}
