using MissileCommand.Pooling;
using System.Collections;
using UnityEngine;

namespace MissileCommand
{
    [RequireComponent(typeof(Collider2D))]
    public class Destruction : MonoBehaviour
    {
        #region Events
        public delegate void DestructionEvent(int enemyValue);
        public static event DestructionEvent OnEnemyDestroyed;
        #endregion

        #region Variables

        [Range(0.5f, 2f)]
        [SerializeField] private float _lifeSpan = 0.5f;
        [Range(1f, 2f)]
        [SerializeField] private float _endScale = 1.4f;
        public Vector3 StartScale { get; private set; }
        private Transform _destructionTransform;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _destructionTransform = transform;
            StartScale = _destructionTransform.localScale;
            GetComponent<Collider2D>().isTrigger = true;
        }
        private void OnEnable() => StartCoroutine(BeginDestruction());

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ITarget target))
                target.Hit();

            if (other.TryGetComponent(out IPointIncreaserOnDeath pointIncreaser))
                OnEnemyDestroyed?.Invoke(pointIncreaser.GetValue());
        }

        #endregion
        private IEnumerator BeginDestruction()
        {
            float time = 0;
            Vector3 startScale = _destructionTransform.localScale;
            Vector3 endScale = Vector3.one * _endScale;

            while (time < _lifeSpan)
            {
                _destructionTransform.localScale = Vector3.Lerp(startScale, endScale, time / _lifeSpan);
                time += Time.deltaTime;
                yield return null;
            }
            if (Pooler<Destruction>.Instance)
                Pooler<Destruction>.Instance.ReturnObject(this);

            else Destroy(gameObject);
        }
    }
}
