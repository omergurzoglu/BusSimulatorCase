using System.Collections;
using Objects.Bus;
using UnityEngine;
using UnityEngine.AI;
namespace Objects.Passengers
{
    public class Passenger : MonoBehaviour,IPassenger
    {
        #region Fields
        private Transform _busDoor;
        private NavMeshAgent _agent;
        public Animator animator;
        private bool _hasReachedDestination;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        #endregion

        #region MonoBehavior
        private void Awake()
        {
            animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
        }
        private void OnEnable() => BusController.BroadCastBusEntryPosForPassengers += GetBusDoorPosAndSetDestination;
        private void OnDisable() => BusController.BroadCastBusEntryPosForPassengers -= GetBusDoorPosAndSetDestination;
        
        #endregion

        #region MainMethods
        private void GetBusDoorPosAndSetDestination(Transform obj)
        {
            _busDoor = obj;
            StartCoroutine(SetDestinationForPassenger());
        }
        private IEnumerator SetDestinationForPassenger()
        {
            yield return new WaitForSeconds(1f);
            _agent.SetDestination(_busDoor.position);
            SetPassengerAnimation(false);
        }

        public void SetPassengerAnimation(bool isIdle) => animator.SetBool(IsMoving, !isIdle);

        #endregion
        
    }

    public interface IPassenger
    {
        
    }
}
