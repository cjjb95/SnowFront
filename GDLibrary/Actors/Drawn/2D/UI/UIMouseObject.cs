/*
Function: 		Represents a combination of texture and text for a mouse cursor which supports object picking, placement, firing new objects (i.e.CDCR related activity).
Author: 		NMCG
Version:		1.0
Date Updated:	25/11/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class UIMouseObject : UITextureObject
    {
        #region Fields
        private string text;
        private SpriteFont spriteFont;
        private Vector2 textOffsetPosition;
        private Color textColor;
        private Vector2 textDimensions;
        private Vector2 textOrigin;
        private MouseManager mouseManager;
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
                this.text = value;
                this.textDimensions = this.spriteFont.MeasureString(this.text);
                this.textOrigin = new Vector2(this.textDimensions.X / 2, this.textDimensions.Y / 2);
            }
        }
        public Color TextColor
        {
            get
            {
                return this.textColor;
            }
            set
            {
                this.textColor = value;
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

        public UIMouseObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, SpriteFont spriteFont,string text, Vector2 textOffsetPosition, Color textColor,
            float layerDepth, Texture2D texture, Rectangle sourceRectangle, Vector2 origin, MouseManager mouseManager)
            : base(id, actorType, statusType, transform, color, spriteEffects, layerDepth, texture, sourceRectangle, origin)
        {
            this.spriteFont = spriteFont;
            this.Text = text;
            this.textOffsetPosition = textOffsetPosition;
            this.textColor = textColor;

            //used to update pointer position
            this.mouseManager = mouseManager;
        }


        public override void Update(GameTime gameTime)
        {
            this.Transform.Translation = this.mouseManager.Position;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw mouse reticule
            spriteBatch.Draw(this.Texture, this.Transform.Translation,
                this.SourceRectangle, this.Color,
                MathHelper.ToRadians(this.Transform.RotationInDegrees),
                Origin, //bug fix for off centre rotation - uses explicitly specified origin and not this.Transform.Origin
                this.Transform.Scale, this.SpriteEffects, this.LayerDepth);

            //draw any additional text
            if (this.text != null)
                spriteBatch.DrawString(this.spriteFont, this.text,
                    this.Transform.Translation + textOffsetPosition, this.textColor, 0, this.textOrigin, 1, SpriteEffects.None, this.LayerDepth);
        }
    }
}
