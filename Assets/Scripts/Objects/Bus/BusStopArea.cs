using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Bus
{
    public class BusStopArea : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject passenger;

        private Transform _thisBusStopTransform;

        #endregion

        #region MonoBehavior
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BusController>(out var bus))
            {
                bus.inBusStop = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<BusController>(out var bus))
            {
                bus.inBusStop = false;
            }
        }

        

        #endregion
        
        private void SpawnPassengers()
        {
            int randomSpawnCount = Random.Range(1, 3);
            for (int i = 0; i < randomSpawnCount; i++)
            {
                Instantiate(passenger, new Vector3(Random.Range(1,3),0,Random.Range(1,3))+_thisBusStopTransform.position, quaternion.identity);
            } 
            
        }
    }
}
