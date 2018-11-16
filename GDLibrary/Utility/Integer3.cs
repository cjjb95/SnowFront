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

        public static Integer3 operator *(Integer3 value, int multiplier)
        {
            return new Integer3(value.X * multiplier, value.Y * multiplier, value.Z * multiplier);
        }

        public static Integer3 operator *(int multiplier, Integer3 value)
        {
            return value * multiplier;
        }

        //to do - add /, + - operator methods

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}
