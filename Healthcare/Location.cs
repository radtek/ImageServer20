using System;
using System.Collections;
using System.Text;

using Iesi.Collections;
using ClearCanvas.Enterprise;


namespace ClearCanvas.Healthcare {


    /// <summary>
    /// Location entity
    /// </summary>
	public partial class Location : Entity
	{
        private void CustomInitialize()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}", this.Bed, this.Room, this.Floor, this.Building, this.Facility.ToString());
        }
    }
}