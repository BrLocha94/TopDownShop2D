namespace Project.Utils
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class ListExtensions
    {
        public static void ConvertDoubleArrayToList<T>(this List<T> list, T[,] doubleArray)
        {
            if (list.Count > 0)
            {
                Debug.Log("List was not empty, cant convert");
                return;
            }

            for (int i = 0; i < doubleArray.GetLength(0); i++)
            {
                for (int j = 0; j < doubleArray.GetLength(1); j++)
                {
                    list.Add(doubleArray[i, j]);
                }
            }
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);

            int random = Random.Range(0, list.Count);
            return list[random];
        }

        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T temp = list[i];
                int randomIndex = Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        public static void TryToAdd<T>(this List<T> list, T target)
        {
            if (target != null && !list.Contains(target))
                list.Add(target);
        }

        public static T Pop<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                T entity = list[0];
                list.RemoveAt(0);
                return entity;
            }

            Debug.Log("List is empty");
            return default;
        }

        public static T PopRandom<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                int randomPosition = Random.Range(0, list.Count);
                T entity = list[randomPosition];
                list.RemoveAt(randomPosition);
                return entity;
            }

            Debug.Log("List is empty");
            return default;
        }

        public static void DebugList<T>(this List<T> list)
        {
            Debug.Log("List:");
            foreach (T entity in list)
            {
                Debug.Log("     Entity: " + entity);
            }
        }
    }
}