using Objects.Passengers;
using UnityEngine;

namespace Objects.Bus
{
    public class DoorCheck : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Passenger>(out var passenger))
            {
                BusStopArea.PassengerList.Remove(passenger);
                Destroy(passenger.gameObject);
                Debug.Log("passenger in");
                //do whatever
            }
        }
    }
}
