using System;
using System.Collections.Generic;
using System.Linq;

namespace jega_test1_ga
{
    class Program
    {
        private static void Main(string[] args)
        {
            //SerializeToFile();
            //OneOfTheMethodsToGetNumbas();
            AnotherNumbasMethod();


            Console.WriteLine("---END---");
            Console.ReadKey();
        }



        private static void AnotherNumbasMethod()
        {
            var games = Utility.Deserialize().Reverse().Where(x => x.Nr > 4000).ToList();
            /*Console.WriteLine("  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30");
            Console.WriteLine("------------------------------------------------------------------------------------------");
            */
            Console.WriteLine("123456789012345678901234567890");
            Console.WriteLine("------------------------------");
            foreach (var game in games)
            {
                /*for (var i = 0; i < 90; i++)
                {
                    if ((i + 1) % 3 == 0 && (game.Numbers.Contains((i + 1) / 3) || game.ExtraNumber == (i + 1) / 3))
                    {
                        Console.Write("@");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }*/
                for (var i = 0; i < 30; i++)
                {
                    if (game.Numbers.Contains(i + 1) || game.ExtraNumber == (i + 1))
                    {
                        Console.Write("@");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }


                Console.WriteLine();
            }
        }

        private static void OneOfTheMethodsToGetNumbas()
        {
            var rnd = new Random();
            var games = Utility.Deserialize().Reverse().ToList();
            for (var i = 0; i < 50; i++)
                Console.WriteLine("ofgames {0}  Sum- {1}", i, GetLastToFuture(rnd, games.Where(x => x.Nr > 5700).ToList(), 5, 50, 25));

        }

        private static void SerializeToFile()
        {
            var games = new List<Game>();
            Utility.ReadFile(games);
            Utility.Serialize(games);
        }

        private static int GetLastToFuture(Random rnd, IEnumerable<Game> games, int howManyCheck, int numberOfTickets,
            int takeThatManyNumberFromHisto)
        {
            var mainSum = 0;
            var chain = new LinkedList<Game>();
            foreach (var game in games)
            {
                if (chain.Count == howManyCheck)
                {
                    var histo = GetHistogram(chain);
                    var tickets =
                        GetSomeTickets(rnd, histo, numberOfTickets,
                            takeThatManyNumberFromHisto); //PopulateSomeTickets(histo);
                    var sum = tickets.Sum(x => TicketScore(x, game)) - tickets.Count;

                    //Console.WriteLine(sum);
                    mainSum += sum;
                    chain.RemoveFirst();
                }
                else if (chain.Count > howManyCheck)
                {
                    chain.RemoveFirst();
                }

                chain.AddLast(game);
            }

            return mainSum;
        }

        private static List<Ticket> GetSomeTickets(Random rnd, List<Number> histo, int numberOfTickets,
            int numberCountToTakeFromHisto) // add ticket count to create // add how many to take from histo
        {
            // percentage of some count of numbers e.g. wheel selection of numbers from half ofsmallests in histo
            var tickets = new List<Ticket>();
            histo = histo.OrderBy(x => x.Count).Take(numberCountToTakeFromHisto).ToList();
            for (var j = 0; j < numberOfTickets; j++) // number of tickets
            {
                var numn = new List<int>();

                var numba = SelectTicket(rnd, histo);

                while (numn.Count < 6)
                {
                    if (numn.Contains(numba))
                        numba = SelectTicket(rnd, histo);
                    else
                        numn.Add(numba);
                }

                tickets.Add(new Ticket() {Numbers = numn});
            }

            return tickets;
        }


        private static IEnumerable<Ticket> PopulateSomeTickets(List<Number> histo)
        {
            var tickets = new List<Ticket>();
            histo = histo.OrderBy(x => x.Count).ToList();
            for (var j = 0;
                j < 7;
                j++) // percentage of some count of numbers e.g. wheel selection of numbers from half ofsmallests in histo
            {
                var numn = new List<int>();
                for (var i = 0; i < 7; i++)
                {
                    if (i != j)
                        numn.Add(histo[i].Numba);
                }

                tickets.Add(new Ticket() {Numbers = numn});
            }

            return tickets;
        }

        private static int TicketScore(Ticket ticket, Game game)
        {
            if (ticket?.Numbers == null || ticket.Numbers.Count != 6)
                return 0;

            var score = 0;

            foreach (var ticketNumber in ticket.Numbers)
            {
                if (game.Numbers.Contains(ticketNumber))
                    score++;
            }

            switch (score)
            {
                case 6:
                    return 50000;

                case 5:
                    if (ticket.Numbers.Contains(game.ExtraNumber))
                    {
                        return 1000;
                    }

                    return 100;

                case 4:
                    if (ticket.Numbers.Contains(game.ExtraNumber))
                    {
                        return 20;
                    }

                    return 4;

                case 3:
                    if (ticket.Numbers.Contains(game.ExtraNumber))
                    {
                        return 2;
                    }

                    return 1;

                default:
                    return 0;
            }
        }

        private static List<Number> GetHistogram(IEnumerable<Game> games)
        {
            var histo = PopulateHistoNumbers();
            foreach (var game in games)
            {
                foreach (var number in game.Numbers)
                {
                    histo.First(x => x.Numba == number).Count++;
                }
            }

            return histo;
        }

        private static List<Number> PopulateHistoNumbers()
        {
            var numbers = new List<Number>();
            for (var i = 1; i < 31; i++)
            {
                numbers.Add(new Number() {Count = 0, Numba = i});
            }

            return numbers;
        }


        public static int SelectTicket(Random rnd, List<Number> numbers)
        {
            // Spin roulette wheel
            var fitness = numbers.Sum(x => x.Count);
            var rouletteWheelPosition = rnd.NextDouble();// * fitness;

            // Find parent
            double spinWheel = 0;
            foreach (var number in numbers)
            {
                spinWheel += (1.0 * number.Count / fitness);
                if (spinWheel >= rouletteWheelPosition)
                {
                    return number.Numba;
                }
            }

            return numbers[numbers.Count() - 1].Numba;
        }
    }
}