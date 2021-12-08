using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMissileShooter))]
    public class PlayerController : MonoBehaviour
    {
        private int _availableMissiles = 100;
        public int AvailableMissiles { get { return _availableMissiles; } }
        private PlayerInput playerInput;
        private PlayerMissileShooter _missileShooter;
        private Transform _playerTransform;

        private void Awake()
        {
            _playerTransform = transform;
            playerInput = GetComponent<PlayerInput>();
            _missileShooter = GetComponent<PlayerMissileShooter>();
        }

        private void OnEnable() => SubscribeToEvents();

        private void OnDisable() => UnSubscribeToEvents();
        private void SubscribeToEvents()
        {
            playerInput.OnFirePressed += OnFirePressed;
            RoundManager.OnRoundStart += RoundManager_OnRoundStart;
        }
        private void UnSubscribeToEvents()
        {
            playerInput.OnFirePressed -= OnFirePressed;
            RoundManager.OnRoundStart -= RoundManager_OnRoundStart;
        }

        private void RoundManager_OnRoundStart(int roundNumber)
        {
            if (MissilesManager.Instance)
                _availableMissiles = MissilesManager.Instance.PlayerMissilesPerRound;

            else _availableMissiles = 10;
        }

        private void OnFirePressed(Vector2 inputPosition)
        {
            if (_availableMissiles > 0)
            {
                _missileShooter.Shoot(inputPosition, _playerTransform.position);
                _availableMissiles--;
            }
        }
    }
}
