using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public class Shared
    {
        public delegate void Caller(int floor);
        public delegate void ElevatorArrivedAt(int floor);
        public enum ACCESS_LEVEL { CONFIDENTIAL, SECRET , TOP_SECRET  };
        public enum ELEVATOR_STATE { MOVING,WAITING_FOR_CALL,OCCUPIED,WAITING_FOR_PPL}
    }
}
