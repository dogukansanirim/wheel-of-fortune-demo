using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace WheelOfFortune.Helper
{
    public static class ListExtension
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
            return list;
        }
    }
}