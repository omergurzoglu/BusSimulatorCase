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
        private void Awake()
        {
            //Get all busStops in the scene
            busStops = FindObjectsOfType<BusStopArea>().ToList();
        }

        private void Start() => DesignateNewSchedule();
        public void DesignateNewSchedule()
        {
            //Get a different BusStop each time the method is called, and send currentScheduledBusStop to subscribers
            
            BusStopArea newScheduledBusStop;
            do { newScheduledBusStop = busStops[Random.Range(0, busStops.Count)]; } 
            while (newScheduledBusStop == currentScheduledBusStop);
            currentScheduledBusStop = newScheduledBusStop;
            OnBroadCastSchedule(currentScheduledBusStop);
        }
        private void OnBroadCastSchedule(BusStopArea obj) => BroadCastSchedule?.Invoke(obj);

        public void DisEmbarkPassengers()
        {
            //Loop through the passenger lists, if passenger is leaving the bus , disembark
            
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