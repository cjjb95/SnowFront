using Microsoft.Xna.Framework;

namespace GDLibrary
{
    /// <summary>
    /// Applied to an object when it is picked up to "animate" removal - it floats upwards and fades away
    /// </summary>
    public class PickupController : Controller
    {
        private float rotationRate;
        private Vector3 translationRate, scaleRate;
        private float alphaDecayRate, alphaDecayThreshold;

        public PickupController(string id, ControllerType controllerType, float rotationRate, Vector3 translationRate, Vector3 scaleRate,
                    float alphaDecayRate, float alphaDecayThreshold)
            : base(id, controllerType)
        {
            this.rotationRate = rotationRate;
            this.translationRate = translationRate;
            this.scaleRate = scaleRate;
            this.alphaDecayRate = alphaDecayRate;
            this.alphaDecayThreshold = alphaDecayThreshold;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            ModelObject parentActor = actor as ModelObject;

            //makes the object spin upwards and fade away after its alpha is lower than a threshold value
            parentActor.Transform.RotateAroundYBy(this.rotationRate);
            parentActor.Transform.TranslateBy(this.translationRate * gameTime.ElapsedGameTime.Milliseconds);
            parentActor.Transform.ScaleBy(this.scaleRate);
            parentActor.ColorParameters.Alpha += this.alphaDecayRate;

            //if alpha less than some threshold value then remove
            if (parentActor.ColorParameters.Alpha < this.alphaDecayThreshold)
            {
                //remove the object
            }
        }
    }
}
