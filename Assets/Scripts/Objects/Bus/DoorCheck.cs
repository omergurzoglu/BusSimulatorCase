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
                //BusStopArea.PassengerList.Remove(passenger);
                passenger.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                passenger.transform.parent = transform;
                passenger.SetPassengerAnimation(true);
                //do whatever
            }
        }
    }
}
