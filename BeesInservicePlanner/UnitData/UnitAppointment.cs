using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeesInservicePlanner.UnitData
{
    public class UnitAppointment
    {
        public bool Locked { get; set; }
        public Unit Unit { get; set; }
        public DateTime TimeSlot { get; set; }

        public UnitAppointment(Unit unit, DateTime timeSlot, bool locked = false)
        {
            this.Unit = unit;
            this.TimeSlot = timeSlot;
            this.Locked = locked;
        }
    }
}
