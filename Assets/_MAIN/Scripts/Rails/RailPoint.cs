using UnityEngine;

namespace RAIL_SHOOTER.RAILS
{
    public class RailPoint : MonoBehaviour
    {
        public enum WaypointType { Normal = 0, Spawn }
        public WaypointType m_waypointType = WaypointType.Normal;

        [SerializeField] private Color m_spawnColor = Color.yellow;
        [SerializeField] private Color m_normalColor = Color.green;

        private void OnDrawGizmos()
        {
            Gizmos.color = m_waypointType == WaypointType.Spawn ? m_spawnColor : m_normalColor;
            Gizmos.DrawSphere(transform.position, 0.15f);
        }
    }
}