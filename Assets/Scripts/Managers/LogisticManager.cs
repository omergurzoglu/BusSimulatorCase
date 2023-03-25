
using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Objects.Bus;
using Random = UnityEngine.Random;

namespace Managers
{
    public class LogisticManager : Singleton<LogisticManager>
    {
        public List<BusStopArea> busStops=new();
        public BusStopArea currentScheduledBusStop;
        

        private IBusStopArea _currentStop;
        private List<IBusStopArea> _passengers = new(); 
        
        public event Action<BusStopArea> BroadCastSchedule; 

        private void Awake() => busStops = FindObjectsOfType<BusStopArea>().ToList();

        private void Start() => DesignateNewSchedule();

        public void DesignateNewSchedule()
        {
            if (busStops is { Count: > 0 })
            {
                currentScheduledBusStop = busStops[Random.Range(0, busStops.Count)];
            }
            OnBroadCastSchedule(currentScheduledBusStop);
        }

        private void OnBroadCastSchedule(BusStopArea obj)
        {
            BroadCastSchedule?.Invoke(obj);
        }
        // public void LoadPassenger()
        // {
        //     var passenger = _currentStop.RemovePassenger();
        //     passenger.EnterBus();
        //     _passengers.Add(passenger);
        // }
        //
        // public void UnloadPassenger()
        // {
        //     var passenger = _passengers[0];
        //     passenger.ExitBus();
        //     _passengers.RemoveAt(0);
        // }


        public void SetNextStop(IBusStopArea nextStop)
        {
            _currentStop = nextStop;
        }
    }
    
}