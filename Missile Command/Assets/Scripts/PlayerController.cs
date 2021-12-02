using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMissile missileToShoot;
        [SerializeField] private Transform firePoint;

        private int _availableMissiles = 0;
        public int AvailableMissiles { get { return _availableMissiles; } }
        private PlayerInput playerInput;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.OnFirePressed += OnFirePressed;
            RoundManager.OnRoundStart += RoundManager_OnRoundStart;
        }

        private void RoundManager_OnRoundStart(int roundNumber)
        {
            _availableMissiles = RoundManager.Instance.PlayerMissilesPerRound;
        }

        private void OnFirePressed(Vector2 inputPosition)
        {
            if (_availableMissiles > 0)
            {
                PlayerMissile missile;

                if (Pooler<PlayerMissile>.Instance)
                    missile = Pooler<PlayerMissile>.Instance.GetObject();

                else missile = Instantiate(missileToShoot, firePoint.position, Quaternion.identity);

                missile.transform.position = firePoint.position;

                missile.SetDestination(inputPosition);
                missile.gameObject.SetActive(true);

                _availableMissiles--;
            }
        }
    }
}
