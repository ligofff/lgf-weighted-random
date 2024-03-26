using System;
using System.Collections.Generic;
using System.Linq;

namespace WeightSelectable
{
    public static class WeightSelectableExtensions
    {
        private static readonly Random Random = new Random();
        
        public static T RandomWithoutWeight<T>(this IEnumerable<T> enumerable)
        {
            T result = default;
            int count = 0;

            foreach(var item in enumerable)
            {
                count++;
                if(Random.Next(count) == 0)
                {
                    result = item;
                }
            }
    
            if (count == 0)
            {
                throw new InvalidOperationException("The sequence is empty.");
            }

            return result;
        }

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

            float choice = (float)Random.NextDouble() * totalWeight;

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
        
        public static List<T> RandomByWeight<T>(this IEnumerable<T> enumerable, int selectedCount) where T : IWeightSelectable
        {
            List<T> localCollection = enumerable.ToList();
            
            if (!localCollection.Any())
            {
                throw new ArgumentException("Empty collection");
            }

            if (localCollection.Count < selectedCount)
            {
                throw new ArgumentException("Number of elements to select should not exceed the total elements in the collection.");
            }

            float totalWeight = localCollection.Sum(item => item.Weight);

            if (totalWeight <= 0)
            {
                throw new ArgumentException("Total weight must be greater than zero.");
            }
            
            List<T> selectedItems = new List<T>();
            
            for (int i = 0; i < selectedCount; i++)
            {
                float choice = (float)Random.NextDouble() * totalWeight;

                for (int j = 0; j < localCollection.Count; j++)
                {
                    if (choice < localCollection[j].Weight)
                    {
                        selectedItems.Add(localCollection[j]);
                        totalWeight -= localCollection[j].Weight;
                        localCollection.RemoveAt(j);
                        break;
                    }

                    choice -= localCollection[j].Weight;
                }
            }

            return selectedItems;
        }
    }
}