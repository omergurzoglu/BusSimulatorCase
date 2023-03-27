using Managers;
using Objects.Passengers;
using UnityEngine;
namespace Objects.Bus
{
    public class BusStopArea : MonoBehaviour
    {
        //Reference to passenger prefab 
        [SerializeField] private Passenger passenger;
        
        //Reference to the class responsible for spawning
        private IPassengerSpawnStrategy _passengerSpawnStrategy;
        private void Start() => _passengerSpawnStrategy = new RandomPassengerSpawnStrategy();
        private void OnTriggerEnter(Collider other)
        {
            //Check if this busStop is the currentScheduledBusStop, also check if bus is inside the busStop
            if (other.TryGetComponent<BusController>(out var bus) && LogisticManager.Instance.currentScheduledBusStop == this)
            {
                //Score related methods
                if (ScoreManager.Instance.timer > 0)
                {
                    ScoreManager.Instance.AddToScore(10);
                    ScoreManager.Instance.AddToTimer(45);
                }
                else if (ScoreManager.Instance.timer<0)
                {
                    ScoreManager.Instance.AddToScore(-10);
                    ScoreManager.Instance.AddToTimer(45);
                }
                
                //Disembark if there are passengers inside the bus
                LogisticManager.Instance.DisEmbarkPassengers();
                
                //Lock the bus
                bus.SetDoorState(true);
                bus.LockBrake(true);
                
                //Spawn passengers
                _passengerSpawnStrategy.SpawnPassengers(transform, () => Instantiate(passenger), LogisticManager.Instance.passengers);
                
                //Send door position to new passengers
                bus.SendDoorTransformToPassengers();
                
                //Release bus
                bus.BusTakeOff();
            }
        }
    }
}
