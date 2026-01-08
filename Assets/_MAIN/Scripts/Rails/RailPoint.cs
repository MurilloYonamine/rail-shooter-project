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

        public WaypointType Type { get => _waypointType; set => _waypointType = value; }
        public CinemachineVirtualCamera VirtualCamera { get; private set; }

        private void OnDrawGizmos()
        {
            Gizmos.color = _waypointType == WaypointType.Spawn ? _spawnColor : _normalColor;

            Gizmos.DrawSphere(
                transform.position,
                radius: 0.15f
            );
        }
    }
}