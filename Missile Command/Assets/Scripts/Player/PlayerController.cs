using UnityEngine;
using MissileCommand.Managers;

namespace MissileCommand
{
    #region Components Requirement
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMissileShooter))]
    [DisallowMultipleComponent]
    #endregion
    public class PlayerController : MonoBehaviour
    {
        #region Events
        public delegate void PlayerAmmoEvent(int ammoLeft);
        public static event PlayerAmmoEvent OnPlayerFire;
        #endregion

        #region Variables

        private int _availableMissiles = 100;
        public int AvailableMissiles { get { return _availableMissiles; } }
        private PlayerInput playerInput;
        private PlayerMissileShooter _missileShooter;
        private Transform _playerTransform;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _playerTransform = transform;
            playerInput = GetComponent<PlayerInput>();
            _missileShooter = GetComponent<PlayerMissileShooter>();
        }

        private void OnEnable() => SubscribeToEvents();

        private void OnDisable() => UnSubscribeToEvents();

        #endregion

        #region Private Methods
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
                OnPlayerFire?.Invoke(AvailableMissiles);
            }
        }

        #endregion
    }
}
