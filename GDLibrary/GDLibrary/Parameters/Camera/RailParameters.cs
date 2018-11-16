/*
Function: 		Represents a bounded rail in 3D along which an object can translate. Typically used by a rail controller attached to a camera which causes the camera to follow a moving object in a room.
Author: 		NMCG
Version:		1.0
Date Updated:	
Bugs:			None
Fixes:			None
*/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class RailParameters
    {
        #region Fields
        private string id;
        private Vector3 start, end, midPoint, look;
        private bool isDirty;
        private float length;
        #endregion

        #region Properties
        public Vector3 Look
        {
            get
            {
                Update();
                return this.look;
            }
        }

        public float Length
        {
            get
            {
                Update();
                return this.length;
            }
        }

        public Vector3 MidPoint
        {
            get
            {
                Update();
                return this.midPoint;
            }
        }
        public Vector3 Start
        {
            get
            {
                return this.start;
            }
            set
            {
                this.start = value;
                this.isDirty = true;
            }
        }
        public Vector3 End
        {
            get
            {
                return this.end;
            }
            set
            {
                this.start = value;
                this.isDirty = true;
            }
        }
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        #endregion

        public RailParameters(string id, Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
            this.id = id;

            this.isDirty = true;
        }


        //Returns true if the position is between start and end, otherwise false
        public bool InsideRail(Vector3 position)
        {
            float distanceToStart = Vector3.Distance(position, start);
            float distanceToEnd = Vector3.Distance(position, end);
            return ((distanceToStart <= length) && (distanceToEnd <= length));
        }


        private void Update()
        {
            if (this.isDirty)
            {
                this.length = Math.Abs(Vector3.Distance(start, end));
                this.look = Vector3.Normalize(end - start);
                this.midPoint = (start + end) / 2;
                this.isDirty = false;
            }
        }

        public override bool Equals(object obj)
        {
            var parameters = obj as RailParameters;
            return parameters != null &&
                   id == parameters.id &&
                   start.Equals(parameters.start) &&
                   end.Equals(parameters.end) &&
                   midPoint.Equals(parameters.midPoint) &&
                   look.Equals(parameters.look);
        }

        public override int GetHashCode()
        {
            var hashCode = -1007939936;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(id);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(start);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(end);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(midPoint);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(look);
            return hashCode;
        }

        public object Clone()
        {
            return new RailParameters(this.id, this.start, this.end); //deep copy
        }

    }
}
