using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class BillboardEffectParameters : EffectParameters
    {

        public BillboardEffectParameters(Effect effect)
            : base(effect, null, Color.White, DefaultAlpha)
        {

        }

        //for objects with colour but no texture e.g. a glass cube
        public BillboardEffectParameters(Effect effect, Color diffusecolor, float alpha)
            : base(effect, null, diffusecolor, alpha)
        {

        }

        //for objects with texture and alpha but no specular or emmissive
        public BillboardEffectParameters(Effect effect, Texture2D texture, Color diffusecolor, float alpha)
            : base(effect, texture, diffusecolor, alpha)
        {

        }

        public override void SetParameters(Camera3D camera, BillboardOrientationParameters billboardParameters)
        {
            this.Effect.CurrentTechnique = this.Effect.Techniques[billboardParameters.Technique];
            this.Effect.Parameters["View"].SetValue(camera.View);
            this.Effect.Parameters["Projection"].SetValue(camera.ProjectionParameters.Projection);
            this.Effect.Parameters["Up"].SetValue(billboardParameters.Up);
            this.Effect.Parameters["Right"].SetValue(billboardParameters.Right);
            this.Effect.Parameters["DiffuseColor"].SetValue(this.DiffuseColor.ToVector4());
            this.Effect.Parameters["DiffuseTexture"].SetValue(this.Texture);
            this.Effect.Parameters["Alpha"].SetValue(this.Alpha);

            //animation specific parameters
            this.Effect.Parameters["IsScrolling"].SetValue(billboardParameters.IsScrolling);
            this.Effect.Parameters["scrollRate"].SetValue(billboardParameters.scrollValue);
            this.Effect.Parameters["IsAnimated"].SetValue(billboardParameters.IsAnimated);
            this.Effect.Parameters["InverseFrameCount"].SetValue(billboardParameters.inverseFrameCount);
            this.Effect.Parameters["CurrentFrame"].SetValue(billboardParameters.currentFrame);


            base.SetParameters(camera);
        }

        public override void SetWorld(Matrix world)
        {
            this.Effect.Parameters["World"].SetValue(world);
        }

        public override EffectParameters GetDeepCopy()
        {
            return new BillboardEffectParameters(this.Effect, //shallow - a reference
                this.Texture, //shallow - a reference
                this.DiffuseColor,//deep
                this.Alpha);//deep
        }

        public override object Clone()
        {
            return GetDeepCopy();
        }
    }
}
