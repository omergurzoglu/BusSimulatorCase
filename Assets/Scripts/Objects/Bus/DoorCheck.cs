using Objects.Passengers;
using UnityEngine;

namespace Objects.Bus
{
    /// <summary>
    /// Locks the passengers when they get close to the busDoor
    /// Sets them as OutGoingPassengers
    /// </summary>
    public class DoorCheck : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Passenger>(out var passenger))
            {
                passenger.passengerInOrOutState = Passenger.PassengerInOrOut.PassengerOut;
                passenger.SetMeshVisibility(false);
                passenger.agent.ResetPath();
                passenger.transform.parent = transform;
                passenger.SetPassengerAnimation(true);
            }
        }
    }
}
