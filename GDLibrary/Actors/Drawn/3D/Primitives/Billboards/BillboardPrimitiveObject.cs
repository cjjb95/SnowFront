/*
Function: 		Allows us to draw billboard primitives by explicitly defining the vertex data.
                Used in you I-CA project.
                 
Author: 		NMCG
Version:		1.0
Date Updated:	27/11/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class BillboardOrientationParameters
    {
        public BillboardType BillboardType { get; set; }
        public Vector3 Up { get;  set; }
        public Vector3 Right { get; set; }
        public string Technique { get; set; }

        //scrolling
        public bool IsScrolling { get; protected set; }
        public Vector2 ScrollRate { get; protected set; }
        public Vector2 scrollValue;

        //animation
        public bool IsAnimated { get; protected set; }
        public int currentFrame { get; protected set; }
        public int startFrame { get; protected set; }
        public int totalFrames { get; protected set; }
        public Vector2 inverseFrameCount { get; protected set; }
        public float frameDelay { get; protected set; }
        private int framesElapsed;


        public void SetScrollRate(Vector2 scrollRate)
        {
            this.IsScrolling = true;
            this.ScrollRate = scrollRate;
            this.scrollValue = Vector2.Zero;
        }

        public void SetAnimationRate(int totalFrames, float frameDelay, int startFrame)
        {
            this.IsAnimated = true;
            this.totalFrames = totalFrames; //remember frames are arranged in a 1 x N strip, so totalFrames == N
            this.inverseFrameCount = new Vector2(1.0f / totalFrames, 1);
            this.frameDelay = frameDelay;
            this.startFrame = startFrame;
            this.currentFrame = startFrame;
        }

        public void UpdateScroll(GameTime gameTime)
        {
            float invDt = 1.0f / (1000 * gameTime.ElapsedGameTime.Milliseconds);

            this.scrollValue.X += this.ScrollRate.X * invDt;
            this.scrollValue.X %= 1;

            this.scrollValue.Y += this.ScrollRate.Y * invDt;
            this.scrollValue.Y %= 1;
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            int frameRate = (int)(frameDelay * 1.0f / gameTime.ElapsedGameTime.TotalSeconds);

            if (framesElapsed >= frameRate)
            {
                framesElapsed = 0;
                this.currentFrame++;
                this.currentFrame %= this.totalFrames;

            }
            else
            {
                framesElapsed++;
            }
        }

        public void ResetAnimation()
        {
            this.framesElapsed = 0;
            this.currentFrame = this.startFrame;
        }
        public void ResetScroll()
        {
            this.scrollValue = Vector2.Zero;
        }
    }

    public class BillboardPrimitiveObject : PrimitiveObject
    {
        #region Variables
        private BillboardOrientationParameters billboardOrientationParameters;
        #endregion

        #region Properties
        public BillboardType BillboardType
        {
            get
            {
                return this.BillboardOrientationParameters.BillboardType;
            }
            set
            {
                if (value == BillboardType.Normal)
                {
                    this.billboardOrientationParameters.Technique = "Normal";
                    this.billboardOrientationParameters.BillboardType = BillboardType.Normal;
                }
                else if (value == BillboardType.Cylindrical)
                {
                    this.billboardOrientationParameters.Technique = "Cylindrical";
                    this.billboardOrientationParameters.BillboardType = BillboardType.Cylindrical;                  
                }
                else
                {
                    this.billboardOrientationParameters.Technique = "Spherical";
                    this.billboardOrientationParameters.BillboardType = BillboardType.Spherical;
                }

                this.billboardOrientationParameters.Up = this.Transform.Up;
                this.billboardOrientationParameters.Right = this.Transform.Right;
            }
        }
        public BillboardOrientationParameters BillboardOrientationParameters
        {
            get
            {
                return this.billboardOrientationParameters;
            }
        }
        #endregion

        public BillboardPrimitiveObject(string id, ActorType actorType, Transform3D transform,
          EffectParameters effectParameters, StatusType statusType, IVertexData vertexData, BillboardType billboardType)
            : base(id, actorType, transform, effectParameters, statusType, vertexData)
        {
            //create blank set of parameters and set type to be Normal - developer can change after instanciation - see Main::InitializeBillboards()
            this.billboardOrientationParameters = new BillboardOrientationParameters();
            this.BillboardType = billboardType;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.billboardOrientationParameters.IsScrolling)
                this.billboardOrientationParameters.UpdateScroll(gameTime);

            if (this.billboardOrientationParameters.IsAnimated)
                this.billboardOrientationParameters.UpdateAnimation(gameTime);

            base.Update(gameTime);
        }

        public new object Clone()
        {
            return new BillboardPrimitiveObject("clone - " + this.ID,
                this.ActorType,  //deep
                (Transform3D)this.Transform.Clone(), //deep
               (BillboardEffectParameters)this.EffectParameters.Clone(), //deep
               this.StatusType, //deep
                this.VertexData, this.BillboardType); //shallow
        }
    }
}
