using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerCity : MonoBehaviour, ITarget
    {
        public delegate void CityEvent();
        public static event CityEvent OnLastCityDestroyed;
        public static List<PlayerCity> AllPlayerCities { get; private set; }

        private void Awake()
        {
            if (AllPlayerCities == null) AllPlayerCities = new List<PlayerCity>();
            AllPlayerCities.Add(this);
        }
        public void Hit()
        {
            Destroy(gameObject);

            AllPlayerCities.Remove(this);
            if (!AllPlayerCities.HasItems())
                OnLastCityDestroyed?.Invoke();
        }
    }
}
