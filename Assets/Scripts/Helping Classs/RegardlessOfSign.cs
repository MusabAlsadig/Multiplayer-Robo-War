using UnityEngine;

namespace HelpingMethods
{
    /// <summary>
    /// or <b>Absolute value Editor</b> <br/>
    /// <br/>
    /// all method in this class will effect the numbers regardless of thier sign<br/>
    /// </summary>
    public static class RegardlessOfSign
    {
        /// <summary>
        /// decrease by 1
        /// </summary>
        /// <param name="number"></param>
        /// <returns><b>Exabple</b> : 3 => 2, -3 => -2</returns>
        public static int Decrease(int number)
        {
            int sign = System.Math.Sign(number);

            int unsignedNumber = Mathf.Abs(number);
            unsignedNumber--;

            return unsignedNumber * sign;
        }

        /// <summary>
        /// increase by 1
        /// </summary>
        /// <param name="number"></param>
        /// <returns><b>Exabple</b> : 3 => 4, -3 => -4</returns>
        public static int Increase(int number)
        {
            int sign = System.Math.Sign(number);

            int unsignedNumber = Mathf.Abs(number);
            unsignedNumber++;

            return unsignedNumber * sign;
        }

        /// <summary>
        /// make sure thet <paramref name="number"/> isn't higher than <paramref name="max"/>
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Clamp(int number, int max)
        {
            int sign = System.Math.Sign(number);
            int unsignedNumber = Mathf.Abs(number);

            max = Mathf.Abs(max);

            if (unsignedNumber > max)
                unsignedNumber = max;

            return unsignedNumber * sign;
        }

        /// <summary>
        /// make sure thet each axie isn't higher than <paramref name="max"/>
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3Int Clamp(Vector3Int cell, int max)
        {
            Vector3Int sign = Helper.GetSign(cell);

            Vector3Int unsignedCell = Helper.Abs(cell);
            unsignedCell.x = Clamp(unsignedCell.x, max);
            unsignedCell.y = Clamp(unsignedCell.y, max);
            unsignedCell.z = Clamp(unsignedCell.z, max);

            return Helper.MultiplyAxies(unsignedCell, sign);
        }
    }
}
