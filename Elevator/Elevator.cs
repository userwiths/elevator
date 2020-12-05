using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Elevator.Shared;

namespace Elevator
{
    public class Elevator
    {
        public Thread thread;
        public static Shared.Caller cl ;
        private object llock = new object();

        public ElevatorArrivedAt OnStop;
        public ELEVATOR_STATE State { get; set; }
        public int CurrentIndex { get; set; }
        public ConcurrentQueue<int> CallOriginFloor { get; set; }
        public List<Floor> Floors { get; set; }
        private List<Personel> people;

        public Elevator() {
            this.CurrentIndex = 0;
            CallOriginFloor = new ConcurrentQueue<int>();

            Floors = new List<Floor>();
            this.people = new List<Personel>();
            cl = new Shared.Caller(Call);
            this.State = ELEVATOR_STATE.WAITING_FOR_CALL;

            this.thread = new Thread(new ThreadStart(LifeCycle));
            this.thread.Start();
        }
        public Elevator(IEnumerable<Floor> floors):this() {
            this.Floors.AddRange(floors);
        }
        public Elevator(IEnumerable<Floor> floors,ElevatorArrivedAt onstop) : this(floors)
        {
            this.OnStop = onstop;
        }

        //Run in another thread.
        public void LifeCycle() {
            int floor=0;
            bool flag = false;
            Caller moveTo = (x)=>{
                while (CurrentIndex != x)
                {
                    Thread.Sleep(1000);
                    if (CurrentIndex > x)
                    {
                        CurrentIndex--;
                    }
                    else
                    {
                        CurrentIndex++;
                    }
                }
            };
            while (true) {
                if (State == ELEVATOR_STATE.WAITING_FOR_CALL && CallOriginFloor.IsEmpty) {
                    Thread.Sleep(500);
                    continue;
                }
                if (State != ELEVATOR_STATE.OCCUPIED) {
                    if (this.CallOriginFloor.TryDequeue(out floor))
                    {
                        moveTo(floor);
                        this.State = ELEVATOR_STATE.WAITING_FOR_PPL;
                        OnStop.Invoke(CurrentIndex);
                    }
                    else {
                        this.State = ELEVATOR_STATE.WAITING_FOR_CALL;
                    }
                }
                if (State == ELEVATOR_STATE.OCCUPIED) {
                    foreach (var person in this.people.Where(x=>x.PersonelState==1).OrderBy(x => x.FloorIntention))
                    {
                        moveTo(person.FloorIntention);
                        if (canAccess(person, this.Floors[CurrentIndex - 1])){
                            Console.WriteLine(
                            "Personel number " + person.Id + " with "
                            + person.AccessLevel.ToString() + " left the elevator at "
                            + this.Floors[CurrentIndex - 1].Name + " floor."
                        );
                            person.PersonelState = 2;
                        }
                        else
                        {
                            flag = true;
                            person.ChooseAnotherFloor();
                        }
                    }
                    if (!flag) {
                        this.people.Clear();
                        this.State = ELEVATOR_STATE.WAITING_FOR_CALL;
                        flag = false;
                    }
                }
            }
        }  

        public void Call(int floor) {
            //this.State = ELEVATOR_STATE.MOVING;
            this.CallOriginFloor.Enqueue(floor);
        }
        public void AddPerson(Personel pers) {
            this.people.Add(pers);
        }
        public void AddPeople(IEnumerable<Personel> people) {
            this.people.AddRange(people);
            this.State = ELEVATOR_STATE.OCCUPIED;
        }

        public bool canAccess(Personel person,Floor floor)
        {
            return person.AccessLevel >= floor.NeededLevel;
        }
    }
}
