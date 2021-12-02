using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public class PlayerCity : MonoBehaviour, ITarget
    {
        public delegate void CityEvent(PlayerCity city);
        public static event CityEvent OnCityDestroyed;
        public static List<PlayerCity> AllPlayerCities { get; private set; }

        private void Awake()
        {
            if (AllPlayerCities == null) AllPlayerCities = new List<PlayerCity>();
            AllPlayerCities.Add(this);
        }
        public void Hit()
        {
            AllPlayerCities.Remove(this);
            OnCityDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
