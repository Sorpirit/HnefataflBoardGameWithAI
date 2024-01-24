namespace Core
{
    public static class ArrayHelpers
    {
        public static bool SequenceEqual<T>(this T[] array1, T[] array2)
        {
            if (array1 == array2)
                return true;
            
            if (array1.Length != array2.Length)
                return false;
            
            for (var i = 0; i < array1.Length; i++)
            {
                if (!array1[i].Equals(array2[i]))
                    return false;
            }

            return true;
        }
        
        public static bool SequenceEqual<T>(this T[,] array1, T[,] array2)
        {
            if (array1 == array2)
                return true;
            
            if (array1.GetLength(0) != array2.GetLength(0) ||
                array1.GetLength(1) != array2.GetLength(1))
                return false;
            
            for (var x = 0; x < array1.GetLength(0); x++)
            for (var y = 0; y < array1.GetLength(1); y++)
            {
                if (!array1[x, y].Equals(array2[x, y]))
                    return false;
            }

            return true;
        }
    }
}