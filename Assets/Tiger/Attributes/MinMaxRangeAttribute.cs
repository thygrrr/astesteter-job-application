using System;

namespace Tiger.Audio
{
    public class MinMaxRangeAttribute : Attribute
    {
        public MinMaxRangeAttribute(float min, float max, bool integer = false)
        {
            Min = min;
            Max = max;
            Integer = integer;
        }
        public float Min { get; private set; }
        public float Max { get; private set; }
        public bool Integer { get; private set; }
    }
}
