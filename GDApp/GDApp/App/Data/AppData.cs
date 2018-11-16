/*
Function: 		Stores common hard-coded variable values used within the game e.g. key mappings, mouse sensitivity
Author: 		NMCG
Version:		1.0
Date Updated:	5/10/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace GDLibrary
{
    public sealed class LerpSpeed
    {
        private static readonly float SpeedMultiplier = 2;
        public static readonly float VerySlow = 0.008f; 
        public static readonly float Slow = SpeedMultiplier * VerySlow;
        public static readonly float Medium = SpeedMultiplier * Slow;
        public static readonly float Fast = SpeedMultiplier * Medium;
        public static readonly float VeryFast = SpeedMultiplier * Fast;
    }

    public sealed class AppData
    {
        #region Common
        public static int IndexMoveForward = 0;
        public static int IndexMoveBackward = 1;
        public static int IndexRotateLeft = 2;
        public static int IndexRotateRight = 3;
        public static int IndexMoveJump = 4;
        public static int IndexMoveCrouch = 5;
        public static int IndexStrafeLeft = 6;
        public static int IndexStrafeRight = 7;
        #endregion

        #region Car
        public static readonly float CarRotationSpeed = 0.1f;
        public static readonly float CarMoveSpeed = 0.09f;
        public static readonly float CarStrafeSpeed = 0.7f * CarMoveSpeed;
        #endregion

        #region Camera
        public static readonly float CameraRotationSpeed = 0.01f;
        public static readonly float CameraMoveSpeed = 0.075f;
        public static readonly float CameraStrafeSpeed = 0.7f * CameraMoveSpeed;
    
        public static readonly Keys[] CameraMoveKeys = { Keys.W, Keys.S, Keys.A, Keys.D, 
                                         Keys.Space, Keys.C, Keys.LeftShift, Keys.RightShift};
        public static readonly Keys[] CameraMoveKeys_Alt1 = { Keys.T, Keys.G, Keys.F, Keys.H };

        //3rd person specific
        public static readonly float CameraThirdPersonScrollSpeedDistanceMultiplier = 0.00125f;
        public static readonly float CameraThirdPersonScrollSpeedElevationMultiplier = 0.001f;
        public static readonly float CameraThirdPersonDistance = 60;
        public static readonly float CameraThirdPersonElevationAngleInDegrees = 30;

        //security camera
        public static readonly Vector3 SecurityCameraRotationAxisYaw = Vector3.UnitX;
        public static readonly Vector3 SecurityCameraRotationAxisPitch = Vector3.UnitY;
        public static readonly Vector3 SecurityCameraRotationAxisRoll = Vector3.UnitZ;

        #endregion

        public static readonly Keys[] ObjectMoveKeys = {
            Keys.NumPad8, Keys.NumPad5,  //forward, backward
            Keys.NumPad4, Keys.NumPad6,  //rotate left, rotate right
            Keys.NumPad1, Keys.NumPad3   //strafe left, strafe right
        };

        #region Menu
        public static readonly string MenuMainID = "main";
        #endregion
    }
}
