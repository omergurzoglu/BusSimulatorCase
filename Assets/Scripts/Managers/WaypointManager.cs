
using Objects.Bus;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class WaypointManager : MonoBehaviour
    {
        [SerializeField] private RawImage image;
        [SerializeField] private BusStopArea busStopTarget;
        private Camera _camera;
        private float _minX, _minY, _maxX, _maxY;

        private void Awake() => _camera=Camera.main;

        private void OnEnable()
        {
            LogisticManager.Instance.BroadCastSchedule += GetNewWaypointForBusStop;
        }

        private void OnDisable()
        {
            LogisticManager.Instance.BroadCastSchedule += GetNewWaypointForBusStop;
        }

        private void GetNewWaypointForBusStop(BusStopArea busStopArea)
        {
            busStopTarget = busStopArea;
        }

        private void Start()
        {
             _minX = image.GetPixelAdjustedRect().width / 2;
             _maxX = Screen.width - _minX;
             _minY = image.GetPixelAdjustedRect().height / 2;
             _maxY = Screen.height - _minY;
        }
        private void Update()
        {
             AdjustWaypoint();
        }
        private void AdjustWaypoint()
        {
            Vector2 waypointPos = _camera.WorldToScreenPoint(busStopTarget.transform.position);
            if(Vector3.Dot((busStopTarget.transform.position-transform.position),transform.forward)<0)
            {
                waypointPos.x = waypointPos.x < Screen.width / 2f ? _maxX : _minX;
            }
            waypointPos.x = Mathf.Clamp(waypointPos.x, _minX, _maxX);
            waypointPos.y = Mathf.Clamp(waypointPos.y, _minY, _maxY);
            image.transform.position = waypointPos;
        }
    }
}