using System;
using System.Collections.Generic;
using System.Linq;

namespace ga_schedule
{
    /**
 * Timetable is the main evaluation class for the class scheduler GA.
 * 
 * A timetable represents a potential solution in human-readable form, unlike an
 * Individual or a chromosome. This timetable class, then, can read a chromosome
 * and develop a timetable from it, and ultimately can evaluate the timetable
 * for its fitness and number of scheduling clashes.
 * 
 * The most important methods in this class are createClasses and calcClashes.
 * 
 * The createClasses method accepts an Individual (really, a chromosome),
 * unpacks its chromosome, and creates Class objects from the genetic
 * information. Class objects are lightweight; they're just containers for
 * information with getters and setters, but it's more convenient to work with
 * them than with the chromosome directly.
 * 
 * The calcClashes method is used by GeneticAlgorithm.calcFitness, and requires
 * that createClasses has been run first. calcClashes looks at the Class objects
 * created by createClasses, and figures out how many hard constraints have been
 * violated.
 * 
 */
    public class Timetable
    {
        private readonly Dictionary<int, Room> _rooms;
        private readonly Dictionary<int, Professor> _professors;
        private readonly Dictionary<int, Module> _modules;
        private readonly Dictionary<int, Group> _groups;
        private readonly Dictionary<int, Timeslot> _timeslots;
        private Class[] _classes;
        private readonly Random _rnd;
        private int _numClasses;

        /**
         * Initialize new Timetable
         */
        public Timetable()
        {
            _rnd = new Random();
            _rooms = new Dictionary<int, Room>();
            _professors = new Dictionary<int, Professor>();
            _modules = new Dictionary<int, Module>();
            _groups = new Dictionary<int, Group>();
            _timeslots = new Dictionary<int, Timeslot>();
        }

        /**
         * "Clone" a timetable. We use this before evaluating a timetable so we have
         * a unique container for each set of classes created by "createClasses".
         * Truthfully, that's not entirely necessary (no big deal if we wipe out and
         * reuse the .classes property here), but Chapter 6 discusses
         * multi-threading for fitness calculations, and in order to do that we need
         * separate objects so that one thread doesn't step on another thread's
         * toes. So this constructor isn't _entirely_ necessary for Chapter 5, but
         * you'll see it in action in Chapter 6.
         * 
         * @param cloneable
         */
        public Timetable(Timetable cloneable)
        {
            _rnd = new Random();
            _rooms = cloneable.GetRooms();
            _professors = cloneable.GetProfessors();
            _modules = cloneable.GetModules();
            _groups = cloneable.GetGroups();
            _timeslots = cloneable.GetTimeslots();
        }

        private Dictionary<int, Group> GetGroups()
        {
            return _groups;
        }

        private Dictionary<int, Timeslot> GetTimeslots()
        {
            return _timeslots;
        }

        private Dictionary<int, Module> GetModules()
        {
            return _modules;
        }

        private Dictionary<int, Professor> GetProfessors()
        {
            return _professors;
        }

        /**
         * Add new room
         * 
         * @param roomId
         * @param roomName
         * @param capacity
         */
        public void AddRoom(int roomId, string roomName, int capacity)
        {
            _rooms.Add(roomId, new Room(roomId, roomName, capacity));
        }

        /**
         * Add new professor
         * 
         * @param professorId
         * @param professorName
         */
        public void AddProfessor(int professorId, string professorName)
        {
            _professors.Add(professorId, new Professor(professorId, professorName));
        }

        /**
         * Add new module
         * 
         * @param moduleId
         * @param moduleCode
         * @param module
         * @param professorIds
         */
        public void AddModule(int moduleId, string moduleCode, string module, int[] professorIds)
        {
            _modules.Add(moduleId, new Module(moduleId, moduleCode, module, professorIds));
        }

        /**
         * Add new group
         * 
         * @param groupId
         * @param groupSize
         * @param moduleIds
         */
        public void AddGroup(int groupId, int groupSize, int[] moduleIds)
        {
            _groups.Add(groupId, new Group(groupId, groupSize, moduleIds));
            _numClasses = 0;
        }

        /**
         * Add new timeslot
         * 
         * @param timeslotId
         * @param timeslot
         */
        public void AddTimeslot(int timeslotId, string timeslot)
        {
            _timeslots.Add(timeslotId, new Timeslot(timeslotId, timeslot));
        }

        /**
         * Create classes using individual's chromosome
         * 
         * One of the two important methods in this class; given a chromosome,
         * unpack it and turn it into an array of Class (with a capital C) objects.
         * These Class objects will later be evaluated by the calcClashes method,
         * which will loop through the Classes and calculate the number of
         * conflicting timeslots, rooms, professors, etc.
         * 
         * While this method is important, it's not really difficult or confusing.
         * Just loop through the chromosome and create Class objects and store them.
         * 
         * @param individual
         */
        public void CreateClasses(Individual individual)
        {
            // Init classes
            _classes = new Class[GetNumClasses()];

            // Get individual's chromosome
            var chromosome = individual.GetChromosome();
            var chromosomePos = 0;
            var classIndex = 0;

            foreach (var group in GetGroupsAsArray())
            {
                var moduleIds = group.GetModuleIds();
                foreach (var moduleId in moduleIds)
                {
                    _classes[classIndex] = new Class(classIndex, group.GetGroupId(), moduleId);

                    // Add timeslot
                    _classes[classIndex].AddTimeslot(chromosome[chromosomePos]);
                    chromosomePos++;

                    // Add room
                    _classes[classIndex].SetRoomId(chromosome[chromosomePos]);
                    chromosomePos++;

                    // Add professor
                    _classes[classIndex].AddProfessor(chromosome[chromosomePos]);
                    chromosomePos++;

                    classIndex++;
                }
            }
        }

