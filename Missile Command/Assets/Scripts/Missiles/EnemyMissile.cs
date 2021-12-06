using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public class EnemyMissile : Missile, ITarget
    {
        [SerializeField] private int _missileValue = 10;

        public delegate void MissileEvent(int missileValue);
        public static event MissileEvent OnEnemyMissileDestroyed;
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
            EnemyMissileSpawner.Instance.SpawnMissile(_missileTransform.position, secondDestination);
        }
        private void OnEnable()
        {
            if (MissilesManager.Instance)
            {
                _movementSpeed = MissilesManager.Instance.EnemyMissileSpeed;

                if (MissilesManager.Instance.ChanceToDuplicate > 0)
                    StartCoroutine(TryToDuplicate());
            }
        }
        private IEnumerator TryToDuplicate()
        {
            yield return new WaitForSeconds(0.5f);

            while (!ReachedDestination())
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
            OnEnemyMissileDestroyed?.Invoke(_missileValue);

            if (Pooler<EnemyMissile>.Instance)
                Pooler<EnemyMissile>.Instance.ReturnObject(this);

            else base.Destroy();
        }
    }
}
