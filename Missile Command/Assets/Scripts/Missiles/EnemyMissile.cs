using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public class EnemyMissile : Missile, ITarget
    {
        [SerializeField] private int _missileValue = 10;

        public delegate void MissileEvent(int missileValue);
        public static event MissileEvent OnEnemyMissileDestroyed;
        public void Hit()
        {
            DestinationReached();
        }

        private void OnEnable()
        {
            if (MissilesManager.Instance)
                _movementSpeed = MissilesManager.Instance.EnemyMissileSpeed;
        }
        protected override void Destroy()
        {
            OnEnemyMissileDestroyed?.Invoke(_missileValue);

            if (Pooler<EnemyMissile>.Instance)
                Pooler<EnemyMissile>.Instance.ReturnObject(this);

            else base.Destroy();
        }
    }
}