        /**
         * Get room from roomId
         * 
         * @param roomId
         * @return room
         */
        public Room GetRoom(int roomId)
        {
            if (!_rooms.ContainsKey(roomId))
            {
                Console.WriteLine("Rooms doesn't contain key " + roomId);
            }
            return _rooms[roomId];
        }

        public Dictionary<int, Room> GetRooms()
        {
            return _rooms;
        }

        /**
         * Get random room
         * 
         * @return room
         */
        public Room GetRandomRoom()
        {
            var roomsArray = _rooms.Values.ToArray();
            var room = roomsArray[(int)(roomsArray.Length * _rnd.NextDouble())];
            return room;
        }

        /**
         * Get professor from professorId
         * 
         * @param professorId
         * @return professor
         */
        public Professor GetProfessor(int professorId)
        {
            return _professors[professorId];
        }

        /**
         * Get module from moduleId
         * 
         * @param moduleId
         * @return module
         */
        public Module GetModule(int moduleId)
        {
            return _modules[moduleId];
        }

        /**
         * Get moduleIds of student group
         * 
         * @param groupId
         * @return moduleId array
         */
        public int[] GetGroupModules(int groupId)
        {
            var group = _groups[groupId];
            return group.GetModuleIds();
        }

        /**
         * Get group from groupId
         * 
         * @param groupId
         * @return group
         */
        public Group GetGroup(int groupId)
        {
            return _groups[groupId];
        }

        /**
         * Get all student groups
         * 
         * @return array of groups
         */
        public Group[] GetGroupsAsArray()
        {
            return _groups.Values.ToArray(); //.toArray(new Group[_groups.size()]);
        }

        /**
         * Get timeslot by timeslotId
         * 
         * @param timeslotId
         * @return timeslot
         */
        public Timeslot GetTimeslot(int timeslotId)
        {
            return _timeslots[timeslotId];
        }

        /**
         * Get random timeslotId
         * 
         * @return timeslot
         */
        public Timeslot GetRandomTimeslot()
        {
            var timeslotArray = _timeslots.Values.ToArray();
            var timeslot = timeslotArray[(int)(timeslotArray.Length * _rnd.NextDouble())];
            return timeslot;
        }

        /**
         * Get classes
         * 
         * @return classes
         */
        public Class[] GetClasses()
        {
            return _classes;
        }

        /**
         * Get number of classes that need scheduling
         * 
         * @return numClasses
         */
        public int GetNumClasses()
        {
            if (_numClasses > 0)
            {
                return _numClasses;
            }

            var numClasses = 0;
            var groups2 = _groups.Values.ToArray();
            foreach (var group in groups2)
            {
                numClasses += group.GetModuleIds().Length;
            }
            _numClasses = numClasses;

            return _numClasses;
        }

        /**
         * Calculate the number of clashes between Classes generated by a
         * chromosome.
         * 
         * The most important method in this class; look at a candidate timetable
         * and figure out how many constraints are violated.
         * 
         * Running this method requires that createClasses has been run first (in
         * order to populate this.classes). The return value of this method is
         * simply the number of constraint violations (conflicting professors,
         * timeslots, or rooms), and that return value is used by the
         * GeneticAlgorithm.calcFitness method.
         * 
         * There's nothing too difficult here either -- loop through this.classes,
         * and check constraints against the rest of the this.classes.
         * 
         * The two inner `for` loops can be combined here as an optimization, but
         * kept separate for clarity. For small values of this.classes.length it
         * doesn't make a difference, but for larger values it certainly does.
         * 
         * @return numClashes
         */
        public int CalcClashes()
        {
            var clashes = 0;

            foreach (var classA in _classes)
            {
                // Check room capacity
                var roomCapacity = GetRoom(classA.GetRoomId()).GetRoomCapacity();
                var groupSize = GetGroup(classA.GetGroupId()).GetGroupSize();

                if (roomCapacity < groupSize)
                {
                    clashes++;
                }

                // Check if room is taken
                foreach (var classB in _classes)
                {
                    if (classA.GetRoomId() == classB.GetRoomId() && classA.GetTimeslotId() == classB.GetTimeslotId()
                            && classA.GetClassId() != classB.GetClassId())
                    {
                        clashes++;
                        break;
                    }
                }

                // Check if professor is available
                foreach (var classB in _classes)
                {
                    if (classA.GetProfessorId() == classB.GetProfessorId() && classA.GetTimeslotId() == classB.GetTimeslotId()
                            && classA.GetClassId() != classB.GetClassId())
                    {
                        clashes++;
                        break;
                    }
                }
            }

            return clashes;
        }
    }
}
