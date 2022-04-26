using System;

namespace RazerBladeSharp
{
    public struct FanSpeed
    {
        public int speed;
        public bool hasValue;
        public bool clamp;

        public int GetSpeed()
        {
            if (speed < 0)
                return 0;

            return speed;
        }
        
        public static FanSpeed Empty => new FanSpeed()
        {
            speed = 0,
            hasValue = false,
            clamp = true
        };

        public static FanSpeed Auto => new FanSpeed()
        {
            speed = 0,
            hasValue = true,
            clamp = true
        };

        public static FanSpeed Parse(string str)
        {
            if (string.IsNullOrEmpty(str))
                return FanSpeed.Empty;

            str = str.Trim();
            if (str.Equals("auto", StringComparison.OrdinalIgnoreCase))
                return FanSpeed.Auto;

            bool clamp = true;
            if (str.EndsWith("u", StringComparison.OrdinalIgnoreCase))
            {
                clamp = false;
                str = str.Substring(0, str.Length - 1);
            }
            
            if (int.TryParse(str, out var speed))
            {
                if (speed < 0)
                    speed = 0;

                if (speed > 255 * 100)
                    speed = 255 * 100;
                
                return new FanSpeed()
                {
                    speed = speed,
                    clamp = clamp,
                    hasValue = true
                };
            }

            return FanSpeed.Auto;
        }
        
        public override string ToString()
        {
            if (hasValue)
                return "Not Set";
            
            if (speed <= 0)
                return "Auto";
            
            if(clamp)
                return $"{speed}";

            return $"{speed} unclamped";
        }
    }
}