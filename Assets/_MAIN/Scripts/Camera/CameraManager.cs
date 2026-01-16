using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAIL_SHOOTER.CAMERA
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Offsets")]
        [SerializeField] private Vector3 _positionOffset = Vector3.zero;
        [SerializeField] private Vector3 _rotationOffset = Vector3.zero;

        [Header("Smoothing")]
        [SerializeField] private float _rotationSmoothTime = 0.3f;

        [SerializeField] private Transform _playerTransform;
        private Camera _mainCamera;
        private Vector3 _velocityRotation;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        private void LateUpdate()
        {
            if (_playerTransform == null) return;
            
            var rotatedOffset = _playerTransform.rotation * _positionOffset;
            _mainCamera.transform.position = _playerTransform.position + rotatedOffset;

            var targetEulerAngles = _playerTransform.eulerAngles + _rotationOffset;
                
            var smoothedEulerAngles = new Vector3(
                Mathf.SmoothDampAngle(_mainCamera.transform.eulerAngles.x, targetEulerAngles.x, ref _velocityRotation.x, _rotationSmoothTime),
                Mathf.SmoothDampAngle(_mainCamera.transform.eulerAngles.y, targetEulerAngles.y, ref _velocityRotation.y, _rotationSmoothTime),
                Mathf.SmoothDampAngle(_mainCamera.transform.eulerAngles.z, targetEulerAngles.z, ref _velocityRotation.z, _rotationSmoothTime)
            );
                
            _mainCamera.transform.rotation = Quaternion.Euler(smoothedEulerAngles);
        }
    }
}