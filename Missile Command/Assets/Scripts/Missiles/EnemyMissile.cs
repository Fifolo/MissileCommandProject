using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public class EnemyMissile : Missile, ITarget, IPointIncreaserOnDeath
    {
        public delegate void EnemyMissileEvent();
        public static event EnemyMissileEvent OnLastMissileDestroyed;
        [SerializeField] private int _missileValue = 10;
        private static List<EnemyMissile> _activeMissiles;
        public static bool ExistsActive { get { return _activeMissiles.HasItems(); } }
        protected override void Awake()
        {
            base.Awake();
            if (_activeMissiles == null) _activeMissiles = new List<EnemyMissile>();
        }
        public void Hit()
        {
            DestinationReached();
        }
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

        protected override void Destroy()
        {
            if (Pooler<EnemyMissile>.Instance)
                Pooler<EnemyMissile>.Instance.ReturnObject(this);

            else base.Destroy();
        }

        public int GetValue() => _missileValue;
    }
}
