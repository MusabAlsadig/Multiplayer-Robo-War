using System.Collections.Generic;

namespace HelpingMethods
{
    public static class Sorter
    {
        #region Arrays
        public static int[] Sort(int[] array)
        {
            int length = array.Length;
            int[] numberOfSmaller = new int[length];

            int currentChecking;
            int[] result = new int[length];

            for (int i = 0; i < length; i++)
            {
                currentChecking = array[i];
                for (int j = 0; j < length; j++)
                {
                    if (i == j)
                        continue;

                    if (currentChecking > array[j])
                        numberOfSmaller[i]++;
                }
            }

            int n;
            for (int i = 0; i < length; i++)
            {
                n = numberOfSmaller[i];
                result[n] = array[i];
            }

            for (int i = 1; i < length; i++)
            {
                if (result[i] == 0)
                    result[i] = result[i - 1];
            }

            return result;
        }

        public static float[] Sort(float[] array)
        {
            int length = array.Length;
            int[] numberOfSmaller = new int[length];

            float currentChecking;
            float[] result = new float[length];

            for (int i = 0; i < length; i++)
            {
                currentChecking = array[i];
                for (int j = 0; j < length; j++)
                {
                    if (i == j)
                        continue;

                    if (currentChecking > array[j])
                        numberOfSmaller[i]++;
                }
            }

            int n;
            for (int i = 0; i < length; i++)
            {
                n = numberOfSmaller[i];
                result[n] = array[i];
            }

            for (int i = 1; i < length; i++)
            {
                if (result[i] == 0)
                    result[i] = result[i - 1];
            }

            return result;
        }

        public static List<T> SortUpward<T>(List<T> objects) where T : ISortable<T>
        {
            int count = objects.Count;
            T[] temp = new T[count];
            List<RefrenceHolder<T>> repeatedObjects = new List<RefrenceHolder<T>>();
            for (int i = 0; i < count; i++)
            {
                T currentTarget = objects[i];
                int index = 0;
                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                    {
                        // no need to compare object to it self
                        continue;
                    }

                    // get the object place by seeing how many objects are smaller than it
                    if (currentTarget.IsHigher(objects[j]))
                        index++;


                }

                if (temp[index] == null)
                    temp[index] = currentTarget;
                else
                    repeatedObjects.Add(new RefrenceHolder<T>(index, currentTarget));
            }


            // assign objects with similer rank/place to ones already in the array
            int offset = 1;
            foreach (var repeated in repeatedObjects)
            {
                int index = repeated.index + offset;
                temp[index] = repeated.value;

                offset++;
            }

            return new List<T>(temp);
        }
        #endregion

        #region Dictionaries

        /// <summary>
        /// from lo to high
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static T[] Sort<T>(Dictionary<float, T> dictionary)
        {
            int length = dictionary.Count;
            T[] result = new T[length];
            float[] keys = new float[length];
            dictionary.Keys.CopyTo(keys, 0);

            keys = Sort(keys);
            float key;
            T value;

            for (int i = 0; i < length; i++)
            {
                key = keys[i];

                value = dictionary[key];

                result[i] = value;

            }


            return result;
        }


        #endregion
    }

}
