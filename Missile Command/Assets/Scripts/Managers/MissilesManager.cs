using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Utils;

namespace MissileCommand
{
    public class MissilesManager : Singleton<MissilesManager>
    {
        [Min(1f)]
        [SerializeField] private float _enemyMissileSpeed = 3f;
        [Min(1f)]
        [SerializeField] private float _playerMissileSpeed = 3f;
        [Range(0.001f, 1f)]
        [SerializeField] private float _stoppingDistance = 0.01f;

        public float EnemyMissileSpeed
        {
            get { return _enemyMissileSpeed; }
            set
            {
                if (value > 1)
                    _enemyMissileSpeed = value;
            }
        }
        public float PlayerMissileSpeed
        {
            get { return _playerMissileSpeed; }
            set
            {
                if (value > 1)
                    _playerMissileSpeed = value;
            }
        }
        public float StoppingDistance
        {
            get { return _stoppingDistance; }
            set
            {
                if (value > 0 && value < 1)
                    _stoppingDistance = value;
            }
        }
    }
}
