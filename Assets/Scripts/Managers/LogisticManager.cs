using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Bus;
using Objects.Passengers;
using Random = UnityEngine.Random;

namespace Managers
{
    public class LogisticManager : Singleton<LogisticManager>
    {
        public List<BusStopArea> busStops=new();
        public BusStopArea currentScheduledBusStop;
        public List<Passenger> passengers = new();
        public event Action<BusStopArea> BroadCastSchedule;
        private void Awake() => busStops = FindObjectsOfType<BusStopArea>().ToList();
        private void Start() => DesignateNewSchedule();
        public void DesignateNewSchedule()
        {
            BusStopArea newScheduledBusStop;
            do { newScheduledBusStop = busStops[Random.Range(0, busStops.Count)]; } 
            while (newScheduledBusStop == currentScheduledBusStop);
            currentScheduledBusStop = newScheduledBusStop;
            OnBroadCastSchedule(currentScheduledBusStop);
        }
        private void OnBroadCastSchedule(BusStopArea obj) => BroadCastSchedule?.Invoke(obj);

        public void DisEmbarkPassengers()
        {
            foreach (var passenger in passengers)
            {
                if (passenger.passengerInOrOutState == Passenger.PassengerInOrOut.PassengerOut)
                {
                    passenger.DisembarkPassenger();
                }
            }
        }
    }
    
}