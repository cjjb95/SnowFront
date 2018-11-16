using Microsoft.Xna.Framework;
using System;
namespace GDLibrary
{
    public class Integer3 : ICloneable
    {
        #region Statics
        public static Integer3 Zero
        {
            get
            {
                return new Integer3(0, 0, 0);
            }
        }
        public static Integer3 One
        {
            get
            {
                return new Integer3(1, 1, 1);
            }
        }
        public static Integer3 UnitX
        {
            get
            {
                return new Integer3(1, 0, 0);
            }
        }
        public static Integer3 UnitY
        {
            get
            {
                return new Integer3(0, 1, 0);
            }
        }
        public static Integer3 UnitZ
        {
            get
            {
                return new Integer3(0, 0, 1);
            }
        }
        #endregion

        #region Fields
        private int x, y, z;
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
        public int Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }
        #endregion

        #region Constructors & Others
        public Integer3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Integer3(float x, float y, float z)
            : this((int)x, (int)y, (int)z)
        {
        }
        public Integer3(double x, double y, double z)
            : this((float)x, (float)y, (float)z)
        {
        }


        public override string ToString()
        {
            return "(x: " + x + ", " + "y: " + y + ", " + "z: " + z + ")";
        }
        public override bool Equals(object obj)
        {
            Integer3 integer = obj as Integer3;
            return integer != null &&
                   x == integer.x &&
                   y == integer.y &&
                   z == integer.z;
        }
        public override int GetHashCode()
        {
            var hashCode = 373119288;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + z.GetHashCode();
            return hashCode;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Operators and Typecast
        //see https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/operator
        public static Integer3 operator +(Integer3 a, Integer3 b)
        {
            return new Integer3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Integer3 operator -(Integer3 a, Integer3 b)
        {
            return new Integer3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Integer3 operator *(Integer3 value, int multiplier)
        {
            return new Integer3(value.X * multiplier, value.Y * multiplier, value.Z * multiplier);
        }

        public static Integer3 operator *(int multiplier, Integer3 value)
        {
            return value * multiplier;
        }

        public static Integer3 operator /(Integer3 value, int divisor)
        {
            return new Integer3(value.X * divisor, value.Y * divisor, value.Z * divisor);
        }

        public static Integer3 operator /(int divisor, Integer3 value)
        {
            return value / divisor;
        }

        //see https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/implicit
        //typecasting requires use of the implicit keyword
        public static implicit operator Vector3(Integer3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }
        public static implicit operator Integer3(Vector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        //TODO - add /, + - operator methods
        #endregion
    }

}
