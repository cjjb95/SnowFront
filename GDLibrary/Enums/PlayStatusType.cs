/*
Function: 		Used by Actor to define the play state for media (e.g. a video, sound, animated billboard) or for a track controller (e.g. play, reset, pause)
Author: 		NMCG
Version:		1.0
Date Updated:	31/8/17
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public enum PlayStatusType
    {
        //notice that we have no powers of two value assignments (unlike with StatusType) since these states are all mutually exclusive
        Play,
        Pause,
        Stop,
        Reset
    }
}
