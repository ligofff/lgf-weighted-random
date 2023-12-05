using System;
using System.Collections.Generic;
using System.Linq;

namespace WeightSelectable
{
    public static class WeightSelectableExtensions
    {
        public static T RandomByWeight<T>(this IEnumerable<T> enumerable) where T : IWeightSelectable
        {
            var list = enumerable.ToList();
            
            if (list == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            if (list.Count == 0)
            {
                throw new ArgumentException("Empty list");
            }

            if (list.Count == 1)
            {
                return list[0];
            }

            var cumulative = list.Select(w => w.Weight).ToList();
            
            for (int i = 1; i < cumulative.Count; i++)
            {
                cumulative[i] += list[i - 1].Weight;
            }

            float random = UnityEngine.Random.Range(0, cumulative[cumulative.Count - 1]);
            int index = cumulative.FindIndex(a => a >= random);
            if (index == -1)
            {
                throw new ArgumentException("Weights must be positive");
            }

            return list[index];
        }
    }
}