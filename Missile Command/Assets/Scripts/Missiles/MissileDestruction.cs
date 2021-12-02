using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    [RequireComponent(typeof(Collider2D))]
    public class MissileDestruction : MonoBehaviour
    {
        [Range(0.5f, 2f)]
        [SerializeField] private float _lifeSpan = 0.5f;
        [Range(1f, 2f)]
        [SerializeField] private float _endScale = 1.4f;
        public Vector3 StartScale { get; private set; }
        private Transform _destructionTransform;
        private void Awake()
        {
            _destructionTransform = transform;
            StartScale = _destructionTransform.localScale;
            GetComponent<Collider2D>().isTrigger = true;
        }
        private void OnEnable()
        {
            StartCoroutine(Destruction());
        }
        private IEnumerator Destruction()
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
            if (Pooler<MissileDestruction>.Instance)
                Pooler<MissileDestruction>.Instance.ReturnObject(this);

            else Destroy(gameObject);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ITarget target))
                target.Hit();
        }
    }
}
