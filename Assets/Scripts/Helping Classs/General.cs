namespace HelpingMethods
{
    public static class General
    {
        public static int GetLastDigit(int number)
        {
            string fullDigits = number.ToString();
            int lastIndex = fullDigits.Length - 1;
            return fullDigits[lastIndex];
        }


    }

}
