namespace ga_schedule
{
    public class Class
    {
        private readonly int _classId;
        private readonly int _groupId;
        private readonly int _moduleId;
        private int _professorId;
        private int _timeslotId;
        private int _roomId;

        /**
         * Initialize new Class
         * 
         * @param classId
         * @param groupId
         * @param moduleId
         */
        public Class(int classId, int groupId, int moduleId)
        {
            _classId = classId;
            _moduleId = moduleId;
            _groupId = groupId;
        }

        /**
         * Add professor to class
         * 
         * @param professorId
         */
        public void AddProfessor(int professorId)
        {
            _professorId = professorId;
        }

        /**
         * Add timeslot to class
         * 
         * @param timeslotId
         */
        public void AddTimeslot(int timeslotId)
        {
            _timeslotId = timeslotId;
        }

        /**
         * Add room to class
         * 
         * @param roomId
         */
        public void SetRoomId(int roomId)
        {
            _roomId = roomId;
        }

        /**
         * Get classId
         * 
         * @return classId
         */
        public int GetClassId()
        {
            return _classId;
        }

        /**
         * Get groupId
         * 
         * @return groupId
         */
        public int GetGroupId()
        {
            return _groupId;
        }

        /**
         * Get moduleId
         * 
         * @return moduleId
         */
        public int GetModuleId()
        {
            return _moduleId;
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
         * Get timeslotId
         * 
         * @return timeslotId
         */
        public int GetTimeslotId()
        {
            return _timeslotId;
        }

        /**
         * Get roomId
         * 
         * @return roomId
         */
        public int GetRoomId()
        {
            return _roomId;
        }
    }
}
