namespace ga_schedule
{
    public class Group
    {
        private readonly int _groupId;
        private readonly int _groupSize;
        private readonly int[] _moduleIds;

        /**
         * Initialize Group
         * 
         * @param groupId
         * @param groupSize
         * @param moduleIds
         */
        public Group(int groupId, int groupSize, int[] moduleIds)
        {
            _groupId = groupId;
            _groupSize = groupSize;
            _moduleIds = moduleIds;
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
         * Get groupSize
         * 
         * @return groupSize
         */
        public int GetGroupSize()
        {
            return _groupSize;
        }

        /**
         * Get array of group's moduleIds
         * 
         * @return moduleIds
         */
        public int[] GetModuleIds()
        {
            return _moduleIds;
        }
    }

}
