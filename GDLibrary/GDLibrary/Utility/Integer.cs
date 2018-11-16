namespace GDLibrary
{
    public class Integer
    {
        #region Statics
        public static Integer Zero
        {
            get
            {
                return new Integer(0);
            }
        }
        public static Integer One
        {
            get
            {
                return new Integer(1);
            }
        }
        #endregion

        #region Fields
        private int value;
        #endregion

        #region Properties
      
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
        #endregion

        #region Constructors & Others
        public Integer(int value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return "Value: " + this.value;
        }
        public override bool Equals(object obj)
        {
            Integer integer = obj as Integer;
            return integer != null &&
                   value == integer.value &&
                   Value == integer.Value;
        }
        public override int GetHashCode()
        {
            var hashCode = 1927018180;
            hashCode = hashCode * -1521134295 + value.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }
        public object Clone()
        {
            return this.MemberwiseClone(); //what's this!?
        }

        #endregion

        #region Operators and Typecast
        //see https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/operator
        public static Integer operator +(Integer a, Integer b)
        {
            return new Integer(a.Value + b.Value);
        }
        public static Integer operator -(Integer a, Integer b)
        {
            return new Integer(a.Value - b.Value);
        }

        public static Integer operator *(Integer x, int multiplier)
        {
            return new Integer(x.Value * multiplier);
        }

        public static Integer operator *(int multiplier, Integer value)
        {
            return value * multiplier;
        }

        public static Integer operator /(Integer x, int divisor)
        {
            return new Integer(x.Value / divisor);
        }

        public static Integer operator /(int divisor, Integer value)
        {
            return value / divisor;
        }

        //to do - add /, *, - operator methods

        //For docs on "implicit" - See https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/implicit
        //typecast from int
        public static implicit operator Integer(int x)
        {
            return new Integer(x);
        }

        //typecast to int
        public static implicit operator int(Integer x)
        {
            return x.Value;
        }
        #endregion

    }
}
