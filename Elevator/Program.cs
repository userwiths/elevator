using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Elevator.Shared;

namespace Elevator
{
    class Program
    {
        static void Main(string[] args)
        {
            Building build = new Building(
                new string[] { "Lobby","G", "S", "T1", "T2" },
                new ACCESS_LEVEL[] {
                    ACCESS_LEVEL.CONFIDENTIAL,
                    ACCESS_LEVEL.CONFIDENTIAL,
                    ACCESS_LEVEL.SECRET,
                    ACCESS_LEVEL.TOP_SECRET,
                    ACCESS_LEVEL.TOP_SECRET
                }
            );

            build.elevators.Add(new Elevator(build.floors,new ElevatorArrivedAt(build.ArrivedAtMethod)));

            build.PopulateWithPersonel(1, 1, 1);

            foreach (var item in build.getThreads())
            {
                item.Join();
            }
            Console.ReadKey();
        }
    }
}
