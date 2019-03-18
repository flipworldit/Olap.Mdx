namespace SystemExt
{
    public class Range<T>
    {
        public T StartValue { get; set; }
        public T EndValue { get; set; }

        public Range(T startValue, T endValue)
        {
            StartValue = startValue;
            EndValue = endValue;
        }   
    }
}