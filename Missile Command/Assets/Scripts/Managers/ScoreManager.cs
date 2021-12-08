using UnityEngine;
using MissileCommand.Utils;

namespace MissileCommand.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        #region Events

        public delegate void ScoreEvent(int score);
        public static event ScoreEvent OnTotalScoreChange;

        #endregion

        #region Variables

        [SerializeField] private int _pointsPerCity = 20;
        [SerializeField] private int _pointsPerAmmo = 15;
        public int TotalScore { get; private set; }
        public int ScoreThisRound { get; private set; }
        private PlayerController player;

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            player = FindObjectOfType<PlayerController>();
            TotalScore = 0;
            ScoreThisRound = 0;
        }
        private void OnEnable() => SubscribeToEvents();
        private void OnDisable() => UnSubscribeToEvents();

        #endregion

        #region Private Methods
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
            UpdateScore(missileValue);
        }

        private void OnRoundFinish(int roundNumber)
        {
            int citiesLeft = PlayerCity.AllPlayerCities.Count;
            int ammoLeft = player.AvailableMissiles;

            UpdateScore(citiesLeft * _pointsPerCity + ammoLeft * _pointsPerAmmo);
        }
        private void UpdateScore(int value)
        {
            TotalScore += value;
            OnTotalScoreChange?.Invoke(TotalScore);

            ScoreThisRound += value;
        }

        #endregion
    }
}
