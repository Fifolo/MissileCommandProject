using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    [DisallowMultipleComponent]
    public class DestructionSpawner : MonoBehaviour
    {
        [SerializeField] private Destruction missileDestruction;
        public void SpawnDestruction()
        {
            if (Pooler<Destruction>.Instance)
            {
                Destruction missileDestruction = Pooler<Destruction>.Instance.GetObject();
                missileDestruction.transform.position = transform.position;
                missileDestruction.gameObject.SetActive(true);
            }
            else
                Instantiate(missileDestruction, transform.position, Quaternion.identity);
        }
    }
}
