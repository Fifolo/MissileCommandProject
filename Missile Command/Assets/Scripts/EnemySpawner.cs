using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Utils;

namespace MissileCommand
{
    public class EnemySpawner : Singleton<EnemySpawner>
    {
        public delegate void EnemyEvent();
        public event EnemyEvent OnNoMoreMissilesToSpawn;

        [SerializeField] private EnemyMissile _enemyMissile;
        [SerializeField] private float _reapeatRate = 1f;
        [SerializeField] private float _spawnRange = 10f;

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
            RoundManager.OnRoundStart += RoundManager_OnRoundStart;
        }

        private void RoundManager_OnRoundStart(int roundNumber)
        {
            _availableMissiles = RoundManager.Instance.EnemyMissilesPerRound;
            StartSpawning();
        }

        private IEnumerator Spawning()
        {
            while (_playerCities.HasItems() && _availableMissiles > 0)
            {
                SpawnMissile();
                yield return new WaitForSeconds(_reapeatRate);
            }

            yield return new WaitForSeconds(2f);
            OnNoMoreMissilesToSpawn?.Invoke();
        }
        private void SpawnMissile()
        {
            PlayerCity targetCity = _playerCities.GetRandomItem();

            Vector2 targetDestination = targetCity.transform.position;
            Vector2 spawnPosition = _spawnerTransform.position;
            spawnPosition.x += Random.Range(-_spawnRange, _spawnRange);

            EnemyMissile missile = Pooler<EnemyMissile>.Instance.GetObject();
            missile.transform.position = spawnPosition;
            missile.SetDestination(targetDestination);
            missile.gameObject.SetActive(true);

            _availableMissiles--;
        }
        private void StopSpawning() => StopCoroutine(_spawningCoroutine);
        private void StartSpawning()
        {
            if (_spawningCoroutine != null)
                StopSpawning();

            _spawningCoroutine = StartCoroutine(Spawning());
        }
    }
}
