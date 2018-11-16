using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class DualTextureEffectParameters : EffectParameters
    {
        #region Fields
        private Texture2D texture2;

        //reset
        private DualTextureEffectParameters originalEffectParameters;
        #endregion

        #region Properties
        public Texture2D Texture2
        {
            get
            {
                return this.texture2;
            }
            set
            {
                this.texture2 = value;
            }
        }
        public new DualTextureEffectParameters OriginalEffectParameters
        {
            get
            {
                return this.originalEffectParameters;
            }
        }
        #endregion

        //used to create the simplest instance of the class - fields will be set by each instanciating object - see Main::InitializeEffects()
        public DualTextureEffectParameters(Effect effect)
            : base(effect)
        {

        }

        public DualTextureEffectParameters(Effect effect, Texture2D texture, Color diffuseColor, float alpha, Texture2D texture2)
            : base(effect, texture,  diffuseColor, alpha)
        {
            Initialize(texture2);

            //store original values in case of reset
            this.originalEffectParameters = new DualTextureEffectParameters(effect);
            this.originalEffectParameters.Initialize(texture2);

        }

        protected void Initialize(Texture2D texture2)
        {
            this.texture2 = texture2;
        }

        protected override void Reset()
        {
            base.Reset();
            this.Initialize(this.originalEffectParameters.Texture2);
        }

        public override void SetParameters(Camera3D camera)
        {
            DualTextureEffect bEffect = this.Effect as DualTextureEffect;
            bEffect.View = camera.View;
            bEffect.Projection = camera.Projection;
            bEffect.DiffuseColor = this.DiffuseColor.ToVector3();
            bEffect.Alpha = this.Alpha;

            if (this.Texture != null && this.texture2 != null)
            {
                bEffect.Texture = this.Texture;
                bEffect.Texture2 = this.texture2;
            }
            base.SetParameters(camera);
        }

        public override void SetWorld(Matrix world)
        {
           (this.Effect as DualTextureEffect).World = world;
        }

        //add equals, gethashcode...

        public override EffectParameters GetDeepCopy()
        {
            return new DualTextureEffectParameters(this.Effect, //shallow - a reference
                this.Texture, //shallow - a reference
                this.DiffuseColor,//deep
                this.Alpha, //deep
                this.texture2);//shallow - a reference
        }

        public override object Clone()
        {
            return GetDeepCopy();
        }


    }
}
