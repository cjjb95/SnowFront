using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class MathUtility
    {
        #region Round
        public static Vector2 Round(Vector2 a, int precision)
        {
            //takes two translations x1,y1 and x2,y2 and interpolates linearly between them using a factor    
            return new Vector2((float)Math.Round(a.X, precision), (float)Math.Round(a.Y, precision));
        }

        public static Vector3 Round(Vector3 a, int precision)
        {
            //takes two translations x1,y1 and x2,y2 and interpolates linearly between them using a factor    
            return new Vector3((float)Math.Round(a.X, precision), 
                (float)Math.Round(a.Y, precision),
                    (float)Math.Round(a.Z, precision));
        }

        #endregion

        #region Lerp
        public static Vector2 Lerp(Vector2 a, Vector2 b, float lerpFactor)
        {
            //takes two translations x1,y1 and x2,y2 and interpolates linearly between them using a factor    
            return new Vector2(MathHelper.Lerp(a.X, b.X, lerpFactor), MathHelper.Lerp(a.Y, b.Y, lerpFactor));
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float lerpFactor)
        {
            //takes two translations x1,y1,z1 and x2,y2,z2 and interpolates linearly between them using a factor    
            return new Vector3(MathHelper.Lerp(a.X, b.X, lerpFactor),
                MathHelper.Lerp(a.Y, b.Y, lerpFactor), MathHelper.Lerp(a.Z, b.Z, lerpFactor));
        }

        public static Color Lerp(Color a, Color b, float lerpFactor)
        {
            //Lerp between R, G, B, and A channels for each color
            return new Color((int)MathHelper.Lerp(a.R, b.R, lerpFactor),
                        (int)MathHelper.Lerp(a.G, b.G, lerpFactor),
                            (int)MathHelper.Lerp(a.B, b.B, lerpFactor),
                                (int)MathHelper.Lerp(a.A, b.A, lerpFactor));
        }

        #endregion

        #region Trig
        //lerps along a sine wave with properties defined by TrigonometricParameters (i.e. max amplitude, phase, speed) - see UISineLerpController
        public static float Sin(TrigonometricParameters trigonometricParameters, float totalElapsedTime)
        {
            //range - max amplitude -> + max amplitude
            float sin = (float)(trigonometricParameters.MaxAmplitude
                * Math.Sin(trigonometricParameters.AngularFrequency
                * MathHelper.ToRadians(totalElapsedTime) + trigonometricParameters.PhaseAngle));
            
            //range 0 -> 2* max amplitude
            sin += trigonometricParameters.MaxAmplitude;
            
            //range 0 -> max amplitude
            sin /= 2.0f;

            return sin;
        }

        #endregion

        #region Camera Related
        public static float GetDistanceFromCamera(Actor3D actor, Camera3D activeCamera)
        {
            return Vector3.Distance(actor.Transform.Translation, activeCamera.Transform.Translation);
        }

        //sets actor::distanceToCamera value i.e. sets how far object is from camera (used to sort semi-transparent objects by distance from camera)
        public static void SetDistanceFromCamera(Actor3D actor, Camera3D activeCamera)
        {
            actor.Transform.DistanceToCamera = Vector3.Distance(actor.Transform.Translation, activeCamera.Transform.Translation);
        }

        //object to target vector, no distance
        public static Vector3 GetNormalizedObjectToTargetVector(Transform3D start, Transform3D target)
        {
            //camera to target object vector
            return Vector3.Normalize(target.Translation - start.Translation);
        }

        //object to target vector, also provides access to distance from object to target
        public static Vector3 GetNormalizedObjectToTargetVector(Transform3D start, Transform3D target, out float distance)
        {
            //camera to target object vector
            Vector3 vectorToTarget = target.Translation - start.Translation;

            //distance from camera to target
            distance = vectorToTarget.Length();

            //camera to target object vector
            vectorToTarget.Normalize();

            return vectorToTarget;
        }
        #endregion
    }
}
