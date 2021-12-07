using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    [RequireComponent(typeof(ObjectMover))]
    [RequireComponent(typeof(EnemyMissileShooter))]
    [RequireComponent(typeof(DestructionSpawner))]
    [RequireComponent(typeof(Collider2D))]
    public class Enemy : MonoBehaviour, ITarget, IPointIncreaserOnDeath
    {
        [Min(1)]
        [SerializeField] private int _enemyValue = 10;
        [Range(1f, 10f)]
        [SerializeField] private float _shootingRate = 5f;

        private List<Vector2> _waypoints;
        private int _destinationIndex = 0;

        private ObjectMover _objectMover;
        private EnemyMissileShooter _missileShooter;
        private DestructionSpawner _destructionSpawner;

        private Coroutine _shootingCoroutine;
        private Transform _enemyTransform;

        private void Awake()
        {
            GetReferences();
            _waypoints = new List<Vector2>();
        }

        private void Start()
        {
            _waypoints.Add(new Vector2(-9f, 3));
            _waypoints.Add(new Vector2(9f, 3));

            _objectMover.SetDestination(_waypoints[0]);
            //_shootingCoroutine = StartCoroutine(Shooting());
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

        private IEnumerator Shooting()
        {
            while (PlayerCity.AllPlayerCities.HasItems())
            {
                yield return new WaitForSeconds(_shootingRate);

                Vector2 destination = PlayerCity.AllPlayerCities.GetRandomItem().transform.position;

                _missileShooter.Shoot(destination, _enemyTransform.position);
            }
        }

        private void PickNewDestination()
        {
            _destinationIndex++;

            if (_destinationIndex >= _waypoints.Count)
                _destinationIndex = 0;

            _objectMover.SetDestination(_waypoints[_destinationIndex]);
        }

        private void GetReferences()
        {
            _enemyTransform = transform;
            _objectMover = GetComponent<ObjectMover>();
            _missileShooter = GetComponent<EnemyMissileShooter>();
            _destructionSpawner = GetComponent<DestructionSpawner>();
        }
        public void InitializeWaypoints(List<Vector2> waypoints)
        {
            if (waypoints.Count > 1)
            {
                _waypoints = waypoints;
                _destinationIndex = 0;
                _objectMover.SetDestination(_waypoints[_destinationIndex]);
            }
        }
        public virtual int GetValue() => _enemyValue;
        public virtual void Hit()
        {
            if (Pooler<Enemy>.Instance)
                Pooler<Enemy>.Instance.ReturnObject(this);

            _destructionSpawner.SpawnDestruction();
            Destroy(gameObject);
        }
    }
}
