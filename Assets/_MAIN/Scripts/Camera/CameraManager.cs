using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAIL_SHOOTER.CAMERA
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        public Vector3 offset = new Vector3(0, 2, -10);
        [SerializeField] private float _smoothTime = 0.1f;
        [SerializeField] private Vector3 _currentVelocity = Vector3.zero;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        private void LateUpdate()
        {
            if (_playerTransform != null)
            {
                _mainCamera.transform.position = Vector3.SmoothDamp(
                    _mainCamera.transform.position,
                    _playerTransform.position + offset,
                    ref _currentVelocity,
                    _smoothTime
                );
            }
        }
    }
}