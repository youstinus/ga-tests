using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace jega_test1_ga
{
    public class Utility
    {
        public static void ReadFile(List<Game> games)
        {
            try
            {
                var read = new StreamReader("..//..//..//data/jega.txt");
                //BufferedReader read = new BufferedReader(fileReader);
                var re = new Regex("Žaidimas nr. \\d+");
                //var format = new SimpleDateFormat("yyyy-MM-dd");
                string line;
                while ((line = read.ReadLine()) != null) // game number
                {
                    if (!re.Match(line).Success) continue;

                    var zaidas = new Game();
                    var indai = line.IndexOf("nr.", StringComparison.Ordinal) + 4;
                    var nr = int.Parse(line.Substring(indai, line.Length - indai));
                    read.ReadLine();
                    var data = DateTime.Parse(read.ReadLine()?.Substring(14)); // Game.GameDate
                    read.ReadLine(); // Skaiciai
                    read.ReadLine(); // Pagrindiniai skaiciai
                    line = read.ReadLine(); // Game.Numbers
                    var listas = new List<int>();
                    for (var i = 0; i < 11; i += 2)
                    {
                        if (line == null) continue;

                        var vienas = int.Parse(line.Substring(i, 2));
                        listas.Add(vienas);
                    }
                    line = read.ReadLine();
                    if (line != null && line[0] == 'P')
                    {
                        line = read.ReadLine();  // Game.ExtraNumber
                        var vienas = int.Parse(line);
                        zaidas.ExtraNumber = vienas;
                        read.ReadLine();
                    }
                    zaidas.GameDate = data;
                    zaidas.Numbers = listas;
                    zaidas.Nr = nr;
                    if (ValidateGame(zaidas))
                        games.Add(zaidas);
                    //System.out.println(nr);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static void Serialize(List<Game> games)
        {
            // To serialize the hashtable and its key/value pairs,  
            // you must first open a stream for writing. 
            // In this case, use a file stream.
            var fs = new FileStream("..//..//..//data//jega.dat", FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            var formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, games);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
        
        public static IEnumerable<Game> Deserialize()
        {
            List<Game> game;
            // Open the file containing the data that you want to deserialize.
            var fs = new FileStream("..//..//..//data//jega.dat", FileMode.Open);
            try
            {
                var formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and 
                // assign the reference to the local variable.
                game = (List<Game>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

            return game;
        }

        public static List<int> MakeList(int max)
        {
            var list = new List<int>();
            for (var i = 0; i < max; i++)
            {
                list.Add(i + 1);
            }

            Shuffle(list);
            return list;
        }

        public static void Shuffle(List<int> list)
        {
            var rnd = new Random();
            for (var i = list.Count - 1; i > 0; i--)
            {
                var index = rnd.Next(i + 1);
                var a = list[index];
                list[index] = list[i];
                list[i] = a;
            }
        }

        public static bool ValidateGame(Game game)
        {
            return game != null
                   && game.GameDate > DateTime.MinValue
                   && game.Nr > 0
                   && game.Numbers.Count == 6
                   && game.ExtraNumber > 0;
        }
    }
}
