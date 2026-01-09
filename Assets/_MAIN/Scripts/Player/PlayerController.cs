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

            railSpawnPoint.ActivateCamera(this.transform);
        }
        // TODO: move in a diffrente script
        private void FixedUpdate()
        {
            if(!_railTrack.HasNextRail(_currentRailIndex)) return;

            int nextRailIndex = _currentRailIndex + 1;
            RailPoint nextRailPoint = _railTrack.GetRailPerIndex(nextRailIndex);

            transform.position = Vector3.MoveTowards(
                transform.position,
                nextRailPoint.transform.position,
                Time.deltaTime * _moveSpeed
            );

            float distance = Vector3.Distance(transform.position, nextRailPoint.transform.position);

            if (distance <= 0.01f)
            {
                if (_railTrack.HasNextRail(_currentRailIndex))
                {
                    RailPoint currentRailPoint = _railTrack.GetRailPerIndex(_currentRailIndex);
                    currentRailPoint.DeactivateCamera();

                    _currentRailIndex++;

                    nextRailPoint.ActivateCamera(this.transform);
                }
            }
        }
    }
}