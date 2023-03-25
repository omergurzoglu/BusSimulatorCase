
using System.Collections.Generic;
using Managers;
using Objects.Passengers;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Bus
{
    public class BusStopArea : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Passenger passenger;

        private Transform _thisBusStopTransform;
        public static List<Passenger> PassengerList=new();

        #endregion

        #region MonoBehavior

        private void Awake()
        {
            _thisBusStopTransform = transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BusController>(out var bus) && LogisticManager.Instance.currentScheduledBusStop==this)
            {
                LogisticManager.Instance.busStops.Remove(this);
                bus.inBusStop = true;
                bus.SetDoorState(true);
                bus.LockBrake(true);
                SpawnPassengers();
                bus.SendDoorTransformToPassengers();
                bus.BusTakeOff();
            }
        }
        
        #endregion
        
        private void SpawnPassengers()
        {
            int randomSpawnCount = Random.Range(1, 3);
            for (int i = 0; i < randomSpawnCount; i++)
            {
                 var newPassenger = Instantiate(passenger, new Vector3(Random.Range(1,5),0,Random.Range(1,5))+_thisBusStopTransform.position, 
                    quaternion.identity);
                 PassengerList.Add(newPassenger);
                 
            } 
            
        }
    }
}
