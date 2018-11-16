/*
Function: 		Encapsulates the parameters for a collectable collidable object (e.g. "ammo", 10)
Author: 		NMCG
Version:		1.0
Date Updated:	14/11/17
Bugs:			None
Fixes:			None
*/
namespace GDLibrary
{
    public class PickupParameters
    {
        #region Fields
        private string description;
        private float value;

        //an optional array to store multiple parameters (used for play with sound/video when we pickup this object)
        private object[] additionalParameters;
        #endregion

        #region Properties
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = (value.Length != 0) ? value : "no description specified";
            }
        }
        public float Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = (value >= 0) ? value : 0;
            }
        }
        public object[] AdditionalParameters
        {
            get
            {
                return this.additionalParameters;
            }
            set
            {
                this.additionalParameters = value;
            }
        }

        #endregion

        public PickupParameters(string description, float value)
            : this(description, value, null)
        {

        }

        public PickupParameters(string description, float value, object[] additionalParameters)
        {
            this.value = value;
            this.description = description;
            this.additionalParameters = additionalParameters;
        }

        public override bool Equals(object obj)
        {
            PickupParameters other = obj as PickupParameters;
            bool bEquals = this.description.Equals(other.Description) && this.value == other.Value;
            return bEquals && ((this.additionalParameters != null && this.additionalParameters.Length != 0) ? this.additionalParameters.Equals(other.additionalParameters) : true);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 11 + this.description.GetHashCode();
            hash = hash * 17 + this.value.GetHashCode();

            if (this.additionalParameters != null && this.additionalParameters.Length != 0)
                hash = hash * 31 + this.additionalParameters.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return "Desc.:" + this.description + ", Value: " + this.value;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
