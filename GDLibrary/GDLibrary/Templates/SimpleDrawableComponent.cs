using Microsoft.Xna.Framework;

//a template class to use as a basis for any new Drawable/-GameComponent classes
namespace GDLibrary
{
    public class SimpleDrawableComponent : DrawableGameComponent
    {
        public SimpleDrawableComponent(Game game)
        : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
