using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Utils;

namespace MissileCommand.Pooling
{
    public abstract class Pooler<T> : Singleton<Pooler<T>> where T : MonoBehaviour
    {
        #region Variables
        [SerializeField] protected T prefabToPool;
        [SerializeField] protected int _amount = 10;

        protected List<T> _freeList;
        protected List<T> _usedList;

        protected Transform _poolerTransform;
        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            _poolerTransform = transform;
            Initialize();
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            _freeList = new List<T>();
            _usedList = new List<T>();

            for (int i = 0; i < _amount; i++)
            {
                T prefab = Instantiate(prefabToPool, _poolerTransform);
                prefab.gameObject.SetActive(false);
                _freeList.Add(prefab);
            }
        }

        #endregion

        #region Public Methods

        public T GetObject()
        {
            T prefabToReturn;

            if (_freeList.HasItems())
            {
                prefabToReturn = _freeList[0];
                _freeList.Remove(prefabToReturn);
                _usedList.Add(prefabToReturn);
            }

            else
            {
                prefabToReturn = Instantiate(prefabToPool, _poolerTransform);
                prefabToReturn.gameObject.SetActive(false);
                _usedList.Add(prefabToReturn);
            }

            return prefabToReturn;
        }
        public virtual void ReturnObject(T objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            objectToReturn.transform.SetParent(_poolerTransform);
            objectToReturn.transform.localPosition = Vector3.zero;
            _freeList.Add(objectToReturn);
            _usedList.Remove(objectToReturn);
        }

        #endregion
    }
}
