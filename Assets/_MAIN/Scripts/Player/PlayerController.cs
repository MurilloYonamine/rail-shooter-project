using RAIL_SHOOTER.RAILS;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerAim m_playerAim;

        [Header("Rails")]
        [SerializeField] private RailTrack m_railTrack;
        private int m_currentRailIndex = 0;
        private Transform m_spawnPoint;

        [SerializeField] private float m_moveSpeed = 5.0f;

        private void Start()
        {
            RailPoint railSpawnPoint = m_railTrack.GetFirstRail();
            m_spawnPoint = railSpawnPoint.transform;

            gameObject.transform.position = m_spawnPoint.position;
        }
        // TODO: move in a diffrente script
        private void FixedUpdate()
        {
            RailPoint currentRailPoint = m_railTrack.GetRailPerIndex(m_currentRailIndex);

            transform.position = Vector3.MoveTowards(
                transform.position,
                currentRailPoint.transform.position,
                Time.deltaTime * m_moveSpeed
            );

            float distance = Vector3.Distance(transform.position, currentRailPoint.transform.position);

            if (distance <= 0)
            {
                if (m_railTrack.HasNextRail(m_currentRailIndex))
                {
                    m_currentRailIndex++;
                }
            }
        }
    }
}