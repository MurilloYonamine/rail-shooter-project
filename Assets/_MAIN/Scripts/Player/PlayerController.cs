using RAIL_SHOOTER.RAILS;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerAim _playerAim;

        [Header("Rails")]
        [SerializeField] private RailTrack _railTrack;
        private int _currentRailIndex = 0;
        private Transform _spawnPoint;

        [SerializeField] private float _moveSpeed = 5.0f;

        private void Start()
        {
            RailPoint railSpawnPoint = _railTrack.GetFirstRail();
            _spawnPoint = railSpawnPoint.transform;

            gameObject.transform.position = _spawnPoint.position;
        }
        // TODO: move in a diffrente script
        private void FixedUpdate()
        {
            RailPoint currentRailPoint = _railTrack.GetRailPerIndex(_currentRailIndex);

            transform.position = Vector3.MoveTowards(
                transform.position,
                currentRailPoint.transform.position,
                Time.deltaTime * _moveSpeed
            );

            float distance = Vector3.Distance(transform.position, currentRailPoint.transform.position);

            if (distance <= 0)
            {
                if (_railTrack.HasNextRail(_currentRailIndex))
                {
                    _currentRailIndex++;
                }
            }
        }
    }
}