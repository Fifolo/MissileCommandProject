using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public class EnemyMissile : Missile, ITarget
    {
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
            if (Pooler<EnemyMissile>.Instance)
                Pooler<EnemyMissile>.Instance.ReturnObject(this);

            else base.Destroy();
        }
    }
}
