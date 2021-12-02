using UnityEngine;

namespace MissileCommand
{
    public abstract class Missile : MonoBehaviour
    {
        [SerializeField] protected MissileDestruction _missileDestruction;
        protected Vector2 _destination;
        protected Vector2 _movementDirection;
        protected float _movementSpeed = 3f;
        protected Transform _missileTransform;
        protected virtual void Awake() => _missileTransform = transform;

        private void Update()
        {
            Move();

            if (ReachedDestination())
            {
                DestinationReached();
            }
        }

        protected virtual bool ReachedDestination()
        {
            return Vector3.Distance(_missileTransform.position, _destination) <= (MissilesManager.Instance == null ? MissilesManager.Instance.StoppingDistance : 0.1f);
        }

        protected virtual void Move()
        {
            _missileTransform.Translate(_movementDirection * _movementSpeed * Time.deltaTime);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ITarget target))
            {
                target.Hit();
                DestinationReached();
            }
        }
        protected void DestinationReached()
        {
            if (_missileDestruction)
            {

                if (Pooler<MissileDestruction>.Instance)
                {
                    MissileDestruction missileDestruction = Pooler<MissileDestruction>.Instance.GetObject();
                    missileDestruction.transform.position = _missileTransform.position;
                    missileDestruction.gameObject.SetActive(true);
                }
                else
                    Instantiate(_missileDestruction, _missileTransform.position, Quaternion.identity);
            }
            Destroy();
        }

        protected virtual void Destroy()
        {
            Destroy(gameObject);
        }

        public virtual void SetDestination(Vector2 destination)
        {
            _destination = destination;
            Vector2 currentPosition = _missileTransform.position;
            _movementDirection = (_destination - currentPosition).normalized;
        }
    }
}
