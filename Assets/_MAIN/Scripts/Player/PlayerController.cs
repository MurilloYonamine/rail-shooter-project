using System;
using RAIL_SHOOTER.RAILS;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        private PlayerComponent[] _playerComponents;
        [SerializeField] private PlayerAim _playerAim;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerShoot _playerShoot;
        
        public PlayerAim PlayerAim => _playerAim;
        public PlayerMovement PlayerMovement => _playerMovement;
        public PlayerShoot PlayerShoot => _playerShoot;

        private void Awake()
        {
            _playerComponents = new PlayerComponent[]
            {
                _playerAim,
                _playerMovement,
                _playerShoot
            };
            ForEachComponent(component => component?.Initialize(this));
            ForEachComponent(component => component?.OnAwake());
        }
        private void Start()
        {
            ForEachComponent(component => component?.OnStart());
        }
        private void OnEnable()
        {
            ForEachComponent(component => component?.OnEnable());
        }
        private void OnDisable()
        {
            ForEachComponent(component => component?.OnDisable());
        }
        private void Update()
        {
            ForEachComponent(component => component?.OnUpdate());
        }
        private void FixedUpdate()
        {
            ForEachComponent(component => component?.OnFixedUpdate());
        }
        private void LateUpdate()
        {
            ForEachComponent(component => component?.OnLateUpdate());
        }

        private void ForEachComponent(Action<PlayerComponent> componentAction)
        {
            for (int i = 0; i < _playerComponents.Length; i++)
            {
                componentAction(_playerComponents[i]);
            }
        }
    }
}