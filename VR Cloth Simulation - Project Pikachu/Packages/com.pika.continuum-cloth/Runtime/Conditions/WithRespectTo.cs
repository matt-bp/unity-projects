using System;

namespace Conditions
{
    public class WithRespectTo<T>
    {
        public T dx0 { get; set; }
        public T dx1 { get; set; }
        public T dx2 { get; set; }

        public T this[int index] =>
            index switch
            {
                0 => dx0,
                1 => dx1,
                2 => dx2,
                _ => throw new ArgumentOutOfRangeException(nameof(index), $"Not expected index: {index}")
            };
    }

    public class WithRespectToV<T>
    {
        public T dv0 { get; set; }
        public T dv1 { get; set; }
        public T dv2 { get; set; }
    }
    
    public class WithRespectTo4<T>
    {
        public T dx0 { get; set; }
        public T dx1 { get; set; }
        public T dx2 { get; set; }
        public T dx3 { get; set; }

        public T this[int index] =>
            index switch
            {
                0 => dx0,
                1 => dx1,
                2 => dx2,
                3 => dx3,
                _ => throw new ArgumentOutOfRangeException(nameof(index), $"Not expected index: {index}")
            };
    }
}