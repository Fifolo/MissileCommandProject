using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Utils;
using System;
using System.Linq;

namespace MissileCommand.Managers
{
    public class RoundManager : Singleton<RoundManager>
    {
        #region Events

        public delegate void RoundEvent(int roundNumber);
        public static event RoundEvent OnRoundStart;
        public static event RoundEvent OnRoundFinish;

        #endregion

        #region Variables

        [Min(1f)]
        [SerializeField] private float _breakTime = 3f;

        [Header("New Round Increments")]
        [Range(0f, 1f)]
        [SerializeField] private float _enemyMissileSpeed = 0.1f;
        [Range(0f, 1f)]
        [SerializeField] private float _playerMissileSpeed = 0.1f;
        [Range(0, 10)]
        [SerializeField] private int _enemyMissilesAmount = 1;
        [Range(0, 10)]
        [SerializeField] private int _playerMissilesAmount = 1;

        public float BreakTimeLeft { get; private set; }
        private List<Condition> _newRoundConditions;
        public int RoundNumber { get; private set; }

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            RoundNumber = 0;
        }

        private void Start()
        {
            InitializeNewRoundConditions();
            StartNewRound();
        }
        private void OnEnable() => SubscribeToEvents();
        private void OnDisable() => UnSubscribeToEvents();

        #endregion

        #region Private Methods
        private void InitializeNewRoundConditions()
        {
            _newRoundConditions = new List<Condition>();

            //works fine if new spawners are not being added at runtime
            List<EnemySpawner> enemySpawners = FindObjectsOfType<EnemySpawner>().ToList();
            List<EnemyMissileSpawner> missileSpawners = FindObjectsOfType<EnemyMissileSpawner>().ToList();

            Func<bool> NoMoreThingsToSpawn() => () =>
            {
                foreach (EnemySpawner spawner in enemySpawners)
                {
                    if (spawner.AvailableEnemies > 0)
                        return false;
                };
                foreach (EnemyMissileSpawner spawner in missileSpawners)
                {
                    if (spawner.AvailableMissiles > 0)
                        return false;
                };
                return true;
            };
            Func<bool> NoMoreEnemiesAlive() => () => !Enemy.ExistsActive;
            Func<bool> NoMoreEnemyMissilesAlive() => () => !EnemyMissile.ExistsActive;

            AddCondition(NoMoreThingsToSpawn());
            AddCondition(NoMoreEnemiesAlive());
            AddCondition(NoMoreEnemyMissilesAlive());
        }

        private void AddCondition(Func<bool> newCondition)
        {
            Condition condition = new Condition(newCondition);
            _newRoundConditions.Add(condition);
        }

        private void SubscribeToEvents()
        {
            EnemyMissile.OnLastMissileDestroyed += OnNoMoreMissiles;
            Enemy.OnLastEnemyDestroyed += OnNoMoreEnemies;
            PlayerCity.OnLastCityDestroyed += OnLastCityDestroyed;
        }

        private void OnLastCityDestroyed()
        {
            if (GameManager.Instance)
                GameManager.Instance.GameOver();
        }

        private void UnSubscribeToEvents()
        {
            PlayerCity.OnLastCityDestroyed -= OnLastCityDestroyed;
            EnemyMissile.OnLastMissileDestroyed -= OnNoMoreMissiles;
            Enemy.OnLastEnemyDestroyed -= OnNoMoreEnemies;
        }

        private void OnNoMoreMissiles()
        {
            if (ConditionsAreMet())
                RoundFinish();
        }

        private void RoundFinish()
        {
            OnRoundFinish?.Invoke(RoundNumber);

            if (PlayerCity.AllPlayerCities.HasItems())
                StartCoroutine(NewRoundCountDown());

            else if (GameManager.Instance)
                GameManager.Instance.GameOver();
        }

        private void OnNoMoreEnemies()
        {
            if (ConditionsAreMet())
                RoundFinish();
        }

        private bool ConditionsAreMet()
        {
            if (!_newRoundConditions.HasItems()) return false;

            foreach (Condition condition in _newRoundConditions)
            {
                if (!condition.IsMet) return false;
            }

            return true;
        }
        private void StartNewRound()
        {
            RoundNumber++;
            OnRoundStart?.Invoke(RoundNumber);
        }

        private IEnumerator NewRoundCountDown()
        {
            UpdateValues();

            float t = 0;
            BreakTimeLeft = _breakTime;
            while (BreakTimeLeft > 0)
            {
                BreakTimeLeft = Mathf.Lerp(_breakTime, 0, t / _breakTime);
                t += Time.deltaTime;
                yield return null;
            }
            StartNewRound();
        }

        private void UpdateValues()
        {
            if (MissilesManager.Instance)
            {
                MissilesManager manager = MissilesManager.Instance;

                manager.EnemyMissileSpeed += _enemyMissileSpeed;
                manager.EnemyMissilesPerRound += _enemyMissilesAmount;
                manager.PlayerMissileSpeed += _playerMissileSpeed;
                manager.PlayerMissilesPerRound += _playerMissilesAmount;
            }
        }

        #endregion

        private class Condition
        {
            public Condition(Func<bool> condition)
            {
                _condition = condition;
            }
            private Func<bool> _condition;
            public bool IsMet { get { return _condition(); } }
        }
    }
}
