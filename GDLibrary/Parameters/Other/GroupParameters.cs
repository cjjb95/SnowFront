/*
Function: 		Encapsulates the group parameters (name, ID, subgroupID) for any drawn 2D or 3D object. These parameters will allow
                us to search the ObjectManager, UIManager, MenuManager, and CameraManager for objects that belong to the same group
                (and subgroup) so that we can change object properties (e.g. StatusType, Color, Texture, enable/disable controllers)
                en masse. This class could be used to send an event to a particular group of controllers or actors.

Author: 		NMCG
Version:		1.0
Date Updated:	12/11/17
Bugs:			None
Fixes:			None
*/
using System;

namespace GDLibrary
{
    public class GroupParameters : ICloneable
    {
        //some defaults in case we're not really interested in using group parameters in any real detail
        public static readonly GroupParameters Zero = new GroupParameters("Zero", 0, 0);
        public static readonly GroupParameters One = new GroupParameters("One", 1, 0);
        public static readonly GroupParameters Two = new GroupParameters("Two", 2, 0);

        #region Fields
        private string name;
        private int uniqueGroupID;
        private int uniqueSubGroupID;
        #endregion

        #region  Properties
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = (value.Length != 0) ? value : "default";
            }
        }
        public int UniqueGroupID
        {
            get
            {
                return this.uniqueGroupID;
            }
            set
            {
                this.uniqueGroupID = (value < 0) ? value : 0; 
            }
        }
        public int UniqueSubGroupID
        {
            get
            {
                return this.uniqueSubGroupID;
            }
            set
            {
                this.uniqueSubGroupID = (value < 0) ? value : 0;
            }
        }
        //allows us to determine if the GroupParameters for an actor were "actively" set by the developer
        public bool WasSet
        {
            get
            {
                return !this.name.Equals("default") && this.uniqueGroupID != -1;
            }
        }
        #endregion

        public GroupParameters()
            : this("default", -1, -1)
        {
        }

        public GroupParameters(int uniqueGroupID)
         : this("common group name", uniqueGroupID, -1)
        {
        }

        public GroupParameters(string name, int uniqueGroupID)
            : this(name, uniqueGroupID, -1)
        {
        }

        public GroupParameters(string name, int uniqueGroupID, int uniqueSubGroupID)
        {
            this.Name = name;
            this.UniqueGroupID = uniqueGroupID;
            this.UniqueSubGroupID = uniqueSubGroupID;
        }

        public override bool Equals(object obj)
        {
            GroupParameters other = obj as GroupParameters;
            return this.name.Equals(other.Name) 
                && this.uniqueGroupID == other.UniqueGroupID
                    && this.uniqueSubGroupID == other.UniqueSubGroupID;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 11 + this.name.GetHashCode();
            hash = hash * 17 + this.uniqueGroupID.GetHashCode();
            hash = hash * 31 + this.uniqueSubGroupID.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return "Name:" + this.name + ", UniqueGroupID: " + this.uniqueGroupID + ", UniqueSubGroupID: " + this.UniqueSubGroupID;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
