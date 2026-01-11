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
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        private void LateUpdate()
        {
            if (_playerTransform != null)
            {
                _mainCamera.transform.position = _playerTransform.position + positionOffset;

               _mainCamera.transform.rotation = Quaternion.Euler(
                    _playerTransform.eulerAngles + rotationOffset
                );
            }
        }
    }
}