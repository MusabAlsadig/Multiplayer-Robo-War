using System;
using System.Collections.Generic;
using UnityEngine;

namespace HelpingMethods.Utilites
{

    public struct NumberWithSign
    {
        private sbyte sign;
        private int value;

        public readonly int Number => value * sign;

        public NumberWithSign(int number)
        {
            if (number < 0)
                sign = -1;
            else
                sign = 1;
            value = Math.Abs(number);

        }
        
        public NumberWithSign(int number, int maxValue)
        {
            if (number < 0)
                sign = -1;
            else
                sign = 1;

            maxValue = Mathf.Abs(maxValue);

            value = Math.Abs(number);

            value = Mathf.Clamp(value, 0, maxValue);

        }


        public static NumberWithSign operator ++(NumberWithSign number)
        {
            number.value++;
            return number;
        }
        
        public static NumberWithSign operator --(NumberWithSign number)
        {
            number.value++;
            return number;
        }

        public static NumberWithSign operator +(NumberWithSign number, int i)
        {
            number.value += i;
            return number;
        }

        public static NumberWithSign operator -(NumberWithSign number, int i)
        {
            number.value -= i;
            return number;
        }


        #region Casts
        public static implicit operator int (NumberWithSign number)
        {
            return number.Number;
        }

        #endregion

    }

}