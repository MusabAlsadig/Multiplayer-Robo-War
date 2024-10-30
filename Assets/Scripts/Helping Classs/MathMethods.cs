using System.Collections.Generic;
using UnityEngine;

namespace HelpingMethods
{
    public static class MathMethods
    {
        public static float ChageValueWithLimits(float value, Vector2 oldLimits, Vector2Int newLimits) =>
            newLimits.y - (value - oldLimits.x) / (oldLimits.y - oldLimits.x) * (newLimits.y - newLimits.x);

        public static float ChageValueWithLimits(float value, float oldMin, float oldMax, float newMin, float newMax) =>
            newMax - (value - oldMin) / (oldMax - oldMin) * (newMax - newMin);

        /// <summary>
        /// get some random Numbers <br/>
        /// <b>Note</b> : no repeted numbers so the results may be less than <paramref name="count"/>
        /// </summary>
        /// <param name="count">max number of wanted results</param>
        public static int[] Randoms(int min, int max, int count)
        {
            if (min > max)
                SwitchValues(ref min, ref max);

            List<int> validNumbers = new List<int>();
            List<int> result = new List<int>();

            int invalidNumber = min - 1;

            if (IsThisNumberBetween(invalidNumber, min, max))
            {
                // just incase
                Debug.LogError("<color=red>the numbers you are using are or exceded int limits</color>\n" +
                    $"used numbers are min = {min}, max = {max}");
            }


            for (int i = min; i < max + min; i++)
            {
                validNumbers.Add(i); // assign all numbers
                result.Add(invalidNumber); // fill with invaludNumber, to later know which ones aren't assigned
            }

            for (int i = 0; i < count; i++)
            {
                if (validNumbers.Count == 0) // no number left
                    break;

                int random = Random.Range(0, validNumbers.Count);
                int index = validNumbers[random] - min;
                result[index] = validNumbers[random];
                validNumbers.RemoveAt(random);
            }

            for (int i = 0; i < result.Count; i++)
            {
                // clear all unassigned numbers
                if (result[i] == invalidNumber)
                {
                    result.RemoveAt(i);
                    i--;
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// get spacific random Numbers <br/>
        /// <b>Note</b> : no repeted numbers so the results may be less than <paramref name="count"/>
        /// </summary>
        /// <param name="count">max number of wanted results</param>
        public static int[] Randoms(int min, int max, int count, System.Random systemRandom)
        {
            if (min > max)
                SwitchValues(ref min, ref max);

            List<int> validNumbers = new List<int>();
            List<int> result = new List<int>();

            int invalidNumber = min - 1;

            if (IsThisNumberBetween(invalidNumber, min, max))
            {
                // just incase
                Debug.LogError("<color=red>the numbers you are using are or exceded int limits</color>\n" +
                    $"used numbers are min = {min}, max = {max}");
            }

            for (int i = min; i < max + min; i++)
            {
                validNumbers.Add(i); // assign all possible numbers
                result.Add(invalidNumber); // fill with invalidNumber, to later know which ones aren't assigned
            }

            for (int i = 0; i < count; i++)
            {
                if (validNumbers.Count == 0) // no number left
                    break;

                int random = systemRandom.Next(0, validNumbers.Count);
                int index = validNumbers[random] - min;
                result[index] = validNumbers[random];
                validNumbers.RemoveAt(random);
            }

            for (int i = 0; i < result.Count; i++)
            {
                // clear all unassigned numbers
                if (result[i] == invalidNumber)
                {
                    result.RemoveAt(i);
                    i--;
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// get a regeneratable random number,<br/>
        /// Apparently/SeemsLike System.Random with the same seed give same result
        /// </summary>
        /// <param name="systemRandom">seeded random to generate the result</param>
        /// <param name="exceptions">unwanted results</param>
        /// <returns></returns>
        public static int RandomNumber(int min, int max, System.Random systemRandom, List<int> exceptions)
        {
            if (min > max)
                SwitchValues(ref min, ref max);

            List<int> validNumbers = new List<int>();
            for (int i = min; i < max + 1; i++)
            {
                if (exceptions.Contains(i))
                    continue;

                validNumbers.Add(i); // assign all possible numbers
            }

            if (validNumbers.Count == 0)
            {
                Debug.LogError("<color=red><b>Important</b></color> : No posible result for this random" +
                                "this will cause some problems on any thing that used this method");
                return -1000; // this will make the error more clear
            }

            int randomIndex = systemRandom.Next(0, validNumbers.Count);

            return validNumbers[randomIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chance">from 1 - 99</param>
        /// <returns></returns>
        public static bool RandomPercentChance(int chance)
        {
            int r = Random.Range(1, 101);

            if (r < chance)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min">included</param>
        /// <param name="max">excluded</param>
        /// <param name="amount"></param>
        /// <returns> number of times this value have returned to min</returns>
        public static int LoopIncreasing(ref int value, int min, int max, int amount = 1)
        {
            int length = max - min;
            int alpha = value - min + amount;
            value = alpha % length + min;
            return alpha / length;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min">included</param>
        /// <param name="max">excluded</param>
        /// <param name="amoun"></param>
        /// <returns> number of times this value have returned to (max - 1)</returns>
        public static int LoopDecreasing(ref int value, int min, int max, int amoun = 1)
        {

            int doneTimes = 0;

            for (int i = 0; i < amoun; i++)
            {
                value--;
                if (value < min)
                {
                    value = max - 1;
                    doneTimes++;
                }
            }

            return doneTimes;
        }


        public static bool IsThisNumberBetween(int number, int min, int max)
        {
            return min < number && number < max;
            // same result as (number > min && number < max)
        }

        public static void SwitchValues(ref int a, ref int b)
        {
            int temp = a;

            a = b;
            b = temp;
        }
        public static void ClampIncreasing(ref int value, int max)
        {
            value++;
            if (value > max)
                value = max;
        }
        public static void ClampDecreasing(ref int value, int min)
        {
            value--;
            if (value < min)
                value = min;
        }

        /// <summary>
        /// example (4 = IV), 
        /// max = 5
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string IntToRomanNumeral(int number)
        {
            string result;
            switch (number)
            {
                case 0:
                    result = "";
                    break;
                case 1:
                    result = "I";
                    break;
                case 2:
                    result = "II";
                    break;
                case 3:
                    result = "III";
                    break;
                case 4:
                    result = "IV";
                    break;
                case 5:
                    result = "V";
                    break;
                default:
                    Debug.LogError("Trying to get " + number + " on roman");
                    result = "still no " + number + " on this script";
                    break;
            }

            return result;
        }

        /// <summary>
        /// get absolute diffrence bettween the absolute value of 2 numbers
        /// </summary>
        public static int AbsDiffrenceBetween(int n1, int n2)
        {
            n1 = Mathf.Abs(n1);
            n2 = Mathf.Abs(n2);

            return Mathf.Abs(n1 - n2);
        }
    }

}
