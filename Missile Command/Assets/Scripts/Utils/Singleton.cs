using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand.Utils
{
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; protected set; }
        public bool IsInitialized { get { return Instance != null; } }

        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = (T)this;
            else Debug.LogWarning($"{name}, trying to instatiate second instance of singleton");
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }
}
