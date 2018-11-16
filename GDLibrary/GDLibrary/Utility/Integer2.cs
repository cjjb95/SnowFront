using System;
using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class Integer2 : ICloneable
    {
        #region Statics
        public static Integer2 Zero
        {
            get
            {
                return new Integer2(0, 0);
            }
        }
        public static Integer2 One
        {
            get
            {
                return new Integer2(1, 1);
            }
        }
        public static Integer2 UnitX
        {
            get
            {
                return new Integer2(1, 0);
            }
        }
        public static Integer2 UnitY
        {
            get
            {
                return new Integer2(0, 1);
            }
        }
        #endregion

        #region Fields
        private int x, y;
        #endregion

        #region Properties
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        #endregion

        #region Constructors & Others
        public Integer2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Integer2(Vector2 value)
            : this(value.X, value.Y)
        {
        }
        public Integer2(float x, float y)
            : this((int)x, (int)y)
        {
        }
        public Integer2(double x, double y)
            : this((float)x, (float)y)
        {
        }

        public override string ToString()
        {
            return "(x: " + x + ", " + "y: " + y + ")";
        }
        public override bool Equals(object obj)
        {
            Integer2 integer = obj as Integer2;
            return integer != null &&
                   x == integer.x &&
                   y == integer.y;
        }
        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion


        #region Operators and Typecast
        //see https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/operator
        public static Integer2 operator +(Integer2 a, Integer2 b)
        {
            return new Integer2(a.x + b.x, a.y + b.y);
        }
        public static Integer2 operator -(Integer2 a, Integer2 b)
        {
            return new Integer2(a.x - b.x, a.y - b.y);
        }


        public static Integer2 operator *(Integer2 value, int multiplier)
        {
            return new Integer2(value.X * multiplier, value.Y * multiplier);
        }
        public static Integer2 operator *(int multiplier, Integer2 value)
        {
            return value * multiplier;
        }

        public static Integer2 operator /(Integer2 value, int divisor)
        {
            return new Integer2(value.X / divisor, value.Y / divisor);
        }
        public static Integer2 operator /(int divisor, Integer2 value)
        {
            return value / divisor;
        }

        //see https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/implicit
        //typecasting requires use of the implicit keyword
        public static implicit operator Vector2(Integer2 value)
        {
            return new Vector2(value.X, value.Y);
        }
        public static implicit operator Integer2(Vector2 value)
        {
            return new Vector2(value.X, value.Y);
        }

        //TODO - add /, + - operator methods
        #endregion



    }

}
