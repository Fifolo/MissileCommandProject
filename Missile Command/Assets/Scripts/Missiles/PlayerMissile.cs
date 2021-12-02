using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public class PlayerMissile : Missile
    {
        private void OnEnable()
        {
            if(MissilesManager.Instance)
            _movementSpeed = MissilesManager.Instance.PlayerMissileSpeed;
        }

        protected override void Destroy()
        {
            if (Pooler<PlayerMissile>.Instance)
                Pooler<PlayerMissile>.Instance.ReturnObject(this);

            else base.Destroy();
        }
    }
}
