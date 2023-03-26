using Managers;
using Objects.Passengers;
using UnityEngine;
namespace Objects.Bus
{
    public class BusStopArea : MonoBehaviour
    {
        [SerializeField] private Passenger passenger;
        private IPassengerSpawnStrategy _passengerSpawnStrategy;
        private void Start() => _passengerSpawnStrategy = new RandomPassengerSpawnStrategy();
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BusController>(out var bus) && LogisticManager.Instance.currentScheduledBusStop == this)
            {
                if (ScoreManager.Instance.timer > 0)
                {
                    ScoreManager.Instance.EditScore(10);
                    ScoreManager.Instance.EditTimer(45);
                }
                else if (ScoreManager.Instance.timer<0)
                {
                    ScoreManager.Instance.EditScore(-10);
                }
                LogisticManager.Instance.DisEmbarkPassengers();
                bus.SetDoorState(true);
                bus.LockBrake(true);
                _passengerSpawnStrategy.SpawnPassengers(transform, () => Instantiate(passenger), LogisticManager.Instance.passengers);
                bus.SendDoorTransformToPassengers();
                bus.BusTakeOff();
            }
        }
    }
}
