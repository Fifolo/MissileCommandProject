using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Utils;

namespace MissileCommand
{
    [RequireComponent(typeof(EnemyMissileShooter))]
    public class EnemyMissileSpawner : Singleton<EnemyMissileSpawner>
    {
        public delegate void EnemyEvent();
        public static event EnemyEvent OnNoMoreMissilesToSpawn;

        [SerializeField] private float _reapeatRate = 1f;
        [SerializeField] private float _spawnRange = 10f;

        private EnemyMissileShooter _missileShooter;

        private List<PlayerCity> _playerCities;
        private int _availableMissiles = 0;
        public int AvailableMissiles { get { return _availableMissiles; } }
        private Coroutine _spawningCoroutine;
        private Transform _spawnerTransform;
        protected override void Awake()
        {
            base.Awake();
            _spawnerTransform = transform;
            _playerCities = PlayerCity.AllPlayerCities;
            _missileShooter = GetComponent<EnemyMissileShooter>();
        }

        private void OnEnable() => RoundManager.OnRoundStart += RoundManager_OnRoundStart;
        private void OnDisable() => RoundManager.OnRoundStart -= RoundManager_OnRoundStart;

        private void RoundManager_OnRoundStart(int roundNumber)
        {
            _availableMissiles = RoundManager.Instance.EnemyMissilesPerRound;
            StartSpawning();
        }

        private IEnumerator Spawning()
        {
            while (_playerCities.HasItems() && _availableMissiles > 0)
            {
                SpawnEnemyMissile();
                _availableMissiles--;
                yield return new WaitForSeconds(_reapeatRate);
            }

            yield return new WaitForSeconds(3f);
            OnNoMoreMissilesToSpawn?.Invoke();
        }
        private void SpawnEnemyMissile(Vector2 spawnPosition, Vector2 destination)
        {
            _missileShooter.Shoot(destination, spawnPosition);
        }
        private void SpawnEnemyMissile(Vector2 spawnPosition)
        {
            Vector2 destination = Vector2.zero;

            if (_playerCities.HasItems())
                destination = _playerCities.GetRandomItem().transform.position;

            SpawnEnemyMissile(spawnPosition, destination);
        }
        private void SpawnEnemyMissile()
        {
            Vector2 spawnPosition = _spawnerTransform.position;
            spawnPosition.x += Random.Range(-_spawnRange, _spawnRange);

            SpawnEnemyMissile(spawnPosition);
        }
        private void StopSpawning() => StopCoroutine(_spawningCoroutine);
        private void StartSpawning()
        {
            if (_spawningCoroutine != null)
                StopSpawning();

            _spawningCoroutine = StartCoroutine(Spawning());
        }
        public void SpawnMissile(Vector2 spawnPosition, Vector2 destination)
        {
            SpawnEnemyMissile(spawnPosition, destination);
        }
    }
}
