namespace ga_schedule
{
    public class Room
    {
        private readonly int _roomId;
        private readonly string _roomNumber;
        private readonly int _capacity;

        /**
         * Initialize new Room
         * 
         * @param roomId
         *            The ID for this classroom
         * @param roomNumber
         *            The room number
         * @param capacity
         *            The room capacity
         */
        public Room(int roomId, string roomNumber, int capacity)
        {
            _roomId = roomId;
            _roomNumber = roomNumber;
            _capacity = capacity;
        }

        /**
         * Return roomId
         * 
         * @return roomId
         */
        public int GetRoomId()
        {
            return _roomId;
        }

        /**
         * Return room number
         * 
         * @return roomNumber
         */
        public string GetRoomNumber()
        {
            return _roomNumber;
        }

        /**
         * Return room capacity
         * 
         * @return capacity
         */
        public int GetRoomCapacity()
        {
            return _capacity;
        }
    }
}
