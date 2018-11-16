/*
Function: 		Represents text drawn in a 2D menu or UI element. 
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
    public class UITextObject : UIObject
    {
        #region Fields
        private string text;
        private SpriteFont spriteFont;
        #endregion

        #region Properties
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = (value.Length >= 0) ? value : "Default";
            }
        }
        public SpriteFont SpriteFont
        {
            get
            {
                return this.spriteFont;
            }
            set
            {
                this.spriteFont = value;
            }
        }
        #endregion

        public UITextObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, float layerDepth, string text, SpriteFont spriteFont)
            : base(id, actorType, statusType, transform, color, spriteEffects, layerDepth)
        {
            this.spriteFont = spriteFont;
            this.text = text;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.spriteFont, this.text, this.Transform.Translation, this.Color,
                MathHelper.ToRadians(this.Transform.RotationInDegrees),
                this.Transform.Origin, this.Transform.Scale, this.SpriteEffects, this.LayerDepth);
        }

        public override bool Equals(object obj)
        {
            UITextObject other = obj as UITextObject;

            if (other == null)
                return false;
            else if (this == other)
                return true;

            return this.text.Equals(other.Text) 
                && this.spriteFont.Equals(other.SpriteFont)
                    && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.text.GetHashCode();
            hash = hash * 17 + this.spriteFont.GetHashCode();
            hash = hash * 7 + base.GetHashCode();
            return hash;
        }

        public new object Clone()
        {
            IActor actor = new UITextObject("clone - " + ID, //deep
                this.ActorType, //deep
                this.StatusType, //deep - enum type
                (Transform2D)this.Transform.Clone(), //deep - calls the clone for Transform3D explicitly
                this.Color, //deep 
                this.SpriteEffects, //deep - enum type
                this.LayerDepth, //deep
                this.text, //deep
                this.spriteFont); //shallow

            //clone each of the (behavioural) controllers, if we have any controllers attached
            if (this.ControllerList != null)
            {
                foreach (IController controller in this.ControllerList)
                    actor.AttachController((IController)controller.Clone());
            }

            return actor;
        }

        public override bool Remove()
        {
            this.text = null;
            return base.Remove();
        }
    }
}
