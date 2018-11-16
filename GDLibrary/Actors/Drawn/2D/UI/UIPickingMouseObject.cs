using GDLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDApp
{
    public class UIPickingMouseObject : UIMouseObject
    {
        #region Fields
        //statics
        private EventDispatcher eventDispatcher;

        private int numberOfReticuleIconsPerTexture = 1;
        #endregion

        #region Properties
        #endregion

        /* A slightly(!) more succinct version of the constructor which doesnt require us to provide quite so many arguments
         * Note: Since the sourceRectangle is now hard-coded (i.e. new Rectangle(0, 0, texture.Width, texture.Height)) then this
         * version of the constructor will NOT allow us to specify a reticule image in a single texture containing multiple reticule textures
         * as in the texture mouseicons.png in the content folder.
         */
        public UIPickingMouseObject(string id, ActorType actorType, Transform2D transform,
            SpriteFont spriteFont, string text, Vector2 textOffsetPosition, Texture2D texture, 
            MouseManager mouseManager, EventDispatcher eventDispatcher)
            : this(id, actorType, StatusType.Update | StatusType.Drawn, transform, Color.White, SpriteEffects.None, spriteFont, 
                  text, textOffsetPosition, Color.White, 0, texture, new Rectangle(0, 0, texture.Width, texture.Height),
                    new Vector2(texture.Width/2, texture.Height/2), mouseManager, eventDispatcher)
        {

        }

        public UIPickingMouseObject(string id, ActorType actorType, StatusType statusType, Transform2D transform, 
            Color color, SpriteEffects spriteEffects, SpriteFont spriteFont, string text, 
            Vector2 textOffsetPosition, Color textColor, float layerDepth, Texture2D texture, Rectangle sourceRectangle, 
            Vector2 origin, MouseManager mouseManager, EventDispatcher eventDispatcher) 
            : base(id, actorType, statusType, transform, color, spriteEffects, spriteFont, text, textOffsetPosition, 
                  textColor, layerDepth, texture, sourceRectangle, origin, mouseManager)
        {
            this.eventDispatcher = eventDispatcher;
            //register with the event dispatcher for the events of interest
            RegisterForEventHandling(eventDispatcher);
        }

        #region Event Handling
        protected virtual void RegisterForEventHandling(EventDispatcher eventDispatcher)
        {
            eventDispatcher.ObjectPickChanged += EventDispatcher_ObjectPickChanged;
        }

        private void EventDispatcher_ObjectPickChanged(EventData eventData)
        {
            if(eventData.EventType == EventActionType.OnObjectPicked)
            {
                //see PickingManager::UpdateEventListeners() for publish point
                this.Text = CollisionUtility.GetMouseStringFromCollidableObject(eventData.AdditionalParameters[0] as CollidableObject, (float)eventData.AdditionalParameters[1]);

                SetAppearance();
            }
            else if (eventData.EventType == EventActionType.OnNonePicked)
            {
                //see PickingManager::UpdateEventListeners() for publish point
                this.Text = eventData.AdditionalParameters[0] as string;

                ResetAppearance();
            }
        }
        #endregion

        protected virtual void SetAppearance()
        {
            //set reticule color and text color
            this.TextColor = this.Color = Color.Red;

            //we could change texture by setting the SourceRectangle (assuming texture contains "numberOfReticuleIconsPerTexture" images)
            this.SourceRectangle = new Rectangle((numberOfReticuleIconsPerTexture - 1) * this.Texture.Width / numberOfReticuleIconsPerTexture, 0, 
                this.Texture.Width / numberOfReticuleIconsPerTexture, this.Texture.Height / numberOfReticuleIconsPerTexture);
        }


        protected virtual void ResetAppearance()
        {
            //reset reticule color and text color
            this.TextColor = this.Color = this.OriginalColor;

            //reset back to the first reticule icon in the texture
            this.SourceRectangle = new Rectangle(0, 0,
                this.Texture.Width / numberOfReticuleIconsPerTexture, this.Texture.Height / numberOfReticuleIconsPerTexture);
        }

    }
}

