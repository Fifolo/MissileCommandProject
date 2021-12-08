using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Utils;
using System;
using System.Linq;

namespace MissileCommand
{
    public class RoundManager : Singleton<RoundManager>
    {
        public delegate void RoundEvent(int roundNumber);
        public static event RoundEvent OnRoundStart;
        public static event RoundEvent OnRoundFinish;

        [Min(1f)]
        [SerializeField] private float _breakTime = 3f;

        public float BreakTimeLeft { get; private set; }
        private List<Condition> _newRoundConditions;
        public int RoundNumber { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            RoundNumber = 1;
        }

        private void Start()
        {
            InitializeNewRoundConditions();
            StartNewRound();
        }

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

        private void OnEnable() => SubscribeToEvents();
        private void OnDisable() => UnSubscribeToEvents();

        private void SubscribeToEvents()
        {
            EnemyMissile.OnLastMissileDestroyed += OnNoMoreMissiles;
            Enemy.OnLastEnemyDestroyed += OnNoMoreEnemies;
        }


        private void UnSubscribeToEvents()
        {
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
            Debug.Log("Starting round " + RoundNumber);
            OnRoundStart?.Invoke(RoundNumber);
            RoundNumber++;
        }

        private IEnumerator NewRoundCountDown()
        {
            float t = 0;
            BreakTimeLeft = _breakTime;
            while (t <= BreakTimeLeft)
            {
                BreakTimeLeft = Mathf.Lerp(_breakTime, 0, t);
                t += Time.deltaTime;
                yield return null;
            }
            StartNewRound();
        }
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
