using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Pooling;
using MissileCommand.Managers;

namespace MissileCommand
{
    public class EnemyMissile : Missile, ITarget, IPointIncreaserOnDeath
    {
        #region Events

        public delegate void EnemyMissileEvent();
        public static event EnemyMissileEvent OnLastMissileDestroyed;

        #endregion

        #region Variables

        [SerializeField] private int _missileValue = 10;
        private static List<EnemyMissile> _activeMissiles;
        public static bool ExistsActive { get { return _activeMissiles.HasItems(); } }

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            if (_activeMissiles == null) _activeMissiles = new List<EnemyMissile>();
        }

        private void OnEnable()
        {
            _activeMissiles.Add(this);
            if (MissilesManager.Instance)
            {
                _objectMover.SetMovementSpeed(MissilesManager.Instance.EnemyMissileSpeed);

                if (MissilesManager.Instance.ChanceToDuplicate > 0)
                    StartCoroutine(TryToDuplicate());
            }
        }
        private void OnDisable()
        {
            _activeMissiles.Remove(this);

            if (!_activeMissiles.HasItems())
            {
                OnLastMissileDestroyed?.Invoke();
            }
        }

        #endregion

        #region Private Methods

        private void Duplicate()
        {
            List<PlayerCity> playerCities = new List<PlayerCity>(PlayerCity.AllPlayerCities);

            PlayerCity firstCity = playerCities.GetRandomItem();
            Vector2 firstDestination = firstCity.transform.position;
            playerCities.Remove(firstCity);

            PlayerCity secondCity = playerCities.GetRandomItem();
            Vector2 secondDestination = secondCity.transform.position;

            SetDestination(firstDestination);
            FindObjectOfType<EnemyMissileSpawner>().SpawnMissile(_missileTransform.position, secondDestination);
        }

        private IEnumerator TryToDuplicate()
        {
            yield return new WaitForSeconds(0.5f);

            while (!_objectMover.ReachedDestination())
            {
                if (CanDuplicate())
                {
                    Duplicate();
                    break;
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private bool CanDuplicate()
        {
            MissilesManager missilesManager = MissilesManager.Instance;

            if (missilesManager)
            {
                float randomNumber = Random.Range(0, 1f);
                if (randomNumber > missilesManager.ChanceToDuplicate) return false;

                return missilesManager.CanDuplicate(_missileTransform.position);
            }
            return false;
        }

        #endregion

        #region Protected Methods
        protected override void Destroy()
        {
            if (Pooler<EnemyMissile>.Instance)
                Pooler<EnemyMissile>.Instance.ReturnObject(this);

            else base.Destroy();
        }
        #endregion

        #region Public Methods
        public void Hit() => DestinationReached();
        public int GetValue() => _missileValue;

        #endregion
    }
}
