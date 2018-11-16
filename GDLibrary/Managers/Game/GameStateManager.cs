/*
Function: 		The "brain" for dynamic events in the game. This class listens for events and responds with changes to the game.
                For example, if the player wins/loses the game then this class will determine what happens as a consequence.
                It may, in this case, show certain UITextObjects, play sounds, reset controllers, generate new collidable objects.
Author: 		NMCG
Version:		1.0
Date Updated:	16/11/17
Bugs:			
Fixes:			None
*/

using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class GameStateManager : PausableGameComponent
    {
        public GameStateManager(Game game, EventDispatcher eventDispatcher, StatusType statusType) 
            : base(game, eventDispatcher, statusType)
        {

        }

        #region Event Handling
        protected override void EventDispatcher_MenuChanged(EventData eventData)
        {
            //did the event come from the main menu and is it a start game event
            if (eventData.EventType == EventActionType.OnStart)
            {
                //turn on update and draw i.e. hide the menu
                this.StatusType = StatusType.Update | StatusType.Drawn;
            }
            //did the event come from the main menu and is it a pause game event
            else if (eventData.EventType == EventActionType.OnPause)
            {
                //turn off update and draw i.e. show the menu since the game is paused
                this.StatusType = StatusType.Off;
            }
        }
        #endregion
    }
}
