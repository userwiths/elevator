using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Elevator.Shared;

namespace Elevator
{
    public class Building
    {
        public ElevatorArrivedAt arrivedAt;
        public List<Personel> personel { get; set; }
        public List<Elevator> elevators { get; set; }
        public List<Floor> floors { get; set; }

        public Building() {
            this.personel = new List<Personel>();
            this.elevators = new List<Elevator>();
            this.floors = new List<Floor>();
            this.arrivedAt = new ElevatorArrivedAt(ArrivedAtMethod);
        }

        public void ArrivedAtMethod(int floor) {
            var personelOnFloor = this.personel.Where(x=>x.floor==floor && x.PersonelState==1).ToList();
            var waitingElevators = this.elevators.Where(x => x.State == ELEVATOR_STATE.WAITING_FOR_PPL).ToList();
            var defaultElevator = waitingElevators.FirstOrDefault();
            if (defaultElevator != null) {
                defaultElevator.AddPeople(personelOnFloor);
            }
        }
        public Building(string[] floors,ACCESS_LEVEL[] levels):this(){
            int i = 0;
            for(i=0;i<floors.Length;i++)
            {
                this.floors.Add(new Floor(floors[i],levels[i]));
            }
        }

        public void PopulateWithPersonel(int confidential,int secret,int top_secret) {
            int i = 0;
            for (i = 0; i < confidential; i++) {
                this.personel.Add(new Personel(ACCESS_LEVEL.CONFIDENTIAL));
            }
            for (i = 0; i < secret; i++){
                this.personel.Add(new Personel(ACCESS_LEVEL.SECRET));
            }
            for (i = 0; i < top_secret; i++){
                this.personel.Add(new Personel(ACCESS_LEVEL.TOP_SECRET));
            }
        }

        public IEnumerable<Thread> getThreads() {
            var result = this.personel.Select(x => x.thread).ToList();
            result.AddRange(this.elevators.Select(x => x.thread).ToList());
            return result;
        }
    }
}
