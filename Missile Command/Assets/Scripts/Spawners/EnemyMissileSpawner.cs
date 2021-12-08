using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    [RequireComponent(typeof(EnemyMissileShooter))]
    public class EnemyMissileSpawner : Spawner<EnemyMissile>
    {
        [SerializeField] private float _spawnRange = 10f;

        private EnemyMissileShooter _missileShooter;

        private int _availableMissiles = 0;
        public int AvailableMissiles { get { return _availableMissiles; } }

        private void Start() => _playerCities = PlayerCity.AllPlayerCities;
        protected override void GetReferences()
        {
            base.GetReferences();
            _missileShooter = GetComponent<EnemyMissileShooter>();
        }

        protected override void OnRoundStart(int roundNumber)
        {
            if (MissilesManager.Instance)
                _availableMissiles = MissilesManager.Instance.EnemyMissilesPerRound;
            else _availableMissiles = 10;

            StartSpawning();
        }

        protected override IEnumerator Spawning()
        {
            while (SpawnCondition())
            {
                SpawnObject();
                yield return new WaitForSeconds(_spawnRate);
            }
        }
        private void SpawnEnemyMissile(Vector2 spawnPosition, Vector2 destination)
        {
            _missileShooter.Shoot(destination, spawnPosition);
            _availableMissiles--;
        }
        private void SpawnEnemyMissile(Vector2 spawnPosition)
        {
            Vector2 destination = Vector2.zero;

            if (_playerCities.HasItems())
                destination = _playerCities.GetRandomItem().transform.position;

            SpawnEnemyMissile(spawnPosition, destination);
        }
        protected override void SpawnObject()
        {
            Vector2 spawnPosition = _spawnerTransform.position;
            spawnPosition.x += Random.Range(-_spawnRange, _spawnRange);

            SpawnEnemyMissile(spawnPosition);
        }
        public void SpawnMissile(Vector2 spawnPosition, Vector2 destination)
        {
            SpawnEnemyMissile(spawnPosition, destination);
        }
        protected override bool SpawnCondition()
        {
            return _playerCities.HasItems() && _availableMissiles > 0;
        }
    }
}
