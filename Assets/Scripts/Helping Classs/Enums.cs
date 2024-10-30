using UnityEngine;

namespace HelpingMethods
{
    public static class Enums
    {

        /// <summary>
        /// Used to get the next index of an enum
        /// <br></br>
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="enumType"></param>
        /// <returns>nex index, or the 1st one if this was last index </returns>
        public static int GetLoopedNextIndex(int currentIndex, System.Type enumType)
        {

            int[] indexes = (int[])System.Enum.GetValues(enumType);

            for (int i = 0; i < indexes.Length; i++)
            {
                if (indexes[i] == currentIndex)
                {
                    if (i == indexes.Length - 1)// this is the last one
                        return indexes[0];
                    else return indexes[i + 1];
                }
            }

            Debug.LogWarning($"index {currentIndex} not found on enum type <color=blue>{enumType}</color>");
            return -1;
        }

        public static int GetLoopedLastIndex(int currentIndex, System.Type enumType)
        {

            int[] indexes = (int[])System.Enum.GetValues(enumType);

            for (int i = 0; i < indexes.Length; i++)
            {
                if (indexes[i] == currentIndex)
                {
                    if (i == 0)// this is the first one
                        return indexes[indexes.Length - 1];
                    else return indexes[i - 1];
                }
            }

            Debug.LogWarning($"index {currentIndex} not found on enum type <color=blue>{enumType}</color>");
            return -1;
        }
    }

}
