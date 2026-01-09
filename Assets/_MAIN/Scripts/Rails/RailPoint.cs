using Cinemachine;
using UnityEngine;

namespace RAIL_SHOOTER.RAILS
{
    public class RailPoint : MonoBehaviour
    {
        public enum WaypointType { Normal = 0, Spawn, End, Pause }

        [SerializeField] private WaypointType _waypointType = WaypointType.Normal;
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _spawnColor = Color.green;
        [SerializeField] private Color _endColor = Color.magenta;
        [SerializeField] private Color _pauseColor = Color.yellow;

        [field: SerializeField, Range(0.1f, 1.0f)] private float _radious = 0.15f;

        private CinemachineVirtualCamera _virtualCamera;

        public WaypointType Type { get => _waypointType; set => _waypointType = value; }
        public CinemachineVirtualCamera VirtualCamera { get => _virtualCamera; set => _virtualCamera = value; }

        private void Awake()
        {
            _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            _virtualCamera?.gameObject.SetActive(false);
        }
        private void OnDrawGizmos() 
        {
            switch (_waypointType)
            {
                case WaypointType.Normal: Gizmos.color = _normalColor; break;
                case WaypointType.Spawn: Gizmos.color = _spawnColor; break;
                case WaypointType.End: Gizmos.color = _endColor; break;
                case WaypointType.Pause: Gizmos.color = _pauseColor; break;
            }

            Gizmos.DrawSphere(
                transform.position,
                radius: _radious
            );
        }
        public void ActivateCamera(Transform playerTransform)
        {
            if (_virtualCamera != null)
            {
                _virtualCamera.Follow = playerTransform;
                _virtualCamera.LookAt = playerTransform;
                _virtualCamera.transform.position = transform.position;

                _virtualCamera.Priority = 20;
            }
            _virtualCamera.gameObject.SetActive(true);
        }
        public void DeactivateCamera()
        {
            if (_virtualCamera != null)
            {
                _virtualCamera.Priority = 0;
            }
            _virtualCamera.gameObject.SetActive(false);
        }
    }
}