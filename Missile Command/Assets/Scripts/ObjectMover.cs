using UnityEngine;

namespace MissileCommand
{
    [DisallowMultipleComponent]
    public class ObjectMover : MonoBehaviour
    {
        #region Variables

        [Min(1)]
        [SerializeField] private float _movementSpeed = 3f;
        [Min(0.1f)]
        private float _stoppingDistance = 0.1f;
        private Vector2 _destination;
        private Vector2 _movementDirection;
        private Transform _moverTransform;

        #endregion

        #region MonoBehaviour
        protected void Awake() => _moverTransform = transform;
        #endregion

        #region Public Methods

        public void Move() => _moverTransform.Translate(_movementDirection * _movementSpeed * Time.deltaTime);

        public bool ReachedDestination() =>
            Vector2.Distance(_moverTransform.position, _destination) <= _stoppingDistance;

        public void Iniialize(Vector2 destination, float movementSpeed, float stoppingDistance)
        {
            SetDestination(destination);
            SetMovementSpeed(movementSpeed);
            SetStoppingDistance(stoppingDistance);
        }

        public void SetStoppingDistance(float stoppingDistance)
        {
            if (stoppingDistance > 0 && stoppingDistance < 2f)
                _stoppingDistance = stoppingDistance;
        }

        public void SetMovementSpeed(float movementSpeed)
        {
            if (movementSpeed > 0 && movementSpeed < 50f)
                _movementSpeed = movementSpeed;
        }

        public void SetDestination(Vector2 destination)
        {
            _destination = destination;
            Vector2 currentPosition = _moverTransform.position;
            _movementDirection = currentPosition.DirectionTo(_destination);
        }

        #endregion
    }
}
