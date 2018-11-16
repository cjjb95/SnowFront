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

        public static Integer2 operator *(Integer2 value, int multiplier)
        {
            return new Integer2(value.X * multiplier, value.Y * multiplier);
        }
        public static Integer2 operator *(int multiplier, Integer2 value)
        {
            return value * multiplier;
        }
        public static implicit operator Vector2(Integer2 value)
        {
            return new Vector2(value.X, value.Y);
        }

        //to do - add /, + - operator methods

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}
