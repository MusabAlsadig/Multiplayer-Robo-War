namespace HelpingMethods
{
    public class RefrenceHolder<T>
    {
        public int index;
        public T value;

        public RefrenceHolder(int _index, T _value)
        {
            index = _index;
            value = _value;
        }
    }

}