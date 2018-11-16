/*
Function: 		Lerps target actor's color between two user specified colors. Can be used to decorate static object and give it a visually interesting behaviour.
Author: 		NMCG
Version:		1.0
Date Updated:	24/10/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using System;
namespace GDLibrary
{
    public class ColorSineLerpController : SineLerpController
    {
        #region Fields
        private Color startColor;
        private Color endColor;
        private int totalElapsedTime;
        #endregion

        #region Properties
        public Color StartColor
        {
            get
            {
                return this.startColor;
            }
            set
            {
                this.startColor = value;
            }
        }
        public Color EndColor
        {
            get
            {
                return this.endColor;
            }
            set
            {
                this.endColor = value;
            }
        }
        #endregion

        public ColorSineLerpController(string id, 
            ControllerType controllerType, 
            Color startColor, Color endColor, TrigonometricParameters trigonometricParameters)
            : base(id, controllerType, trigonometricParameters)
        {
            this.startColor = startColor;
            this.endColor = endColor;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            DrawnActor3D parentActor = actor as DrawnActor3D;

            if (parentActor != null)
            {
                //accumulate elapsed time - note we are not formally resetting this time if the controller becomes inactive - we should mirror the approach used for the UI sine controllers.
                this.totalElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                //sine wave in the range 0 -> max amplitude
                float lerpFactor = MathUtility.SineLerpByElapsedTime(this.TrigonometricParameters, this.totalElapsedTime);
                parentActor.EffectParameters.DiffuseColor = MathUtility.Lerp(this.startColor, this.endColor, lerpFactor);
            }
        }

        public override bool Equals(object obj)
        {
            ColorSineLerpController other = obj as ColorSineLerpController;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.startColor.Equals(other.StartColor)
                    && this.endColor.Equals(other.EndColor)
                        && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.startColor.GetHashCode();
            hash = hash * 17 + this.endColor.GetHashCode();
            hash = hash * 11 + base.GetHashCode();
            return hash;
        }

        public override object Clone()
        {
            return new ColorSineLerpController("clone - " + this.ID, //deep
                this.ControllerType, //deep
                this.startColor,  //deep
                this.endColor, //deep
                 (TrigonometricParameters)this.TrigonometricParameters.Clone()); //deep
        }
    }
}

