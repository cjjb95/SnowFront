/*
Function: 		Encapsulates the (time-domain) co-efficiencts applied to a basic trigonometric function (e.g. sine, cos, tan)
Author: 		NMCG
Version:		1.0
Date Updated:	6/10/17
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public class TrigonometricParameters
    {
        #region Fields
        private float maxAmplitude, angularFrequency, phaseAngle;
        #endregion

        #region Properties
        public float MaxAmplitude
        {
            get
            {
                return this.maxAmplitude;
            }
            set
            {
                this.maxAmplitude = (value > 0) ? value : 1;
            }
        }
        public float AngularFrequency
        {
            get
            {
                return this.angularFrequency;
            }
            set
            {
                this.angularFrequency = (value > 0) ? value : 1;
            }
        }
        public float PhaseAngle
        {
            get
            {
                return this.phaseAngle;
            }
            set
            {
                this.phaseAngle = value;
            }
        }
        #endregion

        public TrigonometricParameters(float maxAmplitude, float angularFrequency, float phaseAngle)
        {
            this.MaxAmplitude = maxAmplitude;
            this.AngularFrequency = angularFrequency;
            this.PhaseAngle = phaseAngle;
        }

        public TrigonometricParameters(float maxAmplitude, float angularFrequency)
            : this(maxAmplitude, angularFrequency, 0)
        {
          
        }

        public override bool Equals(object obj)
        {
            TrigonometricParameters other = obj as TrigonometricParameters;
            return this.maxAmplitude == other.MaxAmplitude
                && this.angularFrequency == other.AngularFrequency
                    && this.phaseAngle == other.PhaseAngle;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.maxAmplitude.GetHashCode();
            hash = hash * 17 + this.angularFrequency.GetHashCode();
            hash = hash * 11 + this.phaseAngle.GetHashCode();
            return hash;
        }

        public object Clone()
        {
            //deep because all variables are either C# types (e.g. primitives, structs, or enums) or  XNA types
            return this.MemberwiseClone();
        }
    }
}
