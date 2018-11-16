/*
Function: 		Represents texture drawn in a 2D menu or UI element. 
Author: 		NMCG
Version:		1.0
Date Updated:	27/9/17
Bugs:			None
Fixes:			None
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class UITextureObject : UIObject
    {
        #region Fields
        private Texture2D texture;
        private Rectangle sourceRectangle, originalSourceRectangle;
        private Vector2 origin;
        #endregion

        #region Properties
        public Vector2 Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
            }
        }
        public Rectangle OriginalSourceRectangle
        {
            get
            {
                return this.originalSourceRectangle;
            }
        }
        public Rectangle SourceRectangle
        {
            get
            {
                return this.sourceRectangle;
            }
            set
            {
                this.sourceRectangle = value;
            }
        }
        public int SourceRectangleWidth
        {
            get
            {
                return this.sourceRectangle.Width;
            }
            set
            {
                this.sourceRectangle.Width = value;
            }
        }
        public int SourceRectangleHeight
        {
            get
            {
                return this.sourceRectangle.Height;
            }
            set
            {
                this.sourceRectangle.Height = value;
            }
        }
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                this.texture = value;
            }
        }
        #endregion

   

        //draws texture using full source rectangle with origin in centre
        public UITextureObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
         Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture)
            : this(id, actorType, statusType, transform, color, spriteEffects, layerDepth, texture, 
                new Rectangle(0, 0, texture.Width, texture.Height), 
                    new Vector2(texture.Width/2.0f, texture.Height/2.0f))
        {

        }

       public UITextureObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
       Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture,
       Rectangle sourceRectangle, Vector2 origin)
       : base(id, actorType, statusType, transform, color, spriteEffects, layerDepth)
        {
            this.Texture = texture;
            this.SourceRectangle = sourceRectangle;
            this.originalSourceRectangle = SourceRectangle;
            this.Origin = origin;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture,  this.Transform.Translation, 
                this.sourceRectangle, this.Color, 
                MathHelper.ToRadians(this.Transform.RotationInDegrees),
                this.Transform.Origin, this.Transform.Scale, this.SpriteEffects, this.LayerDepth);
        }

        public override bool Equals(object obj)
        {
            UITextureObject other = obj as UITextureObject;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.texture.Equals(other.Texture)
                && this.sourceRectangle.Equals(other.SourceRectangle)
                    && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.texture.GetHashCode();
            hash = hash * 17 + this.sourceRectangle.GetHashCode();
            hash = hash * 7 + base.GetHashCode();
            return hash;
        }

        public new object Clone()
        {
            IActor actor = new UITextureObject("clone - " + ID, //deep
                this.ActorType, //deep
                this.StatusType, //deep - enum type
                (Transform2D)this.Transform.Clone(), //deep - calls the clone for Transform3D explicitly
                this.Color, //deep 
                this.SpriteEffects, //deep - enum type
                this.LayerDepth,  //deep 
                this.texture, //shallow
                this.sourceRectangle,  //deep 
                this.origin); //deep 

            //clone each of the (behavioural) controllers, if we have any controllers attached
            if (this.ControllerList != null)
            {
                foreach (IController controller in this.ControllerList)
                    actor.AttachController((IController)controller.Clone());
            }

            return actor;
        }

    }
}
