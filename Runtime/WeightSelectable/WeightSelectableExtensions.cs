using System;
using System.Collections.Generic;
using System.Linq;

namespace WeightSelectable
{
    public static class WeightSelectableExtensions
    {
        private static Random random = new Random();

        public static T RandomByWeight<T>(this IEnumerable<T> enumerable) where T : IWeightSelectable
        {
            if (!enumerable.Any())
            {
                throw new ArgumentException("Empty collection");
            }

            float totalWeight = enumerable.Sum(item => item.Weight);

            if (totalWeight <= 0)
            {
                throw new ArgumentException("Total weight must be greater than zero.");
            }

            float choice = (float)random.NextDouble() * totalWeight;

            foreach (var item in enumerable)
            {
                if (choice < item.Weight)
                {
                    return item;
                }
                choice -= item.Weight;
            }

            throw new InvalidOperationException("The execution should never reach here.");
        }
    }

}