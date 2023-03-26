using Managers;
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
                LogisticManager.Instance.passengerCountInsideBus++;
                passenger.passengerInOrOutState = Passenger.PassengerInOrOut.PassengerOut;
                passenger.SetMeshVisibility(false);
                passenger.agent.ResetPath();
                passenger.transform.parent = transform;
                passenger.SetPassengerAnimation(true);
            }
        }
    }
}
