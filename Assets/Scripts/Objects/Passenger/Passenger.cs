using UnityEngine;
using UnityEngine.AI;

namespace Objects.Passenger
{
    public class Passenger : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform busDoor;

        private NavMeshAgent _agent;
        
        #endregion

        #region MonoBehavior
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            //_agent.SetDestination(busDoor.position);
        }

        #endregion
        
        
    }
}
