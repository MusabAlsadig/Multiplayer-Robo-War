using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using HelpingMethods;


namespace HelpingMethods
{
    /// <summary>
    /// class full of helping methodes
    /// </summary>
    public static class Helper
    {

        /// <summary>
        /// from world direction to tile direction
        /// </summary>
        public static Vector2Int Rotate45Left(Vector2Int normalDirection)
        {
            if (normalDirection == Vector2Int.up) // up
                return Vector2Int.one;
            else if (normalDirection == Vector2Int.one) // up right
                return Vector2Int.right;
            else if (normalDirection == Vector2Int.right) // right
                return new Vector2Int(1, -1);
            else if (normalDirection == new Vector2Int(1, -1)) // down right
                return Vector2Int.down;
            else if (normalDirection == Vector2Int.down) // down
                return -Vector2Int.one;
            else if (normalDirection == -Vector2Int.one) // down left
                return Vector2Int.left;
            else if (normalDirection == Vector2Int.left) // left
                return new Vector2Int(-1, 1);
            else if (normalDirection == new Vector2Int(-1, 1)) // up left
                return Vector2Int.up;
            else if (normalDirection == Vector2Int.zero) // nothing
                return Vector2Int.zero;

            else
            {
                Debug.LogWarning($"forget to add this <color=blue>{normalDirection}</color>");
                return Vector2Int.zero;
            }

        }

        public static int AbsDistanceInCells(Vector3Int a, Vector3Int b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);

            return Mathf.Min(xDistance, yDistance);
        }


        /// <summary>
        /// get sign of each axie
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3Int GetSign(Vector3 point)
        {
            int xSign = System.Math.Sign(point.x);
            int ySign = System.Math.Sign(point.y);
            int zSign = System.Math.Sign(point.z);

            return new Vector3Int(xSign, ySign, zSign);
        }

        public static Vector2Int GetSign(Vector2 point)
        {
            int xSign = System.Math.Sign(point.x);
            int ySign = System.Math.Sign(point.y);

            return new Vector2Int(xSign, ySign);
        }

        public static Vector3Int Abs(Vector3Int cell)
        {
            int x = Mathf.Abs(cell.x);
            int y = Mathf.Abs(cell.y);
            int z = Mathf.Abs(cell.z);

            return new Vector3Int(x, y, z);
        }

        /// <summary>
        /// x*x, y*y
        /// </summary>
        /// <param name="cell_1"></param>
        /// <param name="cell_2"></param>
        /// <returns></returns>
        public static Vector2Int MultiplyAxies(Vector2Int cell_1, Vector2Int cell_2)
        {
            int x = cell_1.x * cell_2.x;
            int y = cell_1.y * cell_2.y;

            return new Vector2Int(x, y);
        }

        /// <summary>
        /// x*x, y*y, z*z
        /// </summary>
        /// <param name="cell_1"></param>
        /// <param name="cell_2"></param>
        /// <returns></returns>
        public static Vector3Int MultiplyAxies(Vector3Int cell_1, Vector3Int cell_2)
        {
            int x = cell_1.x * cell_2.x;
            int y = cell_1.y * cell_2.y;
            int z = cell_1.z * cell_2.z;

            return new Vector3Int(x, y, z);
        }



        #region Redirect methods, Remove Later
        public static int RandomNumber(int min, int max, System.Random systemRandom, List<int> exceptions)
        {
            return HelpingMethods.MathMethods.RandomNumber(min, max, systemRandom, exceptions);
        }
        #endregion

    }

}