using Managers;
using UnityEngine;

namespace Objects.Bus
{
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