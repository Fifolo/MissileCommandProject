using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Utils;
using System;

namespace MissileCommand
{
    public class RoundManager : Singleton<RoundManager>
    {
        public delegate void RoundEvent(int roundNumber);
        public static event RoundEvent OnRoundStart;
        public static event RoundEvent OnRoundFinish;

        [SerializeField] private int _playerMissilesPerRound = 30;
        [SerializeField] private int _enemyMissilesPerRound = 15;

        public int RoundNumber { get; private set; }
        public int PlayerMissilesPerRound { get { return _playerMissilesPerRound; } }
        public int EnemyMissilesPerRound { get { return _enemyMissilesPerRound; } }

        protected override void Awake()
        {
            base.Awake();
            RoundNumber = 0;
        }

        private void OnNoMoreMissilesToSpawn()
        {
            OnRoundFinish?.Invoke(RoundNumber);

            if (PlayerCity.AllPlayerCities.HasItems())
                Invoke("StartNewRound", 3f);

            else if (GameManager.Instance)
                GameManager.Instance.GameOver();
        }

        private void Start()
        {
            EnemyMissileSpawner.OnNoMoreMissilesToSpawn += OnNoMoreMissilesToSpawn;
            StartNewRound();
        }

        private void StartNewRound()
        {
            OnRoundStart?.Invoke(RoundNumber);
            RoundNumber++;
        }
    }
}
