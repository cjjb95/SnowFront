using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class MouseButtonController : Controller
    {
        private int totalElapsedTime;
        private MouseManager mouseManager;

        public MouseButtonController(string id, ControllerType controllerType, MouseManager mouseManager) : base(id, controllerType)
        {
            this.mouseManager = mouseManager;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            DrawnActor2D parentActor = actor as DrawnActor2D;

            TrigonometricParameters trig = new TrigonometricParameters(1, 1);
            

            this.totalElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            
            float lerpFactor = MathUtility.Sin(trig, this.totalElapsedTime);

            parentActor.Color = MathUtility.Lerp(Color.Red, Color.Blue, lerpFactor);

            if(parentActor.Transform.Bounds.Intersects(this.mouseManager.Bounds))
            {
                if(this.mouseManager.IsLeftButtonClicked())
                {
                    int x = 0;
                }
            }

            base.Update(gameTime, actor);
        }
    }
}
