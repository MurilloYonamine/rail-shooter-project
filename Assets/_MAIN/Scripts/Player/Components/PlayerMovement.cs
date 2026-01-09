using System;
using RAIL_SHOOTER.RAILS;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    [Serializable]
    public class PlayerMovement : PlayerComponent
    {
        [SerializeField] private RailTrack _railTrack;
        private int _currentRailIndex = 0;
        private Transform _spawnPoint;
        [SerializeField] private float _moveSpeed = 5.0f;

        public override void OnStart()
        {
            RailPoint railSpawnPoint = _railTrack.GetFirstRail();
            _spawnPoint = railSpawnPoint.transform;

            _player.transform.position = _spawnPoint.position;

            railSpawnPoint.ActivateCamera(_player.transform);
        }
        public override void OnFixedUpdate()
        {
            if (!_railTrack.HasNextRail(_currentRailIndex)) return;

            int nextRailIndex = _currentRailIndex + 1;
            RailPoint nextRailPoint = _railTrack.GetRailPerIndex(nextRailIndex);

            _player.transform.position = Vector3.MoveTowards(
                _player.transform.position,
                nextRailPoint.transform.position,
                Time.deltaTime * _moveSpeed
            );

            float distance = Vector3.Distance(_player.transform.position, nextRailPoint.transform.position);

            if (distance <= 0.01f)
            {
                RailPoint currentRailPoint = _railTrack.GetRailPerIndex(_currentRailIndex);

                if (_railTrack.HasNextRail(_currentRailIndex))
                {
                    currentRailPoint.DeactivateCamera();

                    _currentRailIndex++;

                    nextRailPoint.ActivateCamera(_player.transform);
                }

                if (currentRailPoint.Type == RailPoint.WaypointType.Pause)
                {
                    Debug.Log("Pause at rail point");
                    _moveSpeed = 0f;
                }
            }
        }
    }
}