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


        public Integer(int value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return "Value: " + this.value;
        }

        public static Integer operator *(Integer x, int multiplier)
        {
            return new Integer(x.Value * multiplier);
        }

        public static Integer operator *(int multiplier, Integer value)
        {
            return value * multiplier;
        }

        //to do - add /, + - operator methods

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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
