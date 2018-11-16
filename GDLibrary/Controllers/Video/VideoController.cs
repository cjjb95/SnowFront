/*
Function: 		Sets the texture of a parent actor based on a stream of textures provided by a video player.
                The video player will also automatically play any encoded audio from the video. 
                Note: 
                    - To use this class we needed to add the video.dll in the Dependencies folder.
                    - Videos should be in WMV format.

Author: 		NMCG
Version:		1.0
Date Updated:	28/11/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GDLibrary
{
    public class VideoController : Controller
    {
        //stores the state of the video attached to this controller
        private enum VideoState : sbyte
        {
            Playing,
            Paused,
            Stopped,
            NeverPlayed
        }

        #region Variables
        private VideoPlayer videoPlayer;
        private Video video;
        private Texture2D startTexture;
        private VideoState videoState;
        private float startVolume;

        #endregion

        #region Properties
        public VideoPlayer VideoPlayer
        {
            get
            {
                return this.videoPlayer;
            }
        }
        public Video Video
        { 
            get
            {
                return this.video;
            }
            set
            {
                this.video = value; 
            }
        }
        public float Volume
        {
            get
            {
                return this.videoPlayer.Volume;
            }
            set
            {
                this.videoPlayer.Volume = MathHelper.Clamp(value, 0, 1);
            }
        }
        #endregion

        public VideoController(string id, ControllerType controllerType, EventDispatcher eventDispatcher,
            Texture2D startTexture, Video video, float startVolume)
            : base(id, controllerType)
        {
            //video WMV file
            this.video = video;
            
            //set initial texture?
            this.startTexture = startTexture;

            //set initial volume 0 - 1
            this.startVolume = startVolume;

            //first time
            SetVideoState(VideoState.NeverPlayed);

            //register for events
            RegisterForEventHandling(eventDispatcher);
        }

        #region Event Handling
        protected override void RegisterForEventHandling(EventDispatcher eventDispatcher)
        {
            eventDispatcher.MenuChanged += EventDispatcher_MenuChanged;
            eventDispatcher.VideoChanged += EventDispatcher_VideoChanged;
        }

        //we need to explicitly tell the video object to pause if we're in the menu
        private void EventDispatcher_MenuChanged(EventData eventData)
        {
            //did the event come from the main menu and is it a start game event
            if (eventData.EventType == EventActionType.OnStart)
            {
                if(this.videoPlayer != null) //if video was paused when menu was hidden then play
                    this.videoPlayer.Play(video);
            }
            //did the event come from the main menu and is it a pause game event
            else if (eventData.EventType == EventActionType.OnPause)
            {
                if (this.videoPlayer != null) //if video was playing when menu was shown then pause
                    this.videoPlayer.Pause();
            }
        }

        private void EventDispatcher_VideoChanged(EventData eventData)
        {
            //target controller name is in first channel of additionalParameters
            string targetControllerID = eventData.AdditionalParameters[0] as string;

            //the event was targeted at this controller
            if (targetControllerID.Equals(this.ID))
                ProcessEvent(eventData);
        }
        private void ProcessEvent(EventData eventData)
        {
            if (this.videoPlayer == null)
            {
                this.videoPlayer = new VideoPlayer();
                this.videoPlayer.Volume = 0.1f;
            }

            if (eventData.EventType == EventActionType.OnPlay)
            {

                if (this.videoPlayer.State != MediaState.Playing)
                {
                    this.videoPlayer.Play(video);
                    SetVideoState(VideoState.Playing);
                }
            }
            else if (eventData.EventType == EventActionType.OnPause)
            {
                if (this.videoPlayer.State == MediaState.Playing)
                {
                    this.videoPlayer.Pause();
                    SetVideoState(VideoState.Paused);
                }
            }
            else if (eventData.EventType == EventActionType.OnStop)
            {
                if ((this.videoPlayer.State == MediaState.Playing) || (this.videoPlayer.State == MediaState.Paused))
                {
                    this.videoPlayer.Stop();
                    SetVideoState(VideoState.Stopped);
                }
            }
            else if (eventData.EventType == EventActionType.OnVolumeUp)
            {
                //volume is in second channel of additionalParameters when we send OnVolumeUp/Down event
                float volumeIncrement = (float)eventData.AdditionalParameters[1];
                
                //set through property to clamp range of valid values
                this.Volume += volumeIncrement;
            }
            else if (eventData.EventType == EventActionType.OnVolumeDown)
            {
                //volume is in second channel of additionalParameters when we send OnVolumeUp/Down event
                float volumeIncrement = (float)eventData.AdditionalParameters[1];

                //set through property to clamp range of valid values
                this.Volume -= volumeIncrement;
            }
            else if (eventData.EventType == EventActionType.OnVolumeSet)
            {
                //volume is in second channel of additionalParameters when we send OnVolumeUp/Down event
                float volumeValue = (float)eventData.AdditionalParameters[1];

                //set through property to clamp range of valid values
                this.Volume = volumeValue;
            }
            else if (eventData.EventType == EventActionType.OnMute)
            {
                //turn off
                this.Volume = 0;
            }
        }
        #endregion

        private void SetVideoState(VideoState videoState)
        {
            this.videoState = videoState;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            ModelObject parentModelObject = actor as ModelObject;

            //set the texture if the parent is valid
            if (parentModelObject != null)
            {
                if (videoState == VideoState.Playing)
                    parentModelObject.EffectParameters.Texture = videoPlayer.GetTexture();
                else if (videoState == VideoState.Stopped)
                    parentModelObject.EffectParameters.Texture = startTexture;
            }
        }

        //dispose of the player when the controller goes for garbage collection (i.e. when parent actor is removed)
        public void Dispose()
        {
            this.videoPlayer.Dispose();
        }
    }
}
