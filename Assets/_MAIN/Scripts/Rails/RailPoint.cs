using Cinemachine;
using UnityEngine;

namespace RAIL_SHOOTER.RAILS
{
    public class RailPoint : MonoBehaviour
    {
        public enum WaypointType { Normal = 0, Spawn }

        [SerializeField] private WaypointType _waypointType = WaypointType.Normal;
        [SerializeField] private Color _spawnColor = Color.yellow;
        [SerializeField] private Color _normalColor = Color.green;
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
            Gizmos.color = _waypointType == WaypointType.Spawn ? _spawnColor : _normalColor;

            Gizmos.DrawSphere(
                transform.position,
                radius: 0.15f
            );
        }
        public void ActivateCamera(Transform playerTransform)
        {
            if (_virtualCamera != null)
            {
                //_virtualCamera.Follow = playerTransform;
                _virtualCamera.LookAt = playerTransform;

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