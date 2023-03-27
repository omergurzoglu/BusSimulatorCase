
using Objects.Bus;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    /// <summary>
    /// Repositions the waypoint with respect to the current scheduled busStop
    /// </summary>
    public class WaypointManager : MonoBehaviour
    {
        [SerializeField] private RawImage image;
        private BusStopArea _busStopTarget;
        private Camera _camera;
        private float _minX, _minY, _maxX, _maxY;

        private void Awake() => _camera=Camera.main;

        private void OnEnable() => LogisticManager.Instance.BroadCastSchedule += GetNewWaypointForBusStop;

        private void OnDisable()
        {
            if (LogisticManager.Instance != null) 
            {
                LogisticManager.Instance.BroadCastSchedule -= GetNewWaypointForBusStop;
            }
        }
        private void Start()
        {
             _minX = image.GetPixelAdjustedRect().width / 2;
             _maxX = Screen.width - _minX;
             _minY = image.GetPixelAdjustedRect().height / 2;
             _maxY = Screen.height - _minY;
        }
        private void Update() => AdjustWaypoint();
        
        //Get the new busStop whenever the event is raised
        private void GetNewWaypointForBusStop(BusStopArea busStopArea) => _busStopTarget = busStopArea;

        private void AdjustWaypoint()
        {
            //Reposition waypoints transform via WorldToScreenPoint method every frame
            Vector2 waypointPos = _camera.WorldToScreenPoint(_busStopTarget.transform.position);
            
            
            //If the waypoint is outside the bounds of canvas, hold the waypoint in the edge of screen
            if(Vector3.Dot((_busStopTarget.transform.position-transform.position),transform.forward)<0)
            {
                waypointPos.x = waypointPos.x < Screen.width / 2f ? _maxX : _minX;
            }
            waypointPos.x = Mathf.Clamp(waypointPos.x, _minX, _maxX);
            waypointPos.y = Mathf.Clamp(waypointPos.y, _minY, _maxY);
            image.transform.position = waypointPos;
        }
    }
}