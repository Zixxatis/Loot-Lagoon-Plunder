using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CGames
{
    [Serializable] [JsonObject]
    public struct ColoredGems : IEnumerable<int>, IEquatable<ColoredGems>
    {
        public int Red;
        public int Green;
        public int Blue;

        [JsonIgnore] public readonly int TotalGemsAmount => Red + Green + Blue;

        public ColoredGems(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <returns> Return true if all values in 'comparableGems' are lower or equal.</returns>
        public readonly bool IsEnoughFor(ColoredGems comparableGems)
        {
            return comparableGems.Red <= Red &&
                   comparableGems.Green <= Green &&
                   comparableGems.Blue <= Blue;
        }

        public int this[int index]
        {
            readonly get
            {
                return index switch
                {
                    0 => Red,
                    1 => Green,
                    2 => Blue,
                    _ => throw new IndexOutOfRangeException(),
                };
            }

            set
            {
                switch (index)
                {
                    case 0: 
                        Red += value; 
                        break;

                    case 1: 
                        Green += value; 
                        break;

                    case 2: 
                        Blue += value; 
                        break;

                    default: 
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public int this[GemType index]
        {
            readonly get
            {
                return index switch
                {
                    GemType.Red => Red,
                    GemType.Green => Green,
                    GemType.Blue => Blue,
                    _ => throw new IndexOutOfRangeException(),
                };
            }

            set
            {
                switch (index)
                {
                    case GemType.Red: Red += value; break;
                    case GemType.Green: Green += value; break;
                    case GemType.Blue: Blue += value; break;
                    default: throw new IndexOutOfRangeException();
                };
            }
        }

        public static ColoredGems operator +(ColoredGems left, ColoredGems right)
        {
            return new ColoredGems
            {
                Red = left.Red + right.Red,
                Green = left.Green + right.Green,
                Blue = left.Blue + right.Blue,
            };
        }

        public static ColoredGems operator -(ColoredGems left, ColoredGems right)
        {
            return new ColoredGems
            {
                Red = left.Red - right.Red,
                Green = left.Green - right.Green,
                Blue = left.Blue - right.Blue,
            };
        }
        public override readonly string ToString() => $"{Red}, {Green}, {Blue}";

        public readonly IEnumerator<int> GetEnumerator()
        {
            yield return Red;
            yield return Green;
            yield return Blue;
        }

        readonly IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        readonly bool IEquatable<ColoredGems>.Equals(ColoredGems otherValue)
        {
            return this.Red == otherValue.Red && this.Green == otherValue.Green && this.Blue == otherValue.Blue;
        }

        public override readonly bool Equals(object obj)
        {
            if (obj is ColoredGems otherColoredGems) 
                return Equals(otherColoredGems);
            else
                return false;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Red, Green, Blue);
        }
    }
}