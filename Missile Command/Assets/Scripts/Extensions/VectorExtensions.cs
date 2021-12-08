using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public static class VectorExtensions
    {
        public static Vector2 DirectionTo(this Vector2 origin, Vector2 destination)
        {
            return (destination - origin).normalized;
        }
    }
}
