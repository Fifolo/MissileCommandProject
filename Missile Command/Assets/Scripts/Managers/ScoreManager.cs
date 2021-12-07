using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Utils;

namespace MissileCommand
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public delegate void ScoreEvent(int score);
        public static event ScoreEvent OnTotalScoreChange;

        [SerializeField] private int _pointsPerCity = 20;
        public int TotalScore { get; private set; }
        public int ScoreThisRound { get; private set; }
        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable() => SubscribeToEvents();
        private void OnDisable() => UnSubscribeToEvents();

        private void SubscribeToEvents()
        {
            RoundManager.OnRoundFinish += OnRoundFinish;
            RoundManager.OnRoundStart += OnRoundStart; ;
            Destruction.OnEnemyDestroyed += OnEnemyDestroyed;
        }

        private void UnSubscribeToEvents()
        {
            RoundManager.OnRoundFinish -= OnRoundFinish;
            RoundManager.OnRoundStart -= OnRoundStart; ;
            Destruction.OnEnemyDestroyed -= OnEnemyDestroyed;
        }

        private void OnRoundStart(int roundNumber)
        {
            if (roundNumber < 1)
                TotalScore = 0;

            ScoreThisRound = 0;
        }

        private void OnEnemyDestroyed(int missileValue)
        {
            ScoreThisRound += missileValue;
            TotalScore += missileValue;

            OnTotalScoreChange?.Invoke(TotalScore);
        }

        private void OnRoundFinish(int roundNumber)
        {
            int citiesLeft = PlayerCity.AllPlayerCities.Count;
            TotalScore += citiesLeft * _pointsPerCity;
            ScoreThisRound += citiesLeft * _pointsPerCity;

            OnTotalScoreChange?.Invoke(TotalScore);
        }

        private void Start()
        {
            TotalScore = 0;
            ScoreThisRound = 0;
        }
    }
}
