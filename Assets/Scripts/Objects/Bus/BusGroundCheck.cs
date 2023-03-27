using Managers;
using UnityEngine;

namespace Objects.Bus
{
    /// <summary>
    /// To detect if bus is onRoad or offRoad
    /// </summary>
    public class BusGroundCheck : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 3)
            {
                ScoreManager.Instance.StartPenalty();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 3)
            {
                ScoreManager.Instance.StopPenalty();
            }
        }
    }
}