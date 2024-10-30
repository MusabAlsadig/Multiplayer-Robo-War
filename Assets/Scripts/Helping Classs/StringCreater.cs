using UnityEngine;

namespace HelpingMethods
{
    public static class StringCreater
    {
        /// <summary>
        /// example ( 2999 => 2,999)
        /// </summary>
        /// <param name="money"></param>
        /// <param name="maxDigits"></param>
        /// <returns>money text (or 99999999999+) if its digits are more than max digits</returns>
        public static string CreateMoneyText(int money, int maxDigits = 9)
        {
            char[] c = money.ToString().ToCharArray();
            string s = string.Empty;
            if (c.Length > maxDigits)
            {
                s += "+";
                c = new char[maxDigits];
                for (int i = 0; i < maxDigits; i++)// fill with (9)s
                {
                    c[i] = '9';
                }
            }

            int j = 1;
            // put (,)s on the money text
            for (int i = c.Length - 1; i >= 0; i--)
            {
                s += c[i];
                if (j % 3 == 0 && i != 0) s += ",";
                j++;
            }

            // return the text to the right order
            c = s.ToCharArray();
            s = string.Empty;
            for (int i = c.Length - 1; i >= 0; i--)
            {
                s += c[i];
            }

            return s;
        }

        /// <summary>
        /// <b>Example</b> : change 1 to 1st, 2 to 2nd ....
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ChangeToOrdinalNumber(int number)
        {
            if (number > 10 && number < 14) // exceptions
            {
                return number + "th";
            }
            else if (number == 0)
            {
                Debug.LogError("0 can't be ordinal number");
                return "number 0";
            }

            int lastDidit = General.GetLastDigit(number);

            string suffix = lastDidit switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th", // this option for everything else
            };

            return number + suffix;
        }


        /// <summary>
        /// <b>Exaples</b> : 10 => 10am, 20 => 8pm
        /// </summary>
        /// <param name="hours">from 0 to 24</param>
        /// <returns></returns>
        public static string CreateHourText(int hours)
        {
            int number = hours >= 13 ? hours - 12 : hours;

            string period = hours > 12 ? " PM" : " AM";

            return number.ToString("00") + period;
        }

    }

}
