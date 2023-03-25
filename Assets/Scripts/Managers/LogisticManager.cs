
using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Bus;
using Random = UnityEngine.Random;

namespace Managers
{
    public class LogisticManager : Singleton<LogisticManager>
    {
        public List<BusStopArea> busStops=new();
        public BusStopArea currentScheduledBusStop;
        public event Action<BusStopArea> BroadCastSchedule; 

        private void Awake() => busStops = FindObjectsOfType<BusStopArea>().ToList();

        private void Start() => DesignateNewSchedule();

        private void DesignateNewSchedule()
        {
            currentScheduledBusStop = busStops[Random.Range(0, busStops.Count)];
            OnBroadCastSchedule(currentScheduledBusStop);
        }

        private void OnBroadCastSchedule(BusStopArea obj)
        {
            BroadCastSchedule?.Invoke(obj);
        }
    }
    
}