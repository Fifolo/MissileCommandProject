using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Min(0.5f)]
        [SerializeField] protected float _spawnRate = 5f;
        [SerializeField] protected T _objectToSpawn;

        protected List<PlayerCity> _playerCities;
        protected Transform _spawnerTransform;
        protected Coroutine _spawningCoroutine;

        protected virtual void Awake() => GetReferences();
        protected virtual void GetReferences() => _spawnerTransform = transform;

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

        protected virtual void OnEnable()
        {
            _playerCities = PlayerCity.AllPlayerCities;
            RoundManager.OnRoundStart += OnRoundStart;
        }
        protected virtual void OnDisable() => RoundManager.OnRoundStart -= OnRoundStart;
        protected abstract void OnRoundStart(int roundNumber);
        protected abstract bool SpawnCondition();
        protected abstract IEnumerator Spawning();
    }
}
