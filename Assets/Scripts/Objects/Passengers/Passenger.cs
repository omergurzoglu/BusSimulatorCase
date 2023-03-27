using System.Collections;
using Managers;
using Objects.Bus;
using UnityEngine;
using UnityEngine.AI;
namespace Objects.Passengers
{
    public class Passenger : MonoBehaviour
    {
        #region Fields
        [SerializeField]private SkinnedMeshRenderer skinnedMeshRenderer;
        private Transform _busDoor;
        public NavMeshAgent agent;
        public Animator animator;
        public PassengerInOrOut passengerInOrOutState;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public enum PassengerInOrOut { PassengerIn,PassengerOut }
        #endregion

        #region MonoBehavior
        private void OnEnable() => BusController.BroadCastBusEntryPosForPassengers += GetBusDoorPosAndSetDestination;
        private void OnDisable() => BusController.BroadCastBusEntryPosForPassengers -= GetBusDoorPosAndSetDestination;
        
        #endregion

        #region MainMethods
        private void GetBusDoorPosAndSetDestination(Transform obj)
        {
            //If the passenger is embarking , move to the door position
            
            if (passengerInOrOutState != PassengerInOrOut.PassengerIn) return;
            _busDoor = obj;
            StartCoroutine(SetDestinationForPassenger());
        }
        private IEnumerator SetDestinationForPassenger()
        {
            yield return new WaitForSeconds(1f);
            agent.SetDestination(_busDoor.position);
            SetPassengerAnimation(false);
        }
        public void SetPassengerAnimation(bool isIdle) => animator.SetBool(IsMoving, !isIdle);
        
        private IEnumerator DisembarkPassengerCoroutine()
        {
            //Send outGoing passenger to random point
            agent.SetDestination(transform.localPosition + new Vector3(Random.Range(5,10), 0, Random.Range(5,10)));
            
            //Set back visuals
            SetMeshVisibility(true);
            SetPassengerAnimation(false);
            
            //Delay before destroying the passenger
            yield return new WaitForSeconds(3f);
            
            //Remove from the passenger list
            LogisticManager.Instance.passengers.Remove(this);
            Destroy(gameObject);
        }
        public void DisembarkPassenger() => StartCoroutine(DisembarkPassengerCoroutine());

        public void SetMeshVisibility(bool visible) => skinnedMeshRenderer.enabled = visible;

        #endregion
        
    }
}
