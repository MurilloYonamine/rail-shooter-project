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

        [Header("Smoothing")]
        [SerializeField] private float rotationSmoothTime = 0.3f;

        [SerializeField] private Transform _playerTransform;
        private Camera _mainCamera;
        private Vector3 _velocityRotation;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        private void LateUpdate()
        {
            if (_playerTransform != null)
            {
                Vector3 rotatedOffset = _playerTransform.rotation * positionOffset;
                _mainCamera.transform.position = _playerTransform.position + rotatedOffset;

                Vector3 targetEulerAngles = _playerTransform.eulerAngles + rotationOffset;
                
                Vector3 smoothedEulerAngles = new Vector3(
                    Mathf.SmoothDampAngle(_mainCamera.transform.eulerAngles.x, targetEulerAngles.x, ref _velocityRotation.x, rotationSmoothTime),
                    Mathf.SmoothDampAngle(_mainCamera.transform.eulerAngles.y, targetEulerAngles.y, ref _velocityRotation.y, rotationSmoothTime),
                    Mathf.SmoothDampAngle(_mainCamera.transform.eulerAngles.z, targetEulerAngles.z, ref _velocityRotation.z, rotationSmoothTime)
                );
                
                _mainCamera.transform.rotation = Quaternion.Euler(smoothedEulerAngles);
            }
        }
    }
}