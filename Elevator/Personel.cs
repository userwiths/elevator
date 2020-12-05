using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Elevator.Shared;

namespace Elevator
{
    public class Personel
    {
        
        public Shared.Caller callMethod;
        public static Random rnd = new Random();
        public Thread thread;

        private object llock = new object();
        private int personelstate = 0;
        private string message;

        public int floor=0;
        
        public ACCESS_LEVEL AccessLevel { get; set; }
        public int Id { get; set; }
        public int FloorIntention { get; set; }
        
        public int PersonelState {
            get {
                return this.personelstate;
            }
            set {
                this.personelstate = value;
                switch (personelstate) {
                    case 0:PersonelMessage = "has entered the building.";break;
                    case 1: PersonelMessage = "is waiting for the elevator."; Elevator.cl.Invoke(this.floor); break;
                    case 2: PersonelMessage = "got off from elevator."; break;
                    case 3: PersonelMessage = "exits the building."; this.Stop(); break;
                }
            }
        }
        public string PersonelMessage {
            get {
                return this.message;
            }
            set {
                this.message = value;
                Console.WriteLine("Personel number: "+this.Id+" "+message);
            }
        }

        public Personel() {
            FloorIntention = 0;
            floor = 0;
            Id = rnd.Next(0,1024);

            this.thread = new Thread(new ThreadStart(LifeCycle));
            this.thread.Start();
        }
        public Personel(ACCESS_LEVEL level):this() {
            this.AccessLevel = level;
        }
        public Personel(ACCESS_LEVEL level,Caller call):this(level)
        {
            this.AccessLevel = level;
            this.callMethod = call;
        }

        public void LifeCycle() {
            lock (llock)
            {
                PersonelState = 0;
                int fail_start = 0;
                var randNumber = rnd.Next(20);
                while (this.PersonelState != 3)
                {
                    randNumber = rnd.Next(20);
                    if (personelstate == 1) {
                        Thread.Sleep(1000);
                        if (fail_start++ > 20) {
                            personelstate = 3;
                        }
                       
                        continue;
                    }
                    if (randNumber > 19)
                    {
                        this.PersonelMessage = "Has work on the current floor.";
                    }
                    else
                    {
                        randNumber = rnd.Next(0,30);
                        if (randNumber < 5)
                        {
                            FloorIntention = 1;
                        }
                        else if (randNumber < 10)
                        {
                            FloorIntention = 2;
                        }
                        else if (randNumber < 15)
                        {
                            FloorIntention = 3;
                        }
                        else if (randNumber < 20)
                        {
                            FloorIntention = 4;
                        }
                        else
                        {
                            FloorIntention = 5;
                        }
                        this.PersonelState = 1;
                    }
                    if (rnd.Next(100) > 99)
                    {
                        this.PersonelState = 3;
                    }
                }
            }
        }
        public void ChooseAnotherFloor() {
            int randNumber = rnd.Next(20);
            int oldFloor = FloorIntention;
            while (FloorIntention == oldFloor)
            {
                if (randNumber < 5)
                {
                    FloorIntention = 1;
                }
                else if (randNumber < 10)
                {
                    FloorIntention = 2;
                }
                else if (randNumber >= 15)
                {
                    FloorIntention = 3;
                }
                else
                {
                    FloorIntention = 4;
                }
            }
        }

        public void Start() {
            this.thread = new Thread(new ThreadStart(LifeCycle));
            thread.Start();
        }

        public void Stop() {
            this.thread.Abort();
        }
    }
}
