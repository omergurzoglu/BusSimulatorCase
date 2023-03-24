
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private RawImage image;
        [SerializeField] private Transform target;
        private Camera _camera;
        private float _minX, _minY, _maxX, _maxY;

        private void Awake() => _camera=Camera.main;

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
            Vector2 waypointPos = _camera.WorldToScreenPoint(target.position);
            if(Vector3.Dot((target.position-transform.position),transform.forward)<0)
            {
                if (waypointPos.x < Screen.width / 2f)
                {
                    waypointPos.x = _maxX;
                }
                else
                {
                    waypointPos.x = _minX;
                }
            }
            waypointPos.x = Mathf.Clamp(waypointPos.x, _minX, _maxX);
            waypointPos.y = Mathf.Clamp(waypointPos.y, _minY, _maxY);
            image.transform.position = waypointPos;
        }
    }
}