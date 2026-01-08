using UnityEngine;
namespace RAIL_SHOOTER.RAILS
{
    public class RailTrack : MonoBehaviour
    {
        [SerializeField] private RailPoint[] m_rails;
        [SerializeField] private Color m_lineColor = Color.red;
        private void OnDrawGizmos()
        {
            Gizmos.color = m_lineColor;

            for (int i = 1; i < m_rails.Length; i++)
            {
                RailPoint currentRail = m_rails[i];
                RailPoint previousRail = m_rails[i - 1];

                if ((currentRail != null) && (previousRail != null))
                {
                    Gizmos.DrawLine(
                        previousRail.transform.position,
                        currentRail.transform.position
                    );
                }
            }
        }
        public RailPoint GetRailPerIndex(int index)
        {
            if (index < 0 || index >= m_rails.Length)
            {
                return null;
            }
            return m_rails[index];
        }
        public bool HasNextRail(int currentRailIndex)
        {
            return currentRailIndex < (m_rails.Length - 1);
        }
        public RailPoint GetFirstRail()
        {
            if (m_rails.Length == 0)
            {
                return null;
            }
            return m_rails[0];
        }

        [ContextMenu("Update Rails")]
        private void UpdateRails()
        {
            m_rails = GetComponentsInChildren<RailPoint>();
            for (int i = 0; i < m_rails.Length; i++)
            {
                RailPoint rail = m_rails[i];
                rail.name = $"Rail ({i})";
            }

            RailPoint firstRail = m_rails[0];
            if (firstRail.m_waypointType != RailPoint.WaypointType.Spawn)
            {
                firstRail.m_waypointType = RailPoint.WaypointType.Spawn;
            }
        }
    }
}