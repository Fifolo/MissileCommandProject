using MissileCommand.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Managers;

namespace MissileCommand
{
    public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Variables

        [SerializeField] protected T _objectToSpawn;
        [Min(0.2f)]
        [SerializeField] protected float _minSpawnRate = 5f;
        [Min(0.5f)]
        [SerializeField] protected float _maxSpawnRate = 5f;

        protected List<PlayerCity> _playerCities;
        protected Transform _spawnerTransform;
        protected Coroutine _spawningCoroutine;

        protected virtual void Awake() => GetReferences();
        protected virtual void GetReferences() => _spawnerTransform = transform;

        #endregion

        #region MonoBehaviour
        private void OnValidate()
        {
            if (_minSpawnRate > _maxSpawnRate)
                _minSpawnRate = _maxSpawnRate - 0.1f;

            if (_maxSpawnRate < _minSpawnRate)
                _maxSpawnRate = _minSpawnRate + 0.1f;
        }
        protected virtual void OnEnable()
        {
            _playerCities = PlayerCity.AllPlayerCities;
            RoundManager.OnRoundStart += OnRoundStart;
        }

        protected virtual void OnDisable() => RoundManager.OnRoundStart -= OnRoundStart;

        #endregion

        #region Protected Methods

        protected float GetSpawnRate()
        {
            float randomPercent = Random.Range(0, 1f);
            return Mathf.Lerp(_minSpawnRate, _maxSpawnRate, randomPercent);
        }
        protected void StartSpawning()
        {
            if (_spawningCoroutine != null)
                StopSpawning();

            _spawningCoroutine = StartCoroutine(Spawning());
        }
        protected void StopSpawning() => StopCoroutine(_spawningCoroutine);

        protected virtual void SpawnObject()
        {
            T spawnedObject;
            if (Pooler<T>.Instance)
                spawnedObject = Pooler<T>.Instance.GetObject();

            else spawnedObject = Instantiate(_objectToSpawn, _spawnerTransform.position, Quaternion.identity);

            spawnedObject.gameObject.SetActive(true);
        }
        protected abstract void OnRoundStart(int roundNumber);
        protected abstract bool SpawnCondition();
        protected abstract IEnumerator Spawning();

        #endregion
    }
}
