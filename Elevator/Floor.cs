using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Elevator.Shared;

namespace Elevator
{
    public class Floor
    {
        public string Name { get; set; }
        public ACCESS_LEVEL NeededLevel { get; set; }

        public Floor() { }
        public Floor(string name, ACCESS_LEVEL level) {
            this.Name = name;
            this.NeededLevel = level;
        }
    }
}
