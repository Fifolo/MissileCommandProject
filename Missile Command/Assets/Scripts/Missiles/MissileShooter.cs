using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    [DisallowMultipleComponent]
    public abstract class MissileShooter<T> : MonoBehaviour where T : Missile
    {
        [SerializeField] private T _missile;

        public void Shoot(Vector2 destination, Vector2 spawnPosition)
        {
            T missile;

            if (Pooler<T>.Instance)
                missile = Pooler<T>.Instance.GetObject();

            else missile = Instantiate(_missile, spawnPosition, Quaternion.identity);

            missile.transform.position = spawnPosition;

            missile.SetDestination(destination);
            missile.gameObject.SetActive(true);
        }
    }
}
