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
        [SerializeField] private float _rotationSpeed = 5.0f;
        [SerializeField] private float _modelRotationOffset = 90f;

        private bool _movementEnabled = true;
        public bool MovementEnabled
        {
            get => _movementEnabled;
            set
            {
                _movementEnabled = value;
                if (!_movementEnabled)
                {
                    _player.PlayerAnimator.SetWalking(false, 0f);
                }
            }
        }

        public void SetMovementEnabled(bool enabled)
        {
            MovementEnabled = enabled;
        }

        public override void OnStart()
        {
            if (!_railTrack) return;
            
            RailPoint railSpawnPoint = _railTrack.GetFirstRail();
            _spawnPoint = railSpawnPoint.transform;

            _player.transform.position = _spawnPoint.position;
        }

        public override void OnFixedUpdate()
        {
            if (!_movementEnabled) return;

            if (!_railTrack) return;
            if (!_railTrack.HasNextRail(_currentRailIndex)) return;

            int nextRailIndex = _currentRailIndex + 1;
            var nextRailPoint = _railTrack.GetRailPerIndex(nextRailIndex);

            _player.transform.position = Vector3.MoveTowards(
                _player.transform.position,
                nextRailPoint.transform.position,
                Time.deltaTime * _moveSpeed
            );

            Vector3 directionToNextRail = (nextRailPoint.transform.position - _player.transform.position).normalized;
            
            if (directionToNextRail != Vector3.zero)
            {
                directionToNextRail.y = 0;
                directionToNextRail.Normalize();

                if (directionToNextRail != Vector3.zero)
                {
                    var targetRotation = Quaternion.LookRotation(directionToNextRail, Vector3.up);
                    targetRotation *= Quaternion.Euler(0, _modelRotationOffset, 0);

                    _player.transform.rotation = Quaternion.Slerp(
                        _player.transform.rotation,
                        targetRotation,
                        Time.deltaTime * _rotationSpeed
                    );
                }
            }

            _player.PlayerAnimator.SetWalking(true, _moveSpeed);

            float distance = Vector3.Distance(_player.transform.position, nextRailPoint.transform.position);

            if (!(distance <= 0.01f)) return;
            
            RailPoint currentRailPoint = _railTrack.GetRailPerIndex(_currentRailIndex);

            if (_railTrack.HasNextRail(_currentRailIndex))
            {
                _currentRailIndex++;
            }

            if (currentRailPoint.Type != RailPoint.WaypointType.Pause) return;
            
            _moveSpeed = 0f;
            _player.PlayerAnimator.SetWalking(false, _moveSpeed);
        }
        public void SetMovementLocked(bool locked)
        {
            MovementEnabled = !locked;
        }
    }
}