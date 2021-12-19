using System;
using System.Collections.Generic;

namespace keno_test1_ga
{
    [Serializable]
    public class Game
    {
        public int Nr { get; set; }
        public DateTime GameDate { get; set; }
        public List<int> Numbers { get; set; }
    }
}
