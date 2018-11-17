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
        public static bool exit = false;

        public MouseButtonController(string id, ControllerType controllerType, MouseManager mouseManager) : base(id, controllerType)
        {
            this.mouseManager = mouseManager;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            DrawnActor2D parentActor = actor as DrawnActor2D;

            

            if(parentActor.Transform.Bounds.Intersects(this.mouseManager.Bounds))
            {
                TrigonometricParameters trig = new TrigonometricParameters(1, 0.1f);
                this.totalElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                float lerpFactor = MathUtility.Sin(trig, this.totalElapsedTime);

                parentActor.Color = MathUtility.Lerp(Color.Red, Color.Blue, lerpFactor);
                parentActor.Transform.Scale = MathUtility.Lerp(parentActor.Transform.Scale * 0.999f, parentActor.Transform.Scale * 1.001f  , lerpFactor);
                Console.WriteLine(parentActor.Transform.Scale);

                if (this.mouseManager.IsLeftButtonClicked())
                {
                    if (parentActor.ID.Equals("menu1"))
                    {
                        EventDispatcher.Publish(new EventData("options", null, EventActionType.OnNewMenu, EventCategoryType.Menu));
                    }else if(parentActor.ID.Equals("exit1"))
                    {
                        exit = true;
                    }
                }
            }
            else
            {
                parentActor.Transform.Scale = parentActor.Transform.OriginalTransform2D.Scale;
                parentActor.Color = parentActor.OriginalColor;
            }

            base.Update(gameTime, actor);
        }
    }
}
