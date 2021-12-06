using UnityEngine;
using MissileCommand.Utils;

namespace MissileCommand
{
    public class MissilesManager : Singleton<MissilesManager>
    {
        #region Variables

        [Min(1f)]
        [SerializeField] private float _enemyMissileSpeed = 3f;
        [Min(1f)]
        [SerializeField] private float _playerMissileSpeed = 3f;
        [Range(0.001f, 1f)]
        [SerializeField] private float _stoppingDistance = 0.01f;
        [Range(0, 1f)]
        [SerializeField] private float _chanceToDuplicate = 0f;
        [Range(1f, 0f)]
        [SerializeField] private float _duplicationAllowance = 0.5f;
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

        #endregion

        private void Start()
        {
            float maxHeight = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
            float lowestHeight = Camera.main.ScreenToWorldPoint(Vector2.zero).y;

            _maxDuplicationHeight = Mathf.Lerp(lowestHeight, maxHeight, _duplicationAllowance);
        }

        public bool CanDuplicate(Vector2 currentPosition) => IsAboveSpawnLine(currentPosition.y) && PlayerCity.AllPlayerCities.Count > 1;

        private bool IsAboveSpawnLine(float yPosition) => yPosition > _maxDuplicationHeight;
    }
}
