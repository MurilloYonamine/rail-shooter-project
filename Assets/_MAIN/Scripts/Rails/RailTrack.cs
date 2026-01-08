using UnityEngine;
namespace RAIL_SHOOTER.RAILS
{
    public class RailTrack : MonoBehaviour
    {
        [SerializeField] private RailPoint[] _rails;
        [SerializeField] private Color _lineColor = Color.red;
        private void OnDrawGizmos()
        {
            if (_rails == null || _rails.Length < 2) return;

            Gizmos.color = _lineColor;

            for (int i = 1; i < _rails.Length; i++)
            {
                RailPoint currentRail = _rails[i];
                RailPoint previousRail = _rails[i - 1];

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
            if (index < 0 || index >= _rails.Length)
            {
                return null;
            }
            return _rails[index];
        }
        public bool HasNextRail(int currentRailIndex)
        {
            return currentRailIndex < (_rails.Length - 1);
        }
        public RailPoint GetFirstRail()
        {
            if (_rails.Length == 0)
            {
                return null;
            }
            return _rails[0];
        }

        [ContextMenu("Update Rails")]
        private void UpdateRails()
        {
            _rails = GetComponentsInChildren<RailPoint>();
            for (int i = 0; i < _rails.Length; i++)
            {
                RailPoint rail = _rails[i];
                rail.name = $"Rail ({i})";
            }

            RailPoint firstRail = _rails[0];
            if (firstRail.Type != RailPoint.WaypointType.Spawn)
            {
                firstRail.Type = RailPoint.WaypointType.Spawn;
            }
        }
    }
}