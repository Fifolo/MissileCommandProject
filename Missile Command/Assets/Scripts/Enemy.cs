using MissileCommand.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    #region Components Requirement
    [RequireComponent(typeof(ObjectMover))]
    [RequireComponent(typeof(EnemyMissileShooter))]
    [RequireComponent(typeof(DestructionSpawner))]
    [RequireComponent(typeof(Collider2D))]
    #endregion
    public class Enemy : MonoBehaviour, ITarget, IPointIncreaserOnDeath
    {
        #region Events

        public delegate void EnemyEvent();
        public static event EnemyEvent OnLastEnemyDestroyed;

        #endregion

        #region Variables
        [Min(1)]
        [SerializeField] private int _enemyValue = 10;
        [Range(1f, 10f)]
        [SerializeField] private float _shootingRate = 5f;

        private static List<Enemy> _activeEnemies;
        public static bool ExistsActive { get { return _activeEnemies.HasItems(); } }

        private List<Vector2> _waypoints;
        private int _destinationIndex = 0;

        private ObjectMover _objectMover;
        private EnemyMissileShooter _missileShooter;
        private DestructionSpawner _destructionSpawner;
        private Coroutine _shootingCoroutine;
        private Transform _enemyTransform;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (_activeEnemies == null) _activeEnemies = new List<Enemy>();
            GetReferences();
            _waypoints = new List<Vector2>();
        }
        private void OnEnable() => _activeEnemies.Add(this);
        private void OnDisable()
        {
            _activeEnemies.Remove(this);
            if (!_activeEnemies.HasItems())
                OnLastEnemyDestroyed?.Invoke();
        }

        private void Update()
        {
            if (!_waypoints.HasItems()) return;

            _objectMover.Move();

            if (_objectMover.ReachedDestination())
            {
                PickNewDestination();
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator Shooting()
        {
            while (PlayerCity.AllPlayerCities.HasItems())
            {
                Vector2 destination = PlayerCity.AllPlayerCities.GetRandomItem().transform.position;
                _missileShooter.Shoot(destination, _enemyTransform.position);

                yield return new WaitForSeconds(_shootingRate);
            }
        }
        private void GetReferences()
        {
            _enemyTransform = transform;
            _objectMover = GetComponent<ObjectMover>();
            _missileShooter = GetComponent<EnemyMissileShooter>();
            _destructionSpawner = GetComponent<DestructionSpawner>();
        }

        private void PickNewDestination()
        {
            _destinationIndex++;

            if (_destinationIndex >= _waypoints.Count)
                _destinationIndex = 0;

            _objectMover.SetDestination(_waypoints[_destinationIndex]);
        }

        #endregion

        #region Public Methods

        public void InitializeWaypoints(List<Vector2> waypoints)
        {
            if (waypoints.Count > 1)
            {
                _waypoints = waypoints;
                _destinationIndex = 0;
                _objectMover.SetDestination(_waypoints[_destinationIndex]);
            }
        }
        public void StartShooting()
        {
            if (_shootingCoroutine != null)
                StopShooting();

            _shootingCoroutine = StartCoroutine(Shooting());
        }

        public void StopShooting() => StopCoroutine(_shootingCoroutine);

        public virtual int GetValue() => _enemyValue;
        public virtual void Hit()
        {
            if (_destructionSpawner)
                _destructionSpawner.SpawnDestruction();

            if (Pooler<Enemy>.Instance)
                Pooler<Enemy>.Instance.ReturnObject(this);

            else Destroy(gameObject);
        }

        #endregion
    }
}
