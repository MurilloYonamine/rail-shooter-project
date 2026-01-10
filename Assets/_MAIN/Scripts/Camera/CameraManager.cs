using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAIL_SHOOTER.CAMERA
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Offsets")]
        [SerializeField] private Vector3 positionOffset = Vector3.zero;
        [SerializeField] private Vector3 rotationOffset = Vector3.zero;

        [SerializeField] private Transform _playerTransform;
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
                    _playerTransform.position + positionOffset,
                    ref _currentVelocity,
                    _smoothTime
                );

               _mainCamera.transform.rotation = Quaternion.Euler(
                    _playerTransform.eulerAngles + rotationOffset
                );
            }
        }
    }
}