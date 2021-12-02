using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MissileCommand
{
    public static class CollectionExtensions
    {
        public static bool HasItems<T>(this ICollection<T> collection)
        {
            return collection.Count > 0;
        }
        public static T GetRandomItem<T>(this ICollection<T> collection)
        {
            if (!collection.HasItems()) return default;

            int randomIndex = Random.Range(0, collection.Count);

            return collection.ElementAt(randomIndex);
        }
    }
}
