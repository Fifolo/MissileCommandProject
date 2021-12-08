using UnityEngine;

namespace MissileCommand
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ObjectMover))]
    public abstract class Missile : MonoBehaviour
    {
        #region Varables

        protected Transform _missileTransform;
        protected ObjectMover _objectMover;
        protected DestructionSpawner _destructionSpawner;

        #endregion

        #region MonoBehaviour

        protected virtual void Awake()
        {
            _objectMover = GetComponent<ObjectMover>();
            _destructionSpawner = GetComponent<DestructionSpawner>();
            _missileTransform = transform;
        }

        private void Update()
        {
            _objectMover.Move();

            if (_objectMover.ReachedDestination())
            {
                DestinationReached();
            }
        }

        #endregion

        #region Protected Methods

        protected void DestinationReached()
        {
            if (_destructionSpawner)
            {
                _destructionSpawner.SpawnDestruction();
            }

            Destroy();
        }
        protected virtual void Destroy() => Destroy(gameObject);

        #endregion

        #region Public Methods

        public virtual void SetDestination(Vector2 destination) => _objectMover.SetDestination(destination);

        #endregion
    }
}
