using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    //Wk4
    public class KeyboardManager : GameComponent
    {
        private KeyboardState newState, oldState;

        public KeyboardManager(Game game) : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            this.oldState = this.newState;
            this.newState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public bool IsFirstKeyPress(Keys key)
        {
            return this.newState.IsKeyDown(key) && this.oldState.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return this.newState.IsKeyDown(key);
        }

        public bool AreAnyKeysPressed()
        {
            return this.newState.GetPressedKeys().Length == 0;
        }


    }
}
