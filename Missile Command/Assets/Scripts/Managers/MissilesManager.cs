using UnityEngine;
using MissileCommand.Utils;

namespace MissileCommand.Managers
{
    public class MissilesManager : Singleton<MissilesManager>
    {
        #region Variables

        [Header("Base Attributes")]
        [Min(1f)]
        [SerializeField] private float _enemyMissileSpeed = 3f;
        [Min(1f)]
        [SerializeField] private float _playerMissileSpeed = 3f;
        [Range(0.001f, 0.5f)]
        [SerializeField] private float _stoppingDistance = 0.01f;
        [Range(0, 1f)]
        [SerializeField] private float _chanceToDuplicate = 0f;
        [Range(0f, 1f)]
        [Tooltip("1 - enemy missiles can't duplicate at all\n" + "0 - enemy missiles can duplicate everywhere")]
        [SerializeField] private float _duplicationBlock = 0.5f;
        [Min(5)]
        [Tooltip("Base number of missiles per round for player")]
        [SerializeField] private int _basePlayerMissiles = 10;
        [Min(1)]
        [Tooltip("Base number of missiles per round for enemy spawner")]
        [SerializeField] private int _baseEnemyMissiles = 5;

        private float _maxDuplicationHeight = 0;
        #endregion

        #region Getters and Setters

        public float EnemyMissileSpeed
        {
            get { return _enemyMissileSpeed; }
            set
            {
                if (value > 1)
                    _enemyMissileSpeed = value;
            }
        }
        public float PlayerMissileSpeed
        {
            get { return _playerMissileSpeed; }
            set
            {
                if (value > 1)
                    _playerMissileSpeed = value;
            }
        }
        public float StoppingDistance
        {
            get { return _stoppingDistance; }
            set
            {
                if (value > 0 && value < 1)
                    _stoppingDistance = value;
            }
        }
        public float ChanceToDuplicate
        {
            get { return _chanceToDuplicate; }
            set
            {
                if (value >= 0 && value <= 1)
                    _chanceToDuplicate = value;
            }
        }
        public int PlayerMissilesPerRound
        {
            get { return _basePlayerMissiles; }
            set
            {
                if (value > 0)
                    _basePlayerMissiles = value;
            }
        }
        public int EnemyMissilesPerRound
        {
            get { return _baseEnemyMissiles; }
            set
            {
                if (value > 0)
                    _baseEnemyMissiles = value;
            }
        }

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            SetMaxDuplicationHeight();
            RoundManager.OnRoundFinish += OnRoundFinish;
        }

        private void OnRoundFinish(int roundNumber)
        {
            PlayerMissileSpeed += 0.1f;
            EnemyMissileSpeed += 0.1f;
        }

        #endregion

        #region Private Methods

        private void SetMaxDuplicationHeight()
        {
            float maxHeight = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
            float lowestHeight = Camera.main.ScreenToWorldPoint(Vector2.zero).y;

            _maxDuplicationHeight = Mathf.Lerp(lowestHeight, maxHeight, _duplicationBlock);
        }
        private bool IsAboveSpawnLine(float yPosition) => yPosition > _maxDuplicationHeight;

        #endregion

        #region Public Methods

        public bool CanDuplicate(Vector2 currentPosition) => IsAboveSpawnLine(currentPosition.y) && PlayerCity.AllPlayerCities.Count > 1;

        #endregion
    }
}
