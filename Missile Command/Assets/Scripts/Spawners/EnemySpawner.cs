using MissileCommand.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public class EnemySpawner : Spawner<Enemy>
    {
        #region Variables

        [Min(1)]
        [SerializeField] private int _spawnFromRound = 3;
        [Min(0.5f)]
        [SerializeField] private float _initialWaitTime = 1f;

        private int _availableEnemies = 0;
        public int AvailableEnemies { get { return _availableEnemies; } }

        #endregion

        #region Private Methods
        private void SpawnEnemy(Vector2 spawnPosition)
        {
            List<Vector2> waypoints = new List<Vector2>();
            waypoints.Add(new Vector2(_spawnerTransform.position.x - 7, _spawnerTransform.position.y));
            waypoints.Add(new Vector2(_spawnerTransform.position.x + 7, _spawnerTransform.position.y));

            SpawnEnemy(spawnPosition, waypoints);
        }

        private void SpawnEnemy(Vector2 spawnPosition, List<Vector2> waypoints)
        {
            if (waypoints.Count > 1)
            {
                Enemy enemy;
                if (Pooler<Enemy>.Instance)
                    enemy = Pooler<Enemy>.Instance.GetObject();
                else enemy = Instantiate(_objectToSpawn, spawnPosition, Quaternion.identity);

                enemy.transform.position = spawnPosition;
                enemy.InitializeWaypoints(waypoints);
                enemy.gameObject.SetActive(true);
                enemy.StartShooting();

                _availableEnemies--;
            }
        }

        #endregion

        #region Protected Methods
        
        protected override void OnRoundStart(int roundNumber)
        {
            if (roundNumber >= _spawnFromRound)
            {
                _availableEnemies = roundNumber / 2;
                StartSpawning();
            }
        }

        protected override bool SpawnCondition() => _playerCities.HasItems() && _availableEnemies > 0;

        protected override IEnumerator Spawning()
        {
            yield return new WaitForSeconds(_initialWaitTime);

            while (SpawnCondition())
            {
                SpawnObject();
                yield return new WaitForSeconds(GetSpawnRate());
            }
        }

        protected override void SpawnObject()
        {
            Vector2 spawnPosition = _spawnerTransform.position;
            spawnPosition.x += 5;

            SpawnEnemy(spawnPosition);
        }

        #endregion
    }
}
