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
            RoundManager.OnRoundFinish += OnRoundFinish;
            RoundManager.OnRoundStart += OnRoundStart; ;
            EnemyMissile.OnEnemyMissileDestroyed += OnEnemyMissileDestroyed;
        }

        private void OnRoundStart(int roundNumber)
        {
            if (roundNumber < 1)
                TotalScore = 0;

            ScoreThisRound = 0;
        }

        private void OnEnemyMissileDestroyed(int missileValue)
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
