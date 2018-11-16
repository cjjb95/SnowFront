/*
Function: 		Base class for all "sine lerp-able" 3D controllers e.g. See ColorLerpController
Author: 		NMCG
Version:		1.0
Date Updated:	24/10/17
Bugs:			None
Fixes:			None
*/
using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class SineLerpController : Controller
    {
        #region Fields
        private TrigonometricParameters trigonometricParameters;
        #endregion

        #region Properties
        public TrigonometricParameters TrigonometricParameters
        {
            get
            {
                return this.trigonometricParameters;
            }
            set
            {
                this.trigonometricParameters = value;
            }
        }
        #endregion

        public SineLerpController(string id, ControllerType controllerType, TrigonometricParameters trigonometricParameters)
            : base(id, controllerType)
        {
            this.trigonometricParameters = trigonometricParameters;
        }
    }
}
