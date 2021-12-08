using MissileCommand.Pooling;
using MissileCommand.Managers;

namespace MissileCommand
{
    public class PlayerMissile : Missile
    {
        private void OnEnable()
        {
            if (MissilesManager.Instance)
                _objectMover.SetMovementSpeed(MissilesManager.Instance.PlayerMissileSpeed);
        }

        protected override void Destroy()
        {
            if (Pooler<PlayerMissile>.Instance)
                Pooler<PlayerMissile>.Instance.ReturnObject(this);

            else base.Destroy();
        }
    }
}
