using System;

namespace ga_schedule
{
    internal class Program
    {
        private static void Main()
        {
            // Get a Timetable object with all the available information.
            var timetable = InitializeTimetable();

            // Initialize GA
            var ga = new GeneticAlgorithm(100, 0.01, 0.9, 2, 5);

            // Initialize population
            var population = ga.InitPopulation(timetable);

            // Evaluate population
            ga.EvalPopulation(population, timetable);

            // Keep track of current generation
            var generation = 1;

            // Start evolution loop
            while (ga.IsTerminationConditionMet(generation, 1000) == false
                && ga.IsTerminationConditionMet(population) == false)
            {
                // Print fitness
                Console.WriteLine("G" + generation + " Best fitness: " + population.GetFittest(0).GetFitness());

                // Apply crossover
                population = ga.CrossoverPopulation(population);

                // Apply mutation
                population = ga.MutatePopulation(population, timetable);

                // Evaluate population
                ga.EvalPopulation(population, timetable);

                // Increment the current generation
                generation++;
            }

            // Print fitness
            timetable.CreateClasses(population.GetFittest(0));
            Console.WriteLine();
            Console.WriteLine("Solution found in " + generation + " generations");
            Console.WriteLine("Final solution fitness: " + population.GetFittest(0).GetFitness());
            Console.WriteLine("Clashes: " + timetable.CalcClashes());

            // Print classes
            Console.WriteLine();
            var classes = timetable.GetClasses();
            var classIndex = 1;
            foreach (var bestClass in classes)
            {
                Console.WriteLine("Class " + classIndex + ":");
                Console.WriteLine("Module: " +
                        timetable.GetModule(bestClass.GetModuleId()).GetModuleName());
                Console.WriteLine("Group: " +
                        timetable.GetGroup(bestClass.GetGroupId()).GetGroupId());
                Console.WriteLine("Room: " +
                        timetable.GetRoom(bestClass.GetRoomId()).GetRoomNumber());
                Console.WriteLine("Professor: " +
                        timetable.GetProfessor(bestClass.GetProfessorId()).GetProfessorName());
                Console.WriteLine("Time: " +
                        timetable.GetTimeslot(bestClass.GetTimeslotId()).GetTimeslot());
                Console.WriteLine("-----");
                classIndex++;
            }

            Console.ReadKey();
        }


        /**
     * Creates a Timetable with all the necessary course information.
     * 
     * Normally you'd get this info from a database.
     * 
     * @return
     */
        private static Timetable InitializeTimetable()
        {
            // Create timetable
            var timetable = new Timetable();

            // Set up rooms
            timetable.AddRoom(1, "A1", 15);
            timetable.AddRoom(2, "B1", 30);
            timetable.AddRoom(4, "D1", 20);
            timetable.AddRoom(5, "F1", 25);

            // Set up timeslots
            timetable.AddTimeslot(1, "Mon 9:00 - 11:00");
            timetable.AddTimeslot(2, "Mon 11:00 - 13:00");
            timetable.AddTimeslot(3, "Mon 13:00 - 15:00");
            timetable.AddTimeslot(4, "Tue 9:00 - 11:00");
            timetable.AddTimeslot(5, "Tue 11:00 - 13:00");
            timetable.AddTimeslot(6, "Tue 13:00 - 15:00");
            timetable.AddTimeslot(7, "Wed 9:00 - 11:00");
            timetable.AddTimeslot(8, "Wed 11:00 - 13:00");
            timetable.AddTimeslot(9, "Wed 13:00 - 15:00");
            timetable.AddTimeslot(10, "Thu 9:00 - 11:00");
            timetable.AddTimeslot(11, "Thu 11:00 - 13:00");
            timetable.AddTimeslot(12, "Thu 13:00 - 15:00");
            timetable.AddTimeslot(13, "Fri 9:00 - 11:00");
            timetable.AddTimeslot(14, "Fri 11:00 - 13:00");
            timetable.AddTimeslot(15, "Fri 13:00 - 15:00");

            // Set up professors
            timetable.AddProfessor(1, "Dr P Smith");
            timetable.AddProfessor(2, "Mrs E Mitchell");
            timetable.AddProfessor(3, "Dr R Williams");
            timetable.AddProfessor(4, "Mr A Thompson");

            // Set up modules and define the professors that teach them
            timetable.AddModule(1, "cs1", "Computer Science", new[] { 1, 2 });
            timetable.AddModule(2, "en1", "English", new[] { 1, 3 });
            timetable.AddModule(3, "ma1", "Maths", new[] { 1, 2 });
            timetable.AddModule(4, "ph1", "Physics", new[] { 3, 4 });
            timetable.AddModule(5, "hi1", "History", new[] { 4 });
            timetable.AddModule(6, "dr1", "Drama", new[] { 1, 4 });

            // Set up student groups and the modules they take.
            timetable.AddGroup(1, 10, new[] { 1, 3, 4 });
            timetable.AddGroup(2, 30, new[] { 2, 3, 5, 6 });
            timetable.AddGroup(3, 18, new[] { 3, 4, 5 });
            timetable.AddGroup(4, 25, new[] { 1, 4 });
            timetable.AddGroup(5, 20, new[] { 2, 3, 5 });
            timetable.AddGroup(6, 22, new[] { 1, 4, 5 });
            timetable.AddGroup(7, 16, new[] { 1, 3 });
            timetable.AddGroup(8, 18, new[] { 2, 6 });
            timetable.AddGroup(9, 24, new[] { 1, 6 });
            timetable.AddGroup(10, 25, new[] { 3, 4 });
            return timetable;
        }
    }
}
