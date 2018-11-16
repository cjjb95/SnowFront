/*
Function: 		A stateful variable to store the a succession of boolean states where the number stored is defined by capacity.
Author: 		NMCG
Version:		1.0
Date Updated:	10/10/17
Bugs:			None
Fixes:			None
*/

using System.Collections.Generic;
using System.Text;

namespace GDLibrary
{
    public class StatefulBool
    {
        private int capacity;
        private List<bool> stateList;

        public StatefulBool(int capacity)
        {
            this.capacity = capacity;
            this.stateList = new List<bool>(capacity);
        }

        public void Update(bool state)
        {
            this.stateList.Insert(0, state);

            //ensure that there are always just "capacity" states stored
            if(this.stateList.Count > this.capacity)
                this.stateList.RemoveAt(this.stateList.Count - 1);
        }

        //returns true if state goes from false to true
        public bool IsActivating()
        {
            if (this.stateList.Count >= 2)
                return (this.stateList[0] && !this.stateList[1]);
            else
                return false;
        }

        //returns true if state goes from true to false
        public bool IsDeactivating()
        {
            if (this.stateList.Count >= 2)
                return (!this.stateList[0] && this.stateList[1]);
            else
                return false;
        }

        //returns the last stored state
        public bool IsActive()
        {
            if (this.stateList.Count >= 1)
                return this.stateList[0];
            else
                return false;
        }

        //returns true if active over first two successive states in the list
        public bool IsStillActive()
        {
            if (this.stateList.Count > 1)
                return this.stateList[0] && this.stateList[1];
            else
                return false;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            foreach(bool state in this.stateList)
            {
                str.Append(state);
                str.Append(",");
            }

            return str.ToString();
        }

    }
}
